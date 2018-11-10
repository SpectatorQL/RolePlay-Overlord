using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RolePlayOverlord
{
    public class HostUIController : MonoBehaviour
    {
        [SerializeField] GameObject[] _multiElements = new GameObject[3];
        GameObject _multiActiveElement;

        bool _isHidden;

        public void ShowMultiElement(GameObject elem, int clientIndex)
        {
            ShowMultiElement(elem);

            var multiElem = elem.GetComponent<UIMultiElem>();
            multiElem.MainTextSpace.text = clientIndex + "\nAnd some random giberrish LULZ";

            // TODO: Call element-specific functions here.
        }

        public void ShowMultiElement(GameObject elem)
        {
            _multiActiveElement?.SetActive(false);
            _multiActiveElement = elem;
            _multiActiveElement.SetActive(true);
        }

        public void Show()
        {
            gameObject.SetActive(true);
            _isHidden = false;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            _isHidden = true;
        }

        public void Setup()
        {
            // TODO: Delete this guy once the UI is complete.
            foreach(var e in _multiElements)
            {
                e.SetActive(false);
            }

            Show();
        }
    }
}
