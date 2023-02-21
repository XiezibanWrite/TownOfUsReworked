﻿using HarmonyLib;
using TownOfUsReworked.Enums;
using TownOfUsReworked.Classes;
using TownOfUsReworked.PlayerLayers.Roles;
using TownOfUsReworked.PlayerLayers.Roles.Roles;
using UnityEngine;

namespace TownOfUsReworked.Patches
{
    [HarmonyPatch(typeof(KillButton), nameof(KillButton.Start))]
    public static class KillButtonAwake
    {
        public static void Prefix(KillButton __instance)
        {
            __instance.transform.Find("Text_TMP").gameObject.SetActive(false);
        }
    }

    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public class KillButtonSprite
    {
        public static void Postfix(HudManager __instance)
        {
            if (!GameStates.IsInGame)
                return;

            var role = Role.GetRole(PlayerControl.LocalPlayer);

            if (role == null)
                return;

            if (role.AbilityButtons.Count > 0)
            {
                if (Input.GetKeyDown(KeyCode.Q) && role.PrimaryButton != null)
                    role.PrimaryButton.DoClick();
            }
        }

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

                    HudManager.Instance.AbilityButton.gameObject.SetActive(!ghostRole && !MeetingHud.Instance);
                }
            }
        }
    }
}
