using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RolePlayOverlord.UI
{
    public class HostUIController : MonoBehaviour
    {
        Network _network;
        
        [SerializeField] GameObject _playerInfo;
        [SerializeField] GameObject _gameSettings;
        [SerializeField] GameObject _documentanion;
        [SerializeField] GameObject _placingElement;
        GameObject _mainElement;

        [SerializeField] DocList _docList;

        UIElementGroup _activeElementGroup;

        [SerializeField] GameObject _voiceIcon;

        public void ShowElement(GameObject elem)
        {
            elem.SetActive(true);
        }

        public void ShowPlayerInfo(int clientIndex)
        {
            ShowMainElement(_playerInfo);
            // TODO: Get the relevant component.
            var multiElem = _playerInfo.GetComponent<UIMultiElem>();
            multiElem.MainTextSpace.text = _network.GetClientCharacterInfo(clientIndex)
                + "\nAnd some random gibberish LULZ";
        }

        public void ShowDocument()
        {
            ShowMainElement(_documentanion);
            // TODO: Get the relevant component.
            var multiElem = _documentanion.GetComponent<UIMultiElem>();
            string docName = _docList.ActiveDocButton.DocName;
            multiElem.MainTextSpace.text = _network.GetDocument(docName);
        }

        public void ShowMainElement(GameObject elem)
        {
            _mainElement?.SetActive(false);
            _mainElement = elem;
            _mainElement.SetActive(true);
        }

        public void ShowElementGroup(UIElementGroup elemGroup)
        {
            HideElementGroup();
            _activeElementGroup = elemGroup;
            _activeElementGroup.Show();
        }

        public void HideElement(GameObject elem)
        {
            elem.SetActive(false);
        }

        public void HideMainElement()
        {
            _mainElement.SetActive(false);
        }

        public void HideElementGroup()
        {
            _activeElementGroup?.Hide();
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

            List<string> docs = _network.Docs;
            for(int i = 0; i < docs.Count; ++i)
            {
                var docListButton = Instantiate(_docList.DocButtonPrefab, _docList.transform)
                    .GetComponent<DocListButton>();
                docListButton.DocList = _docList;
                docListButton.TextField.text = docs[i];
                docListButton.DocName = docs[i];

                _docList.DocButtons.Add(docListButton);
            }

            ShowUI();
        }
    }
}
