using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ZombieAnimeGirl : MonoBehaviour
{
    [SerializeField] private int hp;
    private int _initHp;
    [SerializeField] private Animator animator;
    [SerializeField] private float speed;
    [SerializeField] private GameObject bloodExplosionFX;
    [SerializeField] private GameObject player;
    private Rigidbody _rb;
    private bool _ded;
    [SerializeField] private float attackInterval;
    private bool _canAttack;
    private GameObject mainCamera;

    private bool _crawler;

    private void Awake()
    {
        _canAttack = true;
        _initHp = hp;
        _crawler = false;
        player = GameObject.FindWithTag("Player");
        _rb = this.GetComponent<Rigidbody>();
        animator.SetBool("Alt", (Random.Range(0, 10) > 5));
        animator.SetBool("Spawn", true);
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    void Update()
    {
        if(_ded) return;
        AnimationUpdates();
        Vector3 plaPos = mainCamera.GetComponent<Transform>().position;
        Vector3 selfPos = this.transform.position;
        Vector3 trgtDir = new Vector3(plaPos.x - selfPos.x, plaPos.y - selfPos.y, plaPos.z - selfPos.z);
        trgtDir.Normalize();
        if (!_canAttack) return;
        _rb.velocity = new Vector3(trgtDir.x*speed, _rb.velocity.y, trgtDir.z*speed);
        Vector3 plaPosNoY = new Vector3(plaPos.x, selfPos.y, plaPos.z);
        transform.LookAt(plaPosNoY);
        if (_canAttack && (Vector3.Distance(selfPos, plaPos) <= 1.75f))
        {
            Debug.Log("Attacked");
            animator.SetBool("Attack", true);
            player.GetComponent<Player>().Hurt(1);
            _canAttack = false;
            StartCoroutine("AttackCd");
        }
    }

    private void AnimationUpdates()
    {
        float sped = Mathf.Abs(_rb.velocity.x) + Mathf.Abs(_rb.velocity.z);
        animator.SetBool("Move", (sped > 0.125));
        animator.SetBool("Idle", (sped < 0.125));
        animator.SetBool("Crawl", _crawler);
    }
    
    public void Hurt(int amount)
    {
        hp -= amount;
        if (hp <= 0 && !_ded)
        {
            animator.SetBool("Death", true);
            hp = 0;
            if (!_crawler)
            {animator.SetBool("Attack", true);
                _crawler = (Random.Range(0, 10) < 1);
                hp = (_crawler) ? (int)Random.Range(1, _initHp) : 0;
                animator.SetBool("Death", false);
            }
            if (hp <= 0)
            {
                _ded = true;
                GameObject a = Instantiate(bloodExplosionFX);
                a.transform.position = this.transform.position;
                a.transform.Rotate(a.transform.up, Random.Range(0,360));
                _rb.useGravity = false;
                SphereCollider[] f = this.GetComponentsInChildren<SphereCollider>();
                foreach (SphereCollider sphere in f)
                {
                    sphere.enabled = false;
                }
                //Agregar score
                player.GetComponent<Player>().score++;
                _rb.velocity = new Vector3(0, 0, 0);
                this.transform.position = new Vector3(transform.position.x, transform.position.y - 0.75f, transform.position.z);
                animator.SetBool("Explode", true);
                StartCoroutine("Explode");
                Destroy(this.gameObject, 5);
            }
        }
    }

    private IEnumerator Explode()
    {
        yield return new WaitForSeconds(4.9f);
        GameObject a = Instantiate(bloodExplosionFX);
        a.transform.position = this.transform.position + new Vector3(0,1f,0);
        a.transform.Rotate(a.transform.up, Random.Range(0, 360));
    }

    private IEnumerator AttackCd()
    {
        yield return new WaitForSeconds(attackInterval);
        animator.SetBool("Attack", false);
        _canAttack = true;
    }
}
