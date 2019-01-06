using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RolePlayOverlord.UI
{
    public class ResourceButton : MonoBehaviour, IPointerDownHandler
    {
        [HideInInspector] public ResourceType ResourceType;
        [HideInInspector] public string ResourcePath;

        public Text TextField;

        [HideInInspector] public System.Action<ResourceType, string> Cmd;

        public void OnPointerDown(PointerEventData eventData)
        {
            Cmd(ResourceType, ResourcePath);
        }
    }
}
