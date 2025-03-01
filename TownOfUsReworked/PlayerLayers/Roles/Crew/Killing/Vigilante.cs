using static TownOfUsReworked.Languages.Language;
namespace TownOfUsReworked.PlayerLayers.Roles
{
    public class Vigilante : Crew
    {
        public DateTime LastKilled;
        public bool KilledInno;
        public bool PreMeetingDie;
        public bool PostMeetingDie;
        public bool InnoMessage;
        public CustomButton ShootButton;
        public int UsesLeft;
        public bool ButtonUsable => UsesLeft > 0;
        public bool RoundOne;

        public Vigilante(PlayerControl player) : base(player)
        {
            Name = GetString("Vigilante");
            StartText = () => GetString("VigilanteStartText");
            AbilitiesText = () => GetString("VigilanteAbilitiesText");
            Color = CustomGameOptions.CustomCrewColors ? Colors.Vigilante : Colors.Crew;
            RoleType = RoleEnum.Vigilante;
            RoleAlignment = RoleAlignment.CrewKill;
            InspectorResults = InspectorResults.IsCold;
            UsesLeft = CustomGameOptions.VigiBulletCount;
            Type = LayerEnum.Vigilante;
            ShootButton = new(this, "Shoot", AbilityTypes.Direct, "ActionSecondary", Shoot, true);

            if (TownOfUsReworked.IsTest)
                Utils.LogSomething($"{Player.name} is {Name}");
        }

        public float KillTimer()
        {
            var timespan = DateTime.UtcNow - LastKilled;
            var num = Player.GetModifiedCooldown(CustomGameOptions.VigiKillCd) * 1000f;
            var flag2 = num - (float)timespan.TotalMilliseconds < 0f;
            return flag2 ? 0f : (num - (float)timespan.TotalMilliseconds) / 1000f;
        }

        public override void OnMeetingStart(MeetingHud __instance)
        {
            base.OnMeetingStart(__instance);

            if (PreMeetingDie)
                Utils.RpcMurderPlayer(Player, Player);
            else if (InnoMessage)
                Utils.HUD.Chat.AddChat(PlayerControl.LocalPlayer, GetString("VigilanteMessageKillWrong"));
        }

        public bool Exception(PlayerControl player) => player.Is(Faction) || (player.Is(SubFaction) && SubFaction != SubFaction.None) || player == Player.GetOtherLover() || player ==
            Player.GetOtherRival() || (player.Is(ObjectifierEnum.Mafia) && Player.Is(ObjectifierEnum.Mafia));

        public override void UpdateHud(HudManager __instance)
        {
            base.UpdateHud(__instance);
            ShootButton.Update("SHOOT", KillTimer(), CustomGameOptions.VigiKillCd, UsesLeft, ButtonUsable, ButtonUsable && !KilledInno && !RoundOne);
        }

        public void Shoot()
        {
            if (Utils.IsTooFar(Player, ShootButton.TargetPlayer) || KillTimer() != 0f || KilledInno || RoundOne)
                return;

            var target = ShootButton.TargetPlayer;
            var flag4 = target.Is(Faction.Intruder) || target.Is(RoleAlignment.NeutralKill) || target.Is(Faction.Syndicate) || target.Is(RoleEnum.Troll) || (target.Is(RoleEnum.Jester) &&
                CustomGameOptions.VigiKillsJester) || (target.Is(RoleEnum.Executioner) && CustomGameOptions.VigiKillsExecutioner) || (target.Is(RoleEnum.Cannibal) &&
                CustomGameOptions.VigiKillsCannibal) || (target.Is(RoleAlignment.NeutralBen) && CustomGameOptions.VigiKillsNB) || target.Is(RoleAlignment.NeutralNeo) ||
                target.Is(RoleAlignment.NeutralPros) || target.IsFramed() || Player.IsFramed() || Player.NotOnTheSameSide() || target.NotOnTheSameSide() ||
                Player.Is(ObjectifierEnum.Corrupted) || (target.Is(RoleEnum.BountyHunter) && CustomGameOptions.VigiKillsBH) || (target.Is(RoleEnum.Actor) &&
                CustomGameOptions.VigiKillsActor) || target.Is(RoleAlignment.NeutralHarb);
            var interact = Utils.Interact(Player, target, flag4);

            if (interact[3])
            {
                if (flag4 && !Player.IsFramed())
                {
                    KilledInno = false;
                    LastKilled = DateTime.UtcNow;
                }
                else
                {
                    if (CustomGameOptions.MisfireKillsInno)
                        Utils.RpcMurderPlayer(Player, target);

                    if (Local && CustomGameOptions.VigiNotifOptions == VigiNotif.Flash && CustomGameOptions.VigiOptions != VigiOptions.Immediate)
                        Utils.Flash(Color);
                    else if (CustomGameOptions.VigiNotifOptions == VigiNotif.Message && CustomGameOptions.VigiOptions != VigiOptions.Immediate)
                        InnoMessage = true;

                    LastKilled = DateTime.UtcNow;
                    KilledInno = !CustomGameOptions.VigiKillAgain;

                    if (CustomGameOptions.VigiOptions == VigiOptions.Immediate)
                        Utils.RpcMurderPlayer(Player, Player);
                    else if (CustomGameOptions.VigiOptions == VigiOptions.PreMeeting)
                        PreMeetingDie = true;
                    else if (CustomGameOptions.VigiOptions == VigiOptions.PostMeeting)
                        PostMeetingDie = true;
                }

                UsesLeft--;
            }
            else if (interact[0])
                LastKilled = DateTime.UtcNow;
            else if (interact[1])
                LastKilled.AddSeconds(CustomGameOptions.ProtectKCReset);
            else if (interact[2])
                LastKilled.AddSeconds(CustomGameOptions.VestKCReset);
        }
    }
}