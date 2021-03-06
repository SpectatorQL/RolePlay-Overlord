﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RolePlayOverlord.UI
{
    public class ResourceList : MonoBehaviour
    {
        public GameObject ResourceButtonPrefab;
        [HideInInspector] public ResourceButton[][] Buttons;
        int _activeTab = -1;

        public void ShowButtons(int tab)
        {
            Debug.Assert(tab < Buttons.Length);

            DisableActiveButtons();

            for(int i = 0;
                i < Buttons[tab].Length;
                ++i)
            {
                Buttons[tab][i].gameObject.SetActive(true);
            }
            _activeTab = tab;
        }

        void DisableActiveButtons()
        {
            if(_activeTab == -1)
                return;

            for(int i = 0;
                i < Buttons[_activeTab].Length;
                ++i)
            {
                Buttons[_activeTab][i].gameObject.SetActive(false);
            }
        }
    }
}
