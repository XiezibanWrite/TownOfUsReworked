using HarmonyLib;
using TownOfUsReworked.Enums;
using TownOfUsReworked.Classes;
using TownOfUsReworked.PlayerLayers.Roles.Roles;

namespace TownOfUsReworked.PlayerLayers.Roles.NeutralRoles.CannibalMod
{
    [HarmonyPatch(typeof(KillButton), nameof(KillButton.SetTarget))]
    public class KillButtonTarget
    {
        public static bool Prefix(KillButton __instance)
        {
            return !PlayerControl.LocalPlayer.Is(RoleEnum.Cannibal);
        }

        public static void SetTarget(KillButton __instance, DeadBody target, Cannibal role)
        {
            if (role.CurrentTarget && role.CurrentTarget != target)
                role.CurrentTarget.bodyRenderer.material.SetFloat("_Outline", 0f);

            role.CurrentTarget = target;
            
            if (role.CurrentTarget != null && Utils.ButtonUsable(__instance))
            {
                var component = role.CurrentTarget.bodyRenderer;
                component.material.SetFloat("_Outline", 1f);
                component.material.SetColor("_OutlineColor", role.Color);
                __instance.graphic.color = Palette.EnabledColor;
                __instance.graphic.material.SetFloat("_Desat", 0f);
            }
            else
            {
                __instance.graphic.color = Palette.DisabledClear;
                __instance.graphic.material.SetFloat("_Desat", 1f);
            }
        }
    }
}