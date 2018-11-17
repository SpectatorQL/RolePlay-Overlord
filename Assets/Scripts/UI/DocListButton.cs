using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RolePlayOverlord.UI
{
    public class DocListButton : MonoBehaviour, IPointerDownHandler
    {
        [HideInInspector] public DocList DocList;
        public Text TextField;
        [HideInInspector] public string DocName;

        public void OnPointerDown(PointerEventData eventData)
        {
            DocList.ActiveDocButton = this;
            Debug.Log("A DocListButton has just been clicked on.");
        }
    }
}
