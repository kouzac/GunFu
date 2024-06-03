using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using Random = UnityEngine.Random;

public class PortalGun : MonoBehaviour
{
    [SerializeField] private Transform shootFrom;
    [SerializeField] private ParticleSystem[] shootParticles;
    [SerializeField] private float shootingRange;
    [SerializeField] private LayerMask selfLayer;
    [SerializeField] private GameObject[] portalPrefabs;
    [SerializeField] private GameObject rHand, lHand;
    
    private bool _grabbed;
    private XRGrabInteractable _xrInteractable;
    private RaycastHit _hit;
    private Vector3 _dir;
    private int _mode;
    private bool _a, _b;
    private List<GameObject> _portals = new();
    
    void Awake()
    {
        _xrInteractable = this.GetComponent<XRGrabInteractable>();
        WeaponEventsSetup();
    }

    private void Update()
    {
        _dir = shootFrom.forward;
        _dir.Normalize();

        if (CommonUsages.primaryButton.ConvertTo<bool>())
        {
            _mode = 0;
            Debug.Log("ModeA");
        }
        if (CommonUsages.secondaryButton.ConvertTo<bool>())
        {
            _mode = 1;
            Debug.Log("ModeB");
        }
    }

    private void WeaponEventsSetup()
    {
        _xrInteractable.onSelectEntered.AddListener(PickUpWeapon);
        _xrInteractable.onSelectExited.AddListener(DropWeapon);
        _xrInteractable.onActivate.AddListener(ShootStart);
        _xrInteractable.onDeactivate.AddListener(ShootStop);
    }

    private void PickUpWeapon(XRBaseInteractor hand)
    {
        if (hand.containingGroup.groupName == "Left")
        {
            MeshRenderer[] meshes = lHand.GetComponentsInChildren<MeshRenderer>();
            foreach (var mesh in meshes)
            {
                mesh.enabled = false;
            }
        }
        else
        {
            MeshRenderer[] meshes = rHand.GetComponentsInChildren<MeshRenderer>();
            foreach (var mesh in meshes)
            {
                mesh.enabled = false;
            }
        }
        Debug.Log("WeaponPickUp");
        _grabbed = true;
    }
    
    private void DropWeapon(XRBaseInteractor hand)
    {
        if (hand.containingGroup.groupName == "Left")
        {
            MeshRenderer[] meshes = lHand.GetComponentsInChildren<MeshRenderer>();
            foreach (var mesh in meshes)
            {
                mesh.enabled = true;
            }
        }
        else
        {
            MeshRenderer[] meshes = rHand.GetComponentsInChildren<MeshRenderer>();
            foreach (var mesh in meshes)
            {
                mesh.enabled = true;
            }
        }
        Debug.Log("WeaponDrop");
        _grabbed = false;
        _mode = 0;
    }
    
    private void ShootStart(XRBaseInteractor hand)
    {
        Debug.Log("ShootStart");
    }
    
    private void ShootStop(XRBaseInteractor hand)
    {
        Debug.Log("ShootStop");
        _mode = 0;
    }
    
    private void Shoot()
    {
        if(!_grabbed) return;
        shootParticles[_mode].Play();
        if (Physics.Raycast(shootFrom.position, _dir, out _hit, shootingRange))
        {
            if (_hit.collider.CompareTag("Enemy") || _hit.collider.CompareTag("EnemyHead"))
            {
                return;
            }
            float x = _hit.point.x - _dir.x * 0.0125f;
            float y = _hit.point.y - _dir.y * 0.0125f;
            float z = _hit.point.z - _dir.z * 0.0125f;
            Vector3 xyz = new Vector3(x, y, z);
            //GameObject bHole = Instantiate(bulletHolePrefab, xyz, Quaternion.identity);
            //bHole.transform.rotation = Quaternion.LookRotation(_dir);
            //GameObject bHoleE = Instantiate(bulletHoleExtra, xyz, Quaternion.identity);
            //bHoleE.transform.rotation = Quaternion.LookRotation(_dir);
            //bHole.transform.SetParent(bulletHoleContainer.transform);
        }
    }

}
