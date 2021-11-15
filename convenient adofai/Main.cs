﻿using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityModManagerNet;

namespace convenient_adofai
{
    public static class Main
    {
        public static UnityModManager.ModEntry.ModLogger Logger;
        public static Harmony harmony;
        public static bool IsEnabled = false;
        public static bool ispause = false; //OnUpdate에 사용되기 위한 예시
        public static bool restart = false;
        public static bool editor = false;

        public static void Setup(UnityModManager.ModEntry modEntry)
        {
            Logger = modEntry.Logger;
            modEntry.OnToggle = OnToggle;
            modEntry.OnUpdate = OnUpdate; //선택
        }

        //선택
        private static void OnUpdate(UnityModManager.ModEntry modentry, float deltaTime)
        {
            //반복적으로 작동할 구문
            //예시
            if (!scrController.instance || !scrConductor.instance)
            {
                return; //모드가 실행될 때 로그에 NullPointerException이 뜨지 않도록 해줌
            }
            ispause = scrController.instance.paused && scrConductor.instance.isGameWorld;
            //레벨을 플레이 중이고 일시정지 상태가 아니면 true, 레벨을 플레이 하고 있지 않거나 일시정지 상태면 false
            if (ispause)
            {
                if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.R) && !restart && !scrController.instance.isEditingLevel && !UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.E))
                {
                    restart = true;
                    PauseMenu menu = new PauseMenu();
                    menu.PlaySfx(2);
                    scrController controller = new scrController();
                    controller.Restart();
                    GCS.checkpointNum = 0;
                    //GCS.sceneToLoad = this.levelName;
                    scrUIController.instance.WipeToBlack(WipeDirection.StartsFromRight, null);
                    restart = false;
                }
                if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.E) && !editor && scrController.instance.isLevelEditor && !scrController.instance.isEditingLevel && !UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.R))
                {
                    editor = true;
                    PauseMenu menu = new PauseMenu();
                    menu.PlaySfx(2);
                    scrController.instance.TogglePauseGame();
                    scnEditor.instance.SwitchToEditMode(true);
                    DiscordController instance = DiscordController.instance;
                    if (instance != null)
                    {
                        instance.UpdatePresence();
                    }
                    Persistence.UpdateLastOpenedLevel(CustomLevel.instance.levelPath);
                    editor = false;
                }
            }
        }

        private static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            IsEnabled = value;

            if (value)
            {
                //켜질때
                harmony = new Harmony(modEntry.Info.Id);
                harmony.PatchAll(Assembly.GetExecutingAssembly());
            }
            else
            {
                //꺼질때
                harmony.UnpatchAll(modEntry.Info.Id);
            }
            return true;
        }
    }
}
