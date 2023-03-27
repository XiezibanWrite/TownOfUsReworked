using HarmonyLib;
using UnityEngine;
using System.Collections.Generic;
using System;
using TownOfUsReworked.Classes;
using TownOfUsReworked.Extensions;

namespace TownOfUsReworked.Patches
{
    public static class GameStartManagerPatch
    {
        //Thanks to The Other Roles for this code, made a minor change so that MCI works :sweat_smile:
        public readonly static Dictionary<int, PlayerVersion> PlayerVersions = new();
        private static float kickingTimer;
        private static bool versionSent;

        #pragma warning disable
        public static float timer = 600f;
        #pragma warning restore

        [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.OnPlayerJoined))]
        public static class AmongUsClientOnPlayerJoinedPatch
        {
            public static void Postfix()
            {
                if (PlayerControl.LocalPlayer != null && !TownOfUsReworked.MCIActive)
                    Utils.ShareGameVersion();
            }
        }

        [HarmonyPatch(typeof(GameStartManager), nameof(GameStartManager.Start))]
        public static class GameStartManagerStartPatch
        {
            public static void Postfix()
            {
                //Trigger version refresh
                versionSent = false;
                //Reset lobby countdown timer
                timer = 600f;
                //Reset kicking timer
                kickingTimer = 0f;
                //Copy lobby code
                GUIUtility.systemCopyBuffer = InnerNet.GameCode.IntToGameName(AmongUsClient.Instance.GameId);
            }
        }

        [HarmonyPatch(typeof(GameStartManager), nameof(GameStartManager.Update))]
        public static class GameStartManagerUpdatePatch
        {
            private static float startingTimer;
            private static bool update;
            private static string currentText = "";

            public static void Prefix(GameStartManager __instance)
            {
                if (!GameData.Instance )
                    return; //No instance

                update = GameData.Instance.PlayerCount != __instance.LastPlayerCount;
            }

            public static void Postfix(GameStartManager __instance)
            {
                if (!TownOfUsReworked.MCIActive)
                {
                    //Send version as soon as PlayerControl.LocalPlayer exists
                    if (PlayerControl.LocalPlayer != null && !versionSent)
                    {
                        versionSent = true;
                        Utils.ShareGameVersion();
                    }

                    //Check version handshake infos
                    var versionMismatch = false;
                    var message = "";

                    foreach (var client in AmongUsClient.Instance.allClients.ToArray())
                    {
                        if (client.Character == null)
                            continue;

                        if (!PlayerVersions.ContainsKey(client.Id))
                        {
                            versionMismatch = true;
                            message += $"<color=#FF0000FF>{client.Character.Data.PlayerName} has a different or no version of Town Of Us Reworked.\n</color>";
                        }
                        else
                        {
                            var PV = PlayerVersions[client.Id];
                            var diff = TownOfUsReworked.Version.CompareTo(PV.Version);

                            if (diff > 0)
                            {
                                versionMismatch = true;
                                message += $"<color=#FF0000FF>{client.Character.Data.PlayerName} has an older version of Town Of Us Reworked (v{PlayerVersions[client.Id].Version})\n</color>";
                            }
                            else if (diff < 0)
                            {
                                versionMismatch = true;
                                message += $"<color=#FF0000FF>{client.Character.Data.PlayerName} has a newer version of Town Of Us Reworked (v{PlayerVersions[client.Id].Version})\n</color>";
                            }
                            else if (!PV.GuidMatches())
                            {
                                //Version presumably matches, check if Guid matches
                                versionMismatch = true;
                                message += $"<color=#FF0000FF>{client.Character.Data.PlayerName} has a modified version of Town Of Us Reworked v{PlayerVersions[client.Id].Version}" +
                                    $" <size=30%>({PV.Guid})</size>\n</color>";
                            }
                        }
                    }

                    //Display message to the host
                    if (AmongUsClient.Instance.AmHost)
                    {
                        if (versionMismatch)
                        {
                            __instance.StartButton.color = __instance.startLabelText.color = Palette.DisabledClear;
                            __instance.GameStartText.text = message;
                            __instance.GameStartText.transform.localPosition = __instance.StartButton.transform.localPosition + (Vector3.up * 2);
                        }
                        else
                        {
                            __instance.StartButton.color = __instance.startLabelText.color = (__instance.LastPlayerCount >= __instance.MinPlayers) ? Palette.EnabledColor :
                                Palette.DisabledClear;
                            __instance.GameStartText.transform.localPosition = __instance.StartButton.transform.localPosition;
                        }
                    }
                    //Client update with handshake infos
                    else if (!PlayerVersions.ContainsKey(AmongUsClient.Instance.HostId) || TownOfUsReworked.Version.CompareTo(PlayerVersions[AmongUsClient.Instance.HostId].Version) != 0)
                    {
                        kickingTimer += Time.deltaTime;

                        if (kickingTimer > 10)
                        {
                            kickingTimer = 0;
                            AmongUsClient.Instance.ExitGame(DisconnectReasons.ExitGame);
                            SceneChanger.ChangeScene("MainMenu");
                        }

                        __instance.GameStartText.text = "<color=#FF0000FF>The host has no or a different version of Town Of Us Reworked.\nYou will be kicked in" +
                            $" {Math.Round(10 - kickingTimer)}s</color>";
                        __instance.GameStartText.transform.localPosition = __instance.StartButton.transform.localPosition + (Vector3.up * 2);
                    }
                    else if (versionMismatch)
                    {
                        __instance.GameStartText.text = "<color=#FF0000FF>Players With Different Versions:\n</color>" + message;
                        __instance.GameStartText.transform.localPosition = __instance.StartButton.transform.localPosition + (Vector3.up * 2);
                    }
                    else
                    {
                        __instance.GameStartText.transform.localPosition = __instance.StartButton.transform.localPosition;

                        if (__instance.startState != GameStartManager.StartingStates.Countdown && startingTimer <= 0)
                        {
                            __instance.GameStartText.text = string.Empty;
                        }
                        else
                        {
                            __instance.GameStartText.text = $"Starting in {(int)startingTimer + 1}";

                            if (startingTimer <= 0)
                                __instance.GameStartText.text = string.Empty;
                        }
                    }
                }

                //Start Timer
                if (startingTimer > 0)
                    startingTimer -= Time.deltaTime;

                //Lobby timer
                if (!GameData.Instance)
                    return; //No instance

                if (update)
                    currentText = __instance.PlayerCounter.text;

                timer = Mathf.Max(0f, timer -= Time.deltaTime);
                var minutes = (int)timer / 60;
                var seconds = (int)timer % 60;
                var suffix = $" ({minutes}:{seconds})";

                __instance.PlayerCounter.text = $"<size=75%>{currentText}{suffix}</size>";
                __instance.PlayerCounter.autoSizeTextContainer = true;
            }
        }

        [HarmonyPatch(typeof(GameStartManager), nameof(GameStartManager.BeginGame))]
        public static class GameStartManagerBeginGame
        {
            public static bool Prefix()
            {
                //Block game start if not everyone has the same mod version
                var continueStart = true;

                if (!TownOfUsReworked.MCIActive && AmongUsClient.Instance.AmHost)
                {
                    foreach (var client in AmongUsClient.Instance.allClients.GetFastEnumerator())
                    {
                        if (client.Character == null)
                            continue;

                        var dummyComponent = client.Character.GetComponent<DummyBehaviour>();

                        if (dummyComponent?.enabled == true)
                            continue;

                        if (!PlayerVersions.ContainsKey(client.Id))
                        {
                            continueStart = false;
                            break;
                        }

                        PlayerVersion PV = PlayerVersions[client.Id];
                        var diff = TownOfUsReworked.Version.CompareTo(PV.Version);

                        if (diff != 0 || !PV.GuidMatches())
                        {
                            continueStart = false;
                            break;
                        }
                    }
                }

                return continueStart;
            }
        }

        public class PlayerVersion
        {
            public readonly Version Version;
            public readonly Guid Guid;

            public PlayerVersion(Version Version, Guid Guid)
            {
                this.Version = Version;
                this.Guid = Guid;
            }

            public bool GuidMatches() => TownOfUsReworked.Assembly.ManifestModule.ModuleVersionId.Equals(Guid);
        }
    }
}