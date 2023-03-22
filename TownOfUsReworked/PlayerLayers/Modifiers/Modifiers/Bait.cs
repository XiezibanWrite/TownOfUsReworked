using TownOfUsReworked.Classes;
using TownOfUsReworked.CustomOptions;
using TownOfUsReworked.Enums;

namespace TownOfUsReworked.PlayerLayers.Modifiers
{
    public class Bait : Modifier
    {
        public Bait(PlayerControl player) : base(player)
        {
            Name = "Bait";
            TaskText = "- Killing you causes the killer to report your body.";
            Color = CustomGameOptions.CustomModifierColors ? Colors.Bait : Colors.Modifier;
            ModifierType = ModifierEnum.Bait;
            Hidden = !CustomGameOptions.BaitKnows;
        }
    }
}