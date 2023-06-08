namespace TownOfUsReworked.Patches
{
    [HarmonyPatch(typeof(MedScanMinigame), nameof(MedScanMinigame.Begin))]
    public static class MedScanMinigamePatch
    {
        private const float oldHeightFeet = 3f;
        private const float oldHeightInch = 6f;
        private const float oldWeight = 92f;

        public static void Postfix(MedScanMinigame __instance)
        {
            var scale = 1f;

            //Update medical details for Giant and Dwarf modifiers based on game options
            if (PlayerControl.LocalPlayer.Is(ModifierEnum.Giant))
                scale = CustomGameOptions.GiantScale;
            else if (PlayerControl.LocalPlayer.Is(ModifierEnum.Dwarf))
                scale = CustomGameOptions.DwarfScale;

            var newHeightFeet = oldHeightFeet * scale;
            var newHeightInch = oldHeightInch * scale;
            var newWeight = oldWeight * scale;

            while (newHeightFeet.IsInRange(0, 1))
            {
                newHeightInch += 12 * newHeightFeet;
                newHeightFeet--;
            }

            if (newHeightFeet < 0f)
                newHeightFeet = 0f;

            while (newHeightInch >= 12)
            {
                newHeightFeet++;
                newHeightInch -= 12;
            }

            var weightString = $"{newWeight}lb";
            var heightString = $"{newHeightFeet}' {newHeightInch}\"";

            __instance.completeString = __instance.completeString.Replace("3' 6\"", heightString).Replace("92lb", weightString);
        }
    }

    [HarmonyPatch(typeof(MedScanMinigame), nameof(MedScanMinigame.FixedUpdate))]
    public static class MedScanMinigameFixedUpdatePatch
    {
        public static void Prefix(MedScanMinigame __instance)
        {
            if (CustomGameOptions.ParallelMedScans)
            {
                //Allows multiple medbay scans at once
                __instance.medscan.CurrentUser = PlayerControl.LocalPlayer.PlayerId;
                __instance.medscan.UsersList.Clear();
            }
        }
    }
}