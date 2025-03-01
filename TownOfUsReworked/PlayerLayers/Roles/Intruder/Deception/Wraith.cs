namespace TownOfUsReworked.PlayerLayers.Roles
{
    public class Wraith : Intruder
    {
        public CustomButton InvisButton;
        public bool Enabled;
        public DateTime LastInvis;
        public float TimeRemaining;
        public bool IsInvis => TimeRemaining > 0f;

        public Wraith(PlayerControl player) : base(player)
        {
            Name = "Wraith";
            StartText = () => "Sneaky Sneaky";
            AbilitiesText = () => $"- You can turn invisible\n{CommonAbilities}";
            Color = CustomGameOptions.CustomIntColors ? Colors.Wraith : Colors.Intruder;
            RoleType = RoleEnum.Wraith;
            RoleAlignment = RoleAlignment.IntruderDecep;
            Type = LayerEnum.Wraith;
            InvisButton = new(this, "Invis", AbilityTypes.Effect, "Secondary", HitInvis);
            InspectorResults = InspectorResults.Unseen;

            if (TownOfUsReworked.IsTest)
                Utils.LogSomething($"{Player.name} is {Name}");
        }

        public float InvisTimer()
        {
            var timespan = DateTime.UtcNow - LastInvis;
            var num = Player.GetModifiedCooldown(CustomGameOptions.InvisCd) * 1000f;
            var flag2 = num - (float)timespan.TotalMilliseconds < 0f;
            return flag2 ? 0f : (num - (float)timespan.TotalMilliseconds) / 1000f;
        }

        public void Invis()
        {
            Enabled = true;
            TimeRemaining -= Time.deltaTime;
            Utils.Invis(Player, CustomPlayer.Local.Is(Faction.Intruder));

            if (IsDead || Utils.Meeting)
                TimeRemaining = 0f;
        }

        public void Uninvis()
        {
            Enabled = false;
            LastInvis = DateTime.UtcNow;
            Utils.DefaultOutfit(Player);
        }

        public void HitInvis()
        {
            if (InvisTimer() != 0f || IsInvis)
                return;

            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.Action, SendOption.Reliable);
            writer.Write((byte)ActionsRPC.Invis);
            writer.Write(PlayerId);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            TimeRemaining = CustomGameOptions.InvisDuration;
            Invis();
        }

        public override void UpdateHud(HudManager __instance)
        {
            base.UpdateHud(__instance);
            InvisButton.Update("INVIS", InvisTimer(), CustomGameOptions.InvisCd, IsInvis, TimeRemaining, CustomGameOptions.InvisDuration);
        }
    }
}