using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Player : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private ScriptableRendererFeature lowHealth, deathTxt, deathExtra, invertColors;

    private int _maxHealth;
    
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
        if (health <= (_maxHealth / 4))
        {
            lowHealth.SetActive(true);
        }
        if (health <= 0)
        {
            lowHealth.SetActive(false);
            deathTxt.SetActive(true);
            deathExtra.SetActive(true);
            invertColors.SetActive(true);
        }
    }
}
