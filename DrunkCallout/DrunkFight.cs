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
        Random rnd = new Random();
        public DrunkFight()
        {
            float x = rnd.Next(100, 700);
            float y = rnd.Next(100, 700);
            Vector3 offset = new Vector3(x, y, 0);
            InitInfo(World.GetNextPositionOnStreet(Game.PlayerPed.GetPositionOffset(offset)));

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
            
            String[] items = {"Beer", "Wine", "Whiskey", "Moon shine"};
            
            PedData data1 = new PedData();

            List<Item> list1 = new List<Item>();
            float blood1 = rnd.Next(80, 150);
            
            data1.BloodAlcoholLevel = (float)Math.Round(blood1/100,2);
            var itemName1 = items[rnd.Next(items.Length)];
            Item item1 = new Item()
            {
                Name = itemName1,
                IsIllegal = false
            };
            list1.Add(item1);
            data1.Items = list1;
            Utilities.SetPedData(suspect1.NetworkId, data1);

            suspect1.AlwaysKeepTask = true;
            suspect1.BlockPermanentEvents = true;
            API.SetPedIsDrunk(suspect1.GetHashCode(),true);
            suspect1.AttachBlip();
           
          ////////////////////////////////
            PedData data2 = new PedData();

            List<Item> list2 = new List<Item>();
            float blood2 = rnd.Next(80, 150);
            
            data2.BloodAlcoholLevel = blood2/100;
            var itemName2 = items[rnd.Next(items.Length)];
            Item item2 = new Item()
            {
                Name = itemName2,
                IsIllegal = false
            };
            list2.Add(item2);
            data2.Items = list2;
            Utilities.SetPedData(suspect2.NetworkId, data2);

            suspect2.AlwaysKeepTask = true;
            suspect2.BlockPermanentEvents = true;
            API.SetPedIsDrunk(suspect2.GetHashCode(),true);
            suspect2.AttachBlip();
            
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