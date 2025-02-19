using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowStrike : MonoBehaviour
{
    public GameObject shadowslash;
    public float duration;
    public float radius;
    public float rate;

    private Vector3 pos;
    private Quaternion angle;
    void Start()
    {
        duration = 5f;
        radius = 4.0f;
        
    }

    void Update()
    {
        Randomizer();
        Spawner();
    }


    private void Randomizer()
    {
        pos = new Vector3(Random.Range(-radius, radius), Random.Range(2, radius), 0);

    }
    private void Spawner()
    {
        float elapsed = 0f;
        if (elapsed < duration)
        {
            StartCoroutine(Slash());
        }

        if (elapsed >= duration)
        {
            Destroy(gameObject, 1f);
        }
    }

    private IEnumerator Slash()
    {
        yield return new WaitForSeconds(Random.Range(0.25f, 0.65f));
        Instantiate(shadowslash, pos, transform.rotation);
    }
}
