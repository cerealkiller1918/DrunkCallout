using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using FivePD.API;
using FivePD.API.Utils;

namespace DrunkCallout
{
    [CalloutProperties("Drunk Fight","Cerealkiller1918","0.0.1")]
    public class DrunkFight : Callout
    {

        private Ped _suspect1, _suspect2;
        private readonly String[] items = {"Beer", "Wine", "Whiskey", "Moon shine"};
        Random rnd = new Random();

        public DrunkFight()
        {
            
            float offsetX = rnd.Next(100, 700);
            float offsetY = rnd.Next(100, 700);
            InitInfo(World.GetNextPositionOnStreet(Game.PlayerPed.GetPositionOffset(new Vector3(offsetX,offsetY, 0))));

            ShortName = "Drunk Fight";
            CalloutDescription = " A fight had broken out with intoxicated people";
            ResponseCode = 2;
            StartDistance = 75f;

        }

        public async override void OnStart(Ped player)
        {
            base.OnStart(player);

            _suspect1 = await SpawnPed(RandomUtils.GetRandomPed(), Location + 2);
            _suspect2 = await SpawnPed(RandomUtils.GetRandomPed(), Location + 2);
            
            Intoxicated.SetUpIntoxicatedPed(_suspect1,items);
            Intoxicated.SetUpIntoxicatedPed(_suspect2,items);;
            
            _suspect1.Task.FightAgainst(_suspect2);
            _suspect2.Task.FightAgainst(_suspect1);
        }
        
        public async override Task OnAccept()
        {
            InitBlip();
            UpdateData();
        }

    }
}