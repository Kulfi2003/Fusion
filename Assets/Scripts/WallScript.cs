using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WallScript : MonoBehaviour
{
    [SerializeField]
    private float gridSize = 0.5f;

    void Awake()
    {
        SnapToGridFunction();

    }

    private void SnapToGridFunction()
    {

        // Get the current position
        Vector3 position = transform.position;

        // Snap each axis to the nearest multiple of gridSize
        position.x = Mathf.Round(position.x / gridSize) * gridSize;
        position.y = transform.position.y;
        position.z = Mathf.Round(position.z / gridSize) * gridSize;

        //gameObject.isStatic = false;

        // Update the position
        transform.position = position;

        transform.localScale = new (1f,Random.Range(1f,2f),1f); // randomizing y scale

        gameObject.isStatic = true;

    }
}
