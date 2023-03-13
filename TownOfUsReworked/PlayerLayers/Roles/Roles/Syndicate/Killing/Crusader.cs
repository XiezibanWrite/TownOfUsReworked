using System;
using UnityEngine;
using TownOfUsReworked.Enums;
using TownOfUsReworked.CustomOptions;
using TownOfUsReworked.Classes;

namespace TownOfUsReworked.PlayerLayers.Roles
{
    public class Crusader : SyndicateRole
    {
        public bool Enabled;
        public DateTime LastCrusaded;
        public float TimeRemaining;
        public bool OnCrusade => TimeRemaining > 0f;
        public PlayerControl CrusadedPlayer;
        public PlayerControl ClosestCrusade;
        public AbilityButton CrusadeButton;

        public Crusader(PlayerControl player) : base(player)
        {
            Name = "Crusader";
            StartText = "Ambush";
            AbilitiesText = $"- You can crusade players.\n- Ambushed players will be forced to be on alert, and will kill whoever interacts with then.";
            Color = CustomGameOptions.CustomIntColors ? Colors.Crusader : Colors.Syndicate;
            RoleType = RoleEnum.Crusader;
            RoleAlignment = RoleAlignment.SyndicateKill;
            AlignmentName = SyK;
            RoleDescription = "You are a Crusader! You are a religious fanatic who wants to purge the mission! Perform your crusades and cleanse this world of evil!";
            InspectorResults = InspectorResults.SeeksToProtect;
        }

        public float CrusadeTimer()
        {
            var utcNow = DateTime.UtcNow;
            var timeSpan = utcNow - LastCrusaded;
            var num = Utils.GetModifiedCooldown(CustomGameOptions.AlertCd) * 1000f;
            var flag2 = num - (float) timeSpan.TotalMilliseconds < 0f;

            if (flag2)
                return 0f;

            return (num - (float) timeSpan.TotalMilliseconds) / 1000f;
        }

        public void Crusade()
        {
            Enabled = true;
            TimeRemaining -= Time.deltaTime;

            if (Player.Data.IsDead || CrusadedPlayer.Data.IsDead || CrusadedPlayer.Data.Disconnected || MeetingHud.Instance)
                TimeRemaining = 0f;
        }

        public void UnCrusade()
        {
            Enabled = false;
            LastCrusaded = DateTime.UtcNow;
            CrusadedPlayer = null;
        }

        public void Crusade(PlayerControl player2)
        {
            var closestPlayers = Utils.GetClosestPlayers(player2.GetTruePosition(), CustomGameOptions.ChaosDriveCrusadeRadius);

            foreach (var player in closestPlayers)
            {
                Utils.Spread(player2, player);

                if (player.IsVesting() || player.IsProtected() || player2.IsOtherRival(player))
                    continue;
                    
                if (!player.Is(RoleEnum.Pestilence))
                    Utils.RpcMurderPlayer(player2, player, false);
                
                if (player.IsOnAlert() || player.Is(RoleEnum.Pestilence))
                    Utils.RpcMurderPlayer(player, player2, false);
            }
        }
    }
}