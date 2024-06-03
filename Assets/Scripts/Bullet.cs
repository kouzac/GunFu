using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject bloodParticlePrefabA, bloodParticlePrefabB;
    [SerializeField] private GameObject brainParticlePrefab;
    
    private GameObject _player;
    public int dmg;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        Destroy(this.gameObject, 3);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyHead"))
        {
            other.GetComponentInParent<Cloth>().GetComponentInParent<GameObject>().SetActive(false);
            other.GetComponentInParent<SkinnedMeshRenderer>().enabled = false;
            GameObject a = Instantiate(brainParticlePrefab);
            a.transform.position = this.transform.position;
            a.transform.parent = other.transform;
            other.GetComponentInParent<ZombieAnimeGirl>().Hurt(dmg*2);
            other.GetComponent<SphereCollider>().enabled = false;
            Destroy(this.gameObject);
            return;
        }
        if (other.CompareTag("Enemy"))
        {
            GameObject a = (Random.Range(0, 10) > 5) ? Instantiate(bloodParticlePrefabA) : Instantiate(bloodParticlePrefabB);
            a.transform.position = this.transform.position;
            a.transform.LookAt(_player.transform.position);
            a.transform.parent = other.transform;
            other.GetComponentInParent<ZombieAnimeGirl>().Hurt(dmg);
            Destroy(this.gameObject);
        }
        if (!other.CompareTag("Gun"))
        {
           Destroy(this.gameObject);
        }
    }
    
}
