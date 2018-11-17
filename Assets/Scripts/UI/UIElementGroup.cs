using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RolePlayOverlord.UI
{
    public class UIElementGroup : MonoBehaviour
    {
        [SerializeField] GameObject[] _elements;

        public void Show()
        {
            for(int i = 0; i < _elements.Length; ++i)
            {
                _elements[i].SetActive(true);
            }
        }

        public void Hide()
        {
            for(int i = 0; i < _elements.Length; ++i)
            {
                _elements[i].SetActive(false);
            }
        }
    }
}
