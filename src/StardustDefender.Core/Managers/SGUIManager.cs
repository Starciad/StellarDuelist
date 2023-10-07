﻿using StardustDefender.Core.GUI;

using System;
using System.Collections.Generic;
using System.Linq;

namespace StardustDefender.Core.Managers
{
    public static class SGUIManager
    {
        private static readonly Dictionary<Type, SGUI> _GUIs = new();

        internal static void Initialize()
        {
            foreach (Type guiType in SGame.Assembly.GetTypes().Where(x => x.IsSubclassOf(typeof(SGUI))))
            {
                SGUI tempGUI = (SGUI)Activator.CreateInstance(guiType);
                tempGUI.Initialize();

                _GUIs.Add(guiType, tempGUI);
            }
        }
        internal static void Update()
        {
            foreach (SGUI gui in _GUIs.Values)
            {
                gui.Update();
            }
        }
        internal static void Draw()
        {
            foreach (SGUI gui in _GUIs.Values)
            {
                gui.Draw();
            }
        }

        public static T Get<T>() where T : SGUI
        {
            return (T)_GUIs[typeof(T)];
        }
        public static void Enable<T>() where T : SGUI
        {
            _GUIs[typeof(T)].Enable();
        }
        public static void Disable<T>() where T : SGUI
        {
            _GUIs[typeof(T)].Disable();
        }
        public static void DisableAll()
        {
            foreach (SGUI gui in _GUIs.Values)
            {
                gui.Disable();
            }
        }
    }
}