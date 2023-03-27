using HarmonyLib;
using TownOfUsReworked.Enums;
using TownOfUsReworked.Classes;
using TownOfUsReworked.Extensions;
using System;
using UnityEngine;
using System.Linq;
using TownOfUsReworked.Objects;
using TownOfUsReworked.Patches;
using TownOfUsReworked.CustomOptions;

namespace TownOfUsReworked.PlayerLayers.Roles.CrewRoles.CoronerMod
{
    [HarmonyPatch(typeof(AbilityButton), nameof(AbilityButton.DoClick))]
    public static class PerformAbility
    {
        public static bool Prefix(AbilityButton __instance)
        {
            if (Utils.NoButton(PlayerControl.LocalPlayer, RoleEnum.Coroner))
                return true;

            var role = Role.GetRole<Coroner>(PlayerControl.LocalPlayer);

            if (__instance == role.AutopsyButton)
            {
                if (Utils.IsTooFar(role.Player, role.CurrentTarget))
                    return false;

                var playerId = role.CurrentTarget.ParentId;
                var player = Utils.PlayerById(playerId);
                Utils.Spread(role.Player, player);
                var matches = Murder.KilledPlayers.Where(x => x.PlayerId == playerId).ToArray();
                DeadPlayer killed = null;

                if (matches.Length > 0)
                    killed = matches[0];

                if (killed == null)
                {
                    Utils.Flash(Color.red, "ERROR");
                    return false;
                }

                role.ReferenceBody = killed;
                role.UsesLeft = CustomGameOptions.CompareLimit;
                role.LastAutopsied = DateTime.UtcNow;
                Utils.Flash(role.Color, "You are selected a reference!");
                return false;
            }
            else if (__instance == role.CompareButton)
            {
                if (role.ReferenceBody == null)
                    return false;

                if (Utils.IsTooFar(role.Player, role.ClosestPlayer))
                    return false;

                if (role.CompareTimer() != 0f)
                    return false;

                var interact = Utils.Interact(role.Player, role.ClosestPlayer);

                if (interact[3])
                {
                    if (role.ClosestPlayer.PlayerId == role.ReferenceBody.KillerId || role.ClosestPlayer.IsFramed())
                        Utils.Flash(Color.red, $"{role.ClosestPlayer.Data.PlayerName} is the killer!");
                    else
                        Utils.Flash(Color.green, $"{role.ClosestPlayer.Data.PlayerName} is not the killer!");

                    role.UsesLeft--;
                }

                if (interact[0])
                    role.LastCompared = DateTime.UtcNow;
                else if (interact[1])
                    role.LastCompared.AddSeconds(CustomGameOptions.ProtectKCReset);

                return false;
            }

            return true;
        }
    }
}