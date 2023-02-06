using System.Linq;
using HarmonyLib;
using TownOfUsReworked.Classes;
using TownOfUsReworked.Enums;
using UnityEngine;
using TownOfUsReworked.Patches;
using Reactor.Utilities.Extensions;

namespace TownOfUsReworked.PlayerLayers.Roles.NeutralRoles.UndeadMod
{
    [HarmonyPatch(typeof(EndGameManager), nameof(EndGameManager.Start))]
    public static class Outro
    {
        public static void Postfix(EndGameManager __instance)
        {
            var role = Role.AllRoles.FirstOrDefault(x => x.SubFaction == SubFaction.Undead && Role.UndeadWin);

            if (role == null)
                return;

            PoolablePlayer[] array = Object.FindObjectsOfType<PoolablePlayer>();

            foreach (var player in array)
                player.NameText().text = "<color=#" + Color.white.ToHtmlStringRGBA() + ">" + player.NameText().text + "</color>";

            __instance.BackgroundBar.material.color = Colors.Undead;
            var text = Object.Instantiate(__instance.WinText);
            text.text = "The Undead Win!";
            text.color = Colors.Undead;
            var pos = __instance.WinText.transform.localPosition;
            pos.y = 1.5f;
            text.transform.position = pos;
            text.text = $"<size=4>{text.text}</size>";
            
            try
            {
                //SoundManager.Instance.PlaySound(TownOfUsReworked.PhantomWin, false, 1f);
            } catch {}
        }
    }
}