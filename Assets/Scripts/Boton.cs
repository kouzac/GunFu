using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boton : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Transform pos;
    [SerializeField] private GameObject barrera;

    public void click()
    {
        player.transform.position = pos.position;
        barrera.transform.position = player.transform.position;
        player.GetComponent<Player>().Level();
    }
}
