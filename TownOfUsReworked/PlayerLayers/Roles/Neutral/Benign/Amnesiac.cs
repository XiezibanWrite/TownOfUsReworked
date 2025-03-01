namespace TownOfUsReworked.PlayerLayers.Roles
{
    public class Amnesiac : Neutral
    {
        public Dictionary<byte, CustomArrow> BodyArrows = new();
        public CustomButton RememberButton;

        public Amnesiac(PlayerControl player) : base(player)
        {
            Name = "Amnesiac";
            StartText = () => "You Forgor :skull:";
            AbilitiesText = () => "- You can copy over a player's role should you find their body" + (CustomGameOptions.RememberArrows ? "\n- When someone dies, you get an arrow pointing "
                + "to their body" : "");
            RoleType = RoleEnum.Amnesiac;
            RoleAlignment = RoleAlignment.NeutralBen;
            Color = CustomGameOptions.CustomNeutColors ? Colors.Amnesiac : Colors.Neutral;
            Objectives = () => "- Find a dead body, remember their role and then fulfill the win condition for that role\n- If there are less than 7 players alive, you will become a " +
                "<color=#80FF00FF>Thief</color>";
            BodyArrows = new();
            InspectorResults = InspectorResults.DealsWithDead;
            Type = LayerEnum.Amnesiac;
            RememberButton = new(this, "Remember", AbilityTypes.Dead, "ActionSecondary", Remember);

            if (TownOfUsReworked.IsTest)
                Utils.LogSomething($"{Player.name} is {Name}");
        }

        public void DestroyArrow(byte targetPlayerId)
        {
            BodyArrows.FirstOrDefault(x => x.Key == targetPlayerId).Value?.Destroy();
            BodyArrows.Remove(targetPlayerId);
        }

        public override void OnLobby()
        {
            base.OnLobby();
            BodyArrows.Values.ToList().DestroyAll();
            BodyArrows.Clear();
        }

        public void TurnThief()
        {
            var newRole = new Thief(Player);
            newRole.RoleUpdate(this);

            if (Local && !IntroCutscene.Instance)
                Utils.Flash(Colors.Thief);

            if (CustomPlayer.Local.Is(RoleEnum.Seer) && !IntroCutscene.Instance)
                Utils.Flash(Colors.Seer);
        }

        public void Remember()
        {
            if (Utils.IsTooFar(Player, RememberButton.TargetBody))
                return;

            var playerId = RememberButton.TargetBody.ParentId;
            var player = Utils.PlayerById(playerId);
            Utils.Spread(Player, player);
            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.Action, SendOption.Reliable);
            writer.Write((byte)ActionsRPC.Remember);
            writer.Write(PlayerId);
            writer.Write(playerId);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            Remember(this, player);
        }

        public static void Remember(Amnesiac amneRole, PlayerControl other)
        {
            var role = GetRole(other);
            var amnesiac = amneRole.Player;
            var target = other.GetTarget();
            var leader = other.GetLeader();
            var actor = other.GetActorList();

            if (CustomPlayer.Local == amnesiac)
            {
                foreach (var component in amneRole.RememberButton.TargetBody?.bodyRenderers)
                    component.material.SetFloat("_Outline", 0f);

                Utils.Flash(amneRole.Color);
                amneRole.OnLobby();
                ButtonUtils.ResetCustomTimers(false);
            }

            if (CustomPlayer.Local == other)
            {
                Utils.Flash(amneRole.Color);
                role.OnLobby();
                ButtonUtils.ResetCustomTimers(false);
            }

            Role newRole = role.RoleType switch
            {
                RoleEnum.Anarchist => new Anarchist(amnesiac),
                RoleEnum.Arsonist => new Arsonist(amnesiac) { Doused = ((Arsonist)role).Doused },
                RoleEnum.Blackmailer => new Blackmailer(amnesiac) { BlackmailedPlayer = ((Blackmailer)role).BlackmailedPlayer },
                RoleEnum.Bomber => new Bomber(amnesiac),
                RoleEnum.Camouflager => new Camouflager(amnesiac),
                RoleEnum.Cannibal => new Cannibal(amnesiac) { EatNeed = ((Cannibal)role).EatNeed },
                RoleEnum.Enforcer => new Enforcer(amnesiac),
                RoleEnum.Concealer => new Concealer(amnesiac),
                RoleEnum.Consigliere => new Consigliere(amnesiac) { Investigated = ((Consigliere)role).Investigated },
                RoleEnum.Consort => new Consort(amnesiac),
                RoleEnum.Crewmate => new Crewmate(amnesiac),
                RoleEnum.Cryomaniac => new Cryomaniac(amnesiac) { Doused = ((Cryomaniac)role).Doused },
                RoleEnum.Detective => new Detective(amnesiac),
                RoleEnum.Disguiser => new Disguiser(amnesiac),
                RoleEnum.Dracula => new Dracula(amnesiac) { Converted = ((Dracula)role).Converted },
                RoleEnum.Escort => new Escort(amnesiac),
                RoleEnum.Executioner => new Executioner(amnesiac) { TargetPlayer = target },
                RoleEnum.Framer => new Framer(amnesiac) { Framed = ((Framer)role).Framed },
                RoleEnum.Glitch => new Glitch(amnesiac),
                RoleEnum.Godfather => new Godfather(amnesiac),
                RoleEnum.PromotedGodfather => new PromotedGodfather(amnesiac)
                {
                    Investigated = ((PromotedGodfather)role).Investigated,
                    BlackmailedPlayer = ((PromotedGodfather)role).BlackmailedPlayer
                },
                RoleEnum.Grenadier => new Grenadier(amnesiac),
                RoleEnum.GuardianAngel => new GuardianAngel(amnesiac) { TargetPlayer = target },
                RoleEnum.Impostor => new Impostor(amnesiac),
                RoleEnum.Jackal => new Jackal(amnesiac)
                {
                    Recruited = ((Jackal)role).Recruited,
                    EvilRecruit = ((Jackal)role).EvilRecruit,
                    GoodRecruit = ((Jackal)role).GoodRecruit,
                    BackupRecruit = ((Jackal)role).BackupRecruit
                },
                RoleEnum.Jester => new Jester(amnesiac),
                RoleEnum.Juggernaut => new Juggernaut(amnesiac) { JuggKills = ((Juggernaut)role).JuggKills },
                RoleEnum.Sheriff => new Sheriff(amnesiac),
                RoleEnum.Mafioso => new Mafioso(amnesiac) { Godfather = (Godfather)leader },
                RoleEnum.Miner => new Miner(amnesiac),
                RoleEnum.Morphling => new Morphling(amnesiac),
                RoleEnum.Medic => new Medic(amnesiac),
                RoleEnum.Medium => new Medium(amnesiac),
                RoleEnum.Shifter => new Shifter(amnesiac),
                RoleEnum.Rebel => new Rebel(amnesiac),
                RoleEnum.PromotedRebel => new PromotedRebel(amnesiac) { Framed = ((PromotedRebel)role).Framed },
                RoleEnum.Sidekick => new Sidekick(amnesiac) { Rebel = (Rebel)leader },
                RoleEnum.Shapeshifter => new Shapeshifter(amnesiac),
                RoleEnum.Murderer => new Murderer(amnesiac),
                RoleEnum.Survivor => new Survivor(amnesiac) { UsesLeft = ((Survivor)role).UsesLeft },
                RoleEnum.Plaguebearer => new Plaguebearer(amnesiac) { Infected = ((Plaguebearer)role).Infected },
                RoleEnum.Pestilence => new Pestilence(amnesiac),
                RoleEnum.SerialKiller => new SerialKiller(amnesiac),
                RoleEnum.Werewolf => new Werewolf(amnesiac),
                RoleEnum.Janitor => new Janitor(amnesiac),
                RoleEnum.Poisoner => new Poisoner(amnesiac),
                RoleEnum.Teleporter => new Teleporter(amnesiac) { TeleportPoint = ((Teleporter)role).TeleportPoint },
                RoleEnum.Troll => new Troll(amnesiac),
                RoleEnum.Thief => new Thief(amnesiac),
                RoleEnum.VampireHunter => new VampireHunter(amnesiac),
                RoleEnum.Warper => new Warper(amnesiac),
                RoleEnum.Wraith => new Wraith(amnesiac),
                RoleEnum.Mystic => new Mystic(amnesiac),
                RoleEnum.Dictator => new Dictator(amnesiac),
                RoleEnum.Seer => new Seer(amnesiac),
                RoleEnum.Actor => new Actor(amnesiac) { TargetRole = actor },
                RoleEnum.BountyHunter => new BountyHunter(amnesiac) { TargetPlayer = target },
                RoleEnum.Guesser => new Guesser(amnesiac) { TargetPlayer = target },
                RoleEnum.Necromancer => new Necromancer(amnesiac)
                {
                    Resurrected = ((Necromancer)role).Resurrected,
                    KillCount = ((Necromancer)role).KillCount,
                    ResurrectedCount = ((Necromancer)role).ResurrectedCount
                },
                RoleEnum.Whisperer => new Whisperer(amnesiac)
                {
                    Persuaded = ((Whisperer)role).Persuaded,
                    WhisperCount = ((Whisperer)role).WhisperCount,
                    WhisperConversion = ((Whisperer)role).WhisperConversion
                },
                RoleEnum.Betrayer => new Betrayer(amnesiac) { Faction = role.Faction },
                RoleEnum.Ambusher => new Ambusher(amnesiac),
                RoleEnum.Crusader => new Crusader(amnesiac),
                RoleEnum.Altruist => new Altruist(amnesiac) { UsesLeft = ((Altruist)role).UsesLeft },
                RoleEnum.Engineer => new Engineer(amnesiac) { UsesLeft = ((Engineer)role).UsesLeft },
                RoleEnum.Inspector => new Inspector(amnesiac) { Inspected = ((Inspector)role).Inspected },
                RoleEnum.Tracker => new Tracker(amnesiac)
                {
                    TrackerArrows = ((Tracker)role).TrackerArrows,
                    UsesLeft = ((Tracker)role).UsesLeft
                },
                RoleEnum.Stalker => new Stalker(amnesiac) { StalkerArrows = ((Stalker)role).StalkerArrows },
                RoleEnum.Transporter => new Transporter(amnesiac) { UsesLeft = ((Transporter)role).UsesLeft },
                RoleEnum.Mayor => new Mayor(amnesiac) { Revealed = ((Mayor)role).Revealed },
                RoleEnum.Operative => new Operative(amnesiac) { UsesLeft = ((Operative)role).UsesLeft },
                RoleEnum.Veteran => new Veteran(amnesiac) { UsesLeft = ((Veteran)role).UsesLeft },
                RoleEnum.Vigilante => new Vigilante(amnesiac) { UsesLeft = ((Vigilante)role).UsesLeft },
                RoleEnum.Chameleon => new Chameleon(amnesiac) { UsesLeft = ((Chameleon)role).UsesLeft },
                RoleEnum.Coroner => new Coroner(amnesiac)
                {
                    ReferenceBodies = ((Coroner)role).ReferenceBodies,
                    Reported = ((Coroner)role).Reported
                },
                RoleEnum.Monarch => new Monarch(amnesiac)
                {
                    UsesLeft = ((Monarch)role).UsesLeft,
                    ToBeKnighted = ((Monarch)role).ToBeKnighted,
                    Knighted = ((Monarch)role).Knighted
                },
                RoleEnum.Retributionist => new Retributionist(amnesiac)
                {
                    TrackerArrows = ((Retributionist)role).TrackerArrows,
                    Inspected = ((Retributionist)role).Inspected,
                    Selected = ((Retributionist)role).Selected,
                    UsesLeft = ((Retributionist)role).UsesLeft,
                    Reported = ((Retributionist)role).Reported,
                    ReferenceBodies = ((Retributionist)role).ReferenceBodies
                },
                _ => new Amnesiac(amnesiac),
            };

            newRole.RoleUpdate(amneRole);

            if (other.Is(RoleEnum.Dracula))
                ((Dracula)role).Converted.Clear();
            else if (other.Is(RoleEnum.Whisperer))
                ((Whisperer)role).Persuaded.Clear();
            else if (other.Is(RoleEnum.Necromancer))
                ((Necromancer)role).Resurrected.Clear();
            else if (other.Is(RoleEnum.Jackal))
            {
                ((Jackal)role).Recruited.Clear();
                ((Jackal)role).EvilRecruit = null;
                ((Jackal)role).GoodRecruit = null;
                ((Jackal)role).BackupRecruit = null;
            }

            amnesiac.Data.SetImpostor(amnesiac.Is(Faction.Intruder) || (amnesiac.Is(Faction.Syndicate) && CustomGameOptions.AltImps));

            if (amnesiac.Is(Faction.Intruder) || amnesiac.Is(Faction.Syndicate) || (amnesiac.Is(Faction.Neutral) && CustomGameOptions.SnitchSeesNeutrals))
            {
                foreach (var snitch in Ability.GetAbilities<Snitch>(AbilityEnum.Snitch))
                {
                    if (snitch.TasksLeft <= CustomGameOptions.SnitchTasksRemaining && CustomPlayer.Local == amnesiac)
                        LocalRole.AllArrows.Add(snitch.PlayerId, new(amnesiac, Colors.Snitch, 0));
                    else if (snitch.TasksDone && CustomPlayer.Local == snitch.Player)
                        GetRole(snitch.Player).AllArrows.Add(amnesiac.PlayerId, new(snitch.Player, Colors.Snitch));
                }

                foreach (var revealer in GetRoles<Revealer>(RoleEnum.Revealer))
                {
                    if (revealer.Revealed && CustomPlayer.Local == amnesiac)
                        LocalRole.AllArrows.Add(revealer.PlayerId, new(amnesiac, Colors.Revealer, 0));
                }
            }

            if (CustomPlayer.Local == amnesiac)
                ButtonUtils.ResetCustomTimers(false);
        }

        public override void UpdateHud(HudManager __instance)
        {
            base.UpdateHud(__instance);
            RememberButton.Update("REMEMBER");

            if (CustomGameOptions.RememberArrows && !CustomPlayer.LocalCustom.IsDead)
            {
                var validBodies = Utils.AllBodies.Where(x => Utils.KilledPlayers.Any(y => y.PlayerId == x.ParentId &&
                    y.KillTime.AddSeconds(CustomGameOptions.RememberArrowDelay) < System.DateTime.UtcNow));

                foreach (var bodyArrow in BodyArrows.Keys)
                {
                    if (!validBodies.Any(x => x.ParentId == bodyArrow))
                        DestroyArrow(bodyArrow);
                }

                foreach (var body in validBodies)
                {
                    if (!BodyArrows.ContainsKey(body.ParentId))
                        BodyArrows.Add(body.ParentId, new(Player, Color));

                    BodyArrows[body.ParentId]?.Update(body.TruePosition);
                }
            }
            else if (BodyArrows.Count != 0 || CustomPlayer.AllPlayers.Count <= 4)
                OnLobby();

            if (CustomPlayer.AllPlayers.Count <= 4 && !IsDead)
            {
                var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.Change, SendOption.Reliable);
                writer.Write((byte)TurnRPC.TurnThief);
                writer.Write(PlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                TurnThief();
            }
        }
    }
}