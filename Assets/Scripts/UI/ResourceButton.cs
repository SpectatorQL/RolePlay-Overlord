using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RolePlayOverlord.UI
{
    public class ResourceButton : MonoBehaviour, IPointerDownHandler
    {
        [HideInInspector] public ResourceData ResourceData;

        public Text TextField;

        [HideInInspector] public System.Action<ResourceData> Cmd;

        public void OnPointerDown(PointerEventData eventData)
        {
            Cmd(ResourceData);
        }
    }
}
