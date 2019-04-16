using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RolePlayOverlord.FileFormats;
using TMPro;

namespace RolePlayOverlord.UI
{
    public interface UIController
    {
        void UpdateChatWindow(string chat);
    }

    public class HostUIController : MonoBehaviour, UIController
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

        [SerializeField] TMP_Text _chatWindow;

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

        void UIController.UpdateChatWindow(string chat)
        {
            _chatWindow.text = chat;
        }

        public void SendChatMessage(MonoBehaviour go)
        {
            var inputField = (InputField)go;
            _network.CmdOnChatMessage(inputField.text);
        }

        public ResourceButton[] CreateResourceButtons(ModData modData, ResourceTypeID resourceType)
        {
            ResourceButton[] result;

            int len = modData.ResourceTypeEntries[(int)resourceType].Count;
            int firstResourceIndex = modData.ResourceTypeEntries[(int)resourceType].FirstResourceIndex;
            result = new ResourceButton[len];
            for(int i = 0;
                i < len;
                ++i)
            {
                ResourceData resData = new ResourceData
                {
                    ResourceType = resourceType,
                    ID = i
                };

                result[i] = Instantiate(_resourceList.ResourceButtonPrefab, _resourceList.transform)
                        .GetComponent<ResourceButton>();

                result[i].ResourceData = resData;

                int resIndex = firstResourceIndex + i;
                Resource res = modData.Resources[resIndex];
                result[i].TextField.text = IO.FILENAME(res.File);

                result[i].Cmd = _network.CmdOnResourceButtonClick;
            }

            return result;
        }

        public void Setup(Network network, ModData modData)
        {
            /*
               TODO: A lot of things need to be reconsidered before this can become
               even remotely close to shippable code.
            */

            _network = network;
            
            List<ResourceButton[]> resButtonList = new List<ResourceButton[]>();
            for(ResourceTypeID i = 0;
                i < ResourceTypeID.CharacterModel;
                ++i)
            {
                ResourceButton[] resButtons = CreateResourceButtons(modData, i);
                resButtonList.Add(resButtons);
            }
            _resourceList.Buttons = resButtonList.ToArray();

            
            // TODO: Make the Session window also use a ResourceList, though with a different set of buttons.
            string[] documents = modData.LocalData.Documents;
            for(int i = 0;
                i < documents.Length;
                ++i)
            {
                var docListButton = Instantiate(_docList.DocButtonPrefab, _docList.transform)
                    .GetComponent<DocListButton>();
                docListButton.DocList = _docList;
                docListButton.TextField.text = IO.FILENAME(documents[i]);
                docListButton.DocPath = documents[i];

                _docList.AddDocButton(docListButton);
            }
            _docList.AssignButtonIds();

            ShowUI();
        }
    }
}
