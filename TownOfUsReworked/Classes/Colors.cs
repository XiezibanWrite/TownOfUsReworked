﻿using UnityEngine;

namespace TownOfUsReworked.Classes
{
    class Colors
    {
        //Layer Colors
        public readonly static Color32 Role = new Color32(255, 215, 0, 255); //#FFD700FF
        public readonly static Color32 Modifier = new Color32(128, 128, 128, 255); //#7F7F7FFF
        public readonly static Color32 Ability = new Color32(255, 153, 0, 255); //#FF9900FF
        public readonly static Color32 Objectifier = new Color32(221, 88, 91, 255); //#DD585BFF
        public readonly static Color32 Faction = new Color32(0, 230, 109, 255); //#00E66DFF
        public readonly static Color32 SubFaction = new Color32(32, 77, 66, 255); //#204D42FF

        //Faction Colors
        public readonly static Color32 Crew = new Color32(139, 253, 253, 255); //#8BFDFDFF
        public readonly static Color32 Neutral = new Color32(179, 179, 179, 255); //#B3B3B3FF
        public readonly static Color32 Intruder = new Color32(255, 0, 0, 255); //#FF0000FF
        public readonly static Color32 Syndicate = new Color32(0, 128, 0, 255); //#008000FF
        public readonly static Color32 Other = new Color32(128, 128, 0, 255); //#808000FF

        //Subfaction Colors
        public readonly static Color32 Undead = new Color32(123, 137, 104, 255); //#7B8968FF
        public readonly static Color32 Cabal = new Color32(87, 86, 87, 255); //#575657FF
        public readonly static Color32 Reanimated = new Color32(230, 16, 138, 255); //#E6108AFF
        public readonly static Color32 Sect = new Color32(249, 149, 252, 255); //#F995FCFF

        //Crew Colors
        public readonly static Color32 Mayor = new Color32(112, 79, 168, 255); //#704FA8FF
        public readonly static Color32 Vigilante = new Color32(255, 255, 0, 255); //#FFFF00FF
        public readonly static Color32 Engineer = new Color32(255, 166, 10, 255); //#FFA60AFF
        public readonly static Color32 Swapper = new Color32(102, 230, 102, 255); //#66E666FF
        public readonly static Color32 TimeLord = new Color32(0, 0, 255, 255); //#0000FFFF
        public readonly static Color32 Medic = new Color32(0, 102, 0, 255); //#006600FF
        public readonly static Color32 Sheriff = new Color32(255, 204, 128, 255); //#FFCC80FF
        public readonly static Color32 Agent = new Color32(204, 163, 204, 255); //#CCA3CCFF
        public readonly static Color32 Altruist = new Color32(102, 0, 0, 255); //#660000FF
        public readonly static Color32 Veteran = new Color32(153, 128, 64, 255); //#998040FF
        public readonly static Color32 Tracker = new Color32(0, 153, 0, 255); //#009900FF
        public readonly static Color32 Transporter = new Color32(0, 238, 255, 255); //#00EEFFFF
        public readonly static Color32 Medium = new Color32(166, 128, 255, 255); //#A680FFFF
        public readonly static Color32 Coroner = new Color32(77, 153, 230, 255); //#4D99E6FF
        public readonly static Color32 Operative = new Color32(167, 209, 179, 255); //#A7D1B3FF
        public readonly static Color32 Detective = new Color32(77, 77, 255, 255); //#4D4DFFFF
        public readonly static Color32 Shifter = new Color32(223, 133, 31, 255); //#DF851FFF
        public readonly static Color32 VampireHunter = new Color32(192, 192, 192, 255); //#C0C0C0FF
        public readonly static Color32 Escort = new Color32(128, 51, 51, 255); //#803333FF
        public readonly static Color32 Inspector = new Color32 (126, 60, 100, 255); //#7E3C64FF
        public readonly static Color32 Revealer = new Color32(211, 211, 211, 255); //#D3D3D3FF
        public readonly static Color32 Mystic = new Color32(112, 142, 239, 255); //#708EEFFF
        public readonly static Color32 Retributionist = new Color32(141, 15, 140, 255); //#8D0F8CFF
        public readonly static Color32 Chameleon = new Color32(84, 17, 248, 255); //#5411F8FF
        public readonly static Color32 Seer = new Color32(113, 54, 138, 255); //#71368AFF

