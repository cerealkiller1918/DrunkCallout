using System;
using System.IO;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using FivePD.API;
using FivePD.API.Utils;

namespace DrunkCallout
{
    [CalloutProperties("Drunk Biker","Cerealkiller1918","0.0.1")]
    public class DrunkBiker: Callout
    {
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
            Ped driver = await SpawnPed(RandomUtils.GetRandomPed(), Location + 2);
            // need to add a list of bike to chose from.
            Vehicle bike = await SpawnVehicle(VehicleHash.TriBike, Location);
            
            Intoxicated.SetUpIntoxicatedPed(driver);

            PlayerData playerData = Utilities.GetPlayerData();
            string displayName = playerData.DisplayName;
            API.SetDriveTaskMaxCruiseSpeed(driver.GetHashCode(),35f);
            API.SetDriveTaskDrivingStyle(driver.GetHashCode(),524852);
            driver.Task.FleeFrom(player);
            

        }
        
        
    }
}