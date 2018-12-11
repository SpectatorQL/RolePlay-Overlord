using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RolePlayOverlord.UI
{
    public class ResourceList : MonoBehaviour
    {
        public GameObject ResourceButtonPrefab;
        [HideInInspector] public ResourceButton[][] Buttons;
        int _activeResourceType;

        public void ShowButtons(int resourceType)
        {
            DisableActiveButtons();

            for(int i = 0;
                i < Buttons[resourceType].Length;
                ++i)
            {
                Buttons[resourceType][i].gameObject.SetActive(true);
            }
            _activeResourceType = resourceType;
        }

        void DisableActiveButtons()
        {
            for(int i = 0;
                i < Buttons[_activeResourceType].Length;
                ++i)
            {
                Buttons[_activeResourceType][i].gameObject.SetActive(false);
            }
        }

        public void Initialize()
        {
            for(int i = 0;
                i < Buttons.Length;
                ++i)
            {
                for(int j = 0;
                    j < Buttons[i].Length;
                    ++j)
                {
                    Buttons[i][j].gameObject.SetActive(false);
                }
            }
            _activeResourceType = -1;
        }
    }
}
