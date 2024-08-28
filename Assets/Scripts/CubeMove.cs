using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CubeMove : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    public Vector3 velocity = Vector3.zero;
    public bool isMoving = false;
    GameManager manager;

    private void Start()
    {
        manager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        SetVelocity();
    }


    private void OnCollisionEnter(Collision collision)
    {
        isMoving = false;
        velocity = Vector3.zero;
        manager.CheckIfAllStopped();

        //send message to gameManager that this cube has stopped
    }

    private void OnDisable()
    {
        isMoving = false;
        velocity = Vector3.zero;
        manager.CheckIfAllStopped();
    }

    private void SetVelocity()
    {
        rb.velocity = velocity;
    }
}
