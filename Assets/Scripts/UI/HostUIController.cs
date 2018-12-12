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
        [SerializeField] GameObject _documentation;
        [SerializeField] GameObject _placingElement;
        GameObject _mainElement;

        [SerializeField] DocList _docList;
        [SerializeField] ResourceList _resourceList;

        UIElementGroup _activeElementGroup;

        [SerializeField] GameObject _voiceIcon;

        public void ShowElement(GameObject elem)
        {
            elem.SetActive(true);
        }

        public void ShowPlayerInfo(int clientIndex)
        {
            ShowMainElement(_playerInfo);

            var playerInfo = _playerInfo.GetComponent<UIPlayerInfo>();
            playerInfo.MainText.text = _network.GetClientCharacterInfo(clientIndex);
        }

        public void ShowDocument()
        {
            DocListButton activeDocButton = _docList.GetActiveButton();
            if(activeDocButton != null)
            {
                ShowMainElement(_documentation);

                string docPath = activeDocButton.DocPath;
                var document = _documentation.GetComponent<UIDocument>();
                document.ActiveDocument = docPath;
                document.DocumentTitle.text = IO.FILENAME(docPath);
                document.InputField.text = _network.GetDocument(docPath);
            }
        }

        public void UpdateDocument()
        {
            var document = _documentation.GetComponent<UIDocument>();
            string docName = document.ActiveDocument;
            string text = document.InputField.text;
            _network.UpdateDocument(docName, text);
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

        public void Setup(Network network, string[][] modData)
        {
            _network = network;

            // NOTE(SpectatorQL): Scene resources come directly before session resources, that's why.
            int rank = Mod.CLASSMOD;
            ResourceButton[][] buttons = new ResourceButton[rank][];
            for(int i = 0;
                i < rank;
                ++i)
            {
                int len = modData[i].Length;
                buttons[i] = new ResourceButton[len];
                for(int j = 0;
                    j < len;
                    ++j)
                {
                    buttons[i][j] = Instantiate(_resourceList.ResourceButtonPrefab, _resourceList.transform)
                        .GetComponent<ResourceButton>();
                    buttons[i][j].ResourceType = i;
                    buttons[i][j].ResourcePath = modData[i][j];
                    buttons[i][j].TextField.text = IO.FILENAME(modData[i][j]);
                    buttons[i][j].Cmd = _network.CmdOnResourceButtonClick;
                }
            }
            _resourceList.Buttons = buttons;
            _resourceList.Initialize();
            

            for(int i = 0;
                i < modData[Mod.TEXT].Length;
                ++i)
            {
                var docListButton = Instantiate(_docList.DocButtonPrefab, _docList.transform)
                    .GetComponent<DocListButton>();
                docListButton.DocList = _docList;
                docListButton.TextField.text = IO.FILENAME(modData[Mod.TEXT][i]);
                docListButton.DocPath = modData[Mod.TEXT][i];

                _docList.AddDocButton(docListButton);
            }
            _docList.AssignButtonIds();


            ShowUI();
        }
    }
}
