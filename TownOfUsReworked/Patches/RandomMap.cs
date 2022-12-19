using HarmonyLib;
using Hazel;
using System;
using TownOfUsReworked.Lobby.CustomOption;
using AmongUs.GameOptions;

namespace TownOfUsReworked.Patches
{
    [HarmonyPatch]
    public class RandomMap
    {
        public static byte previousMap;
        public static float vision;
        public static int commonTasks;
        public static int shortTasks;
        public static int longTasks;

        [HarmonyPatch(typeof(GameStartManager), nameof(GameStartManager.BeginGame))]
        [HarmonyPrefix]
        public static bool Prefix(GameStartManager __instance)
        {
            if (AmongUsClient.Instance.AmHost)
            {
                previousMap = GameOptionsManager.Instance.currentNormalGameOptions.MapId;
                vision = CustomGameOptions.CrewVision;

                if (!(commonTasks == 0 && shortTasks == 0 && longTasks == 0))
                {
                    commonTasks = CustomGameOptions.CommonTasks;
                    shortTasks = CustomGameOptions.ShortTasks;
                    longTasks = CustomGameOptions.LongTasks;
                }

                var map = (byte)CustomGameOptions.Map;

                if (CustomGameOptions.RandomMapEnabled)
                {
                    map = GetRandomMap();
                    GameOptionsManager.Instance.currentNormalGameOptions.MapId = map;
                }

                GameOptionsManager.Instance.currentNormalGameOptions.RoleOptions.SetRoleRate(RoleTypes.Scientist, 0, 0);
                GameOptionsManager.Instance.currentNormalGameOptions.RoleOptions.SetRoleRate(RoleTypes.Engineer, 0, 0);
                GameOptionsManager.Instance.currentNormalGameOptions.RoleOptions.SetRoleRate(RoleTypes.GuardianAngel, 0, 0);
                GameOptionsManager.Instance.currentNormalGameOptions.RoleOptions.SetRoleRate(RoleTypes.Shapeshifter, 0, 0);
                GameOptionsManager.Instance.currentNormalGameOptions.CrewLightMod = CustomGameOptions.CrewVision;
                GameOptionsManager.Instance.currentNormalGameOptions.ImpostorLightMod = CustomGameOptions.IntruderVision;
                GameOptionsManager.Instance.currentNormalGameOptions.AnonymousVotes = CustomGameOptions.AnonymousVoting;
                GameOptionsManager.Instance.currentNormalGameOptions.VisualTasks = CustomGameOptions.VisualTasks;
                GameOptionsManager.Instance.currentNormalGameOptions.PlayerSpeedMod = CustomGameOptions.PlayerSpeed;
                GameOptionsManager.Instance.currentNormalGameOptions.NumImpostors = CustomGameOptions.IntruderCount;
                GameOptionsManager.Instance.currentNormalGameOptions.GhostsDoTasks = CustomGameOptions.GhostTasksCountToWin;
                GameOptionsManager.Instance.currentNormalGameOptions.TaskBarMode = (AmongUs.GameOptions.TaskBarMode)CustomGameOptions.TaskBarMode;
                GameOptionsManager.Instance.currentNormalGameOptions.ConfirmImpostor = CustomGameOptions.ConfirmEjects;
                GameOptionsManager.Instance.currentNormalGameOptions.VotingTime = CustomGameOptions.VotingTime;
                GameOptionsManager.Instance.currentNormalGameOptions.DiscussionTime = CustomGameOptions.DiscussionTime;
                GameOptionsManager.Instance.currentNormalGameOptions.KillDistance = CustomGameOptions.InteractionDistance;

                unchecked
                {
                    var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.SetSettings,
                        SendOption.Reliable, -1);
                    writer.Write(map);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                }

                if (CustomGameOptions.AutoAdjustSettings)
                    AdjustSettings(map);
            }
            return true;
        }

