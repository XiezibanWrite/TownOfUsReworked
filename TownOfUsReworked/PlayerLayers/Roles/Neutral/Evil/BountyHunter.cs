namespace TownOfUsReworked.PlayerLayers.Roles
{
    public class BountyHunter : Neutral
    {
        public PlayerControl TargetPlayer;
        public bool TargetKilled;
        public bool ColorHintGiven;
        public bool LetterHintGiven;
        public bool RoleHintGiven;
        public bool TargetFound;
        public DateTime LastChecked;
        public CustomButton GuessButton;
        public CustomButton HuntButton;
        public bool ButtonUsable => UsesLeft > 0;
        public bool Failed => TargetPlayer == null || (UsesLeft <= 0 && !TargetFound) || (!TargetKilled && (TargetPlayer.Data.IsDead || TargetPlayer.Data.Disconnected));
        public int UsesLeft;
        private int LettersGiven;
        private bool LettersExhausted;
        private readonly List<string> Letters = new();
        public bool CanHunt => (TargetFound && !TargetPlayer.Data.IsDead && !TargetPlayer.Data.Disconnected) || (TargetKilled && !CustomGameOptions.AvoidNeutralKingmakers);

        public BountyHunter(PlayerControl player) : base(player)
        {
            Name = "Bounty Hunter";
            StartText = () => "Find And Kill Your Target";
            Objectives = () => TargetKilled ? "- You have completed the bounty" : "- Find and kill your target";
            AbilitiesText = () => "- You can guess a player to be your bounty\n- Upon finding the bounty, you can kill them\n- After your bounty has been killed by you, you can kill " +
                "others as many times as you want\n- If your target dies not by your hands, you will become a <color=#678D36FF>Troll</color>";
            Color = CustomGameOptions.CustomNeutColors ? Colors.BountyHunter : Colors.Neutral;
            RoleType = RoleEnum.BountyHunter;
            RoleAlignment = RoleAlignment.NeutralEvil;
            UsesLeft = CustomGameOptions.BountyHunterGuesses;
            Type = LayerEnum.BountyHunter;
            TargetPlayer = null;
            GuessButton = new(this, "BHGuess", AbilityTypes.Direct, "Secondary", Guess, true);
            HuntButton = new(this, "Hunt", AbilityTypes.Direct, "ActionSecondary", Hunt);
            InspectorResults = InspectorResults.TracksOthers;

            if (TownOfUsReworked.IsTest)
                Utils.LogSomething($"{Player.name} is {Name}");
        }

        public float CheckTimer()
        {
            var timespan = DateTime.UtcNow - LastChecked;
            var num = Player.GetModifiedCooldown(CustomGameOptions.BountyHunterCooldown) * 1000f;
            var flag2 = num - (float)timespan.TotalMilliseconds < 0f;
            return flag2 ? 0f : (num - (float)timespan.TotalMilliseconds) / 1000f;
        }

        public void TurnTroll()
        {
            var newRole = new Troll(Player);
            newRole.RoleUpdate(this);

            if (Local && !IntroCutscene.Instance)
                Utils.Flash(Colors.Troll);

            if (CustomPlayer.Local.Is(RoleEnum.Seer) && !IntroCutscene.Instance)
                Utils.Flash(Colors.Seer);
        }

        public override void OnMeetingStart(MeetingHud __instance)
        {
            base.OnMeetingStart(__instance);
            var targetName = TargetPlayer.name;
            var something = "";

            if (!LettersExhausted)
            {
                var random = URandom.RandomRangeInt(0, targetName.Length);
                var random2 = URandom.RandomRangeInt(0, targetName.Length);
                var random3 = URandom.RandomRangeInt(0, targetName.Length);

                if (LettersGiven <= targetName.Length - 3)
                {
                    while (random == random2 || random2 == random3 || random == random3 || Letters.Contains($"{targetName[random]}") || Letters.Contains($"{targetName[random2]}") ||
                        Letters.Contains($"{targetName[random3]}"))
                    {
                        if (random == random2 || Letters.Contains($"{targetName[random2]}"))
                            random2 = URandom.RandomRangeInt(0, targetName.Length);

                        if (random2 == random3 || Letters.Contains($"{targetName[random3]}"))
                            random3 = URandom.RandomRangeInt(0, targetName.Length);

                        if (random == random3 || Letters.Contains($"{targetName[random]}"))
                            random = URandom.RandomRangeInt(0, targetName.Length);
                    }

                    something = $"Your target's name has the Letters {targetName[random]}, {targetName[random2]} and {targetName[random3]} in it!";
                }
                else if (LettersGiven == targetName.Length - 2)
                {
                    while (random == random2 || Letters.Contains($"{targetName[random]}") || Letters.Contains($"{targetName[random2]}"))
                    {
                        if (Letters.Contains($"{targetName[random2]}"))
                            random2 = URandom.RandomRangeInt(0, targetName.Length);

                        if (Letters.Contains($"{targetName[random]}"))
                            random = URandom.RandomRangeInt(0, targetName.Length);

                        if (random == random2)
                            random = URandom.RandomRangeInt(0, targetName.Length);
                    }

                    something = $"Your target's name has the Letters {targetName[random]} and {targetName[random2]} in it!";
                }
                else if (LettersGiven == targetName.Length - 1)
                {
                    while (Letters.Contains($"{targetName[random]}"))
                        random = URandom.RandomRangeInt(0, targetName.Length);

                    something = $"Your target's name has the letter {targetName[random]} in it!";
                }
                else if (LettersGiven == targetName.Length && !LettersExhausted)
                    LettersExhausted = true;

                if (!LettersExhausted)
                {
                    if (LettersGiven <= targetName.Length - 3)
                    {
                        Letters.Add($"{targetName[random]}");
                        Letters.Add($"{targetName[random2]}");
                        Letters.Add($"{targetName[random3]}");
                        LettersGiven += 3;
                    }
                    else if (LettersGiven == targetName.Length - 2)
                    {
                        Letters.Add($"{targetName[random]}");
                        Letters.Add($"{targetName[random2]}");
                        LettersGiven += 2;
                    }
                    else if (LettersGiven == targetName.Length - 1)
                    {
                        Letters.Add($"{targetName[random]}");
                        LettersGiven++;
                    }

                    LetterHintGiven = true;
                }
                else if (!ColorHintGiven)
                {
                    something = $"Your target is a {ColorUtils.LightDarkColors[TargetPlayer.CurrentOutfit.ColorId].ToLower()} color!";
                    ColorHintGiven = true;
                }
                else if (!RoleHintGiven)
                {
                    something = $"Your target is the {GetRole(TargetPlayer)}!";
                    RoleHintGiven = true;
                }
            }

            if (string.IsNullOrEmpty(something))
                return;

            //Ensures only the Bounty Hunter sees this
            if (Utils.HUD && something != "")
                Utils.HUD.Chat.AddChat(PlayerControl.LocalPlayer, something);
        }

        public override void UpdateHud(HudManager __instance)
        {
            base.UpdateHud(__instance);
            GuessButton.Update("GUESS", CheckTimer(), CustomGameOptions.BountyHunterCooldown, UsesLeft, true, !TargetFound);
            HuntButton.Update("HUNT", CheckTimer(), CustomGameOptions.BountyHunterCooldown, true, CanHunt);

            if (Failed && !IsDead)
            {
                var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.Change, SendOption.Reliable);
                writer.Write((byte)TurnRPC.TurnTroll);
                writer.Write(PlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                TurnTroll();
            }
        }

        public void Guess()
        {
            if (Utils.IsTooFar(Player, GuessButton.TargetPlayer) || CheckTimer() != 0f)
                return;

            if (GuessButton.TargetPlayer != TargetPlayer)
            {
                Utils.Flash(new(255, 0, 0, 255));
                UsesLeft--;
            }
            else
            {
                TargetFound = true;
                Utils.Flash(new(0, 255, 0, 255));
            }

            LastChecked = DateTime.UtcNow;
        }

        public void Hunt()
        {
            if (HuntButton.TargetPlayer != TargetPlayer && !TargetKilled)
            {
                Utils.Flash(new(255, 0, 0, 255));
                LastChecked = DateTime.UtcNow;
            }
            else if (HuntButton.TargetPlayer == TargetPlayer && !TargetKilled)
            {
                var interact = Utils.Interact(Player, HuntButton.TargetPlayer, true);

                if (!interact[3])
                    Utils.RpcMurderPlayer(Player, HuntButton.TargetPlayer);

                var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.WinLose, SendOption.Reliable);
                writer.Write((byte)WinLoseRPC.BountyHunterWin);
                writer.Write(PlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                TargetKilled = true;
                LastChecked = DateTime.UtcNow;
            }
            else
            {
                var interact = Utils.Interact(Player, HuntButton.TargetPlayer, true);

                if (interact[0] || interact[3])
                    LastChecked = DateTime.UtcNow;
                else if (interact[1])
                    LastChecked.AddSeconds(CustomGameOptions.ProtectKCReset);
                else if (interact[2])
                    LastChecked.AddSeconds(CustomGameOptions.VestKCReset);
            }
        }
    }
}