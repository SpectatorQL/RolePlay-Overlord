using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RolePlayOverlord.UI
{
    public class HostUIController : MonoBehaviour
    {
        [SerializeField] GameObject[] _multiElements = new GameObject[3];
        GameObject _activeMultiElement;

        bool _isHidden;

        public void ShowElement(GameObject elem)
        {
            elem.SetActive(true);
        }

        public void ShowMultiElement(GameObject elem, int clientIndex)
        {
            ShowMultiElement(elem);

            var multiElem = elem.GetComponent<UIMultiElem>();
            multiElem.MainTextSpace.text = clientIndex + "\nAnd some random giberrish LULZ";

            // TODO: Call element-specific functions here.
        }

        public void ShowMultiElement(GameObject elem)
        {
            _activeMultiElement?.SetActive(false);
            _activeMultiElement = elem;
            _activeMultiElement.SetActive(true);
        }

        public void HideElement(GameObject elem)
        {
            elem.SetActive(false);
        }

        public void HideMultiElement()
        {
            _activeMultiElement.SetActive(false);
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
