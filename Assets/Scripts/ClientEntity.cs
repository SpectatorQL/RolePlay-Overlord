using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;

namespace RolePlayOverlord
{
    public delegate void process_keyboard_input(ClientEntity ent, PlayerController controller);

    public class PlayerController
    {
        public PlayerInput OldInput;
        public PlayerInput NewInput;
    }

    // NOTE(SpectatorQL): This is all tentative and EXTREMELY subject to change.
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PlayerInput
    {
        public bool Debug_SetTexture1;
        public bool Debug_SetTexture2;

        public bool Q;
        public bool E;
        public bool W;
        public bool S;
        public bool A;
        public bool D;

        public bool Tab;

        public bool T;
        public bool V;
        public bool Return;
        public bool Esc;
        public bool F1;
        public bool F2;
        public bool F3;
        public bool F4;
        public bool F5;
        public bool F6;
        public bool F7;

        public bool LmbDown;
        public bool RmbDown;
        public float MouseX;
        public float MouseY;

        public static void ProcessHostKeyboard(ClientEntity ent, PlayerController controller)
        {
            float speed = 0.1f;
            Vector3 newPos = ent.transform.position;
            if(controller.NewInput.W)
            {
                newPos += ent.transform.forward * speed;
            }
            if(controller.NewInput.S)
            {
                newPos -= ent.transform.forward * speed;
            }
            if(controller.NewInput.A)
            {
                newPos -= ent.transform.right * speed;
            }
            if(controller.NewInput.D)
            {
                newPos += ent.transform.right * speed;
            }
            ent.MoveCamera(newPos);

            if(controller.NewInput.Tab)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.lockState = CursorLockMode.Locked;
                ent.HostUIController.OnUIKeyPressed();
            }

            if(controller.NewInput.Debug_SetTexture1)
            {
                ent.Network.RpcSetTexture("test.png");
            }
            if(controller.NewInput.Debug_SetTexture2)
            {
                ent.Network.RpcSetTexture("test2.png");
            }
        }

        public static void ProcessClientKeyboard(ClientEntity ent, PlayerController controller)
        {

        }
    }

    public enum ControlMode
    {
        Host,
        Client_Cam,
        Client_UI,
        Chat
    }

    public class ClientEntity : NetworkBehaviour
    {
        [SyncVar] public string ClientName;
        static int id;

        [HideInInspector] public Network Network;
        
        float _pitch;
        float _yaw;
        float _sensitivity;

        public process_keyboard_input ProcessKeyboardInput = PlayerInput.ProcessClientKeyboard;

        PlayerController _controller = new PlayerController();
        [HideInInspector] public ControlMode ControlMode;

        [HideInInspector] public GameObject UI;
        [HideInInspector] public HostUIController HostUIController;
        [HideInInspector] public PlayerUIController PlayerUIController;

        float _delta;

        public void RotateCamera(float yaw, float pitch)
        {
            _yaw += yaw;
            _pitch -= pitch;

            if(!isServer)
            {
                float rotMin = -30.0f;
                float rotMax = 30.0f;
                Mathf.Clamp(_yaw, rotMin, rotMax);
                Mathf.Clamp(_pitch, rotMin, rotMax);
            }

            Vector3 rotation = new Vector3(_pitch, _yaw, 0.0f);
            transform.eulerAngles = rotation;
        }

        public void MoveCamera(Vector3 pos)
        {
            transform.position = pos;
        }

        void Awake()
        {
            ClientName = (isServer) ? "Server" : ("Client " + ++id);
        }

        void Start()
        {
            if(!isLocalPlayer)
                return;

            GetComponent<Camera>().enabled = true;
            Cursor.lockState = CursorLockMode.Locked;

            _yaw = transform.eulerAngles.y;
            _pitch = transform.eulerAngles.x;
            _sensitivity = 2.0f;
        }

        void Update()
        {
            if(!isLocalPlayer)
                return;

            _controller.OldInput = _controller.NewInput;
            _controller.NewInput = new PlayerInput();

            // Player UI controls
            if(Input.GetKey(KeyCode.Q))
            {
                _controller.NewInput.Q = true;
            }
            if(Input.GetKey(KeyCode.E))
            {
                _controller.NewInput.E = true;
            }
            if(Input.GetKey(KeyCode.W))
            {
                _controller.NewInput.W = true;
            }
            if(Input.GetKey(KeyCode.S))
            {
                _controller.NewInput.S = true;
            }
            if(Input.GetKey(KeyCode.A))
            {
                _controller.NewInput.A = true;
            }
            if(Input.GetKey(KeyCode.D))
            {
                _controller.NewInput.D = true;
            }

            // GM hide/show UI button
            if(Input.GetKeyDown(KeyCode.Tab))
            {
                _controller.NewInput.Tab = true;
            }

            // Chat controls
            if(Input.GetKey(KeyCode.T))
            {
                _controller.NewInput.T = true;
            }
            if(Input.GetKey(KeyCode.V))
            {
                _controller.NewInput.V = true;
            }
            if(Input.GetKey(KeyCode.Return))
            {
                _controller.NewInput.Return = true;
            }
            if(Input.GetKey(KeyCode.F1))
            {
                _controller.NewInput.F1 = true;
            }
            if(Input.GetKey(KeyCode.F2))
            {
                _controller.NewInput.F2 = true;
            }
            if(Input.GetKey(KeyCode.F3))
            {
                _controller.NewInput.F3 = true;
            }
            if(Input.GetKey(KeyCode.F4))
            {
                _controller.NewInput.F4 = true;
            }
            if(Input.GetKey(KeyCode.F5))
            {
                _controller.NewInput.F5 = true;
            }
            if(Input.GetKey(KeyCode.F6))
            {
                _controller.NewInput.F6 = true;
            }
            if(Input.GetKey(KeyCode.F7))
            {
                _controller.NewInput.F7 = true;
            }

            // Debug controls
            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                _controller.NewInput.Debug_SetTexture1 = true;
            }
            if(Input.GetKeyDown(KeyCode.Alpha2))
            {
                _controller.NewInput.Debug_SetTexture2 = true;
            }

            ProcessKeyboardInput(this, _controller);

            
            // Mouse input handling
            if(Input.GetMouseButton(0))
            {
                _controller.NewInput.LmbDown = true;
            }
            if(Input.GetMouseButton(1))
            {
                _controller.NewInput.RmbDown = true;
            }
            float inputX = Input.GetAxisRaw("Mouse X");
            float inputY = Input.GetAxisRaw("Mouse Y");
            inputX *= _sensitivity;
            inputY *= _sensitivity;

            _controller.NewInput.MouseX = inputX;
            _controller.NewInput.MouseY = inputY;

            RotateCamera(inputX, inputY);
            

            _delta += (Time.deltaTime - _delta) * 0.1f;
        }
        
        void OnGUI()
        {
            float fps = 1.0f / _delta;
            
            Rect fpsRect = new Rect(Screen.width / 2, 20.0f, 250.0f, 30.0f);
            GUI.Label(fpsRect, fps.ToString("00.00"));
        }
    }
}