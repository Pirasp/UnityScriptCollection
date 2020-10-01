using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysRigidBodyController : MonoBehaviour
{
    public GameObject playerCamera;
    public float maxSpeed = 5f;
    public float maxMoveForce = 5f;
    public bool sprintEnable = false;
    public float sprintMultiplier = 1.5f;
    public float centerOffset = 1f;
    public float groundDetectorSize = .2f;

    public float rigidBodyDrag = 0f, rigidBodyAngularDrag = 0.05f, rigidBodyMass = 1f;
    
    public float mouseSensitivity = 100.0f;
    public float clampAngle = 80.0f;
 
    private float _rotY = 0.0f; // rotation around the up/y axis
    private float _rotX = 0.0f; // rotation around the right/x axis  
    
    private Rigidbody _rigidbody;
    private BoxCollider _groundDetector;

    private bool _onGround = false;
    private GameObject _collidingObject;

    private void Start()
    {
        if (GetComponent<Rigidbody>())
        {
            _rigidbody = GetComponent<Rigidbody>();
        }
        else
        {
            _rigidbody = this.gameObject.AddComponent<Rigidbody>();
        }

        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        _rigidbody.drag = rigidBodyDrag;
        _rigidbody.angularDrag = rigidBodyAngularDrag;
        _rigidbody.mass = rigidBodyMass;

        // ground detecting collider setup
        _groundDetector = this.gameObject.AddComponent<BoxCollider>();
        _groundDetector.size = new Vector3(groundDetectorSize*2, groundDetectorSize, groundDetectorSize*2);
        _groundDetector.center = new Vector3(0, -centerOffset, 0);
        _groundDetector.isTrigger = true;
    }

    private void Update()
    {
        Movement();
        Look();
        Jump();
    }

    private void Movement()
    {
        
    }

    private void Jump()
    {
        if (_onGround)
        {
            
        }
    }

    private void Look()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = -Input.GetAxis("Mouse Y");
 
        _rotY += mouseX * mouseSensitivity * Time.deltaTime;
        _rotX += mouseY * mouseSensitivity * Time.deltaTime;
 
        _rotX = Mathf.Clamp(_rotX, -clampAngle, clampAngle);
        
        Quaternion localRotation = Quaternion.Euler(_rotX,0f,0f);
        playerCamera.transform.localRotation = localRotation;

        Quaternion rotationChar = Quaternion.Euler(0f, _rotY, 0f);
        transform.rotation = rotationChar;
    }

    private void OnTriggerEnter(Collider other)
    {
        _collidingObject = other.gameObject;
        _onGround = true;
    }

    private void OnTriggerExit(Collider other)
    {
        _collidingObject = null;
        _onGround = false;
    }
}
