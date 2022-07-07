using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using debug = UnityEngine.Debug;
using UnityModManagerNet;

namespace convenient_adofai
{
    public static class Main
    {
        public static UnityModManager.ModEntry.ModLogger Logger;
        public static Harmony harmony;
        public static bool IsEnabled = false;
        public static bool ispause = false;
        public static bool isCLSClicked = false;
        public static bool restart = false;
        public static bool editor = false;

        private static PropertyInfo isEditingLevelProperty =
            AccessTools.Property(typeof(ADOBase), "isEditingLevel");
        public static readonly int ReleaseNumber = (int)AccessTools.Field(typeof(GCNS), "releaseNumber").GetValue(null);

        public static void Setup(UnityModManager.ModEntry modEntry)
        {
            Logger = modEntry.Logger;
            modEntry.OnToggle = OnToggle;
            modEntry.OnUpdate = OnUpdate;
        }

        private static void OnUpdate(UnityModManager.ModEntry modentry, float deltaTime)
        {
            if (!scrController.instance || !scrConductor.instance)
            {
                return;
            }

            bool isEditingLevel = (bool)isEditingLevelProperty.GetValue(
                Main.ReleaseNumber >= 94 ? null : scnEditor.instance);

            ispause = scrController.instance.paused && scrConductor.instance.isGameWorld && !isEditingLevel;
            if (!ispause)
            {
                editor = false;
                restart = false;
            }

            if (scrController.instance.paused && isEditingLevel)
            {
                if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.C) && !isCLSClicked)
                {
                    isCLSClicked = true;
                    GCS.sceneToLoad = "scnCLS";
                    scrUIController.instance.WipeToBlack(WipeDirection.StartsFromRight, null);
                }
            }
            if(scnCLS.instance)
            {
                isCLSClicked = false;
            }
        }

        private static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            IsEnabled = value;

            if (value)
            {
                harmony = new Harmony(modEntry.Info.Id);
                harmony.PatchAll(Assembly.GetExecutingAssembly());
            }
            else
            {
                harmony.UnpatchAll(modEntry.Info.Id);
            }
            return true;
        }
    }
}
