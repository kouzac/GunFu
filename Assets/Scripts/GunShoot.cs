using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GunShoot : MonoBehaviour
{
    [SerializeField] private Transform nozzle;
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private ParticleSystem particle2;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int dmg;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float shootInterval;

    private bool _canShoot;
    private bool _flag;
    private Rigidbody _rBody;
    private XRGrabInteractable _xrInteractable;
    
    void Awake()
    {
        _canShoot = true;
        _xrInteractable = this.GetComponent<XRGrabInteractable>();
        _rBody = this.GetComponent<Rigidbody>();
        WeaponEventsSetup();
    }

    private void Update()
    {
        Shooting();
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
        //hand.GetComponent<MeshThingh>().Hide();
        MeshRenderer[] meshes = hand.GetComponentsInChildren<MeshRenderer>();
        foreach (var mesh in meshes)
        {
            mesh.enabled = true;
        }
    }
    
    private void DropWeapon(XRBaseInteractor hand)
    {
        //hand.GetComponent<MeshThingh>().Show();
        _canShoot = true;
        _flag = false;
        MeshRenderer[] meshes = hand.GetComponentsInChildren<MeshRenderer>();
        foreach (var mesh in meshes)
        {
            mesh.enabled = true;
        }
    }
    
    private void ShootStart(XRBaseInteractor hand)
    {
        _flag = true;
        if(!_canShoot)return;
        Shooting();
        StartCoroutine("ShootInterval");
    }
    
    private void ShootStop(XRBaseInteractor hand)
    {
        StopCoroutine("ShootInterval");
        _canShoot = true;
        _flag = false;
    }
    
    private IEnumerator ShootInterval()
    {
        _canShoot = false;
        yield return new WaitForSeconds(shootInterval);
        _canShoot = true;
    }

    private void Shooting()
    {
        if(!_flag || !_canShoot) return;
        Shoot();
    }
    
    private void Shoot()
    {
        particle.Stop();
        particle.Play();
        particle2.Play();
        GameObject a = Instantiate(bulletPrefab);
        a.GetComponent<Bullet>().dmg = dmg;
        Transform nozzleT = nozzle.transform;
        a.transform.position = nozzleT.position;
        Vector3 dir = nozzleT.forward;
        a.transform.rotation = Quaternion.LookRotation(dir*-1);
        dir.Normalize();
        a.GetComponent<Rigidbody>().AddForce(dir * bulletSpeed, ForceMode.Impulse);
    }
}
