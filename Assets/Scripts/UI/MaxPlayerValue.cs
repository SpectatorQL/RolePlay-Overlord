using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaxPlayerValue : MonoBehaviour
{
    Text currentValue;

    void Start()
    {
        currentValue = GetComponent<Text>();
    }
	
    public void valueUpdate (float value)
    {
        currentValue.text = Convert.ToString(value);
    }
}
