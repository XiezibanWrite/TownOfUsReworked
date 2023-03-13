using HarmonyLib;
using TownOfUsReworked.Enums;
using TownOfUsReworked.Classes;

namespace TownOfUsReworked.PlayerLayers.Roles.SyndicateRoles.GorgonMod
{
    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Start))]
    public class StoneMeetingKill
    {
        public static void Prefix()
        {
            foreach (var gorg in Role.GetRoles(RoleEnum.Gorgon))
            {
                var gorgon = (Gorgon)gorg;

                foreach (var id in gorgon.Gazed)
                {
                    var stoned = Utils.PlayerById(id);

                    if (stoned == null || stoned.Data == null || stoned.Data.Disconnected || stoned.Data.IsDead || stoned.Is(RoleEnum.Pestilence))
                        continue;

                    Utils.RpcMurderPlayer(gorg.Player, stoned, false);
                    stoned.moveable = true;
                }

                gorgon.Gazed.Clear();
            }
        }
    }
}