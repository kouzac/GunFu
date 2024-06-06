using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPosArr;
    [SerializeField] private List<GameObject> enemies;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject enemyPrefab;
    private bool canTry;

    private void Awake()
    {
        canTry = true;
    }

    private void FixedUpdate()
    {
        if(!player.GetComponent<Player>().ded && canTry) StartCoroutine("TrySpawn");
        if (player.GetComponent<Player>().ded)
        {
            foreach (GameObject kk in enemies)
            {
                enemies.Remove(kk);
                Destroy(kk);
            }
        }
    }

    private IEnumerator TrySpawn()
    {
        canTry = false;
        yield return new WaitForSeconds(2.5f);
        Vector3 pos = spawnPosArr[Random.Range(0, spawnPosArr.Length)].position;
        GameObject enemy = Instantiate(enemyPrefab);
        enemy.transform.position = pos;
        enemies.Add(enemy);
        canTry = true;
    }
}
