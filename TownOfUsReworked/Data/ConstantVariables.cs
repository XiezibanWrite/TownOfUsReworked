using AmongUs.GameOptions;
using InnerNet;
using TownOfUsReworked.CustomOptions;
using TownOfUsReworked.Enums;
using HarmonyLib;
using TownOfUsReworked.PlayerLayers.Roles.NeutralRoles.NeutralsMod;
using TownOfUsReworked.PlayerLayers.Objectifiers;
using TownOfUsReworked.PlayerLayers.Roles;
using System.Linq;
using TownOfUsReworked.Extensions;

namespace TownOfUsReworked.Data
{
    //Thanks to Town Of Host for this code
    [HarmonyPatch]
    public static class ConstantVariables
    {
        public static bool IsCountDown => GameStartManager.InstanceExists && GameStartManager.Instance.startState == GameStartManager.StartingStates.Countdown;
        public static bool IsInGame => AmongUsClient.Instance.GameState == InnerNetClient.GameStates.Started && !LobbyBehaviour.Instance;
        public static bool IsLobby => AmongUsClient.Instance.GameState == InnerNetClient.GameStates.Joined || LobbyBehaviour.Instance;
        public static bool IsEnded => AmongUsClient.Instance.GameState == InnerNetClient.GameStates.Ended;
        public static bool IsHnS => GameOptionsManager.Instance.CurrentGameOptions.GameMode == GameModes.HideNSeek;
        public static bool IsNormal => GameOptionsManager.Instance.CurrentGameOptions.GameMode == GameModes.Normal;
        public static bool IsOnlineGame => AmongUsClient.Instance.NetworkMode == NetworkModes.OnlineGame;
        public static bool IsLocalGame => AmongUsClient.Instance.NetworkMode == NetworkModes.LocalGame;
        public static bool IsFreePlay => AmongUsClient.Instance.NetworkMode == NetworkModes.FreePlay;
        public static bool IsRoaming => IsInGame && !MeetingHud.Instance && !Minigame.Instance;
        public static bool IsMeeting => IsInGame && MeetingHud.Instance;
        public static bool IsAA => CustomGameOptions.GameMode == GameMode.AllAny;
        public static bool IsCustom => CustomGameOptions.GameMode == GameMode.Custom;
        public static bool IsClassic => CustomGameOptions.GameMode == GameMode.Classic;
        public static bool IsKilling => CustomGameOptions.GameMode == GameMode.KillingOnly;
        public static bool IsVanilla => CustomGameOptions.GameMode == GameMode.Vanilla;
        public static bool CrewWins => !PlayerControl.AllPlayerControls.ToArray().Any(x => !x.Data.IsDead && !x.Data.Disconnected && (x.Is(Faction.Intruder) || x.Is(Faction.Syndicate) ||
            x.Is(RoleAlignment.NeutralKill) || x.Is(RoleAlignment.NeutralNeo) || x.Is(RoleAlignment.NeutralPros) || x.NotOnTheSameSide()) && !x.IsCrewAlly());
        public static bool IntrudersWin => !PlayerControl.AllPlayerControls.ToArray().Any(x => !x.Data.IsDead && !x.Data.Disconnected && (x.Is(Faction.Crew) || x.NotOnTheSameSide() ||
            x.Is(RoleAlignment.NeutralKill) || x.Is(Faction.Syndicate) || x.Is(RoleAlignment.NeutralNeo) || x.Is(RoleAlignment.NeutralPros)) && !x.IntruderSided());
        public static bool SyndicateWins => !PlayerControl.AllPlayerControls.ToArray().Any(x => !x.Data.IsDead && !x.Data.Disconnected && (x.Is(RoleAlignment.NeutralKill) ||
            x.Is(Faction.Intruder) || x.Is(RoleAlignment.NeutralNeo) || x.Is(Faction.Crew) || x.Is(RoleAlignment.NeutralPros) || x.NotOnTheSameSide()) && !x.SyndicateSided());
        public static bool AllNeutralsWin => (!PlayerControl.AllPlayerControls.ToArray().Any(x => !x.Data.IsDead && !x.Data.Disconnected && (x.NotOnTheSameSide() || x.Is(Faction.Crew) ||
            x.Is(Faction.Syndicate) || x.Is(Faction.Intruder)))) && CustomGameOptions.NoSolo == NoSolo.AllNeutrals;
        public static bool PestOrPBWins => !PlayerControl.AllPlayerControls.ToArray().Any(x => !x.Data.IsDead && !x.Data.Disconnected && (x.Is(Faction.Intruder) ||
            (x.Is(RoleAlignment.NeutralKill) && !x.Is(RoleEnum.Plaguebearer)) || x.Is(RoleAlignment.NeutralNeo) || x.Is(Faction.Syndicate) || (x.Is(RoleAlignment.NeutralPros) &&
            !x.Is(RoleEnum.Pestilence)) || x.Is(ObjectifierEnum.Allied) || x.Is(Faction.Crew) || x.NotOnTheSameSide()));
        public static bool AllNKsWin => (!PlayerControl.AllPlayerControls.ToArray().Any(x => !x.Data.IsDead && !x.Data.Disconnected && (x.Is(Faction.Intruder) ||
            x.Is(RoleAlignment.NeutralNeo) || x.Is(Faction.Syndicate) || x.Is(RoleAlignment.NeutralPros) || x.Is(Faction.Crew) || x.Is(ObjectifierEnum.Allied) || x.NotOnTheSameSide()))) &&
            CustomGameOptions.NoSolo == NoSolo.AllNKs;
        public static bool NoOneWins => !PlayerControl.AllPlayerControls.ToArray().Any(x => !x.Data.IsDead && !x.Data.Disconnected);
        public static bool CabalWin => !PlayerControl.AllPlayerControls.ToArray().Any(x => !x.Data.IsDead && !x.Data.Disconnected && (x.Is(Faction.Intruder) || x.IsBitten() ||
            x.Is(RoleAlignment.NeutralKill) || (x.Is(RoleAlignment.NeutralNeo) && !x.Is(RoleEnum.Jackal)) || x.Is(Faction.Syndicate) || x.Is(RoleAlignment.NeutralPros) ||
            x.Is(Faction.Crew) || x.Is(ObjectifierEnum.Lovers) || x.IsWinningRival() || x.IsResurrected() || x.IsPersuaded()) && !x.IsRecruit());
        public static bool UndeadWin => !PlayerControl.AllPlayerControls.ToArray().Any(x => !x.Data.IsDead && !x.Data.Disconnected && (x.Is(Faction.Intruder) || x.Is(Faction.Crew) ||
            x.Is(RoleAlignment.NeutralKill) || (x.Is(RoleAlignment.NeutralNeo) && !x.Is(RoleEnum.Dracula)) || x.Is(Faction.Syndicate) || x.Is(RoleAlignment.NeutralPros) ||
            x.Is(ObjectifierEnum.Lovers) || x.IsWinningRival() || x.IsResurrected() || x.IsRecruit() || x.IsPersuaded()) && !x.IsBitten());
        public static bool SectWin => !PlayerControl.AllPlayerControls.ToArray().Any(x => !x.Data.IsDead && !x.Data.Disconnected && (x.Is(Faction.Intruder) || x.Is(Faction.Crew) ||
            x.Is(RoleAlignment.NeutralKill) || (x.Is(RoleAlignment.NeutralNeo) && !x.Is(RoleEnum.Whisperer)) || x.Is(Faction.Syndicate) || x.Is(RoleAlignment.NeutralPros) ||
            x.Is(ObjectifierEnum.Lovers) || x.IsWinningRival() || x.IsRecruit() || x.IsResurrected() || x.IsBitten()) && !x.IsPersuaded());
        public static bool ReanimatedWin => !PlayerControl.AllPlayerControls.ToArray().Any(x => !x.Data.IsDead && !x.Data.Disconnected && (x.Is(Faction.Intruder) || x.IsPersuaded() ||
            x.Is(RoleAlignment.NeutralKill) || (x.Is(RoleAlignment.NeutralNeo) && !x.Is(RoleEnum.Necromancer)) || x.Is(Faction.Syndicate) || x.Is(RoleAlignment.NeutralPros) ||
            x.Is(Faction.Crew) || x.Is(ObjectifierEnum.Lovers) || x.IsWinningRival() || x.IsRecruit() || x.IsBitten()) && !x.IsResurrected());
        public static bool GameHasEnded => Role.SyndicateWin || Role.CrewWin || Role.NobodyWins || Role.IntruderWin || Role.AllNeutralsWin || Role.NKWins || Role.PhantomWins ||
            Role.ArsonistWins || Role.CabalWin || Role.SectWin || Role.UndeadWin || Role.ReanimatedWin || Role.CryomaniacWins || Role.JuggernautWins || Role.WerewolfWins ||
            Role.MurdererWins || Role.SerialKillerWins || Role.GlitchWins || Role.InfectorsWin || Objectifier.NobodyWins || Objectifier.LoveWins || Objectifier.RivalWins ||
            Objectifier.TaskmasterWins || Objectifier.OverlordWins || Objectifier.CorruptedWins;

