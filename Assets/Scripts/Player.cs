using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Player : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private Transform tpPoint;
    [SerializeField] private ScriptableRendererFeature lowHealth, deathTxt, deathExtra, invertColors;
    [SerializeField] private GameObject XRRig;

    private int _maxHealth;
    public bool ded = true;
    public int score;
    
    void Awake()
    {
        lowHealth.SetActive(false);
        deathTxt.SetActive(false);
        deathExtra.SetActive(false);
        invertColors.SetActive(false);
        _maxHealth = health;
    }

    public void Hurt(int amount)
    {
        health -= amount;
        lowHealth.SetActive(true);
        StopCoroutine("Ouch");
        if (health <= (_maxHealth / 4))
        {
            lowHealth.SetActive(true);
        }
        else
        {
            StartCoroutine("Ouch");
        }
        if (health <= 0)
        {
            lowHealth.SetActive(false);
            //deathTxt.SetActive(true);
            deathExtra.SetActive(true);
            //invertColors.SetActive(true);
            ded = true;
            StartCoroutine("Tp");
        }
    }

    public void Level()
    {
        ded = false;
        score = 0;
    }

    private IEnumerator Ouch()
    {
        yield return new WaitForSeconds(0.375f);
        lowHealth.SetActive(false);
    }

    private IEnumerator Tp()
    {
        yield return new WaitForSeconds(1);
        XRRig.transform.position = tpPoint.position;
        lowHealth.SetActive(false);
        deathExtra.SetActive(false);
        health = _maxHealth;
        Debug.Log("SCORE " + score.ToString());
    }
}
