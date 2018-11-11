using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RolePlayOverlord.UI
{
    /*
        NOTE(SpectatorQL): This components' purpose is to pass additional data to functions
        of the HostUIController, along with the UI element that is to become active.
        All this is because Unity only allows one parameter at most in the signatures
        of functions called by OnClick.
    */
    public class ClickAction : MonoBehaviour
    {
        [SerializeField] HostUIController _globalController;
        [SerializeField] GameObject _element;

        // TODO: Give these guys more appropriate names.
        public void ShowElement(int clientIndex)
        {
            _globalController.ShowMultiElement(_element, clientIndex);
        }

        public void ShowElement(string docPath)
        {
            _globalController.ShowMultiElement(_element, docPath);
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
