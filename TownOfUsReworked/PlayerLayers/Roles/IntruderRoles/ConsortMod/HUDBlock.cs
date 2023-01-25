﻿using HarmonyLib;
using TownOfUsReworked.Enums;
using TownOfUsReworked.Extensions;
using UnityEngine;
using TownOfUsReworked.PlayerLayers.Roles.Roles;
using System.Linq;
using TownOfUsReworked.Lobby.CustomOption;

namespace TownOfUsReworked.PlayerLayers.Roles.IntruderRoles.ConsortMod
{
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public class HUDBlock
    {
        public static Sprite Roleblock => TownOfUsReworked.Placeholder;

        public static void Postfix(HudManager __instance)
        {
            if (Utils.CannotUseButton(PlayerControl.LocalPlayer, RoleEnum.Consort))
                return;
                
            var role = Role.GetRole<Consort>(PlayerControl.LocalPlayer);

            if (role.BlockButton == null)
            {
                role.BlockButton = Object.Instantiate(__instance.KillButton, __instance.KillButton.transform.parent);
                role.BlockButton.graphic.enabled = true;
                role.BlockButton.GetComponent<AspectPosition>().DistanceFromEdge = TownOfUsReworked.BelowVentPosition;
            }

            if (role.KillButton == null)
            {
                role.KillButton = Object.Instantiate(__instance.KillButton, __instance.KillButton.transform.parent);
                role.KillButton.graphic.enabled = true;
            }

            var notImp = PlayerControl.AllPlayerControls.ToArray().Where(x => !x.Is(Faction.Intruder)).ToList();

            if (role.IsRecruit)
                notImp = PlayerControl.AllPlayerControls.ToArray().Where(x => !x.Is(SubFaction.Cabal)).ToList();

            role.KillButton.gameObject.SetActive(Utils.SetActive(PlayerControl.LocalPlayer));
            role.KillButton.SetCoolDown(role.KillTimer(), CustomGameOptions.IntKillCooldown);
            role.BlockButton.graphic.sprite = Roleblock;
            role.BlockButton.GetComponent<AspectPosition>().Update();
            role.BlockButton.gameObject.SetActive(Utils.SetActive(PlayerControl.LocalPlayer));

            if (role.Enabled)
                role.BlockButton.SetCoolDown(role.TimeRemaining, CustomGameOptions.ConsRoleblockDuration);
            else
                role.BlockButton.SetCoolDown(role.RoleblockTimer(), CustomGameOptions.ConsRoleblockCooldown);

            Utils.SetTarget(ref role.ClosestPlayer, role.BlockButton, notImp);
            Utils.SetTarget(ref role.ClosestPlayer, role.KillButton, notImp);

            var renderer = role.BlockButton.graphic;
            var renderer2 = role.KillButton.graphic;
            
            if (role.ClosestPlayer != null)
            {
                renderer.color = Palette.EnabledColor;
                renderer.material.SetFloat("_Desat", 0f);
                renderer2.color = Palette.EnabledColor;
                renderer2.material.SetFloat("_Desat", 0f);
            }
            else
            {
                renderer.color = Palette.DisabledClear;
                renderer.material.SetFloat("_Desat", 1f);
                renderer2.color = Palette.DisabledClear;
                renderer2.material.SetFloat("_Desat", 1f);
            }
        }
    }
}