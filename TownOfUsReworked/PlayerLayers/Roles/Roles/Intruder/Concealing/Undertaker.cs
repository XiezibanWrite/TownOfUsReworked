using System;
using TownOfUsReworked.Enums;
using TownOfUsReworked.CustomOptions;
using TownOfUsReworked.Classes;

namespace TownOfUsReworked.PlayerLayers.Roles
{
    public class Undertaker : IntruderRole
    {
        public AbilityButton DragButton;
        public AbilityButton DropButton;
        public DateTime LastDragged;
        public DeadBody CurrentTarget;
        public DeadBody CurrentlyDragging;

        public Undertaker(PlayerControl player) : base(player)
        {
            Name = "Undertaker";
            StartText = "Drag Bodies And Hide Them";
            AbilitiesText = "Drag bodies around to hide them from being reported";
            Color = CustomGameOptions.CustomIntColors? Colors.Undertaker : Colors.Intruder;
            LastDragged = DateTime.UtcNow;
            RoleType = RoleEnum.Undertaker;
            RoleAlignment = RoleAlignment.IntruderConceal;
            AlignmentName = IC;
        }

        public float DragTimer()
        {
            var utcNow = DateTime.UtcNow;
            var timeSpan = utcNow - LastDragged;
            var num = Utils.GetModifiedCooldown(CustomGameOptions.DragCd, Utils.GetUnderdogChange(Player)) * 1000f;
            var flag2 = num - (float) timeSpan.TotalMilliseconds < 0f;

            if (flag2)
                return 0f;

            return (num - (float) timeSpan.TotalMilliseconds) / 1000f;
        }
    }
}