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
        public static int buttonPressed = 0;

        [HarmonyPatch(typeof(PauseMenu),"Update")]
        public static class PauseMenuPatch
        {
            public static void Prefix(PauseMenu __instance, bool ___isOnSettingsMenu, bool ___anyButtonPressed)
            {
                if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.R) && !Main.restart && !Main.editor && scrConductor.instance.isGameWorld && !___isOnSettingsMenu)
                {
                    Main.restart = true;
                    __instance.restartButton.Select();
                    buttonPressed = 1;
                }
                else if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.E) && !Main.editor && !Main.restart && !___isOnSettingsMenu)
                {
                    //커스텀 레벨이고, 연습모드가 아닐 때
                    if (!GCS.practiceMode && GCS.standaloneLevelMode && !ADOBase.isOfficialLevel && !RDC.runningOnSteamDeck)
                    {
                        Main.editor = true;
                        __instance.openInEditorButton.Select();
                        buttonPressed = 1;
                    }
                }
                else if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.S) && !Main.editor && !Main.restart && !___isOnSettingsMenu)
                {
                    __instance.settingsButton.Select();
                    buttonPressed = 1;
                }

                if (buttonPressed != 1 && UnityEngine.Input.anyKeyDown)
                {
                    buttonPressed = -1;
                }
                else
                {
                    buttonPressed = 0;
                }//R,E,S를 누르면 1 / 다른 키를 누르면 -1 / 아무 것도 누르지 않으면 0
            }
        }

        [HarmonyPatch(typeof(RDInput), "GetMain")]
        public static class MainPressPatch
        {
            public static void Postfix(ref int __result)
            {
                if (scrController.instance.paused && buttonPressed != -1)
                {
                    __result = 0;
                }
            }
        }
    }
}
