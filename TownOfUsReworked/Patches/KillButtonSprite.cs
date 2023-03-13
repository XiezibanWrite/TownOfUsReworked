﻿using HarmonyLib;
using TownOfUsReworked.Enums;
using TownOfUsReworked.Classes;
using TownOfUsReworked.PlayerLayers.Roles;
using UnityEngine;

namespace TownOfUsReworked.Patches
{
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public class KillButtonSprite
    {
        [HarmonyPatch(typeof(AbilityButton), nameof(AbilityButton.Update))]
        class AbilityButtonUpdatePatch
        {
            static void Postfix()
            {
                if (!GameStates.IsInGame)
                    HudManager.Instance.AbilityButton.gameObject.SetActive(false);
                else if (GameStates.IsHnS)
                    HudManager.Instance.AbilityButton.gameObject.SetActive(!PlayerControl.LocalPlayer.Data.IsImpostor());
                else
                {
                    var ghostRole = false;

                    if (PlayerControl.LocalPlayer.Is(RoleEnum.Revealer))
                    {
                        var haunter = Role.GetRole<Revealer>(PlayerControl.LocalPlayer);

                        if (!haunter.Caught)
                            ghostRole = true;
                    }
                    else if (PlayerControl.LocalPlayer.Is(RoleEnum.Phantom))
                    {
                        var phantom = Role.GetRole<Phantom>(PlayerControl.LocalPlayer);

                        if (!phantom.Caught)
                            ghostRole = true;
                    }

                    HudManager.Instance.AbilityButton.gameObject.SetActive(!ghostRole && !MeetingHud.Instance && PlayerControl.LocalPlayer.Data.IsDead);
                }
            }
        }
    }
}
