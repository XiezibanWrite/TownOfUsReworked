namespace TownOfUsReworked.PlayerLayers.Roles
{
    public class Actor : Neutral
    {
        public bool Guessed;
        public InspectorResults PretendRoles = InspectorResults.None;

        public Actor(PlayerControl player) : base(player)
        {
            Name = "Actor";
            StartText = () => "Play Pretend With The Others";
            Objectives = () => Guessed ? "- You have successfully fooled the crew" : ($"- Get guessed as one of your target roles\n- Your target roles belong to the {PretendRoles} role " +
                "list");
            Color = CustomGameOptions.CustomNeutColors ? Colors.Actor : Colors.Neutral;
            RoleType = RoleEnum.Actor;
            RoleAlignment = RoleAlignment.NeutralEvil;
            Type = LayerEnum.Actor;
            PretendRoles = InspectorResults.None;
            InspectorResults = InspectorResults.Manipulative;

            if (TownOfUsReworked.IsTest)
                Utils.LogSomething($"{Player.name} is {Name}");
        }
    }
}