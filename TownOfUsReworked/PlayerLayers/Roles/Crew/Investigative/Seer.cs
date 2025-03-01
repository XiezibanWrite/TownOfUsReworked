using static TownOfUsReworked.Languages.Language;
namespace TownOfUsReworked.PlayerLayers.Roles
{
    public class Seer : Crew
    {
        public DateTime LastSeered;
        public bool ChangedDead => !AllRoles.Any(x => x.Player != null && !x.IsDead && !x.Disconnected && (x.RoleHistory.Count > 0 || x.Is(RoleEnum.Amnesiac) || x.Is(RoleEnum.Thief) ||
            x.Player.Is(ObjectifierEnum.Traitor) || x.Is(RoleEnum.VampireHunter) || x.Is(RoleEnum.Godfather) || x.Is(RoleEnum.Mafioso) || x.Is(RoleEnum.Shifter) || x.Is(RoleEnum.Guesser) ||
            x.Is(RoleEnum.Rebel) || x.Is(RoleEnum.Mystic) || (x.Is(RoleEnum.Seer) && x != this) || x.Is(RoleEnum.Sidekick) || x.Is(RoleEnum.GuardianAngel) || x.Is(RoleEnum.Executioner) ||
            x.Is(RoleEnum.BountyHunter) || x.Player.Is(ObjectifierEnum.Fanatic)));
        public CustomButton SeerButton;

        public Seer(PlayerControl player) : base(player)
        {
            Name = GetString("Seer");
            RoleType = RoleEnum.Seer;
            Color = CustomGameOptions.CustomCrewColors ? Colors.Seer : Colors.Crew;
            RoleAlignment = RoleAlignment.CrewInvest;
            AbilitiesText = () => GetString("SeerAbilitiesText1");
            StartText = () => GetString("SeerStartText");
            InspectorResults = InspectorResults.GainsInfo;
            Type = LayerEnum.Seer;
            SeerButton = new(this, "Seer", AbilityTypes.Direct, "ActionSecondary", See);

            if (TownOfUsReworked.IsTest)
                Utils.LogSomething($"{Player.name} is {Name}");
        }

        public float SeerTimer()
        {
            var timespan = DateTime.UtcNow - LastSeered;
            var num = Player.GetModifiedCooldown(CustomGameOptions.SeerCooldown) * 1000f;
            var flag2 = num - (float)timespan.TotalMilliseconds < 0f;
            return flag2 ? 0f : (num - (float)timespan.TotalMilliseconds) / 1000f;
        }

        public void TurnSheriff()
        {
            var role = new Sheriff(Player);
            role.RoleUpdate(this);

            if (Local && !IntroCutscene.Instance)
                Utils.Flash(Colors.Sheriff);

            if (CustomPlayer.Local.Is(RoleEnum.Seer) && !IntroCutscene.Instance)
                Utils.Flash(Color);
        }

        public void See()
        {
            if (SeerTimer() != 0f || Utils.IsTooFar(Player, SeerButton.TargetPlayer))
                return;

            var interact = Utils.Interact(Player, SeerButton.TargetPlayer);

            if (interact[3])
            {
                if (GetRole(SeerButton.TargetPlayer).RoleHistory.Count > 0 || SeerButton.TargetPlayer.IsFramed())
                    Utils.Flash(new(255, 0, 0, 255));
                else
                    Utils.Flash(new(0, 255, 0, 255));
            }

            if (interact[0])
                LastSeered = DateTime.UtcNow;
            else if (interact[1])
                LastSeered.AddSeconds(CustomGameOptions.ProtectKCReset);
        }

        public override void UpdateHud(HudManager __instance)
        {
            base.UpdateHud(__instance);
            SeerButton.Update("SEE", SeerTimer(), CustomGameOptions.SeerCooldown);

            if (ChangedDead && !IsDead)
            {
                var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.Change, SendOption.Reliable);
                writer.Write((byte)TurnRPC.TurnSheriff);
                writer.Write(PlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                TurnSheriff();
            }
        }
    }
}