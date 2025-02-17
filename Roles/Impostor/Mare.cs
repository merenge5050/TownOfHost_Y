using AmongUs.GameOptions;

using TownOfHostY.Roles.Core;
using TownOfHostY.Roles.Core.Interfaces;
using static TownOfHostY.Options;

namespace TownOfHostY.Roles.Impostor;

public sealed class Mare : RoleBase, IImpostor
{
    public static readonly SimpleRoleInfo RoleInfo =
        SimpleRoleInfo.Create(
            typeof(Mare),
            player => new Mare(player),
            CustomRoles.Mare,
            () => RoleTypes.Impostor,
            CustomRoleTypes.Impostor,
            11100,
            SetupCustomOption,
            "メアー"
        );
    public Mare(PlayerControl player)
    : base(
        RoleInfo,
        player
    )
    {
        KillCooldownInLightsOut = OptionKillCooldownInLightsOut.GetFloat();
        SpeedInLightsOut = OptionSpeedInLightsOut.GetFloat();

        IsActivateKill = false;
        IsAccelerated = false;

    }

    private static OptionItem OptionKillCooldownInLightsOut;
    private static OptionItem OptionSpeedInLightsOut;
    enum OptionName
    {
        MareAddSpeedInLightsOut,
        MareKillCooldownInLightsOut,
    }
    private float KillCooldownInLightsOut;
    private float SpeedInLightsOut;
    private static bool IsActivateKill;
    private bool IsAccelerated;  //加速済みかフラグ

    public static void SetupCustomOption()
    {
        OptionSpeedInLightsOut = FloatOptionItem.Create(RoleInfo, 10, OptionName.MareAddSpeedInLightsOut, new(0.1f, 0.5f, 0.1f), 0.3f, false)
        .SetValueFormat(OptionFormat.Multiplier);
            OptionKillCooldownInLightsOut = FloatOptionItem.Create(RoleInfo, 11, OptionName.MareKillCooldownInLightsOut, new(2.5f, 180f, 2.5f), 15f, false)
            .SetValueFormat(OptionFormat.Seconds);
    }
    public bool CanUseKillButton() => IsActivateKill;
    public float CalculateKillCooldown() => IsActivateKill ? KillCooldownInLightsOut : DefaultKillCooldown;
    public override void ApplyGameOptions(IGameOptions opt)
    {
        if (IsActivateKill && !IsAccelerated)
        { //停電中で加速済みでない場合
            IsAccelerated = true;
            Main.AllPlayerSpeed[Player.PlayerId] += SpeedInLightsOut;//Mareの速度を加算
        }
        else if (!IsActivateKill && IsAccelerated)
        { //停電中ではなく加速済みになっている場合
            IsAccelerated = false;
            Main.AllPlayerSpeed[Player.PlayerId] -= SpeedInLightsOut;//Mareの速度を減算
        }
    }
    public override void OnFixedUpdate(PlayerControl player)
    {
        if (GameStates.IsInTask && IsActivateKill)
        {
            if (!Utils.IsActive(SystemTypes.Electrical))
            {
                //停電解除されたらキルモード解除
                IsActivateKill = false;
                Player.MarkDirtySettings();
                Utils.NotifyRoles();
            }
        }
    }
    public override bool OnSabotage(PlayerControl player, SystemTypes systemType, byte amount)
    {
        if (systemType == SystemTypes.Electrical)
        {
            if (amount.HasAnyBit(128))
            {
                _ = new LateTask(() =>
                {
                    //まだ停電が直っていなければキル可能モードに
                    if (Utils.IsActive(SystemTypes.Electrical))
                    {
                        IsActivateKill = true;
                        Player.MarkDirtySettings();
                        Utils.NotifyRoles();
                    }
                }, 4.0f, "Mare Activate Kill");
            }
        }
        return true;
    }
    public static bool KnowTargetRoleColor(PlayerControl target, bool isMeeting)
        => !isMeeting && IsActivateKill && target.Is(CustomRoles.Mare);

}