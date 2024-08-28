using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapToGrid : MonoBehaviour
{
    [SerializeField]
    private float gridSize = 0.5f;

    void Start()
    {
        SnapToGridFunction();
    }

    public void SnapToGridFunction()
    {
        // Get the current position
        Vector3 position = transform.position;

        //Debug.Log("initial position of " + gameObject.name + transform.position);

        // Snap each axis to the nearest multiple of gridSize
        position.x = Mathf.Round(position.x / gridSize) * gridSize;
        position.y = transform.position.y;
        position.z = Mathf.Round(position.z / gridSize) * gridSize;

        // Update the position
        transform.position = position;

        //Debug.Log("final position of " + gameObject.name + position);
    }
}
