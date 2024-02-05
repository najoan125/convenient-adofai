using HarmonyLib;
using System.Reflection;
using UnityEngine;
using UnityModManagerNet;

namespace convenient_adofai
{
    public static class Main
    {
        public static UnityModManager.ModEntry.ModLogger Logger;
        public static Harmony harmony;
        public static bool keyDown = false;

        private static bool ispause = false;
        private static bool isCLSClicked = false;

        private static readonly PropertyInfo isEditingLevelProperty =
            AccessTools.Property(typeof(ADOBase), "isEditingLevel");
        private static readonly int ReleaseNumber = (int)AccessTools.Field(typeof(GCNS), "releaseNumber").GetValue(null);

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
                keyDown = false;
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
