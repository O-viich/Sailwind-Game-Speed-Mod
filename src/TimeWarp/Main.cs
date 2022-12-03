using System;
using HarmonyLib;
using System.Reflection;
using UnityModManagerNet;
using UnityEngine;
using System.Collections.Generic;

namespace TimeWarp
{
    internal static class Main {
        public static bool Load(UnityModManager.ModEntry modEntry)
        {
            new Harmony(modEntry.Info.Id).PatchAll(Assembly.GetExecutingAssembly());
            
            mod = modEntry;
            
            settings = TimeWarpSettings.Load<TimeWarpSettings>(mod);
            
            modEntry.OnToggle = OnToggle;
            modEntry.OnGUI = OnGUI;
            modEntry.OnSaveGUI = OnSaveGUI;
            return true;
        }
        
        public static bool OnToggle(UnityModManager.ModEntry modEntry, bool value) {
            enabled = value;
            if (!enabled)
            {
                Time.timeScale = 1f;
            }
            return true;
        }
        
        private static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Game Speed Multiplier (1-5) ", GUILayout.ExpandWidth(false));
            settings.GameSpeed = GUILayout.HorizontalSlider(settings.GameSpeed, 1, 5);
            GUILayout.Label(settings.GameSpeed.ToString("0.00"), GUILayout.ExpandWidth(false));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Reset Game Speed", GUILayout.ExpandWidth(false)))
            {
                settings.GameSpeed = 1f;
            }
            GUILayout.EndHorizontal();
        }

        private static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            settings.Save(modEntry);
            Time.timeScale = settings.GameSpeed;
        }
        
        public static bool enabled;
        public static UnityModManager.ModEntry mod;
        public static TimeWarpSettings settings;
    }
}