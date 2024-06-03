using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCam : MonoBehaviour
{
    private Camera _cam;
    [SerializeField] private Transform playerCamera;
    [SerializeField] private Transform mirror;
    
    void Awake()
    {
        _cam = this.GetComponentInChildren<Camera>();
    }

    void FixedUpdate()
    {
        Vector3 localPlayer = mirror.InverseTransformPoint(playerCamera.position);
        _cam.transform.position = mirror.TransformPoint(new Vector3(localPlayer.x, localPlayer.y, -localPlayer.z));

        Vector3 lookAt = mirror.TransformPoint(new Vector3(-localPlayer.x, localPlayer.y, localPlayer.z));
        _cam.transform.LookAt(lookAt);
    }
}
