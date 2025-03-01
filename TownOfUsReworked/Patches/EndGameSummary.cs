namespace TownOfUsReworked.Patches
{
    [HarmonyPatch]
    public static class Summary
    {
        private static readonly List<PlayerInfo> PlayerRoles = new();
        public static readonly List<PlayerInfo> Disconnected = new();

        public class PlayerInfo
        {
            public string PlayerName;
            public string History;
            public string CachedHistory;

            public PlayerInfo(string name, string history, string cache)
            {
                PlayerName = name;
                History = history;
                CachedHistory = cache;
            }
        }

        public static void AddSummaryInfo(PlayerControl player, bool disconnected = false)
        {
            var summary = "";
            var cache = "";

            var info = player.AllPlayerInfo();
            var role = info[0] as Role;
            var modifier = info[1] as Modifier;
            var ability = info[2] as Ability;
            var objectifier = info[3] as Objectifier;

            if (info[0])
            {
                if (role.RoleHistory.Count != 0)
                {
                    role.RoleHistory.Reverse();

                    foreach (var role2 in role.RoleHistory)
                    {
                        summary += $"{role2.ColorString}{role2.Name}</color> → ";
                        cache += $"{role2.Name} → ";
                    }
                }

                summary += $"{role?.ColorString}{role?.Name}</color>";
                cache += role.Name;

                if (role.SubFaction != SubFaction.None && !player.Is(RoleAlignment.NeutralNeo))
                {
                    summary += $" {role?.SubFactionColorString}{role?.SubFactionSymbol}</color>";
                    cache += $" {role?.SubFactionSymbol}";
                }
            }

            if (objectifier?.ObjectifierType != ObjectifierEnum.None)
            {
                summary += $" {objectifier?.ColoredSymbol}";
                cache += $" {objectifier?.Symbol}";
            }

            if (modifier?.ModifierType != ModifierEnum.None)
            {
                summary += $" ({modifier?.ColorString}{modifier?.Name}</color>)";
                cache += $" ({modifier?.Name})";
            }

            if (ability?.AbilityType != AbilityEnum.None)
            {
                summary += $" [{ability?.ColorString}{ability?.Name}</color>]";
                cache += $" [{ability?.Name}]";
            }

            if (player.IsGATarget())
            {
                summary += " <color=#FFFFFFFF>★</color>";
                cache += " ★";
            }

            if (player.IsExeTarget())
            {
                summary += " <color=#CCCCCCFF>§</color>";
                cache += " §";
            }

            if (player.IsBHTarget())
            {
                summary += " <color=#B51E39FF>Θ</color>";
                cache += " Θ";
            }

            if (player.IsGuessTarget())
            {
                summary += " <color=#EEE5BEFF>π</color>";
                cache += " π";
            }

            if (player == Role.DriveHolder && !CustomGameOptions.GlobalDrive)
            {
                summary += " <color=#008000FF>Δ</color>";
                cache += " Δ";
            }

            if (player.CanDoTasks())
            {
                summary += $" <{role.TasksCompleted}/{role.TotalTasks}>";
                cache += $" <{role.TasksCompleted}/{role.TotalTasks}>";
            }

            if (!disconnected)
            {
                summary += player.DeathReason();
                cache += player.DeathReason();
                PlayerRoles.Add(new(player.Data.PlayerName, summary, cache));
            }
            else
                Disconnected.Add(new(player.Data.PlayerName, summary, cache));
        }

        [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.OnGameEnd))]
        public static class EndGameSummary
        {
            public static void Postfix()
            {
                PlayerRoles.Clear();
                //There's a better way of doing this e.g. switch statement or dictionary. But this works for now.
                //AD says "Done".
                CustomPlayer.AllPlayers.ForEach(x => AddSummaryInfo(x));
            }
        }

        [HarmonyPatch(typeof(EndGameManager), nameof(EndGameManager.SetEverythingUp))]
        public static class ShipStatusSetUpPatch
        {
            public static void Postfix(EndGameManager __instance)
            {
                if (ConstantVariables.IsHnS)
                    return;

                var position = Camera.main.ViewportToWorldPoint(new(0f, 1f, Camera.main.nearClipPlane));
                var roleSummary = UObject.Instantiate(__instance.WinText.gameObject);
                roleSummary.transform.position = new(__instance.Navigation.ExitButton.transform.position.x + 0.1f, position.y - 0.1f, -14f);
                roleSummary.transform.localScale = new(1f, 1f, 1f);

                var roleSummaryText = new StringBuilder();
                var roleSummaryCache = new StringBuilder();
                var winnersText = new StringBuilder();
                var winnersCache = new StringBuilder();
                var losersText = new StringBuilder();
                var losersCache = new StringBuilder();
                var discText = new StringBuilder();
                var discCache = new StringBuilder();

                var winnerCount = 0;
                var loserCount = 0;
                var discCount = 0;

                roleSummaryText.AppendLine("<size=125%><u><b>End Game Summary</b></u>:</size>");
                roleSummaryText.AppendLine();
                roleSummaryCache.AppendLine("End Game Summary:");
                roleSummaryCache.AppendLine();
                winnersText.AppendLine("<size=105%><b>Winners</b></size>");
                losersText.AppendLine("<size=105%><b>Losers</b></size>");
                discText.AppendLine("<size=105%><b>Disconnected</b></size>");
                winnersCache.AppendLine("Winners");
                losersCache.AppendLine("Losers");
                discCache.AppendLine("Disconnected");

                foreach (var data in PlayerRoles)
                {
                    var dataString = $"<size=75%>{data.PlayerName} - {data.History}</size>";
                    var dataCache = $"{data.PlayerName} - {data.CachedHistory}";

                    if (data.PlayerName.IsWinner())
                    {
                        winnersText.AppendLine(dataString);
                        winnersCache.AppendLine(dataCache);
                        winnerCount++;
                    }
                    else
                    {
                        losersText.AppendLine(dataString);
                        losersCache.AppendLine(dataCache);
                        loserCount++;
                    }
                }

                foreach (var data in Disconnected)
                {
                    var dataString = $"<size=75%>{data.PlayerName} - {data.History}</size>";
                    var dataCache = $"{data.PlayerName} - {data.CachedHistory}";
                    discText.AppendLine(dataString);
                    discCache.AppendLine(dataCache);
                    discCount++;
                }

                if (winnerCount == 0)
                {
                    winnersText.AppendLine("<size=75%>No One Won</size>");
                    winnersCache.AppendLine("No One Won");
                }

                if (loserCount == 0)
                {
                    losersText.AppendLine("<size=75%>No One Lost</size>");
                    losersCache.AppendLine("No One Lost");
                }

                roleSummaryText.Append(winnersText);
                roleSummaryText.AppendLine();
                roleSummaryText.Append(losersText);
                roleSummaryCache.Append(winnersCache);
                roleSummaryCache.AppendLine();
                roleSummaryCache.Append(losersCache);

                if (discCount > 0)
                {
                    roleSummaryText.AppendLine();
                    roleSummaryText.Append(discText);
                    roleSummaryCache.AppendLine();
                    roleSummaryCache.Append(discCache);
                }

                var roleSummaryTextMesh = roleSummary.GetComponent<TMP_Text>();
                roleSummaryTextMesh.alignment = TextAlignmentOptions.TopLeft;
                roleSummaryTextMesh.color = Color.white;
                roleSummaryTextMesh.fontSizeMin = 1.5f;
                roleSummaryTextMesh.fontSizeMax = 1.5f;
                roleSummaryTextMesh.fontSize = 1.5f;
                roleSummaryTextMesh.text = $"{roleSummaryText}";
                roleSummaryTextMesh.GetComponent<RectTransform>().anchoredPosition = new(position.x + 3.5f, position.y - 0.1f);

                var text = Path.Combine(Application.persistentDataPath, "Summary-temp");

                try
                {
                    File.WriteAllText(text, roleSummaryCache.ToString());
                    var text2 = Path.Combine(Application.persistentDataPath, "Summary");
                    File.Delete(text2);
                    File.Move(text, text2);
                } catch {}

                PlayerRoles.Clear();
                Disconnected.Clear();

                SpawnInMinigamePatch.ResetGlobalVariable();
            }
        }

        private static bool IsWinner(this string playerName) => TempData.winners.Any(x => x.PlayerName == playerName);

        private static string DeathReason(this PlayerControl player)
        {
            if (player == null)
                return "";

            var role = Role.GetRole(player);

            if (role == null)
                return "";

            var die = role.DeathReason is not DeathReasonEnum.Alive ? $" | {role.DeathReason}" : "";
            var killedBy = "";

            if (role.DeathReason is not DeathReasonEnum.Alive and not DeathReasonEnum.Ejected and not DeathReasonEnum.Suicide and not DeathReasonEnum.Escaped)
                killedBy = role.KilledBy;

            return die + killedBy;
        }
    }
}