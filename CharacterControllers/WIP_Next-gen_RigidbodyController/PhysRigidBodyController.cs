using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PhysRigidBodyController : MonoBehaviour
{
    public GameObject playerCamera;
    public float maxSpeed = 5f;
    public float maxMoveForce = 5f;
    public bool moveInAir = true;
    public float jumpImpulse = 3f;
    public bool sprintEnable = false;
    public float sprintMultiplier = 1.5f;
    public bool crouchEnable = false;
    public float crouchMultiplier = .6f;
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
    private Rigidbody _collidingRigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>() ? GetComponent<Rigidbody>() : this.gameObject.AddComponent<Rigidbody>();
        
        //removes sticking to walls
        GetComponent<Collider>().material.dynamicFriction = 0;
        GetComponent<Collider>().material.staticFriction = 0;
        GetComponent<Collider>().material.frictionCombine = PhysicMaterialCombine.Minimum;

        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        _rigidbody.drag = rigidBodyDrag;
        _rigidbody.angularDrag = rigidBodyAngularDrag;
        _rigidbody.mass = rigidBodyMass;

        // ground detecting collider setup
        _groundDetector = this.gameObject.AddComponent<BoxCollider>();
        _groundDetector.size = new Vector3(groundDetectorSize*2, groundDetectorSize, groundDetectorSize*2);
        _groundDetector.center = new Vector3(0, -centerOffset, 0);
        _groundDetector.isTrigger = true;
        
        //removes icy effect of sticking to walls fix by adding non slip collider to feet
        CapsuleCollider feet = this.gameObject.AddComponent<CapsuleCollider>();
        feet.center = new Vector3(0, -centerOffset, 0);
        feet.radius = .05f;
        feet.height = .05f;

    }

    private void Update()
    {
        if(_onGround || moveInAir)
            Movement();
        Look();
        Jump();
    }

    private void Movement()
    {
        Vector2 xzSpeed = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.z);

        Vector3 relativeSpeed = new Vector3();

        if (_collidingRigidbody)
        {
            relativeSpeed = _collidingRigidbody.velocity - _rigidbody.velocity;
        }
        
        Vector3 axisMove = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if ((xzSpeed.magnitude < maxSpeed || relativeSpeed.magnitude < maxSpeed))
        {
            

            if (axisMove.magnitude > 1)
                axisMove.Normalize();

            float movementMultiplier = maxMoveForce;

            if (sprintEnable)
            {
                try
                {
                    if (Input.GetAxis("Sprint") > .9)
                    {
                        movementMultiplier *= sprintMultiplier;
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("Error in Sprinting! No axis named 'Sprint'?");
                }
            }
            if (crouchEnable)
            {
                try
                {
                    if (Input.GetAxis("Crouch") > .9)
                    {
                        movementMultiplier *= crouchMultiplier;
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("Error in Crouching! No axis named 'Crouch'?");
                }
            }
            
            _rigidbody.AddForce(axisMove*movementMultiplier, ForceMode.Force);

        }
        
    }

    private void Jump()
    {
        if (_onGround && (Input.GetAxis("Jump")>.9f))
        {
            _rigidbody.AddForce(transform.up*jumpImpulse, ForceMode.VelocityChange);
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
        try
        {
            _collidingRigidbody = other.gameObject.GetComponent<Rigidbody>();
        }
        // thrown error ignored, because no rigidbody in ground is no error
        catch (Exception e)
        {
            // ignored
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _collidingObject = null;
        _onGround = false;
        _collidingRigidbody = null;
    }
}
