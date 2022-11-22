﻿using UnityEngine;

namespace TownOfUsReworked.Lobby.Extras.RainbowMod
{
    public static class PalettePatch
    {
        public static void Load()
        {
            Palette.ColorNames = new[]
            {
                StringNames.ColorRed,
                StringNames.ColorBlue,
                StringNames.ColorGreen,
                StringNames.ColorPink,
                StringNames.ColorOrange,
                StringNames.ColorYellow,
                StringNames.ColorBlack,
                StringNames.ColorWhite,
                StringNames.ColorPurple,
                StringNames.ColorBrown,
                StringNames.ColorCyan,
                StringNames.ColorLime,
                StringNames.ColorMaroon,
                StringNames.ColorRose,
                StringNames.ColorBanana,
                StringNames.ColorGray,
                StringNames.ColorTan,
                StringNames.ColorCoral,
                // New colours
                (StringNames)999903,//"Watermelon",
                (StringNames)999904,//"Chocolate",
                (StringNames)999905,//"Sky Blue",
                (StringNames)999906,//"Beige",
                (StringNames)999907,//"Hot Pink",
                (StringNames)999908,//"Turquoise",
                (StringNames)999909,//"Lilac",
                (StringNames)999910,//"Olive",
                (StringNames)999911,//"Azure",
                (StringNames)999912,//"Tomato",
                (StringNames)999913,//"Backrooms",
                (StringNames)999914,//"Gold",
                (StringNames)999915,//"Space",
                (StringNames)999916,//"Ice",
                (StringNames)999917,//"Mint",
                (StringNames)999918,//"Behind the Slaughter",
                (StringNames)999919,//"Forest Green",
                (StringNames)999920,//"Donation",
                (StringNames)999921,//"Cherry",     
                (StringNames)999922,//"Toy",    
                (StringNames)999923,//"Pizzaria",
                (StringNames)999924,//"Starlight",  
                (StringNames)999925,//"Softball",
                (StringNames)999926,//"Dark Jester",
                (StringNames)999927,//"FRESH",  
                (StringNames)999928,//"Goner.",
                (StringNames)999929,//"Psychic Friend",
                (StringNames)999930,//"Frost",
                (StringNames)999931,//"Abyss Green",
                (StringNames)999932,//"Midnight",
                (StringNames)999933,//"<3",
                (StringNames)999934,//"Heat From Fire",
                (StringNames)999935,//"Fire From Heat",
                (StringNames)999936,//"Determination",
                (StringNames)999937,//"Patience",
                (StringNames)999938,//"Bravery",
                (StringNames)999939,//"Integrity",
                (StringNames)999940,//"Perserverance",
                (StringNames)999941,//"Kindness",
                (StringNames)999942,//"Justice",
                (StringNames)999943,//"Purple Plumber",
                (StringNames)999999,//"Rainbow",
            };

            Palette.PlayerColors = new[]
            {
                new Color32(198, 17, 17, byte.MaxValue),
                new Color32(19, 46, 210, byte.MaxValue),
                new Color32(17, 128, 45, byte.MaxValue),
                new Color32(238, 84, 187, byte.MaxValue),
                new Color32(240, 125, 13, byte.MaxValue),
                new Color32(246, 246, 87, byte.MaxValue),
                new Color32(63, 71, 78, byte.MaxValue),
                new Color32(215, 225, 241, byte.MaxValue),
                new Color32(107, 47, 188, byte.MaxValue),
                new Color32(113, 73, 30, byte.MaxValue),
                new Color32(56, byte.MaxValue, 221, byte.MaxValue),
                new Color32(80, 240, 57, byte.MaxValue),
                Palette.FromHex(6233390),
                Palette.FromHex(15515859),
                Palette.FromHex(15787944),
                Palette.FromHex(7701907),
                Palette.FromHex(9537655),
                Palette.FromHex(14115940),
                // New colours
                new Color32(168, 50, 62, byte.MaxValue),
                new Color32(60, 48, 44, byte.MaxValue),
                new Color32(61, 129, 255, byte.MaxValue),
                new Color32(240, 211, 165, byte.MaxValue),
                new Color32(236, 61, 255, byte.MaxValue),
                new Color32(61, 255, 181, byte.MaxValue),
                new Color32(186, 161, 255, byte.MaxValue),
                new Color32(97, 114, 24, byte.MaxValue),
                new Color32(1, 166, 255, byte.MaxValue),
                new Color32(255, 99, 71, byte.MaxValue),
                new Color32(182, 155, 26, byte.MaxValue),
                new Color32(218, 165, 32, byte.MaxValue),
                new Color32(0, 0, 0, byte.MaxValue),
                new Color32(191, 239, 255, byte.MaxValue),
                new Color32(206, 255, 191, byte.MaxValue),
                new Color32(145, 64, 167, byte.MaxValue),
                new Color32(0, 51, 25, byte.MaxValue),
                new Color32(35, 195, 120, byte.MaxValue),
                new Color32(132, 25, 52, byte.MaxValue),
                new Color32(126, 189, 101, byte.MaxValue),
                new Color32(252, 180, 0, byte.MaxValue),
                new Color32(255, 255, 255, byte.MaxValue),
                new Color32(161, 242, 0, byte.MaxValue),
                new Color32(35, 38, 64, byte.MaxValue),
                new Color32(0, 137, 137, byte.MaxValue),
                new Color32(102, 98, 98, byte.MaxValue),
                new Color32(255, 255, 87, byte.MaxValue),
                new Color32(156, 215, 222, byte.MaxValue),
                new Color32(56, 112, 94, byte.MaxValue),
                new Color32(48, 32, 113, byte.MaxValue),
                new Color32(148, 0, 26, byte.MaxValue),
                new Color32(245, 170, 185, byte.MaxValue),
                new Color32(91, 207, 250, byte.MaxValue),
                new Color32(255, 0, 0, byte.MaxValue),
                new Color32(66, 252, 255, byte.MaxValue),
                new Color32(252, 166, 0, byte.MaxValue),
                new Color32(0, 60, 255, byte.MaxValue),
                new Color32(213, 53, 217, byte.MaxValue),
                new Color32(0, 192, 0, byte.MaxValue),
                new Color32(255, 255, 0, byte.MaxValue),
                new Color32(55, 7, 109, byte.MaxValue),
                new Color32(10, 10, 10, byte.MaxValue),
            };
            
            Palette.ShadowColors = new[]
            {
                new Color32(122, 8, 56, byte.MaxValue),
                new Color32(9, 21, 142, byte.MaxValue),
                new Color32(10, 77, 46, byte.MaxValue),
                new Color32(172, 43, 174, byte.MaxValue),
                new Color32(180, 62, 21, byte.MaxValue),
                new Color32(195, 136, 34, byte.MaxValue),
                new Color32(30, 31, 38, byte.MaxValue),
                new Color32(132, 149, 192, byte.MaxValue),
                new Color32(59, 23, 124, byte.MaxValue),
                new Color32(94, 38, 21, byte.MaxValue),
                new Color32(36, 169, 191, byte.MaxValue),
                new Color32(21, 168, 66, byte.MaxValue),
                Palette.FromHex(4263706),
                Palette.FromHex(14586547),
                Palette.FromHex(13810825),
                Palette.FromHex(4609636),
                Palette.FromHex(5325118),
                Palette.FromHex(11813730),
                // New colours
                new Color32(101, 30, 37, byte.MaxValue),
                new Color32(30, 24, 22, byte.MaxValue),
                new Color32(31, 65, 128, byte.MaxValue),
                new Color32(120, 106, 83, byte.MaxValue),
                new Color32(118, 31, 128, byte.MaxValue),
                new Color32(31, 128, 91, byte.MaxValue),
                new Color32(93, 81, 128, byte.MaxValue),
                new Color32(66, 91, 15, byte.MaxValue),
                new Color32(17, 104, 151, byte.MaxValue),
                new Color32(215, 59, 41, byte.MaxValue),
                new Color32(142, 115, 6, byte.MaxValue),
                new Color32(188, 135, 2, byte.MaxValue),
                new Color32(0, 0, 0, byte.MaxValue),
                new Color32(141, 189, 215, byte.MaxValue),
                new Color32(176, 215, 141, byte.MaxValue),
                new Color32(105, 14, 117, byte.MaxValue),
                new Color32(0, 11, 5, byte.MaxValue),
                new Color32(5, 145, 80, byte.MaxValue),
                new Color32(92, 5, 12, byte.MaxValue),
                new Color32(86, 149, 61, byte.MaxValue),
                new Color32(252, 180, 0, byte.MaxValue),
                new Color32(255, 255, 255, byte.MaxValue),
                new Color32(161, 242, 0, byte.MaxValue),
                new Color32(35, 38, 64, byte.MaxValue),
                new Color32(0, 137, 137, byte.MaxValue),
                new Color32(102, 98, 98, byte.MaxValue),
                new Color32(255, 255, 87, byte.MaxValue),
                new Color32(156, 215, 222, byte.MaxValue),
                new Color32(56, 112, 94, byte.MaxValue),
                new Color32(48, 32, 113, byte.MaxValue),
                new Color32(148, 0, 26, byte.MaxValue),
                new Color32(245, 170, 185, byte.MaxValue),
                new Color32(91, 207, 250, byte.MaxValue),
                new Color32(255, 0, 0, byte.MaxValue),
                new Color32(66, 252, 255, byte.MaxValue),
                new Color32(252, 166, 0, byte.MaxValue),
                new Color32(0, 60, 255, byte.MaxValue),
                new Color32(213, 53, 217, byte.MaxValue),
                new Color32(0, 192, 0, byte.MaxValue),
                new Color32(255, 255, 0, byte.MaxValue),
                new Color32(55, 7, 109, byte.MaxValue),
                new Color32(0, 0, 0, byte.MaxValue),
            };
        }
    }
}
