using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RolePlayOverlord.UI
{
    public class DocListButton : MonoBehaviour, IPointerDownHandler
    {
        [HideInInspector] public int Id;
        [HideInInspector] public string DocName;
        public Text TextField;

        [HideInInspector] public DocList DocList;

        public void OnPointerDown(PointerEventData eventData)
        {
            DocList.SetActiveButton(Id);
        }
    }
}
