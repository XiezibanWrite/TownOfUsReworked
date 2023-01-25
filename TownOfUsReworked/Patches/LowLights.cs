using HarmonyLib;
using TownOfUsReworked.Extensions;
using UnityEngine;
using TownOfUsReworked.Enums;
using TownOfUsReworked.Lobby.CustomOption;
using AmongUs.GameOptions;

namespace TownOfUsReworked.Patches
{
    [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.CalculateLightRadius))]
    public static class LowLights
    {
        public static bool Prefix(ShipStatus __instance, [HarmonyArgument(0)] GameData.PlayerInfo player, ref float __result)
        {
            if (GameOptionsManager.Instance.CurrentGameOptions.GameMode == GameModes.HideNSeek)
            {
                if (GameOptionsManager.Instance.currentHideNSeekGameOptions.useFlashlight)
                {
                    if (player.IsImpostor())
                        __result = __instance.MaxLightRadius * GameOptionsManager.Instance.currentHideNSeekGameOptions.ImpostorFlashlightSize;
                    else
                        __result = __instance.MaxLightRadius * GameOptionsManager.Instance.currentHideNSeekGameOptions.CrewmateFlashlightSize;
                }
                else
                {
                    if (player.IsImpostor())
                        __result = __instance.MaxLightRadius * GameOptionsManager.Instance.currentHideNSeekGameOptions.ImpostorLightMod;
                    else
                        __result = __instance.MaxLightRadius * GameOptionsManager.Instance.currentHideNSeekGameOptions.CrewLightMod;
                }

                return false;
            }
            
            if (player == null || player.IsDead)
            {
                __result = __instance.MaxLightRadius;
                return false;
            }

            var switchSystem = __instance.Systems[SystemTypes.Electrical].Cast<SwitchSystem>();

            if (player._object.Is(Faction.Intruder) || (player._object.Is(RoleAlignment.NeutralKill) && CustomGameOptions.NKHasImpVision) || player._object.Is(Faction.Syndicate) ||
                (player._object.Is(Faction.Neutral) && CustomGameOptions.LightsAffectNeutrals))
            {
                __result = __instance.MaxLightRadius * CustomGameOptions.IntruderVision;
                return false;
            }

            if (SubmergedCompatibility.isSubmerged())
            {
                if (player._object.Is(AbilityEnum.Torch))
                    __result = Mathf.Lerp(__instance.MinLightRadius, __instance.MaxLightRadius, 1) * GameOptionsManager.Instance.currentNormalGameOptions.CrewLightMod;

                if (player._object.Is(AbilityEnum.Lighter))
                    __result = Mathf.Lerp(__instance.MinLightRadius, __instance.MaxLightRadius, 1) * GameOptionsManager.Instance.currentNormalGameOptions.ImpostorLightMod;
                    
                return false;
            }

            var t = switchSystem.Value / 255f;
            
            if (player._object.Is(AbilityEnum.Torch))
                t = 1;

            __result = Mathf.Lerp(__instance.MinLightRadius, __instance.MaxLightRadius, t) * CustomGameOptions.CrewVision;
            return false;
        }
    }
}