        //Neutral Colors
        public readonly static Color32 Jester = new Color32(247, 179, 218, 255); //#F7B3DAFF
        public readonly static Color32 Executioner = new Color32(204, 204, 204, 255); //#CCCCCCFF
        public readonly static Color32 Glitch = new Color32(0, 255, 0, 255); //#00FF00FF
        public readonly static Color32 Arsonist = new Color32(238, 118, 0, 255); //#EE7600FF
        public readonly static Color32 Amnesiac = new Color32(34, 255, 255, 255); //#22FFFFFF 
        public readonly static Color32 Survivor = new Color32(221, 221, 0, 255); //#DDDD00FF
        public readonly static Color32 GuardianAngel = new Color32(255, 255, 255, 255); //#FFFFFFFF
        public readonly static Color32 Plaguebearer = new Color32(207, 254, 97, 255); //#CFFE61FF
        public readonly static Color32 Pestilence = new Color32(66, 66, 66, 255); //#424242FF
        public readonly static Color32 Werewolf = new Color32(159, 112, 58, 255); //#9F703AFF
        public readonly static Color32 Cannibal = new Color32(140, 64, 5, 255); //#8C4005FF
        public readonly static Color32 Juggernaut = new Color32(161, 43, 86, 255); //#A12B56FF
        public readonly static Color32 Dracula = new Color32(172, 138, 0, 255); //#AC8A00FF
        public readonly static Color32 Murderer = new Color32(111, 123, 234, 255); //#6F7BEAFF
        public readonly static Color32 SerialKiller = new Color32(51, 110, 255, 255); //#336EFFFF
        public readonly static Color32 Cryomaniac = new Color32(100, 45, 234, 255); //#642DEAFF
        public readonly static Color32 Thief = new Color32(128, 255, 0, 255); //#80FF00FF
        public readonly static Color32 Troll = new Color32(103, 141, 54, 255); //#678D36FF
        public readonly static Color32 Pirate = new Color32(237, 194, 64, 255); //#EDC240FF
        public readonly static Color32 Jackal = new Color32(69, 7, 106, 255); //#45076AFF
        public readonly static Color32 Phantom = new Color32(102, 41, 98, 255); //#662962FF
        public readonly static Color32 Necromancer = new Color32(191, 95, 255, 255); //#BF5FFFFF
        public readonly static Color32 Whisperer = new Color32(45, 106, 165, 255); //#2D6AA5FF
        public readonly static Color32 Guesser = new Color32(238, 229, 190, 255); //#EEE5BEFF
        public readonly static Color32 Actor = new Color32(0, 172, 194, 255); //#00ACC2FF
        public readonly static Color32 BountyHunter = new Color32(181, 30, 57, 255); //#B51E39FF

