using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Teleport : MonoBehaviour
{
    public SteamVR_Input_Sources inputSource;
    public SteamVR_Behaviour_Pose controllerPose;
    public SteamVR_Action_Boolean laserAction;
    public SteamVR_Action_Boolean teleportAction;

    public GameObject laserPrefab;
    public int maxDistance = 100;
    private GameObject laser;
    public GameObject camRig, playerCamera;

    private void Start()
    {
        laser = Instantiate(laserPrefab);
    }

    private void Update()
    {
        if (laserAction.GetState(inputSource))
        {
            RaycastHit hit;

            if (Physics.Raycast(controllerPose.transform.position, transform.forward, out hit, maxDistance))
            {
                Laser.ShowLaser(controllerPose.transform.position, hit.point, laser);
            }
            else
            {
                Laser.ShowLaser(controllerPose.transform.position, 
                    controllerPose.transform.position+controllerPose.transform.forward*maxDistance, laser);
            }
        }
        else
        {
            laser.SetActive(false);
        }

        if (teleportAction.GetLastStateDown(inputSource))
        {
            RaycastHit hit;
            if (Physics.Raycast(controllerPose.transform.position, transform.forward, out hit, maxDistance))
            {
                Vector3 offset = new Vector3(playerCamera.transform.position.x, 0, playerCamera.transform.position.z);
                Vector3 camxz = new Vector3(camRig.transform.position.x, 0, camRig.transform.position.z);
                offset -= camxz;
                camRig.transform.position = hit.point - offset;
            }
        }
    }
}
