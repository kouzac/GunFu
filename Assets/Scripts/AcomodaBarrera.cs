using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcomodaBarrera : MonoBehaviour
{
    [SerializeField] private Transform cam;

    void Awake()
    {
        transform.position = cam.position;
    }
}
