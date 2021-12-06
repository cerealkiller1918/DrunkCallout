using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using FivePD.API;
using FivePD.API.Utils;

namespace DrunkCallout
{
    [CalloutProperties("Drunk Fight","Cerealkiller1918","0.0.1")]
    public class DrunkFight : Callout
    {
        public DrunkFight()
        {
            InitInfo(World.GetNextPositionOnStreet(Game.PlayerPed.GetPositionOffset(OffSets.RandomOffSet())));

            ShortName = "Drunk Fight";
            CalloutDescription = " A fight had broken out with intoxicated people";
            ResponseCode = 2;
            StartDistance = 75f;

        }
        public async override void OnStart(Ped player)
        {
            base.OnStart(player);
            Ped suspect1 = await SpawnPed(RandomUtils.GetRandomPed(), Location + 2);
            Ped suspect2 = await SpawnPed(RandomUtils.GetRandomPed(), Location + 2);
            
            Intoxicated.SetUpIntoxicatedPed(suspect1);
            Intoxicated.SetUpIntoxicatedPed(suspect2);;
            
            suspect1.Task.FightAgainst(suspect2);
            suspect2.Task.FightAgainst(suspect1);
        }
        public async override Task OnAccept()
        {
            InitBlip();
            UpdateData();
        }

    }
}