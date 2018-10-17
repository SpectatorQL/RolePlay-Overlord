using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace RolePlayOverlord
{
    public class MouseCamLook : NetworkBehaviour
    {
        float _yaw;
        float _pitch;
        public float Sensitivity = 2.0f;

        [ClientCallback]
        void Start()
        {
            if(isLocalPlayer)
            {
                _yaw = transform.eulerAngles.y;
                _pitch = transform.eulerAngles.x;
                GetComponent<Camera>().enabled = true;
            }
        }

        [ClientCallback]
        void Update()
        {
            // TODO: Pull this into the ClientEntity Update function
            if(isLocalPlayer && !isServer)
            {
                float rotMin = -30.0f;
                float rotMax = 30.0f;
                float inputX = Input.GetAxisRaw("Mouse X");
                float inputY = Input.GetAxisRaw("Mouse Y");

                _yaw += inputX * Sensitivity;
                _pitch -= inputY * Sensitivity;
                _yaw = Mathf.Clamp(_yaw, rotMin, rotMax);
                _pitch = Mathf.Clamp(_pitch, rotMin, rotMax);

                Vector3 rotation = new Vector3(_pitch, _yaw, 0.0f);
                transform.eulerAngles = rotation;
            }
        }
    }
}