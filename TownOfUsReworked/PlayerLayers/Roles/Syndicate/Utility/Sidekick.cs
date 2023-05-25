namespace TownOfUsReworked.PlayerLayers.Roles
{
    public class Sidekick : SyndicateRole
    {
        public Role FormerRole;
        public Rebel Rebel;
        public bool CanPromote => (Rebel.IsDead || Rebel.Disconnected) && !IsDead;

        public Sidekick(PlayerControl player) : base(player)
        {
            Name = "Sidekick";
            RoleType = RoleEnum.Sidekick;
            StartText = "Succeed The <color=#FFFCCEFF>Rebel</color>";
            AbilitiesText = "- When the <color=#FFFCCEFF>Rebel</color> dies, you will become the new <color=#FFFCCEFF>Rebel</color> with boosted abilities of your former ";
            Color = CustomGameOptions.CustomSynColors ? Colors.Sidekick : Colors.Syndicate;
            RoleAlignment = RoleAlignment.SyndicateUtil;
            Type = LayerEnum.Sidekick;
            InspectorResults = InspectorResults.IsCold;
        }

        public void TurnRebel()
        {
            var newRole = new PromotedRebel(Player)
            {
                FormerRole = FormerRole,
                RoleBlockImmune = FormerRole.RoleBlockImmune,
                RoleAlignment = FormerRole.RoleAlignment
            };

            newRole.RoleUpdate(this);

            if (Player == PlayerControl.LocalPlayer)
                Utils.Flash(Colors.Rebel);

            if (PlayerControl.LocalPlayer.Is(RoleEnum.Seer))
                Utils.Flash(Colors.Seer);
        }

        public override void UpdateHud(HudManager __instance)
        {
            base.UpdateHud(__instance);

            if (CanPromote)
            {
                TurnRebel();
                var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.Change, SendOption.Reliable);
                writer.Write((byte)TurnRPC.TurnRebel);
                writer.Write(PlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
            }
        }
    }
}