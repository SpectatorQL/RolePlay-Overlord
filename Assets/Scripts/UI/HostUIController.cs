using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RolePlayOverlord.UI
{
    public class HostUIController : MonoBehaviour
    {
        [HideInInspector] public Network Network;

        [SerializeField] GameObject[] _multiElements = new GameObject[3];
        GameObject _activeMultiElement;

        [SerializeField] GameObject _voiceIcon;

        bool _isHidden;

        public void ShowElement(GameObject elem)
        {
            elem.SetActive(true);
        }

        // TODO: Make separate functions for the PlayerSettings window and the Documents window.

        public void ShowMultiElement(GameObject elem, int clientIndex)
        {
            ShowMultiElement(elem);

            var multiElem = elem.GetComponent<UIMultiElem>();
            multiElem.MainTextSpace.text = Network.GetClientCharacterInfo(clientIndex) + "\nAnd some random gibberish LULZ";

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

        public void ShowVoiceIcon()
        {
            _voiceIcon.SetActive(true);
        }

        public void HideVoiceIcon()
        {
            _voiceIcon.SetActive(false);
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
