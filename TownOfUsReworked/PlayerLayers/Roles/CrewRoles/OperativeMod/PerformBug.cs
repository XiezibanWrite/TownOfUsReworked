﻿using HarmonyLib;
using TownOfUsReworked.Enums;
using TownOfUsReworked.Classes;
using TownOfUsReworked.PlayerLayers.Roles.Roles;
using System;

namespace TownOfUsReworked.PlayerLayers.Roles.CrewRoles.OperativeMod
{
    [HarmonyPatch(typeof(KillButton), nameof(KillButton.DoClick))]
    public class PerformBug
    {
        public static bool Prefix(KillButton __instance)
        {
            if (Utils.NoButton(PlayerControl.LocalPlayer, RoleEnum.Operative))
                return false;

            var role = Role.GetRole<Operative>(PlayerControl.LocalPlayer);

            if (__instance == role.BugButton)
            {
                if (!Utils.ButtonUsable(__instance))
                    return false;

                if (role.BugTimer() != 0f)
                    return false;

                role.UsesLeft--;
                role.LastBugged = DateTime.UtcNow;
                role.Bugs.Add(BugExtentions.CreateBug(PlayerControl.LocalPlayer.GetTruePosition()));
                return false;
            }

            return false;
        }
    }
}
