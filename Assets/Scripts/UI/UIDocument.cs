using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RolePlayOverlord.UI
{
    public class UIDocument : MonoBehaviour
    {
        public TMP_InputField InputField;
        [HideInInspector] public string ActiveDocument;

        public void BeginEditing()
        {
            InputField.interactable = true;
            InputField.Select();
        }

        public void EndEditing()
        {
            InputField.interactable = false;
        }
    }
}
