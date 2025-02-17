using AmongUs.GameOptions;

using TownOfHostY.Roles.Core;

namespace TownOfHostY
{
    static class CustomRolesHelper
    {
        public static readonly CustomRoles[] AllRoles = EnumHelper.GetAllValues<CustomRoles>();
        public static readonly CustomRoleTypes[] AllRoleTypes = EnumHelper.GetAllValues<CustomRoleTypes>();

        public static bool IsImpostor(this CustomRoles role)
        {
            var roleInfo = role.GetRoleInfo();
            if (roleInfo != null)
                return roleInfo.CustomRoleType == CustomRoleTypes.Impostor;

            return false;
        }
        public static bool IsMadmate(this CustomRoles role)
        {
            var roleInfo = role.GetRoleInfo();
            if (roleInfo != null)
                return roleInfo.CustomRoleType == CustomRoleTypes.Madmate;
            return
                role is
                CustomRoles.SKMadmate or
                CustomRoles.MSchrodingerCat;
        }
        public static bool IsImpostorTeam(this CustomRoles role) => role.IsImpostor() || role.IsMadmate();
        public static bool IsNeutral(this CustomRoles role)
        {
            var roleInfo = role.GetRoleInfo();
            if (roleInfo != null)
                return roleInfo.CustomRoleType == CustomRoleTypes.Neutral;
            return
                role is
                CustomRoles.SchrodingerCat or
                CustomRoles.EgoSchrodingerCat or
                CustomRoles.JSchrodingerCat or
                CustomRoles.DSchrodingerCat or
                CustomRoles.OSchrodingerCat or
                CustomRoles.HASTroll or
                CustomRoles.HASFox;
        }
        public static bool IsCrewmate(this CustomRoles role) => role.GetRoleInfo()?.CustomRoleType == CustomRoleTypes.Crewmate || (!role.IsImpostorTeam() && !role.IsNeutral());
        public static bool IsVanilla(this CustomRoles role)
        {
            return
                role is CustomRoles.Crewmate or
                CustomRoles.Engineer or
                CustomRoles.Scientist or
                CustomRoles.GuardianAngel or
                CustomRoles.Impostor or
                CustomRoles.Shapeshifter;
        }
        public static bool IsCannotPublicRole(this CustomRoles role)
        {
            return
                role is CustomRoles.Puppeteer
                    or CustomRoles.BountyHunter
                    or CustomRoles.Witch
                    or CustomRoles.SerialKiller
                    or CustomRoles.Vampire
                    or CustomRoles.Warlock
                    or CustomRoles.CursedWolf
                    or CustomRoles.EvilDiviner
                    or CustomRoles.MadGuardian
                    or CustomRoles.MadSheriff
                    or CustomRoles.SillySheriff
                    or CustomRoles.Sheriff
                    or CustomRoles.Medic
                    or CustomRoles.Arsonist
                    or CustomRoles.SchrodingerCat
                    or CustomRoles.PlatonicLover
                    or CustomRoles.LoveCutter
                    or CustomRoles.Totocalcio
                    or CustomRoles.Guarding;
        }

        public static bool IsAddAddOn(this CustomRoles role)
        {
            return role.IsMadmate() || 
                role is CustomRoles.Jackal or CustomRoles.JClient;
        }
        public static bool IsAddOn(this CustomRoles role) => role.IsBuffAddOn() || role.IsDebuffAddOn();
        public static bool IsBuffAddOn(this CustomRoles role)
        {
            return
                role is CustomRoles.AddWatch or
                CustomRoles.AddLight or
                CustomRoles.AddSeer or
                CustomRoles.Autopsy or
                CustomRoles.VIP or
                CustomRoles.Revenger or
                CustomRoles.Management or
                CustomRoles.Sending or
                CustomRoles.TieBreaker or
                CustomRoles.Loyalty or
                CustomRoles.PlusVote or
                CustomRoles.Guarding or
                CustomRoles.AddBait or
                CustomRoles.Refusing;
        }
        public static bool IsDebuffAddOn(this CustomRoles role)
        {
            return
                role is
                CustomRoles.Sunglasses or
                CustomRoles.Clumsy or
                CustomRoles.InfoPoor or
                CustomRoles.NonReport;
        }

