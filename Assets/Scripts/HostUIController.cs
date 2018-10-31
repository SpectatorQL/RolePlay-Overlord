using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RolePlayOverlord
{
    public class HostUIController : MonoBehaviour
    {
        public bool IsHidden;

        public void OnUIKeyPressed()
        {
            if(IsHidden)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }

        public void Show()
        {
            gameObject.SetActive(true);
            IsHidden = false;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            IsHidden = true;
        }

        public void Setup()
        {
            Show();
        }
    }
}
