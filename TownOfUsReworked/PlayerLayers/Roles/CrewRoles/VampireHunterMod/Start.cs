using System;
using HarmonyLib;
using TownOfUsReworked.Enums;
using TownOfUsReworked.Lobby.CustomOption;
using TownOfUsReworked.PlayerLayers.Roles.Roles;
using Hazel;
using TownOfUsReworked.Extensions;

namespace TownOfUsReworked.PlayerLayers.Roles.CrewRoles.VampireHunterMod
{
    [HarmonyPatch(typeof(IntroCutscene._CoBegin_d__29), nameof(IntroCutscene._CoBegin_d__29.MoveNext))]
    public static class Start
    {
        public static void Postfix(IntroCutscene._CoBegin_d__29 __instance)
        {
            var VampsExist = false;

            foreach (var player in PlayerControl.AllPlayerControls)
            {
                if (player.Is(SubFaction.Undead))
                {
                    VampsExist = true;
                    break;
                }
            }

            if (!VampsExist)
            {
                foreach (VampireHunter vh in Role.GetRoles(RoleEnum.VampireHunter))
                {
                    vh.TurnVigilante();
                    var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.Change, SendOption.Reliable, -1);
                    writer.Write((byte)TurnRPC.TurnVigilante);
                    writer.Write(vh.Player.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                }
            }
            else
            {
                foreach (var role in Role.GetRoles(RoleEnum.VampireHunter))
                {
                    var vampireHunter = (VampireHunter) role;
                    vampireHunter.LastStaked = DateTime.UtcNow;
                    vampireHunter.LastStaked = vampireHunter.LastStaked.AddSeconds(CustomGameOptions.InitialCooldowns - CustomGameOptions.VigiKillCd);
                }
            }
        }
    }
}