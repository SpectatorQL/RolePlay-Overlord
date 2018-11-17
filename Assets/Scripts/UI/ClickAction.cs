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
        [SerializeField] UIElementGroup _elementGroup;
        
        public void ShowPlayerInfo(int clientIndex)
        {
            _globalController.ShowPlayerInfo(clientIndex);
        }

        public void ShowElementGroup()
        {
            _globalController.ShowElementGroup(_elementGroup);
        }

        public void HideElement()
        {
            _globalController.HideElement(gameObject);
        }

        public void HideMainElement()
        {
            _globalController.HideMainElement();
        }
    }
}