        [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.OnGameEnd))]
        [HarmonyPostfix]
        public static void Postfix(AmongUsClient __instance)
        {
            if (__instance.AmHost)
            {
                if (CustomGameOptions.AutoAdjustSettings)
                {
                    if (CustomGameOptions.SmallMapHalfVision)
                        GameOptionsManager.Instance.currentNormalGameOptions.CrewLightMod = vision;

                    if (GameOptionsManager.Instance.currentNormalGameOptions.MapId == 1)
                        AdjustCooldowns(CustomGameOptions.SmallMapDecreasedCooldown);

                    if (GameOptionsManager.Instance.currentNormalGameOptions.MapId >= 4)
                        AdjustCooldowns(-CustomGameOptions.LargeMapIncreasedCooldown);
                }

                if (CustomGameOptions.RandomMapEnabled)
                    GameOptionsManager.Instance.currentNormalGameOptions.MapId = previousMap;

                GameOptionsManager.Instance.currentNormalGameOptions.NumCommonTasks = commonTasks;
                GameOptionsManager.Instance.currentNormalGameOptions.NumShortTasks = shortTasks;
                GameOptionsManager.Instance.currentNormalGameOptions.NumLongTasks = longTasks;
            }
        }

        public static byte GetRandomMap()
        {
            Random _rnd = new Random();
            float totalWeight = 0;
            totalWeight += CustomGameOptions.RandomMapSkeld;
            totalWeight += CustomGameOptions.RandomMapMira;
            totalWeight += CustomGameOptions.RandomMapPolus;
            totalWeight += CustomGameOptions.RandomMapAirship;

            if (SubmergedCompatibility.Loaded)
                totalWeight += CustomGameOptions.RandomMapSubmerged;

            if (totalWeight == 0)
                return (byte)CustomGameOptions.Map;

            float randomNumber = _rnd.Next(0, (int)totalWeight);

            if (randomNumber < CustomGameOptions.RandomMapSkeld)
                return 0;

            randomNumber -= CustomGameOptions.RandomMapSkeld;

            if (randomNumber < CustomGameOptions.RandomMapMira)
                return 1;

            randomNumber -= CustomGameOptions.RandomMapMira;

            if (randomNumber < CustomGameOptions.RandomMapPolus)
                return 2;

            randomNumber -= CustomGameOptions.RandomMapPolus;

            if (randomNumber < CustomGameOptions.RandomMapAirship)
                return 4;

            randomNumber -= CustomGameOptions.RandomMapAirship;

            if (SubmergedCompatibility.Loaded && randomNumber < CustomGameOptions.RandomMapSubmerged)
                return 5;

            return (byte)CustomGameOptions.Map;
        }

        public static void AdjustSettings(byte map)
        {
            if (map <= 1)
            {
                if (CustomGameOptions.SmallMapHalfVision)
                    GameOptionsManager.Instance.currentNormalGameOptions.CrewLightMod *= 0.5f;

                GameOptionsManager.Instance.currentNormalGameOptions.NumShortTasks += CustomGameOptions.SmallMapIncreasedShortTasks;
                GameOptionsManager.Instance.currentNormalGameOptions.NumLongTasks += CustomGameOptions.SmallMapIncreasedLongTasks;
            }

            if (map == 1)
                AdjustCooldowns(-CustomGameOptions.SmallMapDecreasedCooldown);

            if (map >= 4)
            {
                GameOptionsManager.Instance.currentNormalGameOptions.NumShortTasks -= CustomGameOptions.LargeMapDecreasedShortTasks;
                GameOptionsManager.Instance.currentNormalGameOptions.NumLongTasks -= CustomGameOptions.LargeMapDecreasedLongTasks;
                AdjustCooldowns(CustomGameOptions.LargeMapIncreasedCooldown);
            }
            return;
        }

        public static void AdjustCooldowns(float change)
        {
            Generate.InitialExamineCooldown.Set((float)Generate.InitialExamineCooldown.Value + change, false);
            Generate.InterrogateCooldown.Set((float)Generate.InterrogateCooldown.Value + change, false);
            Generate.TrackCooldown.Set((float)Generate.TrackCooldown.Value + change, false);
            Generate.BugCooldown.Set((float)Generate.BugCooldown.Value + change, false);
            Generate.VigiKillCd.Set((float)Generate.VigiKillCd.Value + change, false);
            Generate.AlertCooldown.Set((float)Generate.AlertCooldown.Value + change, false);
            Generate.RewindCooldown.Set((float)Generate.RewindCooldown.Value + change, false);
            Generate.TransportCooldown.Set((float)Generate.TransportCooldown.Value + change, false);
            Generate.ProtectCd.Set((float)Generate.ProtectCd.Value + change, false);
            Generate.VestCd.Set((float)Generate.VestCd.Value + change, false);
            Generate.DouseCooldown.Set((float)Generate.DouseCooldown.Value + change, false);
            Generate.InfectCooldown.Set((float)Generate.InfectCooldown.Value + change, false);
            Generate.PestKillCooldown.Set((float)Generate.PestKillCooldown.Value + change, false);
            Generate.MimicCooldownOption.Set((float)Generate.MimicCooldownOption.Value + change, false);
            Generate.HackCooldownOption.Set((float)Generate.HackCooldownOption.Value + change, false);
            Generate.GlitchKillCooldownOption.Set((float)Generate.GlitchKillCooldownOption.Value + change, false);
            Generate.JuggKillCooldownOption.Set((float)Generate.JuggKillCooldownOption.Value + change, false);
            Generate.BloodlustCooldown.Set((float)Generate.BloodlustCooldown.Value + change, false);
            Generate.GrenadeCooldown.Set((float)Generate.GrenadeCooldown.Value + change, false);
            Generate.MorphlingCooldown.Set((float)Generate.MorphlingCooldown.Value + change, false);
            Generate.InvisCooldown.Set((float)Generate.InvisCooldown.Value + change, false);
            Generate.PoisonCooldown.Set((float)Generate.PoisonCooldown.Value + change, false);
            Generate.MineCooldown.Set((float)Generate.MineCooldown.Value + change, false);
            Generate.DragCooldown.Set((float)Generate.DragCooldown.Value + change, false);
            Generate.JanitorCleanCd.Set((float)Generate.JanitorCleanCd.Value + change, false);
            Generate.DisguiseCooldown.Set((float)Generate.DisguiseCooldown.Value + change, false);
            Generate.GazeCooldown.Set((float)Generate.GazeCooldown.Value + change, false);
            Generate.IgniteCooldown.Set((float)Generate.IgniteCooldown.Value + change, false);
            Generate.RevealCooldown.Set((float)Generate.RevealCooldown.Value + change, false);
            
            GameOptionsManager.Instance.currentNormalGameOptions.KillCooldown += change;

            if (change % 5 != 0)
            {
                if (change > 0)
                    change -= 2.5f;
                else if (change < 0)
                    change += 2.5f;
            }

            GameOptionsManager.Instance.currentNormalGameOptions.EmergencyCooldown += (int)change;
            return;
        }
    }
}