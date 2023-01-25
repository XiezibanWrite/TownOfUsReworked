using HarmonyLib;
using TownOfUsReworked.Enums;
using TownOfUsReworked.Extensions;
using TownOfUsReworked.PlayerLayers.Roles.Roles;

namespace TownOfUsReworked.PlayerLayers.Roles.CrewRoles.TransporterMod
{
    [HarmonyPatch(typeof(KillButton), nameof(KillButton.DoClick))]
    public class PerformKillButton
    {
        public static bool Prefix(KillButton __instance)
        {
            if (Utils.CannotUseButton(PlayerControl.LocalPlayer, RoleEnum.Transporter))
                return false;

            var role = Role.GetRole<Transporter>(PlayerControl.LocalPlayer);

            if (Utils.CannotUseButton(PlayerControl.LocalPlayer, RoleEnum.Transporter, null, __instance) || __instance != role.TransportButton)
                return false;

            if (role.TransportTimer() != 0f && __instance == role.TransportButton)
                return false;

            if (role.TransportList == null && role.ButtonUsable)
            {
                role.PressedButton = true;
                role.MenuClick = true;
            }

            return false;
        }
    }
}