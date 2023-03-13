﻿using HarmonyLib;
using TownOfUsReworked.Enums;
using TownOfUsReworked.CustomOptions;
using TownOfUsReworked.Classes;

namespace TownOfUsReworked.PlayerLayers.Roles.NeutralRoles.ThiefMod
{
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public static class HUDSteal
    {
        public static void Postfix(HudManager __instance)
        {
            if (Utils.NoButton(PlayerControl.LocalPlayer, RoleEnum.Thief))
                return;

            var role = Role.GetRole<Thief>(PlayerControl.LocalPlayer);

            if (role.StealButton == null)
                role.StealButton = Utils.InstantiateButton();

            role.StealButton.UpdateButton(role, "STEAL", role.KillTimer(), CustomGameOptions.ThiefKillCooldown, TownOfUsReworked.Placeholder, AbilityTypes.Direct);
        }
    }
}