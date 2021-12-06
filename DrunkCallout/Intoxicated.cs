using System;
using System.Collections.Generic;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using FivePD.API;
using FivePD.API.Utils;

namespace DrunkCallout
{
    public static class Intoxicated
    {
        public static void SetUpIntoxicatedPed(Ped suspect, String[] items)
        {
            Random rnd = new Random();
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
    }
}