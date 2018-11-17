using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RolePlayOverlord.UI
{
    public class DocList : MonoBehaviour
    {
        public GameObject DocButtonPrefab;
        [HideInInspector] public List<DocListButton> DocButtons = new List<DocListButton>(16);
        // TODO: Make this just a plain int?
        [HideInInspector] public DocListButton ActiveDocButton;
    }
}
