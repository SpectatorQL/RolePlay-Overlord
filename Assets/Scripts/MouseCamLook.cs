/* 
 * author : jiankaiwang
 * description : The script provides you with basic operations 
 *               of first personal camera look on mouse moving.
 * platform : Unity
 * date : 2017/12
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCamLook : MonoBehaviour
{
    float _yaw;
    float _pitch;
    public float Sensitivity = 2.0f;

    Transform _characterHead;

    void Start() 
    {
        //Cursor.lockState = CursorLockMode.Locked;

        _yaw = transform.eulerAngles.y;
        _pitch = transform.eulerAngles.x;

        _characterHead = transform.parent;
    }
	
	void Update()
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
        _characterHead.eulerAngles = rotation;
    }
}