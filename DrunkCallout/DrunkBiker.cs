using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using FivePD.API;
using FivePD.API.Utils;

namespace DrunkCallout
{
    [CalloutProperties("Drunk Biker", "Cerealkiller1918", "0.0.1")]
    public class DrunkBiker : Callout
    {
        private Ped _driver;
        private Random rnd = new Random();

            public DrunkBiker()
        {
            float x = rnd.Next(100, 700);
            float y = rnd.Next(100, 700);
            Vector3 offset = new Vector3(x, y, 0);
            InitInfo(World.GetNextPositionOnStreet(Game.PlayerPed.GetOffsetPosition(offset)));
            ShortName = "Drunk Biker";
            CalloutDescription = "A person is riding intoxicated";
            ResponseCode = 3;
            StartDistance = 150f;
        }

        public async override void OnStart(Ped player)
        {
            base.OnStart(player);
            _driver = await SpawnPed(RandomUtils.GetRandomPed(), Location + 2);
            // need to add a list of bike to chose from.
            Vehicle bike = await SpawnVehicle(VehicleHash.TriBike, Location);

            String[] items = {"Beer", "Wine", "Whiskey", "Moon shine"}; 
            PedData data = new PedData();

            List<Item> list = new List<Item>();
            float blood = rnd.Next(80, 150);
            
            data.BloodAlcoholLevel = (float)Math.Round(blood/100,2);
            var itemName2 = items[rnd.Next(items.Length)];
            Item item2 = new Item()
            {
                Name = itemName2,
                IsIllegal = false
            };
            list.Add(item2);
            data.Items = list;
            Utilities.SetPedData(_driver.NetworkId, data);

            _driver.AlwaysKeepTask = true;
            _driver.BlockPermanentEvents = true;
            API.SetPedIsDrunk(_driver.GetHashCode(),true);
            _driver.AttachBlip();

            PlayerData playerData = Utilities.GetPlayerData();
            string displayName = playerData.DisplayName;
            API.SetDriveTaskMaxCruiseSpeed(_driver.GetHashCode(), 35f);
            API.SetDriveTaskDrivingStyle(_driver.GetHashCode(), 524852);
            _driver.Task.FleeFrom(player);
            Notify("~o~Officer ~b~" + displayName + ",~o~ the biker is fleeing!");
            bike.AttachBlip();
        }

        public async override Task OnAccept()
        {
            InitBlip();
            PedData pedData = await Utilities.GetPedData(_driver.NetworkId);
            string firstname = pedData.FirstName;
            API.Wait(60000);
            DrawSubtitle("~r~[" + firstname + "] ~s~Are those police lights?", 5000);
            UpdateData();
        }
        
        private static void Notify(string message)
        {
            API.BeginTextCommandThefeedPost("STRING");
            API.AddTextComponentSubstringPlayerName(message);
            API.EndTextCommandThefeedPostTicker(false, true);
        }
        private static void DrawSubtitle(string message, int duration)
        {
            API.BeginTextCommandPrint("STRING");
            API.AddTextComponentSubstringPlayerName(message);
            API.EndTextCommandPrint(duration, false);
        }

    }
}
        
        
    
