using System.Linq;
using HarmonyLib;
using TownOfUsReworked.Enums;
using TownOfUsReworked.Classes;
using UnityEngine;

namespace TownOfUsReworked.PlayerLayers.Objectifiers.LoversMod
{
    [HarmonyPatch(typeof(EndGameManager), nameof(EndGameManager.Start))]
    public static class Outro
    {
        public static void Postfix(EndGameManager __instance)
        {
            var obj = Objectifier.AllObjectifiers.FirstOrDefault(x => x.ObjectifierType == ObjectifierEnum.Lovers && ((Lovers)x).LoveWins);

            if (obj == null)
                return;

            var array = Object.FindObjectsOfType<PoolablePlayer>();

            foreach (var player in array)
                player.NameText().text = Utils.GetEndGameName(player.NameText().text);

            __instance.BackgroundBar.material.color = obj.Color;
            var text = Object.Instantiate(__instance.WinText);
            text.text = "Love Wins!";
            text.color = obj.Color;
            var pos = __instance.WinText.transform.localPosition;
            pos.y = 1.5f;
            text.transform.position = pos;
            text.text = $"<size=4>{text.text}</size>";
        }
    }
}