        //Intruder Colors
        public readonly static Color32 Consigliere = new Color32(255, 255, 153, 255); //#FFFF99FF
        public readonly static Color32 Grenadier = new Color32(133, 170, 91, 255); //#85AA5BFF
        public readonly static Color32 Morphling = new Color32(187, 69, 176, 255); //#BB45B0FF
        public readonly static Color32 Wraith = new Color32(92, 79, 117, 255); //#5C4F75FF
        public readonly static Color32 Undertaker = new Color32(0, 86, 67, 255); //#005643FF
        public readonly static Color32 Camouflager = new Color32(55, 138, 192, 255); //#378AC0FF
        public readonly static Color32 Janitor = new Color32(38, 71, 162, 255); //#2647A2FF
        public readonly static Color32 Miner = new Color32(170, 118, 50, 255); //#AA7632FF
        public readonly static Color32 Blackmailer = new Color32(2, 167, 162, 255); //#02A752FF
        public readonly static Color32 Disguiser = new Color32(64, 180, 255, 255); //#40B4FFFF
        public readonly static Color32 TimeMaster = new Color32(0, 0, 167, 255); //#0000A7FF
        public readonly static Color32 Consort = new Color32(128, 23, 128, 255); //#801780FF
        public readonly static Color32 Teleporter = new Color32(106, 168, 79, 255); //#6AA84FFF
        public readonly static Color32 Godfather = new Color32(64, 76, 8, 255); //#404C08FF
        public readonly static Color32 Mafioso = new Color32(100, 0, 255, 255); //#6400FFFF
        public readonly static Color32 Ambusher = new Color32(43, 210, 156, 255); //#2BD29CFF

        //Syndicate Colors
        public readonly static Color32 Warper = new Color32 (140, 113, 64, 255); //#8C7140FF
        public readonly static Color32 Framer = new Color32(0, 255, 255, 255); //#00FFFFFF
        public readonly static Color32 Rebel = new Color32(255, 252, 206, 255); //#FFFCCEFF
        public readonly static Color32 Sidekick = new Color32(151, 156, 159, 255); //#979C9FFF
        public readonly static Color32 Concealer = new Color32(192, 37, 37, 255); //#C02525FF
        public readonly static Color32 Gorgon = new Color32(126, 77, 0, 255); //#7E4D00FF
        public readonly static Color32 Shapeshifter = new Color32(49, 28, 69, 255); //#311C45FF
        public readonly static Color32 Bomber = new Color32(201, 204, 63, 255); //#C9CC3FFF
        public readonly static Color32 Poisoner = new Color32(181, 0, 76, 255); //#B5004CFF
        public readonly static Color32 Drunkard = new Color32(30, 48, 11, 255); //#1E300BFF
        public readonly static Color32 Beamer = new Color32(0, 40, 245, 255); //#0028F5FF
        public readonly static Color32 Crusader = new Color32(223, 122, 232, 255); //#DF7AE8FF

        //Other Role Colors
        public readonly static Color32 Betrayer = new Color32(17, 128, 106, 255); //#11806AFF

        //Modifier Colors
        public readonly static Color32 Bait = new Color32(0, 179, 179, 255); //#00B3B3FF
        public readonly static Color32 Coward = new Color32(69, 107, 168, 255); //#456BA8FF
        public readonly static Color32 Diseased = new Color32(55, 77, 30, 255); //#374D1EFF
        public readonly static Color32 Drunk = new Color32(117, 128, 0, 255); //#758000FF
        public readonly static Color32 Dwarf = new Color32(255, 128, 128, 255); //#FF8080FF
        public readonly static Color32 Giant = new Color32(255, 179, 77, 255); //#FFB34DFF
        public readonly static Color32 Volatile = new Color32(255, 166, 10, 255); //#FFA60AFF
        public readonly static Color32 Flincher = new Color32(128, 179, 255, 255); //#80B3FFFF
        public readonly static Color32 VIP = new Color32(220, 238, 133, 255); //#DCEE85FF
        public readonly static Color32 Shy = new Color32(16, 2, 197, 255); //#1002C5FF
        public readonly static Color32 Professional = new Color32(134, 11, 122, 255); //#860B7AFF
        public readonly static Color32 Indomitable = new Color32(45, 229, 190, 255); //#2DE5BEFF

