using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace convenient_adofai
{
    public static class RestartPatch
    {
        [HarmonyPatch(typeof(scrController), "Restart")]
        public static class Restart_Patch
        {
            public static bool Prefix()
            {
                if (Main.restart)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
    }
}
