using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RolePlayOverlord.UI
{
    public class DocListButton : MonoBehaviour, IPointerDownHandler
    {
        public DocList DocList;
        public Text TextField;

        public void OnPointerDown(PointerEventData eventData)
        {
            DocList.ActiveDocButton = this;
            Debug.Log("A DocListButton has just been clicked on.");
        }
    }
}
