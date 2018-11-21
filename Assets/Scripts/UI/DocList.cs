using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RolePlayOverlord.UI
{
    public class DocList : MonoBehaviour
    {
        public GameObject DocButtonPrefab;

        const int INITIAL_CAPACITY = 16;
        List<DocListButton> _docButtons = new List<DocListButton>(INITIAL_CAPACITY);
        int _nextId = 0;
        int _activeButtonId;

        public DocListButton GetActiveButton()
        {
            return _docButtons[_activeButtonId];
        }

        public void SetActiveButton(int id)
        {
            _activeButtonId = id;
        }

        public void AddDocButton(DocListButton button)
        {
            _docButtons.Add(button);
        }

        public void AssignButtonIds()
        {
            for(int i = 0; i < _docButtons.Count; ++i)
            {
                _docButtons[i].Id = _nextId++;
            }
        }
    }
}
