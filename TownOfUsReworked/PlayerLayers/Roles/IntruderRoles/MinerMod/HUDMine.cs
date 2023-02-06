using System.Linq;
using HarmonyLib;
using TownOfUsReworked.Enums;
using TownOfUsReworked.Lobby.CustomOption;
using TownOfUsReworked.Classes;
using UnityEngine;
using TownOfUsReworked.PlayerLayers.Roles.Roles;

namespace TownOfUsReworked.PlayerLayers.Roles.IntruderRoles.MinerMod
{
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public class HUDMine
    {
        public static Sprite MineSprite => TownOfUsReworked.MineSprite;

        public static void Postfix(HudManager __instance)
        {
            if (PlayerControl.AllPlayerControls.Count <= 1)
                return;

            if (PlayerControl.LocalPlayer == null)
                return;

            if (PlayerControl.LocalPlayer.Data == null)
                return;

            if (!PlayerControl.LocalPlayer.Is(RoleEnum.Miner))
                return;

            var role = Role.GetRole<Miner>(PlayerControl.LocalPlayer);

            if (role.MineButton == null)
            {
                role.MineButton = Object.Instantiate(__instance.KillButton, __instance.KillButton.transform.parent);
                role.MineButton.graphic.enabled = true;
                role.MineButton.GetComponent<AspectPosition>().DistanceFromEdge = TownOfUsReworked.BelowVentPosition;
                role.MineButton.gameObject.SetActive(false);
            }

            role.MineButton.GetComponent<AspectPosition>().Update();
            role.MineButton.graphic.sprite = MineSprite;
            role.MineButton.gameObject.SetActive(!PlayerControl.LocalPlayer.Data.IsDead && !MeetingHud.Instance && !LobbyBehaviour.Instance);

            role.MineButton.SetCoolDown(role.MineTimer(), CustomGameOptions.MineCd);
            var hits = Physics2D.OverlapBoxAll(PlayerControl.LocalPlayer.transform.position, role.VentSize, 0);
            hits = hits.ToArray().Where(c => (c.name.Contains("Vent") || !c.isTrigger) && c.gameObject.layer != 8 && c.gameObject.layer != 5).ToArray();
            
            if (hits.Count == 0 && PlayerControl.LocalPlayer.moveable == true)
            {
                role.MineButton.graphic.color = Palette.EnabledColor;
                role.MineButton.graphic.material.SetFloat("_Desat", 0f);
                role.CanPlace = true;
            }
            else
            {
                role.MineButton.graphic.color = Palette.DisabledClear;
                role.MineButton.graphic.material.SetFloat("_Desat", 1f);
                role.CanPlace = false;
            }
        }
    }
}