using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillMeAfterSeggs : MonoBehaviour
{
    [SerializeField] private float time;
    
    private void Update()
    {
        Destroy(this.gameObject, time);
    }
}
