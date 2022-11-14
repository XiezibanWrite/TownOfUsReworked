using Hazel;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using Reactor.Utilities;
using Reactor.Utilities.Extensions;
using Object = UnityEngine.Object;
using Reactor.Networking.Extensions;
using TownOfUsReworked.Patches;
using TownOfUsReworked.Enums;
using TownOfUsReworked.Extensions;
using TownOfUsReworked.Lobby.CustomOption;

namespace TownOfUsReworked.PlayerLayers.Roles.Roles
{
    public class Warper : Role
    {
        public KillButton WarpButton;

        public Warper(PlayerControl player) : base(player)
        {
            Name = "Warper";
            ImpostorText = () => "Warp The Crew Away From Each Other";
            TaskText = () => "Separate the Crew";
            Color = CustomGameOptions.CustomSynColors ? Colors.Warper : Colors.Syndicate;
            SubFaction = SubFaction.None;
            RoleType = RoleEnum.Warper;
            Faction = Faction.Syndicate;
            FactionColor = Colors.Syndicate;
            FactionName = "Syndicate";
            AlignmentName = () => "Syndicate (Support)";
            Results = InspResults.TransWarpTeleTask;
            AddToRoleHistory(RoleType);
        }

        public void Warp()
        {
            Dictionary<byte, Vector2> coordinates = GenerateWarpCoordinates();

            unchecked
            {
                var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.Warp,
                    SendOption.Reliable, -1);
                writer.Write((byte)coordinates.Count);

                foreach ((byte key, Vector2 value) in coordinates)
                {
                    writer.Write(key);
                    writer.Write(value);
                }
                AmongUsClient.Instance.FinishRpcImmediately(writer);
            }

            WarpPlayersToCoordinates(coordinates);
        }

        public static void WarpPlayersToCoordinates(Dictionary<byte, Vector2> coordinates)
        {
            if (coordinates.ContainsKey(PlayerControl.LocalPlayer.PlayerId))
            {
                Coroutines.Start(Utils.FlashCoroutine(Colors.Warper));

                if (Minigame.Instance)
                {
                    try
                    {
                        Minigame.Instance.Close();
                    }
                    catch {}
                }

                if (PlayerControl.LocalPlayer.inVent)
                {
                    PlayerControl.LocalPlayer.MyPhysics.RpcExitVent(Vent.currentVent.Id);
                    PlayerControl.LocalPlayer.MyPhysics.ExitAllVents();
                }
            }


            foreach ((byte key, Vector2 value) in coordinates)
            {
                PlayerControl player = Utils.PlayerById(key);
                player.transform.position = value;
            }
        }

        private Dictionary<byte, Vector2> GenerateWarpCoordinates()
        {
            List<PlayerControl> targets = PlayerControl.AllPlayerControls.ToArray().Where(player => !player.Data.IsDead && !player.Data.Disconnected).ToList();

            HashSet<Vent> vents = Object.FindObjectsOfType<Vent>().ToHashSet();

            Dictionary<byte, Vector2> coordinates = new Dictionary<byte, Vector2>(targets.Count);
            foreach (PlayerControl target in targets)
            {
                Vent vent = vents.Random();

                Vector3 destination = SendPlayerToVent(vent);
                coordinates.Add(target.PlayerId, destination);
            }
            return coordinates;
        }

        public static Vector3 SendPlayerToVent(Vent vent)
        {
            Vector2 size = vent.GetComponent<BoxCollider2D>().size;
            Vector3 destination = vent.transform.position;
            destination.y += 0.3636f;
            return destination;
        }
    }
}