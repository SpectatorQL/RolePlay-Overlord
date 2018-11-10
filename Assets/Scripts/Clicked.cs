using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RolePlayOverlord
{
    public class Clicked : MonoBehaviour
    {
        [SerializeField] HostUIController _globalController;
        [SerializeField] GameObject _element;

        public void ShowElement(int clientIndex)
        {
            _globalController.ShowMultiElement(_element, clientIndex);
        }
    }
}
