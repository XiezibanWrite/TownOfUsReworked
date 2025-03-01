namespace TownOfUsReworked.PlayerLayers.Roles
{
    public class Grenadier : Intruder
    {
        public CustomButton FlashButton;
        public bool Enabled;
        public DateTime LastFlashed;
        public float TimeRemaining;
        private static List<PlayerControl> ClosestPlayers = new();
        private static Color32 NormalVision => new(212, 212, 212, 0);
        private static Color32 DimVision => new(212, 212, 212, 51);
        private static Color32 BlindVision => new(212, 212, 212, 255);
        public List<PlayerControl> FlashedPlayers = new();
        public bool Flashed => TimeRemaining > 0f;

        public Grenadier(PlayerControl player) : base(player)
        {
            Name = "Grenadier";
            StartText = () => "Blind The <color=#8CFFFFFF>Crew</color> With Your Magnificent Figure";
            AbilitiesText = () => $"- You can drop a flashbang, which blinds players around you\n{CommonAbilities}";
            Color = CustomGameOptions.CustomIntColors ? Colors.Grenadier : Colors.Intruder;
            RoleType = RoleEnum.Grenadier;
            RoleAlignment = RoleAlignment.IntruderConceal;
            InspectorResults = InspectorResults.DropsItems;
            ClosestPlayers = new();
            FlashedPlayers = new();
            Type = LayerEnum.Grenadier;
            FlashButton = new(this, "Flash", AbilityTypes.Effect, "Secondary", HitFlash);

            if (TownOfUsReworked.IsTest)
                Utils.LogSomething($"{Player.name} is {Name}");
        }

        public float FlashTimer()
        {
            var timespan = DateTime.UtcNow - LastFlashed;
            var num = Player.GetModifiedCooldown(CustomGameOptions.GrenadeCd) * 1000f;
            var flag2 = num - (float)timespan.TotalMilliseconds < 0f;
            return flag2 ? 0f : (num - (float)timespan.TotalMilliseconds) / 1000f;
        }

        public void Flash()
        {
            if (!Enabled)
            {
                ClosestPlayers = Utils.GetClosestPlayers(Player.GetTruePosition(), CustomGameOptions.FlashRadius);
                FlashedPlayers = ClosestPlayers;
            }

            Enabled = true;
            TimeRemaining -= Time.deltaTime;

            if (Utils.Meeting)
                TimeRemaining = 0f;

            //To stop the scenario where the flash and sabotage are called at the same time.
            var system = ShipStatus.Instance.Systems[SystemTypes.Sabotage].Cast<SabotageSystemType>();
            var dummyActive = system.dummy.IsActive;
            var sabActive = system.specials.Any(s => s.IsActive);

            if (sabActive || dummyActive)
                return;

            foreach (var player in ClosestPlayers)
            {
                if (CustomPlayer.Local == player)
                {
                    Utils.HUD.FullScreen.enabled = true;
                    Utils.HUD.FullScreen.gameObject.active = true;

                    if (TimeRemaining > CustomGameOptions.GrenadeDuration - 0.5f)
                    {
                        var fade = (TimeRemaining - CustomGameOptions.GrenadeDuration) * (-2f);

                        if (ShouldPlayerBeBlinded(player))
                            Utils.HUD.FullScreen.color = Color32.Lerp(NormalVision, BlindVision, fade);
                        else if (ShouldPlayerBeDimmed(player))
                            Utils.HUD.FullScreen.color = Color32.Lerp(NormalVision, DimVision, fade);
                        else
                            Utils.HUD.FullScreen.color = NormalVision;
                    }
                    else if (TimeRemaining <= (CustomGameOptions.GrenadeDuration - 0.5f) && TimeRemaining >= 0.5f)
                    {
                        Utils.HUD.FullScreen.enabled = true;
                        Utils.HUD.FullScreen.gameObject.active = true;

                        if (ShouldPlayerBeBlinded(player))
                            Utils.HUD.FullScreen.color = BlindVision;
                        else if (ShouldPlayerBeDimmed(player))
                            Utils.HUD.FullScreen.color = DimVision;
                        else
                            Utils.HUD.FullScreen.color = NormalVision;
                    }
                    else if (TimeRemaining < 0.5f)
                    {
                        var fade2 = (TimeRemaining * -2.0f) + 1.0f;

                        if (ShouldPlayerBeBlinded(player))
                            Utils.HUD.FullScreen.color = Color32.Lerp(BlindVision, NormalVision, fade2);
                        else if (ShouldPlayerBeDimmed(player))
                            Utils.HUD.FullScreen.color = Color32.Lerp(DimVision, NormalVision, fade2);
                        else
                            Utils.HUD.FullScreen.color = NormalVision;
                    }
                    else
                    {
                        Utils.HUD.FullScreen.color = NormalVision;
                        TimeRemaining = 0f;
                    }

                    if (MapBehaviour.Instance)
                        MapBehaviour.Instance.Close();

                    if (Minigame.Instance)
                        Minigame.Instance.Close();
                }
            }
        }

        private static bool ShouldPlayerBeDimmed(PlayerControl player) => (player.Is(Faction.Intruder) || player.Data.IsDead) && !Utils.Meeting;

        private static bool ShouldPlayerBeBlinded(PlayerControl player) => !(player.Is(Faction.Intruder) || player.Data.IsDead || Utils.Meeting);

        public void UnFlash()
        {
            Enabled = false;
            LastFlashed = DateTime.UtcNow;
            Utils.HUD.FullScreen.color = NormalVision;
            FlashedPlayers.Clear();
            var fs = false;

            switch (TownOfUsReworked.VanillaOptions.MapId)
            {
                case 0:
                case 1:
                case 3:
                    var reactor1 = ShipStatus.Instance.Systems[SystemTypes.Reactor].Cast<ReactorSystemType>();
                    var oxygen1 = ShipStatus.Instance.Systems[SystemTypes.LifeSupp].Cast<LifeSuppSystemType>();
                    fs = reactor1.IsActive || oxygen1.IsActive;
                    break;

                case 2:
                    var seismic = ShipStatus.Instance.Systems[SystemTypes.Laboratory].Cast<ReactorSystemType>();
                    fs = seismic.IsActive;
                    break;

                case 4:
                    var reactor = ShipStatus.Instance.Systems[SystemTypes.Reactor].Cast<HeliSabotageSystem>();
                    fs = reactor.IsActive;
                    break;

                case 5:
                    fs = CustomPlayer.Local.myTasks.Any(x => x.TaskType == ModCompatibility.RetrieveOxygenMask);
                    break;

                case 6:
                    var reactor3 = ShipStatus.Instance.Systems[SystemTypes.Reactor].Cast<ReactorSystemType>();
                    var oxygen3 = ShipStatus.Instance.Systems[SystemTypes.LifeSupp].Cast<LifeSuppSystemType>();
                    var seismic2 = ShipStatus.Instance.Systems[SystemTypes.Laboratory].Cast<ReactorSystemType>();
                    fs = reactor3.IsActive || seismic2.IsActive || oxygen3.IsActive;
                    break;
            }

            Utils.HUD.FullScreen.enabled = fs;
        }

        public void HitFlash()
        {
            var system = ShipStatus.Instance.Systems[SystemTypes.Sabotage].Cast<SabotageSystemType>();
            var dummyActive = system.dummy.IsActive;
            var sabActive = system.specials.Any(s => s.IsActive);

            if (sabActive || dummyActive || FlashTimer() != 0f)
                return;

            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.Action, SendOption.Reliable);
            writer.Write((byte)ActionsRPC.FlashGrenade);
            writer.Write(PlayerId);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            TimeRemaining = CustomGameOptions.GrenadeDuration;
            Flash();
        }

        public override void UpdateHud(HudManager __instance)
        {
            base.UpdateHud(__instance);
            var system = ShipStatus.Instance.Systems[SystemTypes.Sabotage].Cast<SabotageSystemType>();
            var dummyActive = system.dummy.IsActive;
            var sabActive = system.specials.Any(s => s.IsActive);
            var condition = !dummyActive && !sabActive;
            FlashButton.Update("FLASH", FlashTimer(), CustomGameOptions.GrenadeCd, Flashed, TimeRemaining, CustomGameOptions.GrenadeDuration, condition);
        }
    }
}
