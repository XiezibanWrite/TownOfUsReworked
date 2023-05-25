namespace TownOfUsReworked.Objects
{
    [HarmonyPatch]
    public class DeadPlayer
    {
        public byte KillerId;
        public byte PlayerId;
        public DateTime KillTime;

        public DeadPlayer(byte killer, byte player)
        {
            PlayerId = player;
            KillerId = killer;
            KillTime = DateTime.UtcNow;
        }
    }
}