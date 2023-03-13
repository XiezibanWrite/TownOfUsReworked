using HarmonyLib;
using TownOfUsReworked.Enums;
using TownOfUsReworked.Classes;
using TownOfUsReworked.CustomOptions;

namespace TownOfUsReworked.PlayerLayers.Roles.NeutralRoles.NecromancerMod
{
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public class HUDRevive
    {
        public static void Postfix(HudManager __instance)
        {
            if (Utils.NoButton(PlayerControl.LocalPlayer, RoleEnum.Necromancer))
                return;

            var role = Role.GetRole<Necromancer>(PlayerControl.LocalPlayer);

            if (role.ResurrectButton == null)
                role.ResurrectButton = Utils.InstantiateButton();

            if (role.KillButton == null)
                role.KillButton = Utils.InstantiateButton();

            role.ResurrectButton.UpdateButton(role, "RESURRECT", role.ResurrectTimer(), CustomGameOptions.ResurrectCooldown + (CustomGameOptions.ResurrectCooldownIncrease *
                role.ResurrectedCount), TownOfUsReworked.RessurectSprite, AbilityTypes.Dead, null, role.ResurrectButtonUsable, true, role.IsResurrecting, role.TimeRemaining, 
                CustomGameOptions.NecroResurrectDuration, role.ResurrectButtonUsable, role.ResurrectUsesLeft);

            role.KillButton.UpdateButton(role, "KILL", role.KillTimer(), CustomGameOptions.NecroKillCooldown + (CustomGameOptions.NecroKillCooldownIncrease * role.KillCount),
                TownOfUsReworked.IntruderKill, AbilityTypes.Direct, null, true, role.KillUsesLeft, role.KillButtonUsable);
        }
    }
}