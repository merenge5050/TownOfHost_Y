using System.Collections.Generic;
using AmongUs.GameOptions;

using TownOfHostY.Modules;
using TownOfHostY.Roles.Core;

namespace TownOfHostY.Roles.Crewmate;
public sealed class Nimrod : RoleBase
{
    public static readonly SimpleRoleInfo RoleInfo =
        SimpleRoleInfo.Create(
            typeof(Nimrod),
            player => new Nimrod(player),
            CustomRoles.Nimrod,
            () => RoleTypes.Crewmate,
            CustomRoleTypes.Crewmate,
            41300,
            null,
            "ニムロッド",
            "#9fcc5b"
        );
    public Nimrod(PlayerControl player)
    : base(
        RoleInfo,
        player
    )
    {
        playerIdList = new();
        IsExecutionMeeting = byte.MaxValue;
    }
    public override void OnDestroy()
    {
        playerIdList.Clear();
    }

    public static List<byte> playerIdList = new();
    private static byte IsExecutionMeeting = byte.MaxValue;

    public override void Add()
    {
        playerIdList.Add(Player.PlayerId);
    }

    public static GameData.PlayerInfo VoteChange(GameData.PlayerInfo Exiled)
    {
        if (Exiled == null || !playerIdList.Contains(Exiled.PlayerId)) return Exiled;

        _ = new LateTask(() =>
        {
            Utils.GetPlayerById(Exiled.PlayerId).NoCheckStartMeeting(Exiled);
            IsExecutionMeeting = Exiled.PlayerId;
        }, 15f, "NimrodExiled");
        return null;
    }
    public override void OnStartMeeting()
    {
        if (IsExecutionMeeting == byte.MaxValue) return;

        Utils.SendMessage(Translator.GetString("IsNimrodMeetingText"),
            title: $"<color={RoleInfo.RoleColorCode}>{Translator.GetString("IsNimrodMeetingTitle")}</color>");
    }

    public override (byte? votedForId, int? numVotes, bool doVote) OnVote(byte voterId, byte sourceVotedForId)
    {
        var (votedForId, numVotes, doVote) = base.OnVote(voterId, sourceVotedForId);
        var baseVote = (votedForId, numVotes, doVote);
        if (IsExecutionMeeting != Player.PlayerId || voterId != Player.PlayerId)
        {
            return baseVote;
        }
        MeetingHudPatch.TryAddAfterMeetingDeathPlayers(CustomDeathReason.Vote, Player.PlayerId);

        if (sourceVotedForId <= 15)
        {
            Utils.GetPlayerById(sourceVotedForId).SetRealKiller(Player);
            PlayerState.GetByPlayerId(sourceVotedForId).DeathReason = CustomDeathReason.Execution;
        }
        MeetingVoteManager.Instance.ClearAndExile(Player.PlayerId, sourceVotedForId);
        IsExecutionMeeting = byte.MaxValue;
        return (votedForId, numVotes, false);
    }
}
