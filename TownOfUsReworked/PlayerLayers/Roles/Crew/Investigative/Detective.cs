using static TownOfUsReworked.Languages.Language;
namespace TownOfUsReworked.PlayerLayers.Roles
{
    public class Detective : Crew
    {
        public DateTime LastExamined;
        public CustomButton ExamineButton;
        private static float Time2;

        public Detective(PlayerControl player) : base(player)
        {
            Name = GetString("Detective");
            StartText = () => GetString("DetectiveStartText");
            AbilitiesText = () => GetString("DetectiveAbilitiesText").Replace("%duration%", $"{CustomGameOptions.RecentKill}");
            Color = CustomGameOptions.CustomCrewColors ? Colors.Detective : Colors.Crew;
            RoleType = RoleEnum.Detective;
            RoleAlignment = RoleAlignment.CrewInvest;
            InspectorResults = InspectorResults.GainsInfo;
            Type = LayerEnum.Detective;
            ExamineButton = new(this, "Examine", AbilityTypes.Direct, "ActionSecondary", Examine);

            if (TownOfUsReworked.IsTest)
                Utils.LogSomething($"{Player.name} is {Name}");
        }

        public float ExamineTimer()
        {
            var timespan = DateTime.UtcNow - LastExamined;
            var num = Player.GetModifiedCooldown(CustomGameOptions.ExamineCd) * 1000f;
            var flag2 = num - (float)timespan.TotalMilliseconds < 0f;
            return flag2 ? 0f : (num - (float)timespan.TotalMilliseconds) / 1000f;
        }

        public override void OnMeetingStart(MeetingHud __instance)
        {
            base.OnMeetingStart(__instance);
            AllPrints.ForEach(x => x.Destroy());
            AllPrints.Clear();
        }

        private static Vector2 Position(PlayerControl player) => player.GetTruePosition() + new Vector2(0, 0.366667f);

        public void Examine()
        {
            if (ExamineTimer() != 0f || Utils.IsTooFar(Player, ExamineButton.TargetPlayer))
                return;

            var interact = Utils.Interact(Player, ExamineButton.TargetPlayer);

            if (interact[3])
            {
                var hasKilled = ExamineButton.TargetPlayer.IsFramed();

                foreach (var player in Utils.KilledPlayers)
                {
                    if (player.KillerId == ExamineButton.TargetPlayer.PlayerId && (DateTime.UtcNow - player.KillTime).TotalSeconds <= CustomGameOptions.RecentKill)
                        hasKilled = true;
                }

                if (hasKilled)
                    Utils.Flash(new(255, 0, 0, 255));
                else
                    Utils.Flash(new(0, 255, 0, 255));
            }

            if (interact[0])
                LastExamined = DateTime.UtcNow;
            else if (interact[1])
                LastExamined.AddSeconds(CustomGameOptions.ProtectKCReset);
        }

        public override void UpdateHud(HudManager __instance)
        {
            base.UpdateHud(__instance);
            ExamineButton.Update("EXAMINE", ExamineTimer(), CustomGameOptions.ExamineCd);

            if (!IsDead)
            {
                Time2 += Time.deltaTime;

                if (Time2 >= CustomGameOptions.FootprintInterval)
                {
                    Time2 -= CustomGameOptions.FootprintInterval;

                    foreach (var player in CustomPlayer.AllPlayers)
                    {
                        if (player.Data.IsDead || player.Data.Disconnected || player == CustomPlayer.Local)
                            continue;

                        if (!AllPrints.Any(print => Vector3.Distance(print.Position, Position(player)) < 0.5f && print.Color.a > 0.5 && print.PlayerId == player.PlayerId))
                            AllPrints.Add(new(player));
                    }

                    for (var i = 0; i < AllPrints.Count; i++)
                    {
                        try
                        {
                            var footprint = AllPrints[i];

                            if (footprint.Update())
                                i--;
                        } catch { /*Assume footprint value is null and allow the loop to continue*/ }
                    }
                }
            }
            else
                OnLobby();
        }
    }
}