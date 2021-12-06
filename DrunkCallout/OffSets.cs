using System;
using CitizenFX.Core;

namespace DrunkCallout
{
    public static class OffSets
    {
        public static Vector3 RandomOffSet()
        {
            Random random = new Random(); 
            float x = random.Next(100, 700);
            float y = random.Next(100, 700);
            return new Vector3(x, y, 0);
        }
    }
}