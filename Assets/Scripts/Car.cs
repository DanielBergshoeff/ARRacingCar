using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(Rigidbody))]
public class Car : MonoBehaviour
{
    public float MoveSpeed = 3.0f;
    public float RotateSpeed = 5.0f;
    public float AddedForce = 10.0f;

    private Rigidbody myRigidBody;

    void Start()
    {
        myRigidBody = GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        UpdateCarPosition();
    }

    /// <summary>
    /// Update car position based on joystick input
    /// </summary>
    private void UpdateCarPosition() {
        Vector3 movement = new Vector3(CrossPlatformInputManager.GetAxis("Horizontal"), 0f, CrossPlatformInputManager.GetAxis("Vertical"));

        transform.Rotate(Vector3.up * CrossPlatformInputManager.GetAxis("Horizontal") * Time.deltaTime * RotateSpeed);
        transform.position = transform.position + transform.forward * CrossPlatformInputManager.GetAxis("Vertical") * Time.deltaTime * MoveSpeed;
    }
}
