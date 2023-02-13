using TownOfUsReworked.Lobby.CustomOption;
using TownOfUsReworked.Enums;
using TownOfUsReworked.Classes;
using Hazel;

namespace TownOfUsReworked.PlayerLayers.Objectifiers.Objectifiers
{
    public class Overlord : Objectifier
    {
        public int OverlordMeetingCount = 0;
        public bool IsAlive => !(Player.Data.IsDead || Player.Data.Disconnected);
        public bool OverlordWins { get; set; }

        public Overlord(PlayerControl player) : base(player)
        {
            Name = "Overlord";
            SymbolName = "β";
            TaskText = "Take over the mission!";
            Color = CustomGameOptions.CustomObjectifierColors ? Colors.Overlord : Colors.Objectifier;
            ObjectifierType = ObjectifierEnum.Overlord;
        }

        internal override bool GameEnd(LogicGameFlowNormal __instance)
        {
            if (Player.Data.IsDead || Player.Data.Disconnected)
                return true;
                
            if (MeetingCountAchieved())
            {
                Wins();
                var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.WinLose, SendOption.Reliable, -1);
                writer.Write((byte)WinLoseRPC.OverlordWin);
                writer.Write(Player.PlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                Utils.EndGame();
                return false;
            }

            return false;
        }

        public override void Wins()
        {
            OverlordWins = true;
        }

        private bool MeetingCountAchieved()
        {
            var flag = false;

            if (OverlordMeetingCount >= CustomGameOptions.OverlordMeetingWinCount && IsAlive)
                flag = true;

            return flag;
        }
    }
}