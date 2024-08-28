using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class backButtonListener : MonoBehaviour
{
    //public UnityEvent events;
    public CubeButton cubeButton;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            print("back button pressed");
            cubeButton.onCubeTouched.Invoke();
        }
    }
}
