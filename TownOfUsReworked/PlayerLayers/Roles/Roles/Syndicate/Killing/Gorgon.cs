using TownOfUsReworked.Enums;
using System.Collections.Generic;
using TownOfUsReworked.Classes;
using Hazel;
using System;
using TownOfUsReworked.Lobby.CustomOption;
using UnityEngine;

namespace TownOfUsReworked.PlayerLayers.Roles.Roles
{
    public class Gorgon : Role
    {
        private KillButton _gazeButton;
        public Dictionary<byte, float> gazeList = new Dictionary<byte, float>();
        public PlayerControl ClosestPlayer;
        public DateTime LastGazed;
        public bool Enabled = false;
        public float TimeRemaining;
        public PlayerControl StonedPlayer;
        public bool Stoned => TimeRemaining > 0f;
        public DateTime LastKilled { get; set; }
        private KillButton _killButton;

        public Gorgon(PlayerControl player) : base(player)
        {
            Name = "Gorgon";
            StartText = "Turn The <color=#8BFDFD>Crew</color> Into Sculptures";
            AbilitiesText = "- You can stone gaze players, that forces them to stand still till a meeting is called.";
            AttributesText = "- Stoned players cannot move and will die when a meeting is called.";
            Color = CustomGameOptions.CustomSynColors ? Colors.Gorgon : Colors.Syndicate;
            RoleType = RoleEnum.Gorgon;
            Faction = Faction.Syndicate;
            FactionName = "Syndicate";
            FactionColor = Colors.Syndicate;
            Results = InspResults.VigVHSurvGorg;
            RoleAlignment = RoleAlignment.SyndicateKill;
            AlignmentName = "Syndicate (Killing)";
            FactionDescription = SyndicateFactionDescription;         
            Objectives = SyndicateWinCon;
            RoleDescription = "You are a Gorgon! Use your gaze of stone to freeze players in place and await their deaths!";
            AlignmentDescription = SyKDescription;
        }

        public override void Loses()
        {
            LostByRPC = true;
        }

        public float KillTimer()
        {
            var utcNow = DateTime.UtcNow;
            var timeSpan = utcNow - LastKilled;
            var num = CustomGameOptions.ChaosDriveKillCooldown * 1000f;
            var flag2 = num - (float)timeSpan.TotalMilliseconds < 0f;

            if (flag2)
                return 0;

            return (num - (float)timeSpan.TotalMilliseconds) / 1000f;
        }

        public KillButton KillButton
        {
            get => _killButton;
            set
            {
                _killButton = value;
                AddToAbilityButtons(value, this);
            }
        }
        
        public KillButton GazeButton
        {
            get => _gazeButton;
            set
            {
                _gazeButton = value;
                AddToAbilityButtons(value, this);
            }
        }
        
        public float GazeTimer()
        {
            var utcNow = DateTime.UtcNow;
            var timeSpan = utcNow - LastGazed;
            var num = CustomGameOptions.GazeCooldown * 1000f;
            var flag2 = num - (float)timeSpan.TotalMilliseconds < 0f;

            if (flag2)
                return 0;

            return (num - (float)timeSpan.TotalMilliseconds) / 1000f;
        }
        
        public void Freeze()
        {
            Enabled = true;
            TimeRemaining -= Time.deltaTime;

            if (MeetingHud.Instance)
                TimeRemaining = 0;
                
            if (TimeRemaining <= 0)
                GazeKill();
        }

        public void GazeKill()
        {
            if (!StonedPlayer.Is(RoleEnum.Pestilence))
            {
                Utils.RpcMurderPlayer(Player, StonedPlayer);

                if (!StonedPlayer.Data.IsDead)
                {
                    try
                    {
                        SoundManager.Instance.PlaySound(PlayerControl.LocalPlayer.KillSfx, false, 1f);
                    } catch {}
                }
            }

            StonedPlayer = null;
            Enabled = false;
            LastGazed = DateTime.UtcNow;
        }

        public override void Wins()
        {
            if (IsRecruit)
                CabalWin = true;
            else
                SyndicateWin = true;
        }

        internal override bool GameEnd(LogicGameFlowNormal __instance)
        {
            if (Player.Data.IsDead || Player.Data.Disconnected)
                return true;

            if (IsRecruit)
            {
                if (Utils.CabalWin())
                {
                    Wins();
                    var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.WinLose, SendOption.Reliable, -1);
                    writer.Write((byte)WinLoseRPC.CabalWin);
                    writer.Write(Player.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    Utils.EndGame();
                    return false;
                }
            }
            else if (Utils.SyndicateWins())
            {
                var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.WinLose, SendOption.Reliable, -1);
                writer.Write((byte)WinLoseRPC.SyndicateWin);
                writer.Write(Player.PlayerId);
                Wins();
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                Utils.EndGame();
                return false;
            }

            return false;
        }

        protected override void IntroPrefix(IntroCutscene._ShowTeam_d__32 __instance)
        {
            if (Player != PlayerControl.LocalPlayer)
                return;
                
            var team = new Il2CppSystem.Collections.Generic.List<PlayerControl>();

            team.Add(PlayerControl.LocalPlayer);

            if (!IsRecruit)
            {
                foreach (var player in PlayerControl.AllPlayerControls)
                {
                    if (player.Is(Faction) && player != PlayerControl.LocalPlayer)
                        team.Add(player);
                }
            }
            else
            {
                var jackal = Player.GetJackal();

                team.Add(jackal.Player);
                team.Add(jackal.GoodRecruit);
            }

            __instance.teamToShow = team;
        }
    }
}