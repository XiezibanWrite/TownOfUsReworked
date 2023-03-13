using System;
using System.Collections.Generic;
using TownOfUsReworked.Enums;
using TownOfUsReworked.CustomOptions;
using TownOfUsReworked.Classes;
using Hazel;

namespace TownOfUsReworked.PlayerLayers.Roles
{
    public class Framer : SyndicateRole
    {
        public AbilityButton FrameButton;
        public List<byte> Framed;
        public DateTime LastFramed;
        public PlayerControl ClosestFrame;

        public Framer(PlayerControl player) : base(player)
        {
            Name = "Framer";
            StartText = "Make Everyone Suspicious";
            AbilitiesText = "- You can frame players.\n- Framed players will die very easily to <color=#FFFF00FF>Vigilantes</color> and <color=#073763FF>Assassins</color>.\n- Framed " +
                "players will appear to have the wrong results to investigative roles till you are dead.";
            RoleType = RoleEnum.Framer;
            RoleAlignment = RoleAlignment.SyndicateDisruption;
            AlignmentName = SD;
            Color = CustomGameOptions.CustomSynColors ? Colors.Framer : Colors.Syndicate;
            RoleDescription = "You are a Framer! This means that you are unrivalled in the art of gaslighting. Framed players always appear to be evil, regardless of their role!";
            Framed = new List<byte>();
        }

        public float FrameTimer()
        {
            var utcNow = DateTime.UtcNow;
            var timeSpan = utcNow - LastFramed;
            var num = Utils.GetModifiedCooldown(CustomGameOptions.FrameCooldown, Utils.GetUnderdogChange(Player)) * 1000f;
            var flag2 = num - (float) timeSpan.TotalMilliseconds < 0f;

            if (flag2)
                return 0f;

            return (num - (float) timeSpan.TotalMilliseconds) / 1000f;
        }

        public void Frame(PlayerControl player)
        {
            if (player.Is(Faction.Syndicate) || Framed.Contains(player.PlayerId))
                return;
            
            Framed.Add(player.PlayerId);
            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.Action, SendOption.Reliable);
            writer.Write((byte)ActionsRPC.Frame);
            writer.Write(Player.PlayerId);
            writer.Write(player.PlayerId);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
        }
    }
}
