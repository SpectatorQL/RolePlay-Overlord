using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace RolePlayOverlord
{
    public delegate void process_keyboard_input(ClientEntity obj, PlayerInput input);
    public delegate void rotate_camera(ClientEntity obj);

    public struct PlayerInput
    {
        public bool MoveForward;
        public bool MoveBackward;
        public bool MoveLeft;
        public bool MoveRight;

        public bool UIKeyPressed;

        public bool Debug_SetTexture1;
        public bool Debug_SetTexture2;

        public static void ProcessHostKeyboard(ClientEntity ent, PlayerInput input)
        {
            float speed = 0.1f;
            Vector3 newPos = ent.transform.position;
            if(input.MoveForward)
            {
                newPos += ent.Cam.transform.forward * speed;
            }
            if(input.MoveBackward)
            {
                newPos -= ent.Cam.transform.forward * speed;
            }
            if(input.MoveLeft)
            {
                newPos -= ent.Cam.transform.right * speed;
            }
            if(input.MoveRight)
            {
                newPos += ent.Cam.transform.right * speed;
            }
            ent.transform.position = newPos;

            if(input.UIKeyPressed)
            {
                if(ent.ControlMode == ControlMode.Camera)
                {
                    ent.ControlMode = ControlMode.Menu;
                    Cursor.lockState = CursorLockMode.None;
                }
                else
                {
                    ent.ControlMode = ControlMode.Camera;
                    Cursor.lockState = CursorLockMode.Locked;
                }
                ent.HostUIController.OnUIKeyPressed();
            }

            if(input.Debug_SetTexture1)
            {
                ent.Network.RpcSetTexture("test.png");
            }
            if(input.Debug_SetTexture2)
            {
                ent.Network.RpcSetTexture("test2.png");
            }
        }

        public static void ProcessHostMouse(ClientEntity ent)
        {
            Vector3 rotation = new Vector3(ent.Pitch, ent.Yaw, 0.0f);
            ent.transform.eulerAngles = rotation;
        }

        public static void ProcessClientKeyboard(ClientEntity ent, PlayerInput input)
        {

        }

        public static void ProcessClientMouse(ClientEntity ent)
        {
            float rotMin = -30.0f;
            float rotMax = 30.0f;
            ent.Yaw = Mathf.Clamp(ent.Yaw, rotMin, rotMax);
            ent.Pitch = Mathf.Clamp(ent.Pitch, rotMin, rotMax);

            ProcessHostMouse(ent);
        }
    }

    public enum ControlMode
    {
        Camera,
        Menu
    }

    public class ClientEntity : NetworkBehaviour
    {
        [HideInInspector] public Network Network;

        [HideInInspector] public Camera Cam;
        [HideInInspector] public float Yaw;
        [HideInInspector] public float Pitch;
        float _sensitivity;

        public process_keyboard_input ProcessKeyboardInput = PlayerInput.ProcessClientKeyboard;
        public rotate_camera RotateCamera = PlayerInput.ProcessClientMouse;

        [HideInInspector] public ControlMode ControlMode;

        [HideInInspector] public GameObject UI;
        [HideInInspector] public HostUIController HostUIController;

        float _delta;

        void Start()
        {
            if(isLocalPlayer)
            {
                Cam = GetComponent<Camera>();
                Cam.enabled = true;
                Cursor.lockState = CursorLockMode.Locked;

                Yaw = transform.eulerAngles.y;
                Pitch = transform.eulerAngles.x;
                _sensitivity = 2.0f;
            }
        }

        void Update()
        {
            if(isLocalPlayer)
            {
                PlayerInput input = new PlayerInput();
                float inputX = Input.GetAxisRaw("Mouse X");
                float inputY = Input.GetAxisRaw("Mouse Y");

                // TODO(SpectatorQL): Refactor this whole thing.
                if(ControlMode == ControlMode.Camera)
                {
                    if(Input.GetKey(KeyCode.W))
                    {
                        input.MoveForward = true;
                    }
                    if(Input.GetKey(KeyCode.S))
                    {
                        input.MoveBackward = true;
                    }
                    if(Input.GetKey(KeyCode.A))
                    {
                        input.MoveLeft = true;
                    }
                    if(Input.GetKey(KeyCode.D))
                    {
                        input.MoveRight = true;
                    }

                    if(Input.GetKeyDown(KeyCode.Tab))
                    {
                        input.UIKeyPressed = true;
                    }

                    if(Input.GetKeyDown(KeyCode.Alpha1))
                    {
                        input.Debug_SetTexture1 = true;
                    }
                    if(Input.GetKeyDown(KeyCode.Alpha2))
                    {
                        input.Debug_SetTexture2 = true;
                    }

                    Yaw += inputX * _sensitivity;
                    Pitch -= inputY * _sensitivity;

                    RotateCamera(this);
                }
                else if(ControlMode == ControlMode.Menu)
                {
                    if(Input.GetKeyDown(KeyCode.Tab))
                    {
                        input.UIKeyPressed = true;
                    }
                }

                ProcessKeyboardInput(this, input);
            }

            _delta += (Time.deltaTime - _delta) * 0.1f;
        }
        
        void OnGUI()
        {
            float fps = 1.0f / _delta;
            
            Rect fpsRect = new Rect(Screen.width / 2, 20.0f, 250.0f, 30.0f);
            GUI.Label(fpsRect, fps.ToString("00.00"));

            Rect rect = new Rect(Screen.width / 2, 100, 200, 30);
            string text = isServer ? "I'm the server!\n" : "I'm a client!\n";
            GUI.Label(rect, text);
        }
    }
}