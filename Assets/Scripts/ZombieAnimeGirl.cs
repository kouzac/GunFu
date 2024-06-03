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

    private bool _crawler;

    private void Awake()
    {
        _initHp = hp;
        _crawler = false;
        player = GameObject.FindWithTag("Player");
        _rb = this.GetComponent<Rigidbody>();
        animator.SetBool("Alt", (Random.Range(0, 10) > 5));
        animator.SetBool("Spawn", true);
    }

    void Update()
    {
        if(_ded) return;
        AnimationUpdates();
        Vector3 plaPos = player.GetComponent<Transform>().position;
        Vector3 selfPos = this.transform.position;
        Vector3 trgtDir = new Vector3(plaPos.x - selfPos.x, plaPos.y - selfPos.y, plaPos.z - selfPos.z);
        trgtDir.Normalize();
        _rb.velocity = new Vector3(trgtDir.x*speed, _rb.velocity.y, trgtDir.z*speed);
        Vector3 plaPosNoY = new Vector3(plaPos.x, selfPos.y, plaPos.z);
        transform.LookAt(plaPosNoY);
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
            {
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
                Destroy(this.gameObject, 5);
            }
        }
    }
}
