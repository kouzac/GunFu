using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraPortal : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Camera otherCam;
    
    void FixedUpdate()
    {
        Transform plaTra = playerCamera.transform;
        
        Quaternion dir = Quaternion.Inverse(transform.rotation) * plaTra.rotation;
        otherCam.transform.localEulerAngles = new Vector3(dir.eulerAngles.x, dir.eulerAngles.y + 180, dir.eulerAngles.z);
        
        Vector3 dist = transform.InverseTransformPoint(plaTra.position);
        otherCam.transform.localPosition = -new Vector3(dist.x, -dist.y, dist.z);
    }
}
