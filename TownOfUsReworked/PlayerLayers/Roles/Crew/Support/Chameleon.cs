using static TownOfUsReworked.Languages.Language;
namespace TownOfUsReworked.PlayerLayers.Roles
{
    public class Chameleon : Crew
    {
        public bool Enabled;
        public DateTime LastSwooped;
        public float TimeRemaining;
        public bool IsSwooped => TimeRemaining > 0f;
        public CustomButton SwoopButton;
        public int UsesLeft;
        public bool ButtonUsable => UsesLeft > 0;

        public Chameleon(PlayerControl player) : base(player)
        {
            Name = GetString("Chameleon");
            StartText = () => GetString("ChameleonStartText");
            AbilitiesText = () => GetString("ChameleonAbilitiesText");
            Color = CustomGameOptions.CustomCrewColors ? Colors.Chameleon : Colors.Crew;
            RoleType = RoleEnum.Chameleon;
            InspectorResults = InspectorResults.Unseen;
            UsesLeft = CustomGameOptions.SwoopCount;
            Type = LayerEnum.Chameleon;
            SwoopButton = new(this, "Swoop", AbilityTypes.Direct, "ActionSecondary", HitSwoop, true);

            if (TownOfUsReworked.IsTest)
                Utils.LogSomething($"{Player.name} is {Name}");
        }

        public float SwoopTimer()
        {
            var timespan = DateTime.UtcNow - LastSwooped;
            var num = Player.GetModifiedCooldown(CustomGameOptions.SwoopCooldown) * 1000f;
            var flag2 = num - (float)timespan.TotalMilliseconds < 0f;
            return flag2 ? 0f : (num - (float)timespan.TotalMilliseconds) / 1000f;
        }

        public void Invis()
        {
            Enabled = true;
            TimeRemaining -= Time.deltaTime;
            Utils.Invis(Player);

            if (Utils.Meeting || IsDead)
                TimeRemaining = 0f;
        }

        public void Uninvis()
        {
            Enabled = false;
            LastSwooped = DateTime.UtcNow;
            Utils.DefaultOutfit(Player);
        }

        public void HitSwoop()
        {
            if (SwoopTimer() != 0f || IsSwooped || !ButtonUsable)
                return;

            TimeRemaining = CustomGameOptions.SwoopDuration;
            Invis();
            UsesLeft--;
            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.Action, SendOption.Reliable);
            writer.Write((byte)ActionsRPC.Swoop);
            writer.Write(PlayerId);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
        }

        public override void UpdateHud(HudManager __instance)
        {
            base.UpdateHud(__instance);
            SwoopButton.Update("SWOOP", SwoopTimer(), CustomGameOptions.SwoopCooldown, UsesLeft, IsSwooped, TimeRemaining, CustomGameOptions.SwoopDuration);
        }
    }
}