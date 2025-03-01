namespace TownOfUsReworked.Extensions
{
    [HarmonyPatch]
    public static class LayerExtentions
    {
        public readonly static List<WinningPlayerData> PotentialWinners = new();
        public static string RoleColorString => $"<color=#{Colors.Role.ToHtmlStringRGBA()}>";
        public static string AlignmentColorString => $"<color=#{Colors.Alignment.ToHtmlStringRGBA()}>";
        public static string ObjectivesColorString => $"<color=#{Colors.Objectives.ToHtmlStringRGBA()}>";
        public static string AttributesColorString => $"<color=#{Colors.Attributes.ToHtmlStringRGBA()}>";
        public static string AbilitiesColorString => $"<color=#{Colors.Abilities.ToHtmlStringRGBA()}>";
        public static string ObjectifierColorString => $"<color=#{Colors.Objectifier.ToHtmlStringRGBA()}>";
        public static string ModifierColorString => $"<color=#{Colors.Modifier.ToHtmlStringRGBA()}>";
        public static string AbilityColorString => $"<color=#{Colors.Ability.ToHtmlStringRGBA()}>";
        public static string SubFactionColorString => $"<color=#{Colors.SubFaction.ToHtmlStringRGBA()}>";

        public static bool Is(this PlayerControl player, RoleEnum roleType) => Role.GetRole(player)?.RoleType == roleType;

        public static bool Is(this PlayerControl player, LayerEnum type, PlayerLayerEnum layer) => PlayerLayer.AllLayers.Find(x => x.Player == player && x.Type == type && x.LayerType ==
            layer);

        public static bool Is(this Role role, RoleEnum roleType) => role?.RoleType == roleType;

        public static bool Is(this Objectifier obj, ObjectifierEnum objectifiertype) => obj?.ObjectifierType == objectifiertype;

        public static bool Is(this PlayerControl player, Role role) => Role.GetRole(player).Player == role.Player;

        public static bool Is(this PlayerControl player, SubFaction subFaction) => Role.GetRole(player)?.SubFaction == subFaction;

        public static bool Is(this PlayerControl player, ModifierEnum modifierType) => Modifier.GetModifier(player)?.ModifierType == modifierType;

        public static bool Is(this PlayerControl player, ObjectifierEnum objectifiertype) => Objectifier.GetObjectifier(player)?.ObjectifierType == objectifiertype;

        public static bool Is(this PlayerControl player, AbilityEnum abilityType) => Ability.GetAbility(player)?.AbilityType == abilityType;

        public static bool Is(this PlayerControl player, Faction faction) => player.GetFaction() == faction;

        public static bool Is(this PlayerControl player, RoleAlignment alignment) => Role.GetRole(player)?.RoleAlignment == alignment;

        public static bool Is(this PlayerVoteArea player, RoleEnum roleType) => Utils.PlayerByVoteArea(player).Is(roleType);

        public static bool Is(this PlayerVoteArea player, Role role) => Utils.PlayerByVoteArea(player).Is(role);

        public static bool Is(this PlayerVoteArea player, SubFaction subFaction) => Utils.PlayerByVoteArea(player).Is(subFaction);

        public static bool Is(this PlayerVoteArea player, ModifierEnum modifierType) => Utils.PlayerByVoteArea(player).Is(modifierType);

        public static bool Is(this PlayerVoteArea player, ObjectifierEnum abilityType) => Utils.PlayerByVoteArea(player).Is(abilityType);

        public static bool Is(this PlayerVoteArea player, AbilityEnum abilityType) => Utils.PlayerByVoteArea(player).Is(abilityType);

        public static bool Is(this PlayerVoteArea player, Faction faction) => Utils.PlayerByVoteArea(player).Is(faction);

        public static bool Is(this PlayerVoteArea player, RoleAlignment alignment) => Utils.PlayerByVoteArea(player).Is(alignment);

        public static Faction GetFaction(this PlayerControl player)
        {
            if (player == null)
                return Faction.None;

            var role = Role.GetRole(player);

            if (role == null)
                return player.Data.IsImpostor() ? Faction.Intruder : Faction.Crew;

            return role.Faction;
        }

        public static SubFaction GetSubFaction(this PlayerControl player)
        {
            if (player == null)
                return SubFaction.None;

            var role = Role.GetRole(player);

            if (role == null)
                return SubFaction.None;

            return role.SubFaction;
        }

        public static RoleEnum GetRole(this PlayerControl player)
        {
            if (player == null)
                return RoleEnum.None;

            var role = Role.GetRole(player);

            if (!role)
                return RoleEnum.None;

            return role.RoleType;
        }

        public static RoleEnum GetRole(this PlayerVoteArea player) => Utils.PlayerByVoteArea(player).GetRole();

        public static AbilityEnum GetAbility(this PlayerControl player)
        {
            if (player == null)
                return AbilityEnum.None;

            var ability = Ability.GetAbility(player);

            if (!ability)
                return AbilityEnum.None;

            return ability.AbilityType;
        }

        public static RoleAlignment GetAlignment(this PlayerControl player)
        {
            if (player == null)
                return RoleAlignment.None;

            var role = Role.GetRole(player);

            if (role == null)
                return RoleAlignment.None;

            return role.RoleAlignment;
        }

        public static RoleAlignment GetAlignment(this PlayerVoteArea player) => Utils.PlayerByVoteArea(player).GetAlignment();

        public static Faction GetFaction(this PlayerVoteArea player) => Utils.PlayerByVoteArea(player).GetFaction();

        public static SubFaction GetSubFaction(this PlayerVoteArea player) => Utils.PlayerByVoteArea(player).GetSubFaction();

        public static bool IsRecruit(this PlayerControl player) => Role.GetRole(player).IsRecruit;

        public static bool IsBitten(this PlayerControl player) => Role.GetRole(player).IsBitten;

        public static bool IsPersuaded(this PlayerControl player) => Role.GetRole(player).IsPersuaded;

        public static bool IsResurrected(this PlayerControl player) => Role.GetRole(player).IsResurrected;

        public static bool Diseased(this PlayerControl player) => Role.GetRole(player).Diseased;

        public static bool IsCrewDefect(this PlayerControl player) => Role.GetRole(player).IsCrewDefect;

        public static bool IsIntDefect(this PlayerControl player) => Role.GetRole(player).IsIntDefect;

        public static bool IsSynDefect(this PlayerControl player) => Role.GetRole(player).IsSynDefect;

        public static bool IsNeutDefect(this PlayerControl player) => Role.GetRole(player).IsNeutDefect;

        public static bool IsDefect(this PlayerControl player) => player.IsCrewDefect() || player.IsIntDefect() || player.IsSynDefect() || player.IsNeutDefect();

        public static bool IsRecruit(this PlayerVoteArea player) => Utils.PlayerByVoteArea(player).IsRecruit();

        public static bool IsBitten(this PlayerVoteArea player) => Utils.PlayerByVoteArea(player).IsBitten();

        public static bool IsPersuaded(this PlayerVoteArea player) => Utils.PlayerByVoteArea(player).IsPersuaded();

        public static bool IsResurrected(this PlayerVoteArea player) => Utils.PlayerByVoteArea(player).IsResurrected();

        public static bool NotOnTheSameSide(this PlayerControl player)
        {
            var traitorFlag = player.IsTurnedTraitor();
            var fanaticFlag = player.IsTurnedFanatic();
            var recruitFlag = player.IsRecruit();
            var bittenFlag = player.IsBitten();
            var sectFlag = player.IsPersuaded();
            var revivedFlag = player.IsResurrected();
            var rivalFlag = player.IsWinningRival();
            var corruptedFlag = player.Is(ObjectifierEnum.Corrupted);
            var loverFlag = player.HasAliveLover();
            var mafFlag = player.Is(ObjectifierEnum.Mafia);
            return traitorFlag || recruitFlag || sectFlag || revivedFlag || rivalFlag || fanaticFlag || corruptedFlag || bittenFlag || loverFlag || mafFlag ||
                !Role.GetRole(player).Faithful;
        }

        public static bool HasAliveLover(this PlayerControl player) => player.Is(ObjectifierEnum.Lovers) && Objectifier.GetObjectifier<Lovers>(player).LoversAlive;

        public static bool CanDoTasks(this PlayerControl player)
        {
            if (player == null)
                return false;

            if (Role.GetRole(player) == null)
                return !player.Data.IsImpostor();

            var crewflag = player.Is(Faction.Crew);
            var neutralflag = player.Is(Faction.Neutral);
            var intruderflag = player.Is(Faction.Intruder);
            var syndicateflag = player.Is(Faction.Syndicate);

            var phantomflag = player.Is(RoleEnum.Phantom);

            var sideflag = player.NotOnTheSameSide();
            var taskmasterflag = player.Is(ObjectifierEnum.Taskmaster);
            var defectflag = player.IsCrewDefect();

            var flag1 = crewflag && !sideflag;
            var flag2 = neutralflag && (taskmasterflag || phantomflag);
            var flag3 = intruderflag && (taskmasterflag || defectflag);
            var flag4 = syndicateflag && (taskmasterflag || defectflag);
            return flag1 || flag2 || flag3 || flag4;
        }

        public static bool IsMoving(this PlayerControl player) => Role.GetRoles<Transporter>(RoleEnum.Transporter).Any(x => (x.TransportPlayer1 == player || x.TransportPlayer2) &&
            x.Transporting) || Role.GetRoles<Retributionist>(RoleEnum.Retributionist).Any(x => (x.TransportPlayer1 == player || x.TransportPlayer2) && x.OnEffect) ||
            Role.GetRoles<Warper>(RoleEnum.Warper).Any(x => x.WarpPlayer1 == player && x.Warping) || Role.GetRoles<PromotedRebel>(RoleEnum.PromotedRebel).Any(x => x.WarpPlayer1 == player
            && x.OnEffect);

        public static bool IsGATarget(this PlayerControl player) => Role.GetRoles<GuardianAngel>(RoleEnum.GuardianAngel).Any(x => x.TargetPlayer == player);

        public static bool IsExeTarget(this PlayerControl player) => Role.GetRoles<Executioner>(RoleEnum.Executioner).Any(x => x.TargetPlayer == player);

        public static bool IsBHTarget(this PlayerControl player) => Role.GetRoles<BountyHunter>(RoleEnum.BountyHunter).Any(x => x.TargetPlayer == player);

        public static bool IsGuessTarget(this PlayerControl player) => Role.GetRoles<Guesser>(RoleEnum.Guesser).Any(x => x.TargetPlayer == player);

        public static PlayerControl GetTarget(this PlayerControl player)
        {
            var role = Role.GetRole(player);

            if (!role.HasTarget())
                return null;

            if (player.Is(RoleEnum.Executioner))
                return ((Executioner)role).TargetPlayer;
            else if (player.Is(RoleEnum.GuardianAngel))
                return ((GuardianAngel)role).TargetPlayer;
            else if (player.Is(RoleEnum.Guesser))
                return ((Guesser)role).TargetPlayer;
            else if (player.Is(RoleEnum.BountyHunter))
                return ((BountyHunter)role).TargetPlayer;

            return null;
        }

        public static Role GetLeader(this PlayerControl player)
        {
            if (!player.Is(RoleEnum.Mafioso) && !player.Is(RoleEnum.Sidekick))
                return null;

            var role = Role.GetRole(player);

            if (role == null)
                return null;

            if (player.Is(RoleEnum.Mafioso))
                return ((Mafioso)role).Godfather;
            else if (player.Is(RoleEnum.Sidekick))
                return ((Sidekick)role).Rebel;

            return null;
        }

        public static Role GetActorList(this PlayerControl player)
        {
            if (!player.Is(RoleEnum.Actor))
                return null;

            var role = Role.GetRole(player);

            if (role == null)
                return null;

            if (player.Is(RoleEnum.Actor))
                return ((Actor)role).TargetRole;

            return null;
        }

        public static bool IsGATarget(this PlayerVoteArea player) => Utils.PlayerByVoteArea(player).IsGATarget();

        public static bool IsExeTarget(this PlayerVoteArea player) => Utils.PlayerByVoteArea(player).IsExeTarget();

        public static bool IsBHTarget(this PlayerVoteArea player) => Utils.PlayerByVoteArea(player).IsBHTarget();

        public static bool IsGuessTarget(this PlayerVoteArea player) => Utils.PlayerByVoteArea(player).IsGuessTarget();

        public static bool IsTarget(this PlayerControl player) => player.IsBHTarget() || player.IsGuessTarget() || player.IsGATarget() || player.IsExeTarget();

        public static bool IsTarget(this PlayerVoteArea player) => player.IsBHTarget() || player.IsGuessTarget() || player.IsGATarget() || player.IsExeTarget();

        public static bool CanDoTasks(this PlayerVoteArea player) => Utils.PlayerByVoteArea(player).CanDoTasks();

        public static Jackal GetJackal(this PlayerControl player) => Role.GetRoles<Jackal>(RoleEnum.Jackal).Find(role => role.Recruited.Contains(player.PlayerId));

        public static Necromancer GetNecromancer(this PlayerControl player) => Role.GetRoles<Necromancer>(RoleEnum.Necromancer).Find(role => role.Resurrected.Contains(player.PlayerId));

        public static Dracula GetDracula(this PlayerControl player) => Role.GetRoles<Dracula>(RoleEnum.Dracula).Find(role => role.Converted.Contains(player.PlayerId));

        public static Whisperer GetWhisperer(this PlayerControl player) => Role.GetRoles<Whisperer>(RoleEnum.Whisperer).Find(role => role.Persuaded.Contains(player.PlayerId));

        public static Jackal GetJackal(this PlayerVoteArea player) => Utils.PlayerByVoteArea(player).GetJackal();

        public static Necromancer GetNecromancer(this PlayerVoteArea player) => Utils.PlayerByVoteArea(player).GetNecromancer();

        public static Dracula GetDracula(this PlayerVoteArea player) => Utils.PlayerByVoteArea(player).GetDracula();

        public static Whisperer GetWhisperer(this PlayerVoteArea player) => Utils.PlayerByVoteArea(player).GetWhisperer();

        public static bool IsShielded(this PlayerControl player) => Role.GetRoles<Medic>(RoleEnum.Medic).Any(role => player == role.ShieldedPlayer);

        public static bool IsKnighted(this PlayerControl player) => Role.GetRoles<Monarch>(RoleEnum.Monarch).Any(role => role.Knighted.Contains(player.PlayerId));

        public static bool IsSpelled(this PlayerControl player) => Role.GetRoles<Spellslinger>(RoleEnum.Spellslinger).Any(role => role.Spelled.Contains(player.PlayerId));

        public static bool IsArsoDoused(this PlayerControl player) => Role.GetRoles<Arsonist>(RoleEnum.Arsonist).Any(role => role.Doused.Contains(player.PlayerId));

        public static bool IsCryoDoused(this PlayerControl player) => Role.GetRoles<Cryomaniac>(RoleEnum.Cryomaniac).Any(role => role.Doused.Contains(player.PlayerId));

        public static bool IsProtectedMonarch(this PlayerControl player) => Role.GetRoles<Monarch>(RoleEnum.Monarch).Any(role => role.Protected && role.Player == player);

        public static bool IsFaithful(this PlayerControl player) => Role.GetRole(player).Faithful;

        public static bool IsBlackmailed(this PlayerControl player)
        {
            var bmFlag = Role.GetRoles<Blackmailer>(RoleEnum.Blackmailer).Any(role => role.BlackmailedPlayer == player);
            var gfFlag = Role.GetRoles<PromotedGodfather>(RoleEnum.PromotedGodfather).Any(role => role.BlackmailedPlayer == player);
            return bmFlag || gfFlag;
        }

        public static bool IsSilenced(this PlayerControl player)
        {
            var silFlag = Role.GetRoles<Silencer>(RoleEnum.Silencer).Any(role => role.SilencedPlayer == player);
            var rebFlag = Role.GetRoles<PromotedRebel>(RoleEnum.PromotedRebel).Any(role => role.SilencedPlayer == player);
            return silFlag || rebFlag;
        }

        public static Silencer GetSilencer(this PlayerControl player) => Role.GetRoles<Silencer>(RoleEnum.Silencer).Find(x => x.SilencedPlayer == player);

        public static bool IsBombed(this PlayerControl player) => Role.GetRoles<Enforcer>(RoleEnum.Enforcer).Any(role => player == role.BombedPlayer);

        public static bool IsRetShielded(this PlayerControl player) => Role.GetRoles<Retributionist>(RoleEnum.Retributionist).Any(role => player == role.ShieldedPlayer &&
            role.RevivedRole?.RoleType == RoleEnum.Medic);

        public static bool IsShielded(this PlayerVoteArea player) => Utils.PlayerByVoteArea(player).IsShielded();

        public static bool IsKnighted(this PlayerVoteArea player) => Utils.PlayerByVoteArea(player).IsKnighted();

        public static bool IsSpelled(this PlayerVoteArea player) => Utils.PlayerByVoteArea(player).IsSpelled();

        public static bool IsArsoDoused(this PlayerVoteArea player) => Utils.PlayerByVoteArea(player).IsArsoDoused();

        public static bool IsCryoDoused(this PlayerVoteArea player) => Utils.PlayerByVoteArea(player).IsCryoDoused();

        public static bool IsProtectedMonarch(this PlayerVoteArea player) => Utils.PlayerByVoteArea(player).IsProtectedMonarch();

        public static bool IsRetShielded(this PlayerVoteArea player) => Utils.PlayerByVoteArea(player).IsRetShielded();

        public static Medic GetMedic(this PlayerControl player) => Role.GetRoles<Medic>(RoleEnum.Medic).Find(role => role.ShieldedPlayer == player);

        public static Enforcer GetEnforcer(this PlayerControl player) => Role.GetRoles<Enforcer>(RoleEnum.Enforcer).Find(role => player == role.BombedPlayer);

        public static Retributionist GetRetMedic(this PlayerControl player) => Role.GetRoles<Retributionist>(RoleEnum.Retributionist).Find(role => player == role.ShieldedPlayer &&
            role.RevivedRole?.RoleType == RoleEnum.Medic);

        public static Crusader GetCrusader(this PlayerControl player) => Role.GetRoles<Crusader>(RoleEnum.Crusader).Find(role => player == role.CrusadedPlayer);

        public static PromotedRebel GetRebCrus(this PlayerControl player) => Role.GetRoles<PromotedRebel>(RoleEnum.PromotedRebel).Find(role => player == role.CrusadedPlayer);

        public static bool IsOnAlert(this PlayerControl player)
        {
            var vetFlag = Role.GetRoles<Veteran>(RoleEnum.Veteran).Any(role => role.OnAlert && player == role.Player);
            var retFlag = Role.GetRoles<Retributionist>(RoleEnum.Retributionist).Any(role => role.OnEffect && role.IsVet && player == role.Player);
            return vetFlag || retFlag;
        }

        public static bool IsVesting(this PlayerControl player) => Role.GetRoles<Survivor>(RoleEnum.Survivor).Any(role => role.Vesting && player == role.Player);

        public static bool IsMarked(this PlayerControl player) => Role.GetRoles<Ghoul>(RoleEnum.Ghoul).Any(role => player == role.MarkedPlayer);

        public static bool IsAmbushed(this PlayerControl player) => Role.GetRoles<Ambusher>(RoleEnum.Ambusher).Any(role => role.OnAmbush && player == role.AmbushedPlayer);

        public static bool IsGFAmbushed(this PlayerControl player) => Role.GetRoles<PromotedGodfather>(RoleEnum.PromotedGodfather).Any(role => role.OnEffect && role.IsAmb && player ==
            role.AmbushedPlayer);

        public static bool IsCrusaded(this PlayerControl player) => Role.GetRoles<Crusader>(RoleEnum.Crusader).Any(role => role.OnCrusade && player == role.CrusadedPlayer);

        public static bool IsRebCrusaded(this PlayerControl player) => Role.GetRoles<PromotedRebel>(RoleEnum.PromotedRebel).Any(role => role.OnEffect && role.IsCrus && player ==
            role.CrusadedPlayer);

        public static bool IsProtected(this PlayerControl player) => Role.GetRoles<GuardianAngel>(RoleEnum.GuardianAngel).Any(role => role.Protecting && player == role.TargetPlayer);

        public static bool IsInfected(this PlayerControl player) => Role.GetRoles<Plaguebearer>(RoleEnum.Plaguebearer).Any(role => role.Infected.Contains(player.PlayerId) ||
            player == role.Player);

        public static bool IsFramed(this PlayerControl player) => Role.GetRoles<Framer>(RoleEnum.Framer).Any(role => role.Framed.Contains(player.PlayerId));

        public static bool IsInfected(this PlayerVoteArea player) => Utils.PlayerByVoteArea(player).IsInfected();

        public static bool IsFramed(this PlayerVoteArea player) => Utils.PlayerByVoteArea(player).IsFramed();

        public static bool IsMarked(this PlayerVoteArea player) => Utils.PlayerByVoteArea(player).IsMarked();

        public static bool IsWinningRival(this PlayerControl player) => player.Is(ObjectifierEnum.Rivals) && Objectifier.GetObjectifier<Rivals>(player).RivalDead;

        public static bool IsTurnedTraitor(this PlayerControl player) => player.IsIntTraitor() || player.IsSynTraitor();

        public static bool IsTurnedFanatic(this PlayerControl player) => player.IsIntFanatic() || player.IsSynFanatic();

        public static bool IsTurnedTraitor(this PlayerVoteArea player) => Utils.PlayerByVoteArea(player).IsTurnedTraitor();

        public static bool IsTurnedFanatic(this PlayerVoteArea player) => Utils.PlayerByVoteArea(player).IsTurnedFanatic();

        public static bool IsUnturnedFanatic(this PlayerControl player) => !player.IsIntFanatic() && !player.IsSynFanatic();

        public static bool IsIntFanatic(this PlayerControl player) => Role.GetRole(player).IsIntFanatic;

        public static bool IsSynFanatic(this PlayerControl player) => Role.GetRole(player).IsSynFanatic;

        public static bool IsIntTraitor(this PlayerControl player) => Role.GetRole(player).IsIntTraitor;

        public static bool IsSynTraitor(this PlayerControl player) => Role.GetRole(player).IsSynTraitor;

        public static bool IsCrewAlly(this PlayerControl player) => Role.GetRole(player).IsCrewAlly;

        public static bool IsSynAlly(this PlayerControl player) => Role.GetRole(player).IsSynAlly;

        public static bool IsIntAlly(this PlayerControl player) => Role.GetRole(player).IsIntAlly;

        public static bool IsIntFanatic(this PlayerVoteArea player) => Utils.PlayerByVoteArea(player).IsIntFanatic();

        public static bool IsSynFanatic(this PlayerVoteArea player) => Utils.PlayerByVoteArea(player).IsSynFanatic();

        public static bool IsIntTraitor(this PlayerVoteArea player) => Utils.PlayerByVoteArea(player).IsIntTraitor();

        public static bool IsSynTraitor(this PlayerVoteArea player) => Utils.PlayerByVoteArea(player).IsSynTraitor();

        public static bool IsCrewAlly(this PlayerVoteArea player) => Utils.PlayerByVoteArea(player).IsCrewAlly();

        public static bool IsSynAlly(this PlayerVoteArea player) => Utils.PlayerByVoteArea(player).IsSynAlly();

        public static bool IsIntAlly(this PlayerVoteArea player) => Utils.PlayerByVoteArea(player).IsIntAlly();

        public static bool IsOtherRival(this PlayerControl player, PlayerControl refPlayer) => Objectifier.GetObjectifiers<Rivals>(ObjectifierEnum.Rivals).Any(x => x.Player == player &&
            x.OtherRival == refPlayer);

        public static bool IsOtherLover(this PlayerControl player, PlayerControl refPlayer) => Objectifier.GetObjectifiers<Lovers>(ObjectifierEnum.Lovers).Any(x => x.Player == player &&
            x.OtherLover == refPlayer);

        public static bool SyndicateSided(this PlayerControl player) => player.IsSynTraitor() || player.IsSynFanatic() || player.IsSynAlly() || (player.Is(Faction.Syndicate) &&
            player.Is(RoleEnum.Betrayer)) || player.IsSynDefect();

        public static bool IntruderSided(this PlayerControl player) => player.IsIntTraitor() || player.IsIntAlly() || player.IsIntFanatic() || (player.Is(Faction.Intruder) &&
            player.Is(RoleEnum.Betrayer)) || player.IsIntDefect();

        public static bool CrewSided(this PlayerControl player) => player.IsCrewAlly() || player.IsCrewDefect();

        public static bool SyndicateSided(this PlayerVoteArea player) => player.IsSynTraitor() || player.IsSynFanatic() || player.IsSynAlly();

        public static bool IntruderSided(this PlayerVoteArea player) => player.IsIntTraitor() || player.IsIntAlly() || player.IsIntFanatic();

        public static bool CrewSided(this PlayerVoteArea player) => player.IsCrewAlly();

        public static void EndGame() => GameManager.Instance.RpcEndGame(GameOverReason.ImpostorByVote, false);

        public static bool Last(PlayerControl player) => (ConstantVariables.LastImp && player.Is(Faction.Intruder)) || (ConstantVariables.LastSyn && player.Is(Faction.Syndicate));

        public static bool TasksDone()
        {
            var allCrew = new List<PlayerControl>();
            var crewWithNoTasks = new List<PlayerControl>();

            foreach (var player in CustomPlayer.AllPlayers)
            {
                if (player.CanDoTasks() && player.Is(Faction.Crew) && (!player.Data.IsDead || (player.Data.IsDead && CustomGameOptions.GhostTasksCountToWin)))
                {
                    allCrew.Add(player);

                    if (Role.GetRole(player).TasksDone)
                        crewWithNoTasks.Add(player);
                }
            }

            return allCrew.Count == crewWithNoTasks.Count;
        }

        public static bool Sabotaged()
        {
            if (ShipStatus.Instance.Systems != null)
            {
                if (ShipStatus.Instance.Systems.ContainsKey(SystemTypes.LifeSupp))
                {
                    var lifeSuppSystemType = ShipStatus.Instance.Systems[SystemTypes.LifeSupp].Cast<LifeSuppSystemType>();

                    if (lifeSuppSystemType.Countdown < 0f)
                        return true;
                }

                if (ShipStatus.Instance.Systems.ContainsKey(SystemTypes.Laboratory))
                {
                    var reactorSystemType = ShipStatus.Instance.Systems[SystemTypes.Laboratory].Cast<ReactorSystemType>();

                    if (reactorSystemType.Countdown < 0f)
                        return true;
                }

                if (ShipStatus.Instance.Systems.ContainsKey(SystemTypes.Reactor))
                {
                    var reactorSystemType = ShipStatus.Instance.Systems[SystemTypes.Reactor].Cast<ICriticalSabotage>();

                    if (reactorSystemType.Countdown < 0f)
                        return true;
                }
            }

            return false;
        }

        public static bool IsPostmortal(this PlayerControl player) => player.Is(RoleEnum.Revealer) || player.Is(RoleEnum.Phantom) || player.Is(RoleEnum.Ghoul) ||
            player.Is(RoleEnum.Banshee);

        public static bool Caught(this PlayerControl player)
        {
            var role = Role.GetRole(player);

            if (role == null || !player.IsPostmortal())
                return false;

            if (player.Is(RoleEnum.Phantom))
                return ((Phantom)role).Caught;
            else if (player.Is(RoleEnum.Revealer))
                return ((Revealer)role).Caught;
            else if (player.Is(RoleEnum.Ghoul))
                return ((Ghoul)role).Caught;
            else if (player.Is(RoleEnum.Banshee))
                return ((Banshee)role).Caught;

            return true;
        }

        public static float GetModifiedSpeed(this PlayerControl player) => player.IsMimicking(out var mimicked) ? player.GetSpeed() : mimicked.GetSpeed();

        public static float GetSpeed(this PlayerControl player)
        {
            var result = 1f;

            if (LobbyBehaviour.Instance || (DoUndo.IsCamoed && CustomGameOptions.CamoHideSize))
                return result;

            if (player.Is(ModifierEnum.Dwarf))
                result = CustomGameOptions.DwarfSpeed;
            else if (player.Is(ModifierEnum.Giant))
                result = CustomGameOptions.GiantSpeed;
            else if (player.Is(ModifierEnum.Drunk))
                result = Modifier.GetModifier<Drunk>(player).Modify;

            if (player.IsDragging())
                result *= CustomGameOptions.DragModifier;

            if (Role.GetRoles<Drunkard>(RoleEnum.Drunkard).Any(x => x.Confused && (x.HoldsDrive || (x.ConfusedPlayer == player && !x.HoldsDrive))) ||
                Role.GetRoles<PromotedRebel>(RoleEnum.PromotedRebel).Any(x => x.OnEffect && (x.HoldsDrive || (x.ConfusedPlayer == player && !x.HoldsDrive)) && x.IsDrunk))
            {
                result *= -1;
            }

            if (Role.GetRoles<TimeKeeper>(RoleEnum.TimeKeeper).Any(x => x.Controlling))
            {
                if (!player.Is(Faction.Syndicate) || (player.Is(Faction.Syndicate) && ((Role.GetRoles<TimeKeeper>(RoleEnum.TimeKeeper).Any(x => x.Controlling && x.HoldsDrive) &&
                    !CustomGameOptions.TimeRewindImmunity) || (Role.GetRoles<TimeKeeper>(RoleEnum.TimeKeeper).Any(x => x.Controlling && !x.HoldsDrive) &&
                    !CustomGameOptions.TimeFreezeImmunity))))
                {
                    result *= 0;
                }
            }

            if (Role.GetRoles<PromotedRebel>(RoleEnum.PromotedRebel).Any(x => x.OnEffect))
            {
                if (!player.Is(Faction.Syndicate) || (player.Is(Faction.Syndicate) && ((Role.GetRoles<PromotedRebel>(RoleEnum.PromotedRebel).Any(x => x.OnEffect && x.HoldsDrive &&
                    x.IsTK) && !CustomGameOptions.TimeRewindImmunity) || (Role.GetRoles<PromotedRebel>(RoleEnum.PromotedRebel).Any(x => x.OnEffect && !x.HoldsDrive && x.IsTK) &&
                    !CustomGameOptions.TimeFreezeImmunity))))
                {
                    result *= 0;
                }
            }

            return result;
        }

        public static bool IsDragging(this PlayerControl player) => (player.Is(RoleEnum.Janitor) && Role.GetRole<Janitor>(player).CurrentlyDragging) ||
            (player.Is(RoleEnum.PromotedGodfather) && Role.GetRole<PromotedGodfather>(player).CurrentlyDragging);

        public static float GetModifiedSize(this PlayerControl player) => player.IsMimicking(out var mimicked) ? mimicked.GetSize() : player.GetSize();

        public static float GetSize(this PlayerControl player)
        {
            if (LobbyBehaviour.Instance || (DoUndo.IsCamoed && CustomGameOptions.CamoHideSize))
                return 1f;
            else if (player.Is(ModifierEnum.Dwarf))
                return CustomGameOptions.DwarfScale;
            else if (player.Is(ModifierEnum.Giant))
                return CustomGameOptions.GiantScale;
            else
                return 1f;
        }

        public static bool IsMimicking(this PlayerControl player, out PlayerControl mimicked)
        {
            var role = Role.GetRole(player);
            mimicked = player;

            if (!role)
                return false;

            if (Role.GetRoles<Shapeshifter>(RoleEnum.Shapeshifter).Any(x => x.Shapeshifted))
            {
                if (Utils.CachedMorphs.ContainsKey(player))
                    mimicked = Utils.CachedMorphs[player];
                else
                {
                    var ss = (Shapeshifter)Role.AllRoles.Find(x => x.Type == LayerEnum.Shapeshifter && ((Shapeshifter)x).Shapeshifted && (player == ((Shapeshifter)x).ShapeshiftPlayer1 ||
                        player == ((Shapeshifter)x).ShapeshiftPlayer2));

                    if (ss != null)
                    {
                        if (ss.ShapeshiftPlayer1 == player)
                            mimicked = ss.ShapeshiftPlayer2;
                        else if (ss.ShapeshiftPlayer2 == player)
                            mimicked = ss.ShapeshiftPlayer1;
                    }
                }

                return true;
            }
            else if (Role.GetRoles<PromotedRebel>(RoleEnum.PromotedRebel).Any(x => x.OnEffect && x.IsSS))
            {
                if (Utils.CachedMorphs.ContainsKey(player))
                    mimicked = Utils.CachedMorphs[player];
                else
                {
                    var reb = (PromotedRebel)Role.AllRoles.Find(x => x.Type == LayerEnum.PromotedRebel && ((PromotedRebel)x).OnEffect && (player == ((PromotedRebel)x).ShapeshiftPlayer1 ||
                        player == ((PromotedRebel)x).ShapeshiftPlayer2));

                    if (reb != null)
                    {
                        if (reb.ShapeshiftPlayer1 == player)
                            mimicked = reb.ShapeshiftPlayer2;
                        else if (reb.ShapeshiftPlayer2 == player)
                            mimicked = reb.ShapeshiftPlayer1;
                    }
                }

                return true;
            }
            else if (role.Type == LayerEnum.Morphling)
            {
                var morph = (Morphling)role;
                mimicked = morph.MorphedPlayer;
                return morph.Morphed;
            }
            else if (Role.GetRoles<Disguiser>(RoleEnum.Disguiser).Any(x => x.Disguised))
            {
                var disg = (Disguiser)Role.AllRoles.Find(x => x.Type == LayerEnum.Disguiser && ((Disguiser)x).Disguised && player == ((Disguiser)x).DisguisedPlayer);

                if (disg != null)
                    mimicked = disg.DisguisePlayer;

                return disg != null;
            }
            else if (role.Type == LayerEnum.PromotedGodfather)
            {
                var gf = (PromotedGodfather)role;
                mimicked = gf.MorphedPlayer;
                return gf.OnEffect && gf.IsMorph;
            }
            else if (Role.GetRoles<PromotedGodfather>(RoleEnum.PromotedGodfather).Any(x => x.OnEffect && x.IsDisg))
            {
                var gf = (PromotedGodfather)Role.AllRoles.Find(x => x.Type == LayerEnum.PromotedGodfather && ((PromotedGodfather)x).OnEffect && player == ((Disguiser)x).DisguisedPlayer);

                if (gf != null)
                    mimicked = gf.DisguisePlayer;

                return gf != null;
            }
            else if (role.Type == LayerEnum.Glitch)
            {
                var glitch = (Glitch)role;
                mimicked = glitch.MimicTarget;
                return glitch.IsUsingMimic;
            }

            return false;
        }

        public static bool CanVent(this PlayerControl player)
        {
            var playerInfo = player?.Data;

            if (ConstantVariables.IsHnS)
                return playerInfo?.IsImpostor() == true;
            else if (player == null || playerInfo == null || (playerInfo.IsDead && !player.IsPostmortal()) || playerInfo.Disconnected || (int)CustomGameOptions.WhoCanVent is 3 ||
                player.inMovingPlat || player.onLadder || Utils.Meeting || ConstantVariables.Inactive)
            {
                return false;
            }
            else if (player.inVent || CustomGameOptions.WhoCanVent == WhoCanVentOptions.Everyone)
                return true;

            var playerRole = Role.GetRole(player);
            var mainflag = false;

            if (playerRole == null)
                mainflag = playerInfo.IsImpostor();
            else if (player.Is(ObjectifierEnum.Mafia))
                mainflag = CustomGameOptions.MafVent;
            else if (player.Is(ObjectifierEnum.Corrupted))
                mainflag = CustomGameOptions.CorruptedVent;
            else if (player.IsRecruit())
                mainflag = CustomGameOptions.RecruitVent;
            else if (player.IsResurrected())
                mainflag = CustomGameOptions.ResurrectVent;
            else if (player.IsPersuaded())
                mainflag = CustomGameOptions.PersuadedVent;
            else if (player.IsBitten())
                mainflag = CustomGameOptions.UndeadVent;
            else if (player.Is(Faction.Syndicate) && !player.SyndicateSided())
                mainflag = (((Syndicate)playerRole).HoldsDrive && (int)CustomGameOptions.SyndicateVent is 1) || (int)CustomGameOptions.SyndicateVent is 0;
            else if (player.Is(Faction.Intruder) && !player.IntruderSided())
            {
                var flag = (player.Is(RoleEnum.Morphling) && !CustomGameOptions.MorphlingVent) || (player.Is(RoleEnum.Wraith) && !CustomGameOptions.WraithVent) ||
                    (player.Is(RoleEnum.Grenadier) && !CustomGameOptions.GrenadierVent) || (player.Is(RoleEnum.Teleporter) && !CustomGameOptions.TeleVent);

                if (player.Is(RoleEnum.Janitor))
                {
                    var janitor = (Janitor)playerRole;
                    mainflag = (int)CustomGameOptions.JanitorVentOptions is 3 || (janitor.CurrentlyDragging && (int)CustomGameOptions.JanitorVentOptions is 1) ||
                        (janitor.CurrentlyDragging == null && (int)CustomGameOptions.JanitorVentOptions is 2);
                }
                else if (player.Is(RoleEnum.PromotedGodfather))
                {
                    var gf = (PromotedGodfather)playerRole;
                    mainflag = (int)CustomGameOptions.JanitorVentOptions is 3 || (gf.CurrentlyDragging && (int)CustomGameOptions.JanitorVentOptions is 1) ||
                        (gf.CurrentlyDragging == null && (int)CustomGameOptions.JanitorVentOptions is 2);
                }
                else if (flag)
                    mainflag = false;
                else
                    mainflag = CustomGameOptions.IntrudersVent;
            }
            else if (player.Is(Faction.Crew) && !player.Is(RoleEnum.Revealer))
            {
                if (player.Is(AbilityEnum.Tunneler))
                    mainflag =  playerRole.TasksDone;
                else
                    mainflag = player.Is(RoleEnum.Engineer) || CustomGameOptions.CrewVent;
            }
            else if (player.Is(Faction.Neutral))
            {
                var flag = ((player.Is(RoleEnum.Murderer) && CustomGameOptions.MurdVent) || (player.Is(RoleEnum.Glitch) && CustomGameOptions.GlitchVent) ||
                    (player.Is(RoleEnum.Juggernaut) && CustomGameOptions.JuggVent) || (player.Is(RoleEnum.Pestilence) && CustomGameOptions.PestVent) ||
                    (player.Is(RoleEnum.Jester) && CustomGameOptions.JesterVent) || (player.Is(RoleEnum.Plaguebearer) && CustomGameOptions.PBVent) ||
                    (player.Is(RoleEnum.Arsonist) && CustomGameOptions.ArsoVent) || (player.Is(RoleEnum.Executioner) && CustomGameOptions.ExeVent) ||
                    (player.Is(RoleEnum.Cannibal) && CustomGameOptions.CannibalVent) || (player.Is(RoleEnum.Dracula) && CustomGameOptions.DracVent) ||
                    (player.Is(RoleEnum.Survivor) && CustomGameOptions.SurvVent) || (player.Is(RoleEnum.Actor) && CustomGameOptions.ActorVent) ||
                    (player.Is(RoleEnum.GuardianAngel) && CustomGameOptions.GAVent) || (player.Is(RoleEnum.Amnesiac) && CustomGameOptions.AmneVent) ||
                    (player.Is(RoleEnum.Werewolf) && CustomGameOptions.WerewolfVent) || (player.Is(RoleEnum.Jackal) && CustomGameOptions.JackalVent) ||
                    (player.Is(RoleEnum.BountyHunter) && CustomGameOptions.BHVent)) && CustomGameOptions.NeutralsVent;

                if (player.Is(RoleEnum.SerialKiller))
                {
                    var role2 = (SerialKiller)playerRole;
                    mainflag = (int)CustomGameOptions.SKVentOptions is 0 || (role2.Lusted && (int)CustomGameOptions.SKVentOptions is 1) || (!role2.Lusted &&
                        (int)CustomGameOptions.SKVentOptions is 2);
                }
                else
                    mainflag = flag;
            }
            else if (player.Is(RoleEnum.Betrayer))
                mainflag = CustomGameOptions.BetrayerVent;
            else if (player.IsPostmortal() && player.inVent)
                mainflag = true;

            return mainflag;
        }

        public static InspectorResults GetInspResults(this PlayerControl player)
        {
            if (player == null)
                return InspectorResults.None;

            var role = Role.GetRole(player);

            if (role == null)
                return InspectorResults.None;

            return role.InspectorResults;
        }

        public static InspectorResults GetInspResults(this PlayerVoteArea player) => Utils.PlayerByVoteArea(player).GetInspResults();

        public static bool IsBlocked(this PlayerControl player) => PlayerLayer.GetLayers(player)?.Any(x => x.IsBlocked) == true;

        public static bool SeemsEvil(this PlayerControl player)
        {
            var intruderFlag = player.Is(Faction.Intruder) && !player.Is(ObjectifierEnum.Traitor) && !player.Is(ObjectifierEnum.Fanatic) && !player.Is(RoleEnum.PromotedGodfather);
            var syndicateFlag = player.Is(Faction.Syndicate) && !player.Is(ObjectifierEnum.Traitor) && !player.Is(ObjectifierEnum.Fanatic) && !player.Is(RoleEnum.PromotedRebel);
            var traitorFlag = player.IsTurnedTraitor() && CustomGameOptions.TraitorColourSwap;
            var fanaticFlag = player.IsTurnedFanatic() && CustomGameOptions.FanaticColourSwap;
            var nkFlag = player.Is(RoleAlignment.NeutralKill) && !CustomGameOptions.NeutKillingRed;
            var neFlag = player.Is(RoleAlignment.NeutralEvil) && !CustomGameOptions.NeutEvilRed;
            var framedFlag = player.IsFramed();
            return intruderFlag || syndicateFlag || traitorFlag || nkFlag || neFlag || framedFlag || fanaticFlag;
        }

        public static bool SeemsGood(this PlayerControl player) => !SeemsEvil(player) || Role.DriveHolder == player;

        public static bool SeemsEvil(this PlayerVoteArea player) => Utils.PlayerByVoteArea(player).SeemsEvil();

        public static bool SeemsGood(this PlayerVoteArea player) => Utils.PlayerByVoteArea(player).SeemsGood();

        public static bool IsBlockImmune(PlayerControl player) => Role.GetRole(player).RoleBlockImmune;

        public static bool HasTarget(this Role role) => role.RoleType is RoleEnum.Executioner or RoleEnum.GuardianAngel or RoleEnum.Guesser or RoleEnum.BountyHunter;

        public static List<PlayerLayer> AllPlayerInfo(this PlayerControl player) => new()
        {
            Role.GetRole(player),
            Modifier.GetModifier(player),
            Ability.GetAbility(player),
            Objectifier.GetObjectifier(player)
        };

        public static List<PlayerLayer> AllPlayerInfo(this PlayerVoteArea player) => Utils.PlayerByVoteArea(player).AllPlayerInfo();

        public static PlayerControl GetOtherLover(this PlayerControl player)
        {
            if (!player.Is(ObjectifierEnum.Lovers))
                return null;

            return Objectifier.GetObjectifier<Lovers>(player).OtherLover;
        }

        public static PlayerControl GetOtherRival(this PlayerControl player)
        {
            if (!player.Is(ObjectifierEnum.Rivals))
                return null;

            return Objectifier.GetObjectifier<Rivals>(player).OtherRival;
        }

        public static bool NeutralHasUnfinishedBusiness(PlayerControl player)
        {
            if (player.Is(RoleEnum.GuardianAngel))
            {
                var ga = Role.GetRole<GuardianAngel>(player);
                return ga.TargetAlive;
            }
            else if (player.Is(RoleEnum.Executioner))
            {
                var exe = Role.GetRole<Executioner>(player);
                return exe.TargetVotedOut;
            }
            else if (player.Is(RoleEnum.Jester))
            {
                var jest = Role.GetRole<Jester>(player);
                return jest.VotedOut;
            }
            else if (player.Is(RoleEnum.Guesser))
            {
                var guess = Role.GetRole<Guesser>(player);
                return guess.TargetGuessed;
            }
            else if (player.Is(RoleEnum.BountyHunter))
            {
                var bh = Role.GetRole<BountyHunter>(player);
                return bh.TargetKilled;
            }
            else if (player.Is(RoleEnum.Actor))
            {
                var act = Role.GetRole<Actor>(player);
                return act.Guessed;
            }
            else if (player.Is(RoleEnum.Troll))
            {
                var troll = Role.GetRole<Troll>(player);
                return troll.Killed;
            }

            return false;
        }

        public static string RoleCardInfo(this PlayerControl player)
        {
            var info = player.AllPlayerInfo();

            var role = info[0] as Role;
            var modifier = info[1] as Modifier;
            var ability = info[2] as Ability;
            var objectifier = info[3] as Objectifier;

            var objectives = $"{ObjectivesColorString}Goal:";
            var abilities = $"{AbilitiesColorString}Abilities:";
            var attributes = $"{AttributesColorString}Attributes:";
            var roleName = $"{RoleColorString}Role: <b>";
            var objectifierName = $"{ObjectifierColorString}Objectifier: <b>";
            var abilityName = $"{AbilityColorString}Ability: <b>";
            var modifierName = $"{ModifierColorString}Modifier: <b>";
            var alignment = $"{AlignmentColorString}Alignment: <b>";
            var subfaction = $"{SubFactionColorString}Sub-Faction: <b>";

            if (info[0])
            {
                roleName += $"{role.ColorString}{role.Name}</color>";
                objectives += $"\n{role.ColorString}{role.Objectives()}</color>";
                alignment += $"{role.RoleAlignment.AlignmentName(true)}";
                subfaction += $"{role.SubFactionColorString}{role.SubFactionName} {role.SubFactionSymbol}</color>";
            }
            else
            {
                roleName += "None";
                alignment += "None";
                subfaction += "None";
            }

            roleName += "</b></color>";
            alignment += "</b></color>";
            subfaction += "</b></color>";

            if (info[3] && !objectifier.Hidden)
            {
                objectives += $"\n{objectifier.ColorString}{objectifier.TaskText()}</color>";
                objectifierName += $"{objectifier.ColorString}{objectifier.Name} {objectifier.Symbol}</color>";
            }
            else
                objectifierName += "None φ";

            objectifierName += "</b></color>";

            if (info[2] && !ability.Hidden && ability.AbilityType != AbilityEnum.None)
                abilityName += $"{ability.ColorString}{ability.Name}</color>";
            else
                abilityName += "None";

            abilityName += "</b></color>";

            if (info[1] && !modifier.Hidden && modifier.ModifierType != ModifierEnum.None)
                modifierName += $"{modifier.ColorString}{modifier.Name}</color>";
            else
                modifierName += "None";

            modifierName += "</b></color>";

            if (player.IsRecruit())
            {
                var jackal = player.GetJackal();
                objectives += $"\n<color=#{Colors.Cabal.ToHtmlStringRGBA()}>- You are a member of the Cabal. Help {jackal.PlayerName} in taking over the mission $</color>";
            }
            else if (player.IsResurrected())
            {
                var necromancer = player.GetNecromancer();
                objectives += $"\n<color=#{Colors.Reanimated.ToHtmlStringRGBA()}>- You are a member of the Reanimated. Help {necromancer.PlayerName} in taking over the mission Σ</color>";
            }
            else if (player.IsPersuaded())
            {
                var whisperer = player.GetWhisperer();
                objectives += $"\n<color=#{Colors.Sect.ToHtmlStringRGBA()}>- You are a member of the Sect. Help {whisperer.PlayerName} in taking over the mission Λ</color>";
            }
            else if (player.IsBitten())
            {
                var dracula = player.GetDracula();
                objectives += $"\n<color=#{Colors.Undead.ToHtmlStringRGBA()}>- You are a member of the Undead. Help {dracula.PlayerName} in taking over the mission γ</color>";
                abilities += $"\n{role.ColorString}- Attempting to interact with a <color=#C0C0C0FF>Vampire Hunter</color> will force them to kill you</color>";
            }

            if (objectives == $"{ObjectivesColorString}Objectives:")
                objectives += "\n- None";

            objectives += "</color>";

            if (info[0])
                abilities += $"\n{role.ColorString}{role.AbilitiesText()}</color>";

            if (info[2] && !ability.Hidden && ability.AbilityType != AbilityEnum.None)
                abilities += $"\n{ability.ColorString}{ability.TaskText()}</color>";

            if (abilities == $"{AbilitiesColorString}Abilities:")
                abilities += "\n- None";

            abilities += "</color>";

            if (info[1] && !modifier.Hidden && modifier.ModifierType != ModifierEnum.None)
                attributes += $"\n{modifier.ColorString}{modifier.TaskText()}</color>";

            if (player.IsGuessTarget() && CustomGameOptions.GuesserTargetKnows)
                attributes += "\n<color=#EEE5BEFF>- Someone wants to guess you π</color>";

            if (player.IsExeTarget() && CustomGameOptions.ExeTargetKnows)
                attributes += "\n<color=#CCCCCCFF>- Someone wants you ejected §</color>";

            if (player.IsGATarget() && CustomGameOptions.GATargetKnows)
                attributes += "\n<color=#FFFFFFFF>- Someone wants to protect you ★</color>";

            if (player.IsBHTarget())
                attributes += "\n<color=#B51E39FF>- There is a bounty on your head Θ</color>";

            if (player.Is(Faction.Syndicate) && ((Syndicate)role).HoldsDrive)
                attributes += "\n<color=#008000FF>- You have the power of the Chaos Drive Δ</color>";

            if (!player.CanDoTasks())
                attributes += "\n<color=#ABCDEFFF>- Your tasks are fake</color>";

            if (player.Data.IsDead)
                attributes += "\n<color=#FF0000FF>- You are dead</color>";

            if (attributes == $"{AttributesColorString}Attributes:")
                attributes += "\n- None";

            attributes += "</color>";
            return $"{roleName}\n{alignment}\n{subfaction}\n{objectifierName}\n{abilityName}\n{modifierName}\n{objectives}\n{abilities}\n{attributes}";
        }

        public static void RegenTask(this PlayerControl player)
        {
            try
            {
                foreach (var task2 in player.myTasks)
                {
                    var task3 = task2?.TryCast<ImportantTextTask>();

                    if (task3.Text.Contains("Sabotage and kill everyone") || task3.Text.Contains("Fake Tasks") || task3.Text.Contains("tasks to win"))
                        player.myTasks.Remove(task3);
                }
            } catch {}
        }

        public static void RoleUpdate(this Role newRole, Role former)
        {
            CustomButton.AllButtons.Where(x => x.Owner == former).ToList().ForEach(x => x.Destroy());
            CustomArrow.AllArrows.Where(x => x.Owner == former.Player).ToList().ForEach(x => x.Destroy());
            former.OnLobby();
            former.Ignore = true;
            former.Player = null;
            newRole.RoleHistory.Add(former);
            newRole.RoleHistory.AddRange(former.RoleHistory);
            newRole.Faction = former.Faction;
            newRole.SubFaction = former.SubFaction;
            newRole.FactionColor = former.FactionColor;
            newRole.SubFactionColor = former.SubFactionColor;
            newRole.DeathReason = former.DeathReason;
            newRole.KilledBy = former.KilledBy;
            newRole.IsBitten = former.IsBitten;
            newRole.IsRecruit = former.IsRecruit;
            newRole.IsResurrected = former.IsResurrected;
            newRole.IsPersuaded = former.IsPersuaded;
            newRole.IsIntFanatic = former.IsIntFanatic;
            newRole.IsIntTraitor = former.IsIntTraitor;
            newRole.IsSynFanatic = former.IsSynFanatic;
            newRole.IsSynTraitor = former.IsSynTraitor;
            newRole.IsIntAlly = former.IsIntAlly;
            newRole.IsSynAlly = former.IsSynAlly;
            newRole.IsCrewAlly = former.IsCrewAlly;
            newRole.IsBlocked = former.IsBlocked;
            newRole.Diseased = former.Diseased;
            newRole.IsIntDefect = former.IsIntDefect;
            newRole.IsSynDefect = former.IsSynDefect;
            newRole.IsCrewDefect = former.IsCrewDefect;
            newRole.IsNeutDefect = former.IsNeutDefect;
            newRole.AllArrows = former.AllArrows;
            Role.AllRoles.Remove(former);
            PlayerLayer.AllLayers.Remove(former);

            if (newRole.Local || former.Local)
                ButtonUtils.ResetCustomTimers(false);
        }

        public static void SetImpostor(this GameData.PlayerInfo player, bool impostor)
        {
            if (player == null)
                return;

            player.Role.TeamType = impostor ? RoleTeamTypes.Impostor : RoleTeamTypes.Crewmate;
            var imp = player.IsDead ? RoleTypes.ImpostorGhost : RoleTypes.Impostor;
            var crew = player.IsDead ? RoleTypes.CrewmateGhost : RoleTypes.Crewmate;
            RoleManager.Instance.SetRole(player.Object, impostor ? imp : crew);
        }

        public static string AlignmentName(this RoleAlignment alignment, bool withColors = false) => alignment switch
        {
            RoleAlignment.CrewSupport => withColors ? "<color=#8CFFFFFF>Crew (<color=#1D7CF2FF>Support</color>)</color>" : "Crew (Support)",
            RoleAlignment.CrewInvest => withColors ? "<color=#8CFFFFFF>Crew (<color=#1D7CF2FF>Investigative</color>)</color>" : "Crew (Investigative)",
            RoleAlignment.CrewProt => withColors ? "<color=#8CFFFFFF>Crew (<color=#1D7CF2FF>Protective</color>)</color>" : "Crew (Protective)",
            RoleAlignment.CrewKill => withColors ? "<color=#8CFFFFFF>Crew (<color=#1D7CF2FF>Killing</color>)</color>" : "Crew (Killing)",
            RoleAlignment.CrewUtil => withColors ? "<color=#8CFFFFFF>Crew (<color=#1D7CF2FF>Utility</color>)</color>" : "Crew (Utility)",
            RoleAlignment.CrewSov => withColors ? "<color=#8CFFFFFF>Crew (<color=#1D7CF2FF>Sovereign</color>)</color>" : "Crew (Sovereign)",
            RoleAlignment.CrewAudit => withColors ? "<color=#8CFFFFFF>Crew (<color=#1D7CF2FF>Auditor</color>)</color>" : "Crew (Auditor)",
            RoleAlignment.CrewDecep => withColors ? "<color=#8CFFFFFF>Crew (<color=#1D7CF2FF>Deception</color>)</color>" : "Crew (Deception)",
            RoleAlignment.CrewConceal => withColors ? "<color=#8CFFFFFF>Crew (<color=#1D7CF2FF>Concealing</color>)</color>" : "Crew (Concealing)",
            RoleAlignment.CrewPower => withColors ? "<color=#8CFFFFFF>Crew (<color=#1D7CF2FF>Power</color>)</color>" : "Crew (Power)",
            RoleAlignment.CrewDisrup => withColors ? "<color=#8CFFFFFF>Crew (<color=#1D7CF2FF>Disruption</color>)</color>" : "Crew (Disruption)",
            RoleAlignment.IntruderSupport => withColors ? "<color=#FF0000FF>Intruder (<color=#1D7CF2FF>Support</color>)</color>" : "Intruder (Support)",
            RoleAlignment.IntruderConceal => withColors ? "<color=#FF0000FF>Intruder (<color=#1D7CF2FF>Concealing</color>)</color>" : "Intruder (Concealing)",
            RoleAlignment.IntruderDecep => withColors ? "<color=#FF0000FF>Intruder (<color=#1D7CF2FF>Deception</color>)</color>" : "Intuder (Deception)",
            RoleAlignment.IntruderKill => withColors ? "<color=#FF0000FF>Intruder (<color=#1D7CF2FF>Killing</color>)</color>" : "Intruder (Killing)",
            RoleAlignment.IntruderUtil => withColors ? "<color=#FF0000FF>Intruder (<color=#1D7CF2FF>Utility</color>)</color>" : "Intruder (Utility)",
            RoleAlignment.IntruderInvest => withColors ? "<color=#FF0000FF>Intruder (<color=#1D7CF2FF>Investigative</color>)</color>" : "Intruder (Investigative)",
            RoleAlignment.IntruderAudit => withColors ? "<color=#FF0000FF>Intruder (<color=#1D7CF2FF>Auditor</color>)</color>" : "Intruder (Auditor)",
            RoleAlignment.IntruderProt => withColors ? "<color=#FF0000FF>Intruder (<color=#1D7CF2FF>Protective</color>)</color>" : "Intruder (Protective)",
            RoleAlignment.IntruderSov => withColors ? "<color=#FF0000FF>Intruder (<color=#1D7CF2FF>Sovereign</color>)</color>" : "Intruder (Sovereign)",
            RoleAlignment.IntruderDisrup=> withColors ? "<color=#FF0000FF>Intruder (<color=#1D7CF2FF>Disruption</color>)</color>" : "Intruder (Disruption)",
            RoleAlignment.IntruderPower => withColors ? "<color=#FF0000FF>Intruder (<color=#1D7CF2FF>Power</color>)</color>" : "Intruder (Power)",
            RoleAlignment.NeutralKill => withColors ? "<color=#B3B3B3FF>Neutral (<color=#1D7CF2FF>Killing</color>)</color>" : "Neutral (Killing)",
            RoleAlignment.NeutralNeo => withColors ? "<color=#B3B3B3FF>Neutral (<color=#1D7CF2FF>Neophyte</color>)</color>" : "Neutral (Neophyte)",
            RoleAlignment.NeutralEvil => withColors ? "<color=#B3B3B3FF>Neutral (<color=#1D7CF2FF>Evil</color>)</color>" : "Neutral (Evil)",
            RoleAlignment.NeutralBen => withColors ? "<color=#B3B3B3FF>Neutral (<color=#1D7CF2FF>Benign</color>)</color>" : "Neutral (Benign)",
            RoleAlignment.NeutralPros => withColors ? "<color=#B3B3B3FF>Neutral (<color=#1D7CF2FF>Proselyte</color>)</color>" : "Neutral (Proselyte)",
            RoleAlignment.NeutralSupport => withColors ? "<color=#B3B3B3FF>Neutral (<color=#1D7CF2FF>Support</color>)</color>" : "Neutral (Support)",
            RoleAlignment.NeutralInvest => withColors ? "<color=#B3B3B3FF>Neutral (<color=#1D7CF2FF>Investigative</color>)</color>" : "Neutral (Investigative)",
            RoleAlignment.NeutralProt => withColors ? "<color=#B3B3B3FF>Neutral (<color=#1D7CF2FF>Protective</color>)</color>" : "Neutral (Protective)",
            RoleAlignment.NeutralUtil => withColors ? "<color=#B3B3B3FF>Neutral (<color=#1D7CF2FF>Utility</color>)</color>" : "Neutral (Utility)",
            RoleAlignment.NeutralSov => withColors ? "<color=#B3B3B3FF>Neutral (<color=#1D7CF2FF>Sovereign</color>)</color>" : "Neutral (Sovereign)",
            RoleAlignment.NeutralAudit => withColors ? "<color=#B3B3B3FF>Neutral (<color=#1D7CF2FF>Auditor</color>)</color>" : "Neutral (Auditor)",
            RoleAlignment.NeutralConceal => withColors ? "<color=#B3B3B3FF>Neutral (<color=#1D7CF2FF>Concealing</color>)</color>" : "Neutral (Concealing)",
            RoleAlignment.NeutralDecep => withColors ? "<color=#B3B3B3FF>Neutral (<color=#1D7CF2FF>Deception</color>)</color>" : "Neutral (Deception)",
            RoleAlignment.NeutralPower => withColors ? "<color=#B3B3B3FF>Neutral (<color=#1D7CF2FF>Power</color>)</color>" : "Neutral (Power)",
            RoleAlignment.NeutralDisrup => withColors ? "<color=#B3B3B3FF>Neutral (<color=#1D7CF2FF>Apocalypse</color>)</color>" : "Neutral (Disruption)",
            RoleAlignment.NeutralApoc => withColors ? "<color=#B3B3B3FF>Neutral (<color=#1D7CF2FF>Disruption</color>)</color>" : "Neutral (Apocalypse)",
            RoleAlignment.NeutralHarb => withColors ? "<color=#B3B3B3FF>Neutral (<color=#1D7CF2FF>Harbinger</color>)</color>" : "Neutral (Harbinger)",
            RoleAlignment.SyndicateKill => withColors ? "<color=#008000FF>Syndicate (<color=#1D7CF2FF>Killing</color>)</color>" : "Syndicate (Killing)",
            RoleAlignment.SyndicateSupport => withColors ? "<color=#008000FF>Syndicate (<color=#1D7CF2FF>Support</color>)</color>" : "Syndicate (Support)",
            RoleAlignment.SyndicateDisrup => withColors ? "<color=#008000FF>Syndicate (<color=#1D7CF2FF>Disruption</color>)</color>" : "Syndicate (Disruption)",
            RoleAlignment.SyndicatePower => withColors ? "<color=#008000FF>Syndicate (<color=#1D7CF2FF>Power</color>)</color>" : "Syndicate (Power)",
            RoleAlignment.SyndicateUtil => withColors ? "<color=#008000FF>Syndicate (<color=#1D7CF2FF>Utility</color>)</color>" : "Syndicate (Utility)",
            RoleAlignment.SyndicateInvest => withColors ? "<color=#008000FF>Syndicate (<color=#1D7CF2FF>Investigative</color>)</color>" : "Syndicate (Investigative)",
            RoleAlignment.SyndicateAudit => withColors ? "<color=#008000FF>Syndicate (<color=#1D7CF2FF>Auditor</color>)</color>" : "Syndicate (Auditor)",
            RoleAlignment.SyndicateSov => withColors ? "<color=#008000FF>Syndicate (<color=#1D7CF2FF>Sovereign</color>)</color>" : "Syndicate (Sovereign)",
            RoleAlignment.SyndicateProt => withColors ? "<color=#008000FF>Syndicate (<color=#1D7CF2FF>Protective</color>)</color>" : "Syndicate (Protective)",
            RoleAlignment.SyndicateConceal => withColors ? "<color=#008000FF>Syndicate (<color=#1D7CF2FF>Concealing</color>)</color>" : "Syndicate (Concealing)",
            RoleAlignment.SyndicateDecep => withColors ? "<color=#008000FF>Syndicate (<color=#1D7CF2FF>Deception</color>)</color>" : "Syndicate (Deception)",
            RoleAlignment.None => "Invalid",
            _ => "Invalid",
        };

        public static RoleAlignment GetNewAlignment(this RoleAlignment alignment, Faction faction)
        {
            if (faction == Faction.Crew)
            {
                return alignment switch
                {
                    RoleAlignment.NeutralKill => RoleAlignment.CrewKill,
                    RoleAlignment.IntruderSupport => RoleAlignment.CrewSupport,
                    RoleAlignment.IntruderConceal => RoleAlignment.CrewConceal,
                    RoleAlignment.IntruderDecep => RoleAlignment.CrewDecep,
                    RoleAlignment.IntruderUtil => RoleAlignment.CrewUtil,
                    RoleAlignment.IntruderKill => RoleAlignment.CrewKill,
                    RoleAlignment.SyndicateDisrup => RoleAlignment.CrewDisrup,
                    RoleAlignment.SyndicatePower => RoleAlignment.CrewPower,
                    RoleAlignment.SyndicateSupport => RoleAlignment.CrewSupport,
                    RoleAlignment.SyndicateKill => RoleAlignment.CrewSupport,
                    RoleAlignment.SyndicateUtil => RoleAlignment.CrewUtil,
                    _ => alignment
                };
            }
            else if (faction == Faction.Intruder)
            {
                return alignment switch
                {
                    RoleAlignment.CrewSupport => RoleAlignment.IntruderSupport,
                    RoleAlignment.CrewInvest => RoleAlignment.IntruderInvest,
                    RoleAlignment.CrewProt => RoleAlignment.IntruderProt,
                    RoleAlignment.CrewKill => RoleAlignment.IntruderKill,
                    RoleAlignment.CrewUtil => RoleAlignment.IntruderUtil,
                    RoleAlignment.CrewSov => RoleAlignment.IntruderSov,
                    RoleAlignment.CrewAudit => RoleAlignment.IntruderAudit,
                    RoleAlignment.NeutralKill => RoleAlignment.IntruderKill,
                    RoleAlignment.SyndicateDisrup => RoleAlignment.IntruderDisrup,
                    RoleAlignment.SyndicatePower => RoleAlignment.IntruderPower,
                    RoleAlignment.SyndicateSupport => RoleAlignment.IntruderSupport,
                    RoleAlignment.SyndicateKill => RoleAlignment.IntruderKill,
                    RoleAlignment.SyndicateUtil => RoleAlignment.IntruderUtil,
                    _ => alignment
                };
            }
            else if (faction == Faction.Syndicate)
            {
                return alignment switch
                {
                    RoleAlignment.CrewSupport => RoleAlignment.SyndicateSupport,
                    RoleAlignment.CrewInvest => RoleAlignment.SyndicateInvest,
                    RoleAlignment.CrewProt => RoleAlignment.SyndicateProt,
                    RoleAlignment.CrewKill => RoleAlignment.SyndicateKill,
                    RoleAlignment.CrewUtil => RoleAlignment.SyndicateUtil,
                    RoleAlignment.CrewSov => RoleAlignment.SyndicateSov,
                    RoleAlignment.CrewAudit => RoleAlignment.SyndicateAudit,
                    RoleAlignment.NeutralKill => RoleAlignment.SyndicateKill,
                    RoleAlignment.IntruderSupport => RoleAlignment.SyndicateSupport,
                    RoleAlignment.IntruderConceal => RoleAlignment.SyndicateConceal,
                    RoleAlignment.IntruderDecep => RoleAlignment.SyndicateDecep,
                    RoleAlignment.IntruderUtil => RoleAlignment.SyndicateUtil,
                    RoleAlignment.IntruderKill => RoleAlignment.SyndicateKill,
                    _ => alignment
                };
            }
            else if (faction == Faction.Neutral)
            {
                return alignment switch
                {
                    RoleAlignment.CrewSupport => RoleAlignment.NeutralSupport,
                    RoleAlignment.CrewInvest => RoleAlignment.NeutralInvest,
                    RoleAlignment.CrewProt => RoleAlignment.NeutralProt,
                    RoleAlignment.CrewKill => RoleAlignment.NeutralKill,
                    RoleAlignment.CrewUtil => RoleAlignment.NeutralUtil,
                    RoleAlignment.CrewSov => RoleAlignment.NeutralSov,
                    RoleAlignment.CrewAudit => RoleAlignment.NeutralAudit,
                    RoleAlignment.IntruderSupport => RoleAlignment.NeutralSupport,
                    RoleAlignment.IntruderConceal => RoleAlignment.NeutralConceal,
                    RoleAlignment.IntruderDecep => RoleAlignment.NeutralDecep,
                    RoleAlignment.IntruderUtil => RoleAlignment.NeutralUtil,
                    RoleAlignment.IntruderKill => RoleAlignment.NeutralKill,
                    RoleAlignment.SyndicateDisrup => RoleAlignment.NeutralDisrup,
                    RoleAlignment.SyndicatePower => RoleAlignment.NeutralDisrup,
                    RoleAlignment.SyndicateSupport => RoleAlignment.NeutralSupport,
                    RoleAlignment.SyndicateKill => RoleAlignment.NeutralKill,
                    RoleAlignment.SyndicateUtil => RoleAlignment.NeutralUtil,
                    _ => alignment
                };
            }

            return alignment;
        }
    }
}