        public static bool SameNKWins(RoleEnum nk) => nk != RoleEnum.Plaguebearer && !PlayerControl.AllPlayerControls.ToArray().Any(x => !x.Data.IsDead && !x.Data.Disconnected &&
            (x.Is(Faction.Intruder) || (x.Is(RoleAlignment.NeutralKill) && !x.Is(nk)) || x.Is(RoleAlignment.NeutralNeo) || x.Is(Faction.Syndicate) || x.Is(RoleAlignment.NeutralPros) ||
            x.Is(ObjectifierEnum.Allied) || x.Is(Faction.Crew) || x.NotOnTheSameSide())) && CustomGameOptions.NoSolo == NoSolo.SameNKs;

        public static bool SoloNKWins(RoleEnum nk, PlayerControl player) => nk != RoleEnum.Plaguebearer && !PlayerControl.AllPlayerControls.ToArray().Any(x => !x.Data.IsDead &&
            !x.Data.Disconnected && (x.Is(Faction.Intruder) || (x.Is(RoleAlignment.NeutralKill) && x != player) || x.Is(RoleAlignment.NeutralNeo) || x.Is(Faction.Syndicate) ||
            x.Is(RoleAlignment.NeutralPros) || x.Is(ObjectifierEnum.Allied) || x.Is(Faction.Crew) || x.NotOnTheSameSide())) && CustomGameOptions.NoSolo == NoSolo.Never;

