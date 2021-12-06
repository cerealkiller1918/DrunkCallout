using System;
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

        public DrunkBiker()
        {
            InitInfo(World.GetNextPositionOnStreet(Game.PlayerPed.GetOffsetPosition(OffSets.RandomOffSet())));
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

            Intoxicated.SetUpIntoxicatedPed(_driver);

            PlayerData playerData = Utilities.GetPlayerData();
            string displayName = playerData.DisplayName;
            API.SetDriveTaskMaxCruiseSpeed(_driver.GetHashCode(), 35f);
            API.SetDriveTaskDrivingStyle(_driver.GetHashCode(), 524852);
            _driver.Task.FleeFrom(player);
            Killer.Notify("~o~Officer ~b~" + displayName + ",~o~ the biker is fleeing!");
            bike.AttachBlip();
        }

        public async override Task OnAccept()
        {
            InitBlip();
            PedData pedData = await Utilities.GetPedData(_driver.NetworkId);
            string firstname = pedData.FirstName;
            API.Wait(60000);
            Killer.DrawSubtitle("~r~[" + firstname + "] ~s~Are those police lights?", 5000);
            UpdateData();
        }

    }
}
        
        
    
