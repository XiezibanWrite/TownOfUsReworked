﻿using HarmonyLib;
using TownOfUsReworked.Enums;
using TownOfUsReworked.Classes;
using System;
using TownOfUsReworked.Extensions;

namespace TownOfUsReworked.PlayerLayers.Roles.CrewRoles.OperativeMod
{
    [HarmonyPatch(typeof(AbilityButton), nameof(AbilityButton.DoClick))]
    public static class PerformBug
    {
        public static bool Prefix(AbilityButton __instance)
        {
            if (Utils.NoButton(PlayerControl.LocalPlayer, RoleEnum.Operative))
                return true;

            var role = Role.GetRole<Operative>(PlayerControl.LocalPlayer);

            if (__instance == role.BugButton)
            {
                if (role.BugTimer() != 0f)
                    return false;

                role.UsesLeft--;
                role.LastBugged = DateTime.UtcNow;
                role.Bugs.Add(BugExtensions.CreateBug(PlayerControl.LocalPlayer.GetTruePosition()));
                return false;
            }

            return true;
        }
    }
}