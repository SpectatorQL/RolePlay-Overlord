using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RolePlayOverlord.UI
{
    public class DocList : MonoBehaviour
    {
        [SerializeField] GameObject _docButtonPrefab;
        List<Button> _docButtons = new List<Button>(16);
        Button _activeButton;

        public void CreateNewDocButton(string docName)
        {
            Button button = Instantiate(_docButtonPrefab, transform).GetComponent<Button>();
            button.GetComponentInChildren<Text>().text = docName;

            _docButtons.Add(button);
        }
    }
}