        public static bool IsKilledSchrodingerCat(this CustomRoles role)
        {
            return role is
                CustomRoles.SchrodingerCat or
                CustomRoles.MSchrodingerCat or
                CustomRoles.CSchrodingerCat or
                CustomRoles.EgoSchrodingerCat or
                CustomRoles.DSchrodingerCat or
                CustomRoles.JSchrodingerCat or
                CustomRoles.OSchrodingerCat;
        }
        public static bool IsDirectKillRole(this CustomRoles role)
        {
            return role is
                CustomRoles.Arsonist or
                CustomRoles.PlatonicLover or
                CustomRoles.Totocalcio or
                CustomRoles.MadSheriff;
        }

        public static CustomRoleTypes GetCustomRoleTypes(this CustomRoles role)
        {
            CustomRoleTypes type = CustomRoleTypes.Crewmate;

            var roleInfo = role.GetRoleInfo();
            if (roleInfo != null)
                return roleInfo.CustomRoleType;

            if (role.IsImpostor()) type = CustomRoleTypes.Impostor;
            if (role.IsNeutral()) type = CustomRoleTypes.Neutral;
            if (role.IsMadmate()) type = CustomRoleTypes.Madmate;
            return type;
        }
        public static int GetCount(this CustomRoles role)
        {
            if (role.IsVanilla())
            {
                var roleOpt = Main.NormalOptions.RoleOptions;
                return role switch
                {
                    CustomRoles.Engineer => roleOpt.GetNumPerGame(RoleTypes.Engineer),
                    CustomRoles.Scientist => roleOpt.GetNumPerGame(RoleTypes.Scientist),
                    CustomRoles.Shapeshifter => roleOpt.GetNumPerGame(RoleTypes.Shapeshifter),
                    CustomRoles.GuardianAngel => roleOpt.GetNumPerGame(RoleTypes.GuardianAngel),
                    CustomRoles.Crewmate => roleOpt.GetNumPerGame(RoleTypes.Crewmate),
                    _ => 0
                };
            }
            else
            {
                return Options.GetRoleCount(role);
            }
        }
        public static int GetChance(this CustomRoles role)
        {
            if (role.IsVanilla())
            {
                var roleOpt = Main.NormalOptions.RoleOptions;
                return role switch
                {
                    CustomRoles.Engineer => roleOpt.GetChancePerGame(RoleTypes.Engineer),
                    CustomRoles.Scientist => roleOpt.GetChancePerGame(RoleTypes.Scientist),
                    CustomRoles.Shapeshifter => roleOpt.GetChancePerGame(RoleTypes.Shapeshifter),
                    CustomRoles.GuardianAngel => roleOpt.GetChancePerGame(RoleTypes.GuardianAngel),
                    CustomRoles.Crewmate => roleOpt.GetChancePerGame(RoleTypes.Crewmate),
                    _ => 0
                };
            }
            else
            {
                return Options.GetRoleChance(role);
            }
        }
        public static bool IsEnable(this CustomRoles role) => role.GetCount() > 0;
        public static bool CanMakeMadmate(this CustomRoles role)
        {
            if (role.GetRoleInfo() is SimpleRoleInfo info)
            {
                return info.CanMakeMadmate;
            }

            return false;
        }
        public static RoleTypes GetRoleTypes(this CustomRoles role)
        {
            var roleInfo = role.GetRoleInfo();
            if (roleInfo != null)
                return roleInfo.BaseRoleType.Invoke();
            return role switch
            {
                CustomRoles.GM => RoleTypes.GuardianAngel,

                _ => role.IsImpostor() ? RoleTypes.Impostor : RoleTypes.Crewmate,
            };
        }
    }
    public enum CountTypes
    {
        OutOfGame,
        None,
        Crew,
        Impostor,
        Jackal,
    }
}