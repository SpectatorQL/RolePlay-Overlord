using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace RolePlayOverlord
{
    public delegate void process_input(ClientEntity ent, PlayerController controller);

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

        public bool Alpha1;
        public bool Alpha2;
        public bool Alpha3;
        public bool Alpha4;
        public bool Alpha5;
        public bool Alpha6;

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


            if(controller.NewInput.V)
            {
            }
            else
            {
            }
        }

        public static void ProcessClientKeyboard(ClientEntity ent, PlayerController controller)
        {

        }

        public static void ProcessHostMouse(ClientEntity ent, PlayerController controller)
        {
            if(controller.NewInput.RmbDown)
            {
                ent.RotateHostCamera(controller.NewInput.MouseX, controller.NewInput.MouseY);
            }
        }

        public static void ProcessClientMouse(ClientEntity ent, PlayerController controller)
        {
            ent.RotateClientCamera(controller.NewInput.MouseX, controller.NewInput.MouseY);
        }
    }

    public enum ControlMode
    {
        Host,
        Client_Cam,
        Client_UI,
        Chat
    }
}
