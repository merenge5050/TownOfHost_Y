using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;

using TownOfHostY.Modules;
using TownOfHostY.Roles.Core;
using static TownOfHostY.Translator;

namespace TownOfHostY
{
    [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.OnGameEnd))]
    class EndGamePatch
    {
        public static Dictionary<byte, string> SummaryText = new();
        public static Dictionary<byte, string> SDSummaryText = new();
        public static string KillLog = "";
        static Task taskSendDiscord;
        public static void Postfix(AmongUsClient __instance, [HarmonyArgument(0)] ref EndGameResult endGameResult)
        {
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            GameStates.InGame = false;

            Logger.Info("-----------ゲーム終了-----------", "Phase");
            Main.NormalOptions.NumImpostors = ChangeRoleSettings.ImpostorSetNum;
            if (!GameStates.IsModHost) return;
            SummaryText = new();
            SDSummaryText = new();
            foreach (var id in PlayerState.AllPlayerStates.Keys)
            {
                SummaryText[id] = Utils.SummaryTexts(true, id, disableColor: false);
                SDSummaryText[id] = Utils.SummaryTexts(false, id, disableColor: false);
            }
            var sb = new StringBuilder(GetString("KillLog") + ":");
            foreach (var kvp in PlayerState.AllPlayerStates.OrderBy(x => x.Value.RealKiller.Item1.Ticks))
            {
                var date = kvp.Value.RealKiller.Item1;
                if (date == DateTime.MinValue) continue;
                var killerId = kvp.Value.GetRealKiller();
                var targetId = kvp.Key;
                sb.Append($"\n{date:T} {Main.AllPlayerNames[targetId]}({Utils.GetTrueRoleName(targetId, false)}{Utils.GetSubRolesText(targetId)}) [{Utils.GetVitalText(kvp.Key)}]");
                if (killerId != byte.MaxValue && killerId != targetId)
                    sb.Append($"\n\t\t⇐ {Main.AllPlayerNames[killerId]}({Utils.GetTrueRoleName(killerId, false)}{Utils.GetSubRolesText(killerId)})");
            }
            KillLog = sb.ToString();

            Main.NormalOptions.KillCooldown = Options.DefaultKillCooldown;
            //winnerListリセット
            TempData.winners = new Il2CppSystem.Collections.Generic.List<WinningPlayerData>();
            var winner = new List<PlayerControl>();
            foreach (var pc in Main.AllPlayerControls)
            {
                if (CustomWinnerHolder.WinnerIds.Contains(pc.PlayerId)) winner.Add(pc);
            }
            foreach (var team in CustomWinnerHolder.WinnerRoles)
            {
                winner.AddRange(Main.AllPlayerControls.Where(p => p.Is(team) && !winner.Contains(p)));
            }

            //HideAndSeek専用
            if (Options.CurrentGameMode == CustomGameMode.HideAndSeek &&
                CustomWinnerHolder.WinnerTeam != CustomWinner.Draw && CustomWinnerHolder.WinnerTeam != CustomWinner.None)
            {
                winner = new();
                foreach (var pc in Main.AllPlayerControls)
                {
                    var role = PlayerState.GetByPlayerId(pc.PlayerId).MainRole;
                    if (role.GetCustomRoleTypes() == CustomRoleTypes.Impostor)
                    {
                        if (CustomWinnerHolder.WinnerTeam == CustomWinner.Impostor)
                            winner.Add(pc);
                    }
                    else if (role.GetCustomRoleTypes() == CustomRoleTypes.Crewmate)
                    {
                        if (CustomWinnerHolder.WinnerTeam == CustomWinner.Crewmate)
                            winner.Add(pc);
                    }
                    else if (role == CustomRoles.HASTroll && pc.Data.IsDead)
                    {
                        //トロールが殺されていれば単独勝ち
                        winner = new()
                        {
                            pc
                        };
                        break;
                    }
                    else if (role == CustomRoles.HASFox && CustomWinnerHolder.WinnerTeam != CustomWinner.HASTroll && !pc.Data.IsDead)
                    {
                        winner.Add(pc);
                        CustomWinnerHolder.AdditionalWinnerTeams.Add(AdditionalWinners.HASFox);
                    }
                }
            }
            Main.winnerList = new();
            foreach (var pc in winner)
            {
                if (CustomWinnerHolder.WinnerTeam is not CustomWinner.Draw && pc.Is(CustomRoles.GM)) continue;

                TempData.winners.Add(new WinningPlayerData(pc.Data));
                Main.winnerList.Add(pc.PlayerId);
            }

            if (PlayerControl.LocalPlayer.PlayerId == 0)
            {
                taskSendDiscord = Task.Run(() =>
            {
                if (CustomWinnerHolder.WinnerTeam == CustomWinner.Draw)
                    Logger.Info("廃村のため試合結果の送信をキャンセル", "Webhook");
                else
                {
                    //0:Imp 1:Mad 2:Crew 3:Neu
                    bool[] isSend = { false, false, false, false };
                    StringBuilder[] sendMasagge = { new StringBuilder(), new StringBuilder(), new StringBuilder(), new StringBuilder() };
                    foreach (var IS in PlayerState.AllPlayerStates)
                    {
                        var id = IS.Key; var state = IS.Value;
                        var role = state.MainRole;
                        if (role.IsVanilla()) continue;
                        int roleTypeNumber = (int)role.GetCustomRoleTypes();

                        if (isSend[roleTypeNumber]) sendMasagge[roleTypeNumber].Append("/");
                        if (Main.winnerList.Contains(id)) sendMasagge[roleTypeNumber].Append("★");
                        sendMasagge[roleTypeNumber].Append(Utils.GetTrueRoleName(id).RemoveHtmlTags());
                        isSend[roleTypeNumber] = true;
                    }
                    var hostName = SendDiscord.HostRandomName + ":arrow_forward:";
                    for (int i = 0; i < 4; i++)
                    {
                        if (isSend[i]) SendDiscord.SendWebhook((SendDiscord.MassageType)i, hostName + sendMasagge[i].ToString());
                    }
                }
            });
            }

            Main.VisibleTasksCount = false;
            if (AmongUsClient.Instance.AmHost)
            {
                Main.RealOptionsData.Restore(GameOptionsManager.Instance.CurrentGameOptions);
                GameOptionsSender.AllSenders.Clear();
                GameOptionsSender.AllSenders.Add(new NormalGameOptionsSender());
                /* Send SyncSettings RPC */
            }
            //オブジェクト破棄
            CustomRoleManager.Dispose();
        }
    }
    [HarmonyPatch(typeof(EndGameManager), nameof(EndGameManager.SetEverythingUp))]
    class SetEverythingUpPatch
    {
        public static string LastWinsText = "";

        public static void Postfix(EndGameManager __instance)
        {
            if (!Main.playerVersion.ContainsKey(0)) return;
            //#######################################
            //          ==勝利陣営表示==
            //#######################################

            //__instance.WinText.alignment = TMPro.TextAlignmentOptions.Right;
            var WinnerTextObject = UnityEngine.Object.Instantiate(__instance.WinText.gameObject);
            WinnerTextObject.transform.position = new(__instance.WinText.transform.position.x/* + 2.4f*/, __instance.WinText.transform.position.y - 0.5f, __instance.WinText.transform.position.z);
            WinnerTextObject.transform.localScale = new(0.6f, 0.6f, 0.6f);
            var WinnerText = WinnerTextObject.GetComponent<TMPro.TextMeshPro>(); //WinTextと同じ型のコンポーネントを取得
            WinnerText.fontSizeMin = 3f;
            WinnerText.text = "";

            string CustomWinnerText = "";
            string AdditionalWinnerText = "";
            string CustomWinnerColor = Utils.GetRoleColorCode(CustomRoles.Crewmate);

            var winnerRole = (CustomRoles)CustomWinnerHolder.WinnerTeam;
            if (winnerRole >= 0)
            {
                CustomWinnerText = Utils.GetRoleName(winnerRole);
                CustomWinnerColor = Utils.GetRoleColorCode(winnerRole);
                if (winnerRole.IsNeutral())
                {
                    __instance.BackgroundBar.material.color = Utils.GetRoleColor(winnerRole);
                }
            }
            if (AmongUsClient.Instance.AmHost && PlayerState.GetByPlayerId(0).MainRole == CustomRoles.GM)
            {
                __instance.WinText.text = "Game Over";
                __instance.WinText.color = Utils.GetRoleColor(CustomRoles.GM);
                __instance.BackgroundBar.material.color = Utils.GetRoleColor(CustomRoles.GM);
            }
            switch (CustomWinnerHolder.WinnerTeam)
            {
                //通常勝利
                case CustomWinner.Crewmate:
                    CustomWinnerColor = Utils.GetRoleColorCode(CustomRoles.Engineer);
                    break;
                //特殊勝利
                case CustomWinner.Terrorist:
                    __instance.Foreground.material.color = Color.red;
                    break;
                case CustomWinner.Lovers:
                    __instance.BackgroundBar.material.color = Utils.GetRoleColor(CustomRoles.Lovers);
                    break;
                //引き分け処理
                case CustomWinner.Draw:
                    __instance.WinText.text = GetString("ForceEnd");
                    __instance.WinText.color = Color.white;
                    __instance.BackgroundBar.material.color = Color.gray;
                    WinnerText.text = GetString("ForceEndText");
                    WinnerText.color = Color.gray;
                    break;
                //全滅
                case CustomWinner.None:
                    __instance.WinText.text = "";
                    __instance.WinText.color = Color.black;
                    __instance.BackgroundBar.material.color = Color.gray;
                    WinnerText.text = GetString("EveryoneDied");
                    WinnerText.color = Color.gray;
                    break;
            }

            foreach (var additionalWinners in CustomWinnerHolder.AdditionalWinnerTeams)
            {
                var addWinnerRole = (CustomRoles)additionalWinners;
                var addWinnerName = (additionalWinners == AdditionalWinners.Pursuer) ? GetString("Pursuer") : Utils.GetRoleName(addWinnerRole);
                AdditionalWinnerText += "＆" + Utils.ColorString(Utils.GetRoleColor(addWinnerRole), addWinnerName);
            }
            if (CustomWinnerHolder.WinnerTeam is not CustomWinner.Draw and not CustomWinner.None)
            {
                WinnerText.text = $"<color={CustomWinnerColor}>{CustomWinnerText}{AdditionalWinnerText}{GetString("Win")}</color>";
            }
            LastWinsText = WinnerText.text.RemoveHtmlTags();

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //#######################################
            //           ==最終結果表示==
            //#######################################

            var Pos = Camera.main.ViewportToWorldPoint(new Vector3(0f, 1f, Camera.main.nearClipPlane));
            var RoleSummaryObject = UnityEngine.Object.Instantiate(__instance.WinText.gameObject);
            RoleSummaryObject.transform.position = new Vector3(__instance.Navigation.ExitButton.transform.position.x + 0.1f, Pos.y - 0.1f, -15f);
            RoleSummaryObject.transform.localScale = new Vector3(1f, 1f, 1f);

            StringBuilder sb = new($"{GetString("RoleSummaryText")}");
            List<byte> cloneRoles = new(PlayerState.AllPlayerStates.Keys);
            foreach (var id in Main.winnerList)
            {
                sb.Append($"\n<color={CustomWinnerColor}>★</color> ").Append(EndGamePatch.SummaryText[id]);
                cloneRoles.Remove(id);
            }
            foreach (var id in cloneRoles)
            {
                sb.Append($"\n　 ").Append(EndGamePatch.SummaryText[id]);
            }
            var RoleSummary = RoleSummaryObject.GetComponent<TMPro.TextMeshPro>();
            RoleSummary.alignment = TMPro.TextAlignmentOptions.TopLeft;
            RoleSummary.color = Color.white;
            RoleSummary.outlineWidth *= 1.2f;
            RoleSummary.fontSizeMin = RoleSummary.fontSizeMax = RoleSummary.fontSize = 1.25f;

            var RoleSummaryRectTransform = RoleSummary.GetComponent<RectTransform>();
            RoleSummaryRectTransform.anchoredPosition = new Vector2(Pos.x + 3.5f, Pos.y - 0.1f);
            RoleSummary.text = sb.ToString();

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //参考：https://github.com/Hyz-sui/TownOfHost-H

            if (PlayerControl.LocalPlayer.PlayerId == 0)
            {
                if (CustomWinnerHolder.WinnerTeam == CustomWinner.Draw)
                    Logger.Info("廃村のため試合結果の送信をキャンセル", "Webhook");
                else
                {
                    var resultMessage = new StringBuilder();
                    resultMessage.Append($">>> {Main.ForkId} {Main.PluginVersion} ");
                    if (AmongUsClient.Instance.IsGamePublic) resultMessage.Append("◆");
                    else if (GameStates.IsLocalGame) resultMessage.Append("▽");
                    resultMessage.Append(SendDiscord.HostRandomName).Append($" {DateTime.Now.ToString("T")}------------\n");
                    foreach (var id in Main.winnerList)
                    {
                        resultMessage.Append(SendDiscord.ColorIdToDiscordEmoji(Palette.PlayerColors.IndexOf(Main.PlayerColors[id]), !PlayerState.GetByPlayerId(id).IsDead)).Append(":star:").Append(EndGamePatch.SDSummaryText[id].RemoveHtmlTags()).Append("\n");
                    }
                    foreach (var id in cloneRoles)
                    {
                        resultMessage.Append(SendDiscord.ColorIdToDiscordEmoji(Palette.PlayerColors.IndexOf(Main.PlayerColors[id]), !PlayerState.GetByPlayerId(id).IsDead)).Append("\u3000").Append(EndGamePatch.SDSummaryText[id].RemoveHtmlTags()).Append("\n");
                    }
                    SendDiscord.SendWebhook(SendDiscord.MassageType.Result, resultMessage.ToString(), GetString("LastResult"));
                }
            }
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //Utils.ApplySuffix();
        }
    }
}