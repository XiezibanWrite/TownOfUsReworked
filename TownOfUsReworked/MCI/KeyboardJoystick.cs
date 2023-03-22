﻿using HarmonyLib;
using UnityEngine;
using TownOfUsReworked.PlayerLayers.Roles;
using TownOfUsReworked.Classes;

namespace TownOfUsReworked.MCI
{
    [HarmonyPatch(typeof(KeyboardJoystick), nameof(KeyboardJoystick.Update))]
    public sealed class Keyboard_Joystick
    {
        public static int controllingFigure;

        public static void Postfix()
        {
            if (!GameStates.IsLocalGame)
                return; //You must ensure you are only playing on local

            if (Input.GetKeyDown(KeyCode.F1))
            {
                if (!GameStates.IsLobby)
                    return; //Don't try to add bots when you are playtesting

                controllingFigure = PlayerControl.LocalPlayer.PlayerId;

                if (PlayerControl.AllPlayerControls.Count >= 15 && TownOfUsReworked.LobbyCapped)
                    return; //Toggle lobby limit

                MCIUtils.CleanUpLoad();
                MCIUtils.CreatePlayerInstance();

                if (!InstanceControl.MCIActive)
                    InstanceControl.MCIActive = true;
            }

            if (!InstanceControl.MCIActive)
                return;

            if (Input.GetKeyDown(KeyCode.F2))
            {
                controllingFigure++;

                if (controllingFigure >= PlayerControl.AllPlayerControls.Count)
                    controllingFigure = 0;

                InstanceControl.SwitchTo((byte)controllingFigure);
            }

            if (Input.GetKeyDown(KeyCode.F3))
            {
                controllingFigure--;

                if (controllingFigure < 0)
                    controllingFigure = PlayerControl.AllPlayerControls.Count - 1;

                InstanceControl.SwitchTo((byte)controllingFigure);
            }

            if (Input.GetKeyDown(KeyCode.F4))
            {
                if (GameStates.IsLobby)
                    return;

                Role.NobodyWins = true;
                Utils.EndGame();
                InstanceControl.MCIActive = false;
            }

            if (Input.GetKeyDown(KeyCode.F5))
            {
                if (GameStates.IsLobby)
                    return;

                ShipStatus.Instance.RpcRepairSystem(SystemTypes.Doors, 79);
                ShipStatus.Instance.RpcRepairSystem(SystemTypes.Doors, 80);
                ShipStatus.Instance.RpcRepairSystem(SystemTypes.Doors, 81);
                ShipStatus.Instance.RpcRepairSystem(SystemTypes.Doors, 82);
                ShipStatus.Instance.RpcRepairSystem(SystemTypes.LifeSupp, 16);
                ShipStatus.Instance.RpcRepairSystem(SystemTypes.Reactor, 16);
                ShipStatus.Instance.RpcRepairSystem(SystemTypes.Laboratory, 16);
                ShipStatus.Instance.RpcRepairSystem(SystemTypes.Reactor, 16 | 0);
                ShipStatus.Instance.RpcRepairSystem(SystemTypes.Reactor, 16 | 1);
                ShipStatus.Instance.RpcRepairSystem(SystemTypes.Comms, 16 | 0);
                ShipStatus.Instance.RpcRepairSystem(SystemTypes.Comms, 16 | 1);
                ShipStatus.Instance.RpcRepairSystem(SystemTypes.Comms, 0);
                Utils.DefaultOutfitAll();
            }

            if (Input.GetKeyDown(KeyCode.F6))
            {
                if (GameStates.IsLobby)
                    return;

                foreach (var task in PlayerControl.LocalPlayer.myTasks)
                    PlayerControl.LocalPlayer.RpcCompleteTask(task.Id);
            }

            if (Input.GetKeyDown(KeyCode.F7))
            {
                if (GameStates.IsLobby)
                    return;

                HudManager.Instance.StartCoroutine(HudManager.Instance.CoFadeFullScreen(Color.clear, Color.black));
                HudManager.Instance.StartCoroutine(HudManager.Instance.CoShowIntro());
            }

            if (Input.GetKeyDown(KeyCode.F8))
            {
                if (!GameStates.IsLobby)
                    return;

                MCIUtils.RemoveAllPlayers();
            }

            if (Input.GetKeyDown(KeyCode.F9))
            {
                if (!GameStates.IsLobby)
                    return;

                TownOfUsReworked.LobbyCapped = !TownOfUsReworked.LobbyCapped;
                HudManager.Instance.Chat.AddChat(PlayerControl.LocalPlayer, $"The lobby limit is now {(TownOfUsReworked.LobbyCapped ? "" : "un")}capped.");
            }

            if (Input.GetKeyDown(KeyCode.F10))
            {
                if (!GameStates.IsLobby)
                    return;

                MCIUtils.RemovePlayer((byte)(InstanceControl.clients.Count));
            }

            if (Input.GetKeyDown(KeyCode.F11))
            {
                if (!GameStates.IsLobby)
                    return;

                TownOfUsReworked.Persistence = !TownOfUsReworked.Persistence;
                HudManager.Instance.Chat.AddChat(PlayerControl.LocalPlayer, $"The robots will now{(TownOfUsReworked.Persistence ? "" : " no longer")} persist.");
            }
        }
    }
}