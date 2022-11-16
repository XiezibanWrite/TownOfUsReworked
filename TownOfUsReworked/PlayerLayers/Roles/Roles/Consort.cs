using TownOfUsReworked.Enums;
using TownOfUsReworked.Patches;
using Il2CppSystem.Collections.Generic;
using TownOfUsReworked.Lobby.CustomOption;
using System;
using TownOfUsReworked.Extensions;
using UnityEngine;

namespace TownOfUsReworked.PlayerLayers.Roles.Roles
{
    public class Consort : Role
    {
        public bool ConsWin;
        public PlayerControl ClosestPlayer;
        public DateTime LastBlock { get; set; }
        public float TimeRemaining;
        public KillButton _roleblockButton;

        public Consort(PlayerControl player) : base(player)
        {
            Name = "Consort";
            Faction = Faction.Intruders;
            RoleType = RoleEnum.Consort;
            ImpostorText = () => "Roleblock The Crew And Stop Them From Progressing";
            TaskText = () => "Block people from using their abilities";
            Color = CustomGameOptions.CustomImpColors ? Colors.Consort : Colors.Crew;
            FactionName = "Intruder";
            FactionColor = Colors.Intruder;
            RoleAlignment = RoleAlignment.IntruderSupport;
            AlignmentName = () => "Intruder (Support)";
            IntroText = "Kill those who oppose you";
            Results = InspResults.EscConsGliPois;
            SubFaction = SubFaction.None;
            AddToRoleHistory(RoleType);
        }

        protected override void IntroPrefix(IntroCutscene._ShowTeam_d__21 __instance)
        {
            var intTeam = new List<PlayerControl>();

            foreach (var player in PlayerControl.AllPlayerControls)
            {
                if (player.Is(Faction.Intruders))
                    intTeam.Add(player);
            }
            __instance.teamToShow = intTeam;
        }

        public KillButton RoleblockButton
        {
            get => _roleblockButton;
            set
            {
                _roleblockButton = value;
                ExtraButtons.Clear();
                ExtraButtons.Add(value);
            }
        }

        public float RoleblockTimer()
        {
            var utcNow = DateTime.UtcNow;
            var timeSpan = utcNow - LastBlock;
            var num = CustomGameOptions.MimicCooldown * 1000f;
            var flag2 = num - (float) timeSpan.TotalMilliseconds < 0f;

            if (flag2)
                return 0;

            return (num - (float) timeSpan.TotalMilliseconds) / 1000f;
        }

        public void Roleblock()
        {
            TimeRemaining -= Time.deltaTime;
            Utils.Block(Player, ClosestPlayer);

            if (Player.Data.IsDead)
                TimeRemaining = 0f;
        }

        public void Unroleblock()
        {
            TimeRemaining -= Time.deltaTime;
            Utils.Block(Player, ClosestPlayer);

            if (Player.Data.IsDead)
                TimeRemaining = 0f;
        }
    }
}