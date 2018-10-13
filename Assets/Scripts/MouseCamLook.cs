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

    void Start() 
    {
        _yaw = transform.eulerAngles.y;
        _pitch = transform.eulerAngles.x;
    }
	
	void Update()
    {
        float rotMin = -30.0f;
        float rotMax = 30.0f;
        float inputX = Input.GetAxisRaw("Mouse X");
        float inputY = Input.GetAxisRaw("Mouse Y");

        _yaw += inputX * Sensitivity;
        _pitch -= inputY * Sensitivity;
        if(_yaw > rotMax)
        {
            _yaw = rotMax;
        }
        if(_yaw < rotMin)
        {
            _yaw = rotMin;
        }
        if(_pitch > rotMax)
        {
            _pitch = rotMax;
        }
        if(_pitch < rotMin)
        {
            _pitch = rotMin;
        }

        Vector3 rotation = new Vector3(_pitch, _yaw, 0.0f);
        transform.eulerAngles = rotation;
    }
}