using TownOfUsReworked.Enums;
using TownOfUsReworked.CustomOptions;
using TownOfUsReworked.Classes;

namespace TownOfUsReworked.PlayerLayers.Roles
{
    public class Actor : NeutralRole
    {
        public bool Guessed;
        public InspectorResults PretendRoles;

        public Actor(PlayerControl player) : base(player)
        {
            Name = "Actor";
            StartText = "Play Pretend WIth The Others";
            Objectives = $"- Get guessed as one of your target roles.\n- Your target roles belong to the {PretendRoles} role list.";
            Color = CustomGameOptions.CustomNeutColors ? Colors.Actor : Colors.Neutral;
            RoleType = RoleEnum.Actor;
            RoleAlignment = RoleAlignment.NeutralEvil;
            AlignmentName = NE;
            RoleDescription = "You are an Actor! You are a crazed performer who wants to die! Get guessed as one of the roles you are pretending to be!";
        }
    }
}