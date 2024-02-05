using HarmonyLib;
using System.Collections.Generic;
using System.Linq;

namespace convenient_adofai
{
    public static class RestartPatch
    {
        public static int buttonPressed = 0;

        [HarmonyPatch(typeof(PauseMenu),"Update")]
        public static class PauseMenuPatch
        {
            public static void Prefix(PauseMenu __instance, bool ___isOnSettingsMenu, List<GeneralPauseButton> ___currentButtons)
            {
                if (!Main.keyDown && !___isOnSettingsMenu)
                {
                    if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.R) && (scrConductor.instance.isGameWorld || scnCLS.instance))
                    {
                        if (scrConductor.instance.isGameWorld)
                            __instance.restartButton.Select();
                        else
                        {
                            foreach (PauseButton button in ___currentButtons.Cast<PauseButton>())
                            {
                                if (button.buttonType == PauseMenu.ButtonType.Refresh)
                                {
                                    button.Select();
                                    break;
                                }
                            }
                        }
                        buttonPressed = 1;
                        Main.keyDown = true;
                    }
                    else if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.W) && scnCLS.instance)
                    {
                        __instance.steamWorkshopButton.Select();
                        buttonPressed = 1;
                        Main.keyDown = true;
                    }
                    else if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.E))
                    {
                        //커스텀 레벨이고, 연습모드가 아닐 때
                        if (!GCS.practiceMode && !ADOBase.isOfficialLevel && !RDC.runningOnSteamDeck)
                        {
                            __instance.openInEditorButton.Select();
                            buttonPressed = 1;
                            Main.keyDown = true;
                        }
                    }
                    else if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.S))
                    {
                        __instance.settingsButton.Select();
                        buttonPressed = 1;
                    }
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
