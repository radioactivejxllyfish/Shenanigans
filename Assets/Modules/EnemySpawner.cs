using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject _enemyType1;
    public GameObject _enemyType2;
    public float radius;
    public float time;
    public Vector3 position;
    void Start()
    {
        StartCoroutine("SpawnDelay");
    }

    void Update()
    {
        radius = 12f;
        time = Random.Range(3f, 15f);
        position = new Vector3(Random.Range(-radius, radius), Random.Range(-radius, radius), 0f);
    }


    private IEnumerator SpawnDelay()
    {
        while (true)
        {
            Instantiate(_enemyType1, position , Quaternion.identity);
            
            yield return new WaitForSeconds(time);
        }
    }
}