        //Ability Colors
        public readonly static Color32 Assassin = new Color32(7, 55, 99, 255); //#073763FF
        public readonly static Color32 Torch = new Color32(255, 255, 153, 255); //#FFFF99FF
        public readonly static Color32 Tunneler = new Color32(233, 30, 99, 255); //#E91E63FF
        public readonly static Color32 Lighter = new Color32(26, 255, 116, 255); //#1AFF74FF
        public readonly static Color32 ButtonBarry = new Color32(230, 0, 255, 255); //#E600FFFF
        public readonly static Color32 Tiebreaker = new Color32(153, 230, 153, 255); //#99E699FF
        public readonly static Color32 Snitch = new Color32(212, 174, 55, 255); //#D4AF37FF
        public readonly static Color32 Underdog = new Color32(132, 26, 127, 255); //#841A7FFF
        public readonly static Color32 Insider = new Color32(38, 252, 251, 255); //#26FCFBFF
        public readonly static Color32 Radar = new Color32(255, 0, 128, 255); //#FF0080FF
        public readonly static Color32 Multitasker = new Color32(255, 128, 77, 255); //#FF804DFF
        public readonly static Color32 Ruthless = new Color32(33, 96, 221, 255); //#2160DDFF
        public readonly static Color32 Ninja = new Color32(168, 67, 0, 0); //#A84300FF

        //Objectifier Colors
        public readonly static Color32 Lovers = new Color32(255, 102, 204, 255); //#FF66CCFF
        public readonly static Color32 Traitor = new Color32(55, 13, 67, 255); //#370D43FF
        public readonly static Color32 Rivals = new Color32(61, 45, 44, 255); //#3D2D2CFF
        public readonly static Color32 Fanatic = new Color32(103, 141, 54, 255); //#678D36FF
        public readonly static Color32 Taskmaster = new Color32(171, 171, 255, 255); //#ABABFFFF
        public readonly static Color32 Overlord = new Color32(0, 128, 128, 255); //#008080FF
        public readonly static Color32 Corrupted = new Color32(69, 69, 255, 255); //#4545FFFF
        public readonly static Color32 Allied = new Color32(69, 69, 169, 255); //#4545A9FF

        //Other
        public readonly static Color32 Stalemate = new Color32(239, 230, 230, 255); //#E6E6E6FF
        public readonly static Color32 Alignment = new Color32(29, 124, 242, 255); //#1D7CF2FF
        public readonly static Color32 Status = new Color32(155, 89, 182, 255); //#9B59B6FF
        public readonly static Color32 Clear = new Color32(0, 0, 0, 0); //#00000000
        public readonly static Color32 Objectives = new Color32(177, 72, 226, 255); //#B148E2FF
        public readonly static Color32 Attributes = new Color32(236, 28, 69, 255); //#EC1C45FF
        public readonly static Color32 Abilities = new Color32(32, 102, 148, 255); //#206694FF

        //Color Storage For Colors I Will Use Later
        //#dcee85 #6c29ab #800000 #808000 #008000 #800080 #000080 #2dff00 #e74c3c #992d22 #00FFFD #917ac0 #Eac1d2 #286e58 #db4f20 #abd432 #2e3b97 #ffd100 #fffcce #40b4ff #2684c1 #a82626
        //#4e4e4e #fffead #1abc9c #2ecc71 #1f8b4c #3498db #ad1457 #f1c40f #c27c0e #e67e22 #ffd2fb #ff7900 #805bc4 #95a5a6 #979c9f #888888 #ff7272 #f25ff1 #FF00FF
        //#6a1515 #569d29 #f1612b #7d86e1 #612bef #e7dae2 #F6AAB7 #EC62A5 #00EEFF #78c689 #e1c849 #a7c596 #fccc52 #6b2d2a #aab43e #FCBA03 #ff351f #F8CD46 #FF4D00 #7EFBC2
        //#8637C2 #3769FE #4d4d4d #8ff731 #2672FF #916640

        //Symbol Storage For Objectifiers I Will Make Later
        //⟡ ☆ ♡ ♧ ♤ ø ▶ ❥ ✔ ε Δ Γ ι κ ν σ τ υ φ ψ Ψ ω χ
    }
}
