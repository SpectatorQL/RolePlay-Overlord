using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RolePlayOverlord.UI
{
    public class HostUIController : MonoBehaviour
    {
        Network _network;

        [SerializeField] GameObject[] _multiElements = new GameObject[4];
        GameObject _mainElement;

        [SerializeField] GameObject _voiceIcon;

        public void ShowElement(GameObject elem)
        {
            elem.SetActive(true);
        }

        public void ShowPlayerInfo(GameObject elem, int clientIndex)
        {
            ShowMainElement(elem);
            // TODO: Get the relevant component.
            var multiElem = elem.GetComponent<UIMultiElem>();
            multiElem.MainTextSpace.text = _network.GetClientCharacterInfo(clientIndex)
                + "\nAnd some random gibberish LULZ";
        }

        public void ShowDocument(GameObject elem, string docName)
        {
            ShowMainElement(elem);
            // TODO: Get the relevant component.
            var multiElem = elem.GetComponent<UIMultiElem>();
            multiElem.MainTextSpace.text = _network.GetDocument(docName);
        }

        public void ShowMainElement(GameObject elem)
        {
            _mainElement?.SetActive(false);
            _mainElement = elem;
            _mainElement.SetActive(true);
        }

        public void HideElement(GameObject elem)
        {
            elem.SetActive(false);
        }

        public void HideMainElement()
        {
            _mainElement.SetActive(false);
        }

        public void ShowVoiceIcon()
        {
            _voiceIcon.SetActive(true);
        }

        public void HideVoiceIcon()
        {
            _voiceIcon.SetActive(false);
        }

        public void ShowUI()
        {
            gameObject.SetActive(true);
        }

        public void HideUI()
        {
            gameObject.SetActive(false);
        }

        public void Setup(Network network)
        {
            _network = network;

            // TODO: Delete this guy once the UI is complete.
            foreach(var e in _multiElements)
            {
                e.SetActive(false);
            }

            ShowUI();
        }
    }
}
