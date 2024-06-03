using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Random = UnityEngine.Random;

public class GunShootRayCast : MonoBehaviour
{
    [SerializeField] private Transform nozzle;
    [SerializeField] private ParticleSystem shootParticle;
    [SerializeField] private ParticleSystem shootParticleExtra;
    [SerializeField] private ParticleSystem usedBullet;
    [SerializeField] private GameObject bloodParticlePrefabA, bloodParticlePrefabB, brainParticlePrefab;
    [SerializeField] private int dmg;
    [SerializeField] private float shootingRange;
    [SerializeField] private float shootInterval;
    [SerializeField] private bool hasAutoShoot;
    [SerializeField] private Transform laserStart;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private LayerMask selfLayer;
    [SerializeField] private GameObject laserEnd;
    [SerializeField] private GameObject bulletHoleContainer, bulletHolePrefab, bulletHoleExtra;
    [SerializeField] private GameObject rHand, lHand;

    private bool _canShoot;
    private bool _isShooting;
    private bool _grabbed;
    private XRGrabInteractable _xrInteractable;
    private RaycastHit _hit;
    private Transform _laserEnd;
    private Vector3 _dir;
    
    void Awake()
    {
        _canShoot = true;
        _xrInteractable = this.GetComponent<XRGrabInteractable>();
        WeaponEventsSetup();
    }

    private void Update()
    {
        _dir = nozzle.forward;
        _dir.Normalize();
        
        if (hasAutoShoot && _canShoot) Shooting();
        LaserThing();
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
            //transform.position = lHand.transform.position;
            //this.GetComponent<Rigidbody>().Move(lHand.transform.position, lHand.transform.rotation);   NADA SIRVE ALV!!
            //this.transform.SetLocalPositionAndRotation(Vector3.zero, this.transform.rotation);
        }
        else
        {
            MeshRenderer[] meshes = rHand.GetComponentsInChildren<MeshRenderer>();
            foreach (var mesh in meshes)
            {
                mesh.enabled = false;
            }
            //transform.position = rHand.transform.position;
            //this.GetComponent<Rigidbody>().Move(rHand.transform.position, rHand.transform.rotation);   AAAAAAAAAAAAA!!!!
            //this.transform.SetLocalPositionAndRotation(Vector3.zero, this.transform.rotation);
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
        StopCoroutine("ShootInterval");
        _canShoot = true;
        _isShooting = false;
    }
    
    private void ShootStart(XRBaseInteractor hand)
    {
        Debug.Log("ShootStart");
        _isShooting = true;
    }
    
    private void ShootStop(XRBaseInteractor hand)
    {
        Debug.Log("ShootStop");
        StopCoroutine("ShootInterval");
        _canShoot = true;
        _isShooting = false;
    }
    
    private IEnumerator ShootInterval()
    {
        _canShoot = false;
        yield return new WaitForSeconds(shootInterval);
        _canShoot = true;
    }

    private void Shooting()
    {
        if (_canShoot && _isShooting && _grabbed)
        {
            Shoot();
        }
        StartCoroutine("ShootInterval");
    }
    
    private void Shoot()
    {
        if(!_canShoot || !_grabbed || !_isShooting) return;
        ParticleSystem shoot = (Random.Range(0, 10) > 5)? shootParticleExtra : shootParticle;
        shoot.Play();
        usedBullet.Play();
        if (Physics.Raycast(nozzle.position, _dir, out _hit, shootingRange))
        {
            if (_hit.collider.CompareTag("EnemyHead"))
            {
                GameObject a = Instantiate(brainParticlePrefab);
                a.transform.position = _hit.point;
                a.transform.parent = _hit.collider.transform;
                _hit.collider.GetComponentInParent<ZombieAnimeGirl>().Hurt(dmg*2);
                return;
            }
            if (_hit.collider.CompareTag("Enemy"))
            {
                GameObject a = (Random.Range(0, 10) > 5) ? Instantiate(bloodParticlePrefabA) : Instantiate(bloodParticlePrefabB);
                a.transform.position = _hit.point;
                a.transform.LookAt(nozzle);
                a.transform.parent = _hit.collider.transform;
                _hit.collider.GetComponentInParent<ZombieAnimeGirl>().Hurt(dmg);
            }
            else
            {
                float x = _hit.point.x - _dir.x * 0.0125f;
                float y = _hit.point.y - _dir.y * 0.0125f;
                float z = _hit.point.z - _dir.z * 0.0125f;
                Vector3 xyz = new Vector3(x, y, z);
                GameObject bHole = Instantiate(bulletHolePrefab, xyz, Quaternion.identity);
                bHole.transform.rotation = Quaternion.LookRotation(_dir);
                GameObject bHoleE = Instantiate(bulletHoleExtra, xyz, Quaternion.identity);
                bHoleE.transform.rotation = Quaternion.LookRotation(_dir);
                bHole.transform.SetParent(bulletHoleContainer.transform);
            }
        }
    }

    private void LaserThing()
    {
        if (Physics.Raycast(laserStart.position, _dir, out RaycastHit hit, shootingRange, ~selfLayer))
        {
            Vector3[] points = new[] { laserStart.position, hit.point };
            lineRenderer.SetPositions(points);
            laserEnd.SetActive(true);
            laserEnd.transform.position = hit.point;
        }
        else
        {
            laserEnd.SetActive(false);
            Vector3 start = laserStart.position;
            Ray r = new Ray(start, _dir);
            Vector3 end = r.GetPoint(shootingRange);
            Vector3[] points = new[] { start, end };
            lineRenderer.SetPositions(points);
        }
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(nozzle.position, nozzle.forward * shootingRange, new Color(1,0,0,0.25f));
    }
}
