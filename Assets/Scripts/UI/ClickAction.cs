using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RolePlayOverlord.UI
{
    public class ClickAction : MonoBehaviour
    {
        [SerializeField] HostUIController _globalController;
        [SerializeField] GameObject _element;

        public void ShowElement(int clientIndex)
        {
            _globalController.ShowMultiElement(_element, clientIndex);
        }

        public void HideElement()
        {
            _globalController.HideElement(gameObject);
        }

        public void HideMultiElement()
        {
            _globalController.HideMultiElement();
        }
    }
}
