using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

/*
 * Move by pulling yourself through space with your controllers
 */

public class PinchMove : MonoBehaviour
{
    public SteamVR_Input_Sources inputSource;
    public SteamVR_Behaviour_Pose controllerPoseLeft;
    public SteamVR_Behaviour_Pose controllerPoseRight;
    public SteamVR_Action_Boolean pinchMoveLeft;
    public SteamVR_Action_Boolean pinchMoveRight;

    public bool moveIn3D = false;
    public GameObject camRig;

    private Vector3 lastPosL, lastPosR;
    private bool firstL = true, firstR = true;

    private void Update()
    {

        //linear Movement
        if (pinchMoveLeft.GetState(inputSource) ^ pinchMoveRight.GetState(inputSource))
        {
            if (pinchMoveLeft.GetState(inputSource))
            {
                if (firstL)
                {
                    lastPosL = controllerPoseLeft.transform.position;
                    firstL = false;
                }

                Vector3 movVec = lastPosL - controllerPoseLeft.transform.position;
                if (!moveIn3D)
                    movVec.y = 0;
                camRig.transform.position += movVec;
                lastPosL = controllerPoseLeft.transform.position;
            }
            else if(pinchMoveRight.GetState(inputSource))
            {
                if (firstR)
                {
                    lastPosR = controllerPoseRight.transform.position;
                    firstR = false;
                }

                Vector3 movVec = lastPosR - controllerPoseRight.transform.position;
                if (!moveIn3D)
                    movVec.y = 0;
                camRig.transform.position += movVec;
                lastPosL = controllerPoseRight.transform.position;
            }
        }
        //rotation
        else if (pinchMoveLeft.GetState(inputSource) && pinchMoveRight.GetState(inputSource))
        {
            
        }

        if (pinchMoveLeft.GetStateUp(inputSource))
            firstL = true;
        if (pinchMoveRight.GetStateUp(inputSource))
            firstR = true;
    }
}
