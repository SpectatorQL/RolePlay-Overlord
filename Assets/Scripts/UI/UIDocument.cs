using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RolePlayOverlord.UI
{
    public class UIDocument : MonoBehaviour
    {
        public InputField InputField;
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
