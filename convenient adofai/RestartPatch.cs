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
        [HarmonyPatch(typeof(PauseMenu),"Update")]
        public static class PauseMenuPatch
        {
            public static void Prefix(PauseMenu __instance, bool ___isOnSettingsMenu, bool ___anyButtonPressed)
            {
                if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.R) && !Main.restart && !Main.editor && scrConductor.instance.isGameWorld && !___isOnSettingsMenu)
                {
                    Main.restart = true;
                    __instance.restartButton.Select();
                    ___anyButtonPressed = true;
                }
                else if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.E) && !Main.editor && !Main.restart && !___isOnSettingsMenu)
                {
                    //커스텀 레벨이고, 연습모드가 아닐 때
                    if (!GCS.practiceMode && GCS.standaloneLevelMode && !ADOBase.isOfficialLevel && !RDC.runningOnSteamDeck)
                    {
                        Main.editor = true;
                        __instance.openInEditorButton.Select();
                        ___anyButtonPressed = true;
                    }
                }
                else if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.S) && !Main.editor && !Main.restart)
                {
                    __instance.settingsButton.Select();
                    ___anyButtonPressed = true;
                }
            }
        }
    }
}
