using HarmonyLib;
using Hazel;
using TownOfUsReworked.Enums;
using TownOfUsReworked.PlayerLayers.Roles.Roles;

namespace TownOfUsReworked.PlayerLayers.Roles.NeutralRoles.JesterMod
{
    [HarmonyPatch(typeof(GameManager), nameof(GameManager.RpcEndGame))]
    public class EndGame
    {
        public static bool Prefix(GameManager __instance, [HarmonyArgument(0)] GameOverReason reason)
        {
            foreach (Jester jest in Role.GetRoles(RoleEnum.Jester))
            {
                if (!jest.VotedOut)
                {
                    var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte) CustomRPC.WinLose, SendOption.Reliable, -1);
                    writer.Write((byte)WinLoseRPC.JesterLose);
                    writer.Write(jest.Player.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    jest.Loses();
                }
            }
            
            return true;
        }
    }
}