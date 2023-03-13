using System;
using UnityEngine;
using TownOfUsReworked.Enums;
using TownOfUsReworked.CustomOptions;
using TownOfUsReworked.Classes;
using TownOfUsReworked.PlayerLayers.Roles.IntruderRoles.TimeMasterMod;

namespace TownOfUsReworked.PlayerLayers.Roles
{
    public class TimeMaster : IntruderRole
    {
        public AbilityButton FreezeButton;
        public bool Enabled = false;
        public float TimeRemaining;
        public DateTime LastFrozen;
        public bool Frozen => TimeRemaining > 0f;

        public TimeMaster(PlayerControl player) : base(player)
        {
            Name = "Time Master";
            StartText = "Freeze Time To Stop The <color=#8BFDFDFF>Crew</color>";
            AbilitiesText = "Freeze time to stop the <color=#8BFDFDFF>Crew</color> from moving";
            Color = CustomGameOptions.CustomIntColors ? Colors.TimeMaster : Colors.Intruder;
            LastFrozen = DateTime.UtcNow;
            RoleType = RoleEnum.TimeMaster;
            RoleAlignment = RoleAlignment.IntruderSupport;
            AlignmentName = IS;
        }
        
        public float FreezeTimer()
        {
            var utcNow = DateTime.UtcNow;
            var timeSpan = utcNow - LastFrozen;
            var num = Utils.GetModifiedCooldown(CustomGameOptions.FreezeCooldown, Utils.GetUnderdogChange(Player)) * 1000f;
            var flag2 = num - (float)timeSpan.TotalMilliseconds < 0f;

            if (flag2)
                return 0f;

            return (num - (float)timeSpan.TotalMilliseconds) / 1000f;
        }

        public void TimeFreeze()
        {
            Enabled = true;
            TimeRemaining -= Time.deltaTime;
        }

        public void Unfreeze()
        {
            Enabled = false;
            LastFrozen = DateTime.UtcNow;
            Freeze.FreezeFunctions.UnfreezeAll();
        }
    }
}