        public static bool CorruptedWin(PlayerControl player)
        {
            if (!player.Is(ObjectifierEnum.Corrupted))
                return false;
            else if (CustomGameOptions.AllCorruptedWin)
                return !PlayerControl.AllPlayerControls.ToArray().Any(x => !x.Data.IsDead && !x.Data.Disconnected && !x.Is(ObjectifierEnum.Corrupted));
            else
                return !PlayerControl.AllPlayerControls.ToArray().Any(x => !x.Data.IsDead && !x.Data.Disconnected && !x.Is(ObjectifierEnum.Corrupted) && x != player);
        }

        public static bool LoversWin(PlayerControl player)
        {
            if (!player.Is(ObjectifierEnum.Lovers))
                return false;

            var flag1 = PlayerControl.AllPlayerControls.ToArray().Count(x => !x.Data.IsDead && !x.Data.Disconnected) <= 3;
            var flag2 = Objectifier.GetObjectifier<Lovers>(player).LoversAlive();
            return flag1 && flag2;
        }

        public static bool RivalsWin(PlayerControl player)
        {
            if (!player.Is(ObjectifierEnum.Rivals))
                return false;

            var flag1 = PlayerControl.AllPlayerControls.ToArray().Count(x => !x.Data.IsDead && !x.Data.Disconnected) <= 2;
            var flag2 = Objectifier.GetObjectifier<Rivals>(player).RivalDead();
            return flag1 && flag2;
        }
    }
}