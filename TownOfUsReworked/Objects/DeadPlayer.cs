namespace TownOfUsReworked.Objects
{
    public class DeadPlayer
    {
        public byte KillerId;
        public byte PlayerId;
        public DateTime KillTime;

        public PlayerControl Killer => Utils.PlayerById(KillerId);
        public PlayerControl Body => Utils.PlayerById(PlayerId);
        public PlayerControl Reporter;
        public float KillAge;

        public DeadPlayer(byte killer, byte player)
        {
            PlayerId = player;
            KillerId = killer;
            KillTime = DateTime.UtcNow;
        }

        public string ParseBodyReport()
        {
            var report = $"{Body.Data.PlayerName}'s Report:\n";
            var killerRole = Role.GetRole(Killer);
            var bodyRole = Role.GetRole(Body);

            if (!(Role.GetRoles<Grenadier>(RoleEnum.Grenadier).Any(x => x.Flashed && x.FlashedPlayers.Contains(Reporter)) ||
                Role.GetRoles<PromotedGodfather>(RoleEnum.PromotedGodfather).Any(x => x.OnEffect && x.IsGren && x.FlashedPlayers.Contains(Reporter))))
            {
                report += $"They died approximately {Math.Round(KillAge / 1000)}s ago!\n";
                report += $"They were a {bodyRole.Name}!\n";

                if (Body == Killer)
                    report += "There is evidence of self-harm!";
                else
                {
                    if (CustomGameOptions.CoronerReportRole)
                        report += $"They were killed by a {killerRole.Name}!\n";
                    else if (Killer.Is(Faction.Crew))
                        report += "The killer is from the Crew!\n";
                    else if (Killer.Is(Faction.Neutral))
                        report += "The killer is a Neutral!\n";
                    else if (Killer.Is(Faction.Intruder))
                        report += "The killer is an Intruder!\n";
                    else if (Killer.Is(Faction.Syndicate))
                        report += "The killer is from the Syndicate!\n";

                    report += $"The killer is a {ColorUtils.LightDarkColors[Killer.CurrentOutfit.ColorId].ToLower()} color!\n";

                    if (CustomGameOptions.CoronerReportName && CustomGameOptions.CoronerKillerNameTime <= Math.Round(KillAge / 1000))
                        report += $"They were killed by {Killer.Data.PlayerName}!";
                }
            }
            else
                report += "You have been blinded so you cannot tell what happened to the body!";

            return report;
        }
    }
}