using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace RolePlayOverlord
{
    public class ClientEntity : NetworkBehaviour
    {
        float _yaw;
        float _pitch;
        public float Sensitivity = 2.0f;

        [ClientCallback]
        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        [ClientCallback]
        void Update()
        {
            if(isServer)
            {
                Camera cam = GetComponent<Camera>();
                float speed = 0.1f;
                // TODO: Enable diagonal movement
                if(Input.GetKey(KeyCode.W))
                {
                    transform.position += cam.transform.forward * speed;
                }
                else if(Input.GetKey(KeyCode.S))
                {
                    transform.position -= cam.transform.forward * speed;
                }
                else if(Input.GetKey(KeyCode.A))
                {
                    transform.position -= cam.transform.right * speed;
                }
                else if(Input.GetKey(KeyCode.D))
                {
                    transform.position += cam.transform.right * speed;
                }
                

                float inputX = Input.GetAxisRaw("Mouse X");
                float inputY = Input.GetAxisRaw("Mouse Y");
                _yaw += inputX * Sensitivity;
                _pitch -= inputY * Sensitivity;

                Vector3 rotation = new Vector3(_pitch, _yaw, 0.0f);
                transform.eulerAngles = rotation;
            }
        }
        
        void OnGUI()
        {
            Rect rect = new Rect(Screen.width / 2, 100, 200, 30);
            string text = isServer ? "I'm the server!\n" : "I'm a client!\n";
            GUI.Label(rect, text);
        }
    }
}