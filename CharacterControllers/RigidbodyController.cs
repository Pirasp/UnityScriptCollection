using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Universal first person Rigidbody Controller
 * Automatic Rigidbody setup, starting hight callibration may be needed (offset between player feet and center)
 * made to use a capsule collider to be able to climb stairs
 *
 * uses the input axes: Horizontal, Vertical and Jump
 *
 * made by Pirasp
 * https://github.com/Pirasp
 */

public class RigidbodyController : MonoBehaviour
{
    public GameObject camera;
    public float hightOffset = 1f;
    public float maxSpeed = 3f;
    public float jumpImpuls = 3f;
    [Tooltip("the length of the ground detection ray underneath the character. Increase if character can't jump on shallow slopes")]
    public float groundDetectionRayOvershoot = 0.05f;
    private Rigidbody _rigidbody;
    [Tooltip("Only enable this option if you made an axis named 'Sprint'")]
    public bool sprintingEnable = false;

    public float sprintMultiplyer = 1.2f;
    
    public float mouseSensitivity = 100.0f;
    public float clampAngle = 80.0f;
 
    private float _rotY = 0.0f; // rotation around the up/y axis
    private float _rotX = 0.0f; // rotation around the right/x axis    

    [Tooltip("Enable this option if this script disables the cursor in main menu.")]
    public bool cursorVisibleOnStartup = false;

    private bool firstStartup = true;

    private void Start()
    {
        //setup of rigidbody component if not existent before
        if (!GetComponent<Rigidbody>())
        {
            this.gameObject.AddComponent<Rigidbody>();
            Debug.Log("No Rigidbody Component found, added dynamically!");
        }

        //drag is set to 0 to avoid wallrunning
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.drag = 0;
        _rigidbody.angularDrag = 0;
        
        //set rotation locks
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;

    }

    private void Update()
    {
        Movement();
        Rotation();
        Jump();

        //this is in update because it needs to run with the first frame not before
        if (firstStartup)
        {
            ChangeCursorVisibility(cursorVisibleOnStartup);
            firstStartup = false;
        }
    }

    //mouse look
    private void Rotation()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = -Input.GetAxis("Mouse Y");
 
        _rotY += mouseX * mouseSensitivity * Time.deltaTime;
        _rotX += mouseY * mouseSensitivity * Time.deltaTime;
 
        _rotX = Mathf.Clamp(_rotX, -clampAngle, clampAngle);
        
        Quaternion localRotation = Quaternion.Euler(_rotX,0f,0f);
        camera.transform.localRotation = localRotation;

        Quaternion rotationChar = Quaternion.Euler(0f, _rotY, 0f);
        transform.rotation = rotationChar;

        //Quaternion localRotation = Quaternion.Euler(_rotX, _rotY, 0.0f);
        //transform.rotation = localRotation;
    }

    //move using input axes
    private void Movement()
    {

        Vector3 savedVelocity = _rigidbody.velocity;

        Vector3 fVec = transform.forward * Input.GetAxis("Vertical");
        Vector3 sVec = transform.right * Input.GetAxis("Horizontal");
        
        sVec += fVec;
        
        if (sVec.magnitude > 1)
            sVec.Normalize();

        sVec *= maxSpeed;
        
        if (sprintingEnable)
        {
            try
            {
                if (Input.GetAxis("Sprint") > .9)
                    sVec *= sprintMultiplyer;
            }
            catch (Exception e)
            {
                Debug.LogError("No 'Sprint' axis found! Please create one or disable sprinting!");
            }
        }

        _rigidbody.velocity = new Vector3(sVec.x, savedVelocity.y, sVec.z);

    }
    
    //Jump using Jump axis
    private void Jump()
    {
        if (Input.GetAxis("Jump") > .9f)
        {
            if (Physics.Raycast(transform.position, -transform.up, hightOffset + groundDetectionRayOvershoot))
            {
                Vector3 v = _rigidbody.velocity;
                v.y = jumpImpuls;
                _rigidbody.velocity = v;
            }
        }
    }

    public void ChangeCursorVisibility(bool cursorVisible)
    {
        Cursor.visible = cursorVisible;
    }
}
