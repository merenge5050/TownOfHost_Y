namespace TownOfHostY.Roles.Core.Interfaces;

public interface IAdditionalWinner
{
    public bool CheckWin(out AdditionalWinners winnerType);
}
