using HarmonyLib;
using Il2CppSystem.Collections.Generic;
using Reactor.Utilities;
using System.Linq;
using TownOfUsReworked.Enums;
using TownOfUsReworked.PlayerLayers.Roles;
using TownOfUsReworked.PlayerLayers.Roles.Roles;
using TownOfUsReworked.PlayerLayers.Objectifiers.PhantomMod;
using TownOfUsReworked.PlayerLayers.Objectifiers.LoversMod;
using TownOfUsReworked.PlayerLayers.Objectifiers;
using TownOfUsReworked.Extensions;

namespace TownOfUsReworked.Patches
{
    [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.OnGameEnd))]
    public class AmongUsClient_OnGameEnd
    {
        public static void Postfix(AmongUsClient __instance, [HarmonyArgument(0)] EndGameResult endGameResult)
        {
            Utils.potentialWinners.Clear();

            foreach (var player in PlayerControl.AllPlayerControls)
                Utils.potentialWinners.Add(new WinningPlayerData(player.Data));
        }
    }

    [HarmonyPatch(typeof(EndGameManager), nameof(EndGameManager.Start))]
    public class EndGameManager_SetEverythingUp
    {
        public static void Prefix()
        {
            var toRemoveColorIds = Role.AllRoles.Where(o => o.LostByRPC).Select(o => o.Player.Data.DefaultOutfit.ColorId).ToArray();
            var toRemoveWinners = TempData.winners.ToArray().Where(o => toRemoveColorIds.Contains(o.ColorId)).ToArray();

            for (int i = 0; i < toRemoveWinners.Count(); i++)
                TempData.winners.Remove(toRemoveWinners[i]);

            if (Role.NobodyWins)
            {
                TempData.winners = new List<WinningPlayerData>();
                TempData.winners.Clear();
                return;
            }

            if (Role.NBOnlyWin)
            {
                var winners = new List<WinningPlayerData>();

                foreach (var role in Role.GetRoles(RoleEnum.Survivor))
                {
                    var surv = (Survivor)role;

                    if (!surv.Player.Data.IsDead && !surv.Player.Data.Disconnected)
                        winners.Add(Utils.potentialWinners.Where(x => x.PlayerName == surv.PlayerName).ToList()[0]);
                }

                foreach (var role in Role.GetRoles(RoleEnum.GuardianAngel))
                {
                    var ga = (GuardianAngel)role;

                    if (!ga.target.Data.IsDead && !ga.Player.Data.Disconnected)
                        winners.Add(Utils.potentialWinners.Where(x => x.PlayerName == ga.PlayerName).ToList()[0]);
                }

                TempData.winners = new List<WinningPlayerData>();

                foreach (var win in winners)
                    TempData.winners.Add(win);

                return;
            }

            foreach (var role in Role.AllRoles)
            {
                var type = role.RoleType;
                var winners = new List<WinningPlayerData>();

                if (type == RoleEnum.Glitch)
                {
                    var glitch = (Glitch)role;

                    if (glitch.GlitchWins)
                    {
                        

                        foreach (var role2 in Role.GetRoles(RoleEnum.Survivor))
                        {
                            var surv = (Survivor)role2;

                            if (!surv.Player.Data.IsDead && !surv.Player.Data.Disconnected)
                                winners.Add(Utils.potentialWinners.Where(x => x.PlayerName == surv.PlayerName).ToList()[0]);
                        }
                            
                        foreach (var role2 in Role.GetRoles(RoleEnum.GuardianAngel))
                        {
                            var ga = (GuardianAngel)role2;

                            if (!ga.target.Data.IsDead && !ga.Player.Data.Disconnected)
                                winners.Add(Utils.potentialWinners.Where(x => x.PlayerName == ga.PlayerName).ToList()[0]);
                        }

                        foreach (var role2 in Role.GetRoles(RoleEnum.Glitch))
                        {                            
                            var glitch2 = (Glitch)role2;
                            winners.Add(Utils.potentialWinners.Where(x => x.PlayerName == glitch2.PlayerName).ToList()[0]);
                        }

                        foreach (var role2 in Role.GetRoles(RoleEnum.Jester))
                        {
                            var jest = (Jester)role2;

                            if (jest.VotedOut)
                                winners.Add(Utils.potentialWinners.Where(x => x.PlayerName == jest.PlayerName).ToList()[0]);
                        }

                        foreach (var role2 in Role.GetRoles(RoleEnum.Executioner))
                        {
                            var exe = (Executioner)role2;

                            if (exe.TargetVotedOut)
                                winners.Add(Utils.potentialWinners.Where(x => x.PlayerName == exe.PlayerName).ToList()[0]);
                        }

                        TempData.winners = new List<WinningPlayerData>();

                        foreach (var win in winners)
                            TempData.winners.Add(win);

                        return;
                    }
                }
                else if (type == RoleEnum.Juggernaut)
                {
                    var juggernaut = (Juggernaut)role;

                    if (juggernaut.JuggernautWins)
                    {
                        foreach (var role2 in Role.GetRoles(RoleEnum.Survivor))
                        {
                            var surv = (Survivor)role2;

                            if (!surv.Player.Data.IsDead && !surv.Player.Data.Disconnected)
                                winners.Add(Utils.potentialWinners.Where(x => x.PlayerName == surv.PlayerName).ToList()[0]);
                                
                        }
                            
                        foreach (var role2 in Role.GetRoles(RoleEnum.GuardianAngel))
                        {
                            var ga = (GuardianAngel)role2;

                            if (!ga.target.Data.IsDead && !ga.Player.Data.Disconnected)
                                winners.Add(Utils.potentialWinners.Where(x => x.PlayerName == ga.PlayerName).ToList()[0]);
                        }

                        foreach (var role2 in Role.GetRoles(RoleEnum.Juggernaut))
                        {
                            var jugg = (Juggernaut)role2;
                            winners.Add(Utils.potentialWinners.Where(x => x.PlayerName == jugg.PlayerName).ToList()[0]);
                        }

                        foreach (var role2 in Role.GetRoles(RoleEnum.Jester))
                        {
                            var jest = (Jester)role2;

                            if (jest.VotedOut)
                                winners.Add(Utils.potentialWinners.Where(x => x.PlayerName == jest.PlayerName).ToList()[0]);
                        }

                        foreach (var role2 in Role.GetRoles(RoleEnum.Executioner))
                        {
                            var exe = (Executioner)role2;

                            if (exe.TargetVotedOut)
                                winners.Add(Utils.potentialWinners.Where(x => x.PlayerName == exe.PlayerName).ToList()[0]);
                        }

                        TempData.winners = new List<WinningPlayerData>();

                        foreach (var win in winners)
                            TempData.winners.Add(win);

                        return;
                    }
                }
                else if (type == RoleEnum.Arsonist)
                {
                    var arsonist = (Arsonist)role;

                    if (arsonist.ArsonistWins)
                    {
                        foreach (var role2 in Role.GetRoles(RoleEnum.Survivor))
                        {
                            var surv = (Survivor)role2;

                            if (!surv.Player.Data.IsDead && !surv.Player.Data.Disconnected)
                                winners.Add(Utils.potentialWinners.Where(x => x.PlayerName == surv.PlayerName).ToList()[0]);
                        }
                            
                        foreach (var role2 in Role.GetRoles(RoleEnum.GuardianAngel))
                        {
                            var ga = (GuardianAngel)role2;

                            if (!ga.target.Data.IsDead && !ga.Player.Data.Disconnected)
                                winners.Add(Utils.potentialWinners.Where(x => x.PlayerName == ga.PlayerName).ToList()[0]);
                        }

                        foreach (var role2 in Role.GetRoles(RoleEnum.Arsonist))
                        {
                            var arso = (Arsonist)role2;
                            winners.Add(Utils.potentialWinners.Where(x => x.PlayerName == arso.PlayerName).ToList()[0]);
                        }

                        foreach (var role2 in Role.GetRoles(RoleEnum.Jester))
                        {
                            var jest = (Jester)role2;

                            if (jest.VotedOut)
                                winners.Add(Utils.potentialWinners.Where(x => x.PlayerName == jest.PlayerName).ToList()[0]);
                        }

                        foreach (var role2 in Role.GetRoles(RoleEnum.Executioner))
                        {
                            var exe = (Executioner)role2;

                            if (exe.TargetVotedOut)
                                winners.Add(Utils.potentialWinners.Where(x => x.PlayerName == exe.PlayerName).ToList()[0]);
                        }

                        TempData.winners = new List<WinningPlayerData>();

                        foreach (var win in winners)
                            TempData.winners.Add(win);

                        return;
                    }
                }
                else if (type == RoleEnum.Plaguebearer)
                {
                    var plaguebearer = (Plaguebearer)role;

                    if (plaguebearer.PlaguebearerWins)
                    {
                        foreach (var role2 in Role.GetRoles(RoleEnum.Survivor))
                        {
                            var surv = (Survivor)role2;

                            if (!surv.Player.Data.IsDead && !surv.Player.Data.Disconnected)
                                winners.Add(Utils.potentialWinners.Where(x => x.PlayerName == surv.PlayerName).ToList()[0]);
                        }
                            
                        foreach (var role2 in Role.GetRoles(RoleEnum.GuardianAngel))
                        {
                            var ga = (GuardianAngel)role2;

                            if (!ga.target.Data.IsDead && !ga.Player.Data.Disconnected)
                                winners.Add(Utils.potentialWinners.Where(x => x.PlayerName == ga.PlayerName).ToList()[0]);
                        }
                            
                        foreach (var role2 in Role.GetRoles(RoleEnum.Plaguebearer))
                        {
                            var pb = (Plaguebearer)role2;
                            winners.Add(Utils.potentialWinners.Where(x => x.PlayerName == pb.PlayerName).ToList()[0]);
                        }
                            
                        foreach (var role2 in Role.GetRoles(RoleEnum.Pestilence))
                        {
                            var pest = (Pestilence)role2;
                            winners.Add(Utils.potentialWinners.Where(x => x.PlayerName == pest.PlayerName).ToList()[0]);
                        }

                        foreach (var role2 in Role.GetRoles(RoleEnum.Jester))
                        {
                            var jest = (Jester)role2;

                            if (jest.VotedOut)
                                winners.Add(Utils.potentialWinners.Where(x => x.PlayerName == jest.PlayerName).ToList()[0]);
                        }

                        foreach (var role2 in Role.GetRoles(RoleEnum.Executioner))
                        {
                            var exe = (Executioner)role2;

                            if (exe.TargetVotedOut)
                                winners.Add(Utils.potentialWinners.Where(x => x.PlayerName == exe.PlayerName).ToList()[0]);
                        }

                        TempData.winners = new List<WinningPlayerData>();

                        foreach (var win in winners)
                            TempData.winners.Add(win);

                        return;
                    }
                }
                else if (type == RoleEnum.Pestilence)
                {
                    var pestilence = (Pestilence)role;

                    if (pestilence.PestilenceWins)
                    {
                        foreach (var role2 in Role.GetRoles(RoleEnum.Survivor))
                        {
                            var surv = (Survivor)role2;

                            if (!surv.Player.Data.IsDead && !surv.Player.Data.Disconnected)
                                winners.Add(Utils.potentialWinners.Where(x => x.PlayerName == surv.PlayerName).ToList()[0]);
                        }
                            
                        foreach (var role2 in Role.GetRoles(RoleEnum.GuardianAngel))
                        {
                            var ga = (GuardianAngel)role2;

                            if (!ga.target.Data.IsDead && !ga.Player.Data.Disconnected)
                                winners.Add(Utils.potentialWinners.Where(x => x.PlayerName == ga.PlayerName).ToList()[0]);
                        }
                            
                        foreach (var role2 in Role.GetRoles(RoleEnum.Plaguebearer))
                        {
                            var pb = (Plaguebearer)role2;
                            winners.Add(Utils.potentialWinners.Where(x => x.PlayerName == pb.PlayerName).ToList()[0]);
                        }
                            
                        foreach (var role2 in Role.GetRoles(RoleEnum.Pestilence))
                        {
                            var pest = (Pestilence)role2;
                            winners.Add(Utils.potentialWinners.Where(x => x.PlayerName == pest.PlayerName).ToList()[0]);
                        }

                        foreach (var role2 in Role.GetRoles(RoleEnum.Jester))
                        {
                            var jest = (Jester)role2;

                            if (jest.VotedOut)
                                winners.Add(Utils.potentialWinners.Where(x => x.PlayerName == jest.PlayerName).ToList()[0]);
                        }

                        foreach (var role2 in Role.GetRoles(RoleEnum.Executioner))
                        {
                            var exe = (Executioner)role2;

                            if (exe.TargetVotedOut)
                                winners.Add(Utils.potentialWinners.Where(x => x.PlayerName == exe.PlayerName).ToList()[0]);
                        }

                        TempData.winners = new List<WinningPlayerData>();

                        foreach (var win in winners)
                            TempData.winners.Add(win);

                        return;
                    }
                }
                else if (type == RoleEnum.SerialKiller)
                {
                    var serialkiller = (SerialKiller)role;

                    if (serialkiller.SerialKillerWins)
                    {
                        foreach (var role2 in Role.GetRoles(RoleEnum.Survivor))
                        {
                            var surv = (Survivor)role2;

                            if (!surv.Player.Data.IsDead && !surv.Player.Data.Disconnected)
                                winners.Add(Utils.potentialWinners.Where(x => x.PlayerName == surv.PlayerName).ToList()[0]);
                        }
                                
                        foreach (var role2 in Role.GetRoles(RoleEnum.GuardianAngel))
                        {
                            var ga = (GuardianAngel)role2;

                            if (!ga.target.Data.IsDead && !ga.Player.Data.Disconnected)
                                winners.Add(Utils.potentialWinners.Where(x => x.PlayerName == ga.PlayerName).ToList()[0]);
                        }

                        foreach (var role2 in Role.GetRoles(RoleEnum.SerialKiller))
                        {
                            var sk = (SerialKiller)role2;
                            winners.Add(Utils.potentialWinners.Where(x => x.PlayerName == sk.PlayerName).ToList()[0]);
                        }

                        foreach (var role2 in Role.GetRoles(RoleEnum.Jester))
                        {
                            var jest = (Jester)role2;

                            if (jest.VotedOut)
                                winners.Add(Utils.potentialWinners.Where(x => x.PlayerName == jest.PlayerName).ToList()[0]);
                        }

                        foreach (var role2 in Role.GetRoles(RoleEnum.Executioner))
                        {
                            var exe = (Executioner)role2;

                            if (exe.TargetVotedOut)
                                winners.Add(Utils.potentialWinners.Where(x => x.PlayerName == exe.PlayerName).ToList()[0]);
                        }

                        TempData.winners = new List<WinningPlayerData>();

                        foreach (var win in winners)
                            TempData.winners.Add(win);

                        return;
                    }
                }
                else if (type == RoleEnum.Murderer)
                {
                    var murderer = (Murderer)role;

                    if (murderer.MurdWins)
                    {
                        foreach (var role2 in Role.GetRoles(RoleEnum.Survivor))
                        {
                            var surv = (Survivor)role2;

                            if (!surv.Player.Data.IsDead && !surv.Player.Data.Disconnected)
                                winners.Add(Utils.potentialWinners.Where(x => x.PlayerName == surv.PlayerName).ToList()[0]);
                        }
                            
                        foreach (var role2 in Role.GetRoles(RoleEnum.GuardianAngel))
                        {
                            var ga = (GuardianAngel)role2;

                            if (!ga.target.Data.IsDead && !ga.Player.Data.Disconnected)
                                winners.Add(Utils.potentialWinners.Where(x => x.PlayerName == ga.PlayerName).ToList()[0]);
                        }

                        foreach (var role2 in Role.GetRoles(RoleEnum.Murderer))
                        {
                            var murd = (Murderer)role2;
                            winners.Add(Utils.potentialWinners.Where(x => x.PlayerName == murd.PlayerName).ToList()[0]);
                        }

                        foreach (var role2 in Role.GetRoles(RoleEnum.Jester))
                        {
                            var jest = (Jester)role2;

                            if (jest.VotedOut)
                                winners.Add(Utils.potentialWinners.Where(x => x.PlayerName == jest.PlayerName).ToList()[0]);
                        }

                        foreach (var role2 in Role.GetRoles(RoleEnum.Executioner))
                        {
                            var exe = (Executioner)role2;

                            if (exe.TargetVotedOut)
                                winners.Add(Utils.potentialWinners.Where(x => x.PlayerName == exe.PlayerName).ToList()[0]);
                        }

                        TempData.winners = new List<WinningPlayerData>();

                        foreach (var win in winners)
                            TempData.winners.Add(win);
                            
                        return;
                    }
                }
                else if (type == RoleEnum.Cannibal)
                {
                    var cannibal = (Cannibal)role;

                    if (cannibal.EatNeed == 0)
                    {
                        foreach (var role2 in Role.GetRoles(RoleEnum.Survivor))
                        {
                                var surv = (Survivor)role2;

                            if (!surv.Player.Data.IsDead && !surv.Player.Data.Disconnected)
                                winners.Add(Utils.potentialWinners.Where(x => x.PlayerName == surv.PlayerName).ToList()[0]);
                        }
                            
                        foreach (var role2 in Role.GetRoles(RoleEnum.GuardianAngel))
                        {
                            var ga = (GuardianAngel)role2;

                            if (!ga.target.Data.IsDead && !ga.Player.Data.Disconnected)
                                winners.Add(Utils.potentialWinners.Where(x => x.PlayerName == ga.PlayerName).ToList()[0]);
                        }

                        foreach (var role2 in Role.GetRoles(RoleEnum.Jester))
                        {
                            var jest = (Jester)role2;

                            if (jest.VotedOut)
                                winners.Add(Utils.potentialWinners.Where(x => x.PlayerName == jest.PlayerName).ToList()[0]);
                        }

                        foreach (var role2 in Role.GetRoles(RoleEnum.Executioner))
                        {
                            var exe = (Executioner)role2;

                            if (exe.TargetVotedOut)
                                winners.Add(Utils.potentialWinners.Where(x => x.PlayerName == exe.PlayerName).ToList()[0]);
                        }

                        TempData.winners = new List<WinningPlayerData>();

                        foreach (var win in winners)
                            TempData.winners.Add(win);
                            
                        return;
                    }
                }
                else if (type == RoleEnum.Taskmaster)
                {
                    var taskmaster = (Taskmaster)role;

                    if (taskmaster.WinTasksDone)
                    {
                        foreach (var role2 in Role.GetRoles(RoleEnum.Survivor))
                        {
                            var surv = (Survivor)role2;

                            if (!surv.Player.Data.IsDead && !surv.Player.Data.Disconnected)
                                winners.Add(Utils.potentialWinners.Where(x => x.PlayerName == surv.PlayerName).ToList()[0]);
                        }
                            
                        foreach (var role2 in Role.GetRoles(RoleEnum.GuardianAngel))
                        {
                            var ga = (GuardianAngel)role2;

                            if (!ga.target.Data.IsDead && !ga.Player.Data.Disconnected)
                                winners.Add(Utils.potentialWinners.Where(x => x.PlayerName == ga.PlayerName).ToList()[0]);
                        }

                        foreach (var role2 in Role.GetRoles(RoleEnum.Jester))
                        {
                            var jest = (Jester)role2;

                            if (jest.VotedOut)
                                winners.Add(Utils.potentialWinners.Where(x => x.PlayerName == jest.PlayerName).ToList()[0]);
                        }

                        foreach (var role2 in Role.GetRoles(RoleEnum.Executioner))
                        {
                            var exe = (Executioner)role2;

                            if (exe.TargetVotedOut)
                                winners.Add(Utils.potentialWinners.Where(x => x.PlayerName == exe.PlayerName).ToList()[0]);
                        }

                        TempData.winners = new List<WinningPlayerData>();

                        foreach (var win in winners)
                            TempData.winners.Add(win);
                            
                        return;
                    }
                }

                winners.Clear();
            }

            foreach (var objectifier in Objectifier.AllObjectifiers)
            {
                var type = objectifier.ObjectifierType;
                var winners = new List<WinningPlayerData>();

                if (type == ObjectifierEnum.Lovers)
                {
                    var lover = (Lovers)objectifier;

                    if (lover.LoveCoupleWins)
                    {
                        var otherLover = lover.OtherLover;

                        foreach (var player in Utils.potentialWinners)
                        {
                            if (player.PlayerName == lover.PlayerName | player.PlayerName == otherLover.PlayerName)
                                winners.Add(player);
                        }

                        TempData.winners = new List<WinningPlayerData>();

                        foreach (var win in winners)
                            TempData.winners.Add(win);

                        return;
                    }
                }
                else if (type == ObjectifierEnum.Phantom)
                {
                    var phantom = (Phantom)objectifier;

                    if (phantom.CompletedTasks)
                    {
                        foreach (var role2 in Role.GetRoles(RoleEnum.Survivor))
                        {
                            var surv = (Survivor)role2;
                            if (!surv.Player.Data.IsDead && !surv.Player.Data.Disconnected)
                                winners.Add(Utils.potentialWinners.Where(x => x.PlayerName == surv.PlayerName).ToList()[0]);
                        }
                            
                        foreach (var role2 in Role.GetRoles(RoleEnum.GuardianAngel))
                        {
                            var ga = (GuardianAngel)role2;

                            if (!ga.target.Data.IsDead && !ga.Player.Data.Disconnected)
                                winners.Add(Utils.potentialWinners.Where(x => x.PlayerName == ga.PlayerName).ToList()[0]);
                        }

                        foreach (var role2 in Role.GetRoles(RoleEnum.Jester))
                        {
                            var jest = (Jester)role2;

                            if (jest.VotedOut)
                                winners.Add(Utils.potentialWinners.Where(x => x.PlayerName == jest.PlayerName).ToList()[0]);
                        }

                        foreach (var role2 in Role.GetRoles(RoleEnum.Executioner))
                        {
                            var exe = (Executioner)role2;

                            if (exe.TargetVotedOut)
                                winners.Add(Utils.potentialWinners.Where(x => x.PlayerName == exe.PlayerName).ToList()[0]);
                        }

                        TempData.winners = new List<WinningPlayerData>();

                        foreach (var win in winners)
                            TempData.winners.Add(win);
                        
                        return;
                    }
                }
            }
        }
    }
}
