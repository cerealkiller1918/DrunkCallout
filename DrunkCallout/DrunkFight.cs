using System;
using System.Collections.Generic;
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
            
            
            SetUpIntoxicated(_suspect1);
            SetUpIntoxicated(_suspect2);
            
            _suspect1.Task.FightAgainst(_suspect2);
            _suspect2.Task.FightAgainst(_suspect1);
        }

        private void SetUpIntoxicated(Ped suspect)
        {
            PedData data = new PedData();

            List<Item> list = new List<Item>();
            float blood = rnd.Next(80, 150);
            
            data.BloodAlcoholLevel = blood/100;
            var itemName2 = items[rnd.Next(items.Length)];
            Item item2 = new Item()
            {
                Name = itemName2,
                IsIllegal = false
            };
            list.Add(item2);
            data.Items = list;
            Utilities.SetPedData(suspect.NetworkId, data);

            suspect.AlwaysKeepTask = true;
            suspect.BlockPermanentEvents = true;
            API.SetPedIsDrunk(suspect.GetHashCode(),true);
            suspect.AttachBlip();
        }

        public async override Task OnAccept()
        {
            InitBlip();
            UpdateData();
        }

    }
}