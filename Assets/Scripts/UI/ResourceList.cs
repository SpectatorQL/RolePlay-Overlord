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

        public void Initialize()
        {
            for(int i = 0;
                i < Buttons.Rank;
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
