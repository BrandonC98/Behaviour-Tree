using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities 
{

    public static float MoveOverTime(float speed)
    {

        float step;

        step = speed * Time.deltaTime;

        return step;

    }

    public static class Tags
    {

        public static string RedTeam = "Red";

        public static string RedAmmoBox = "RedAmmoBox";

        public static string BlueTeam = "Blue";

        public static string BlueAmmoBox = "BlueAmmoBox";

        public static string Obsticle = "Obsticle";

    }


}
