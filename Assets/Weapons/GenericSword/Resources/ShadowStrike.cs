using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowStrike : MonoBehaviour
{
    public GameObject shadowslash;
    public float duration;
    public float radius;
    public float rate;
    private bool slashing = true;


    private ParticleSystem particles;
    private Vector3 pos;
    private Quaternion angle;
    void Start()
    {
        particles = GetComponent<ParticleSystem>();
        duration = 5f;
        radius = 4.0f;
        StartCoroutine(Destruction());

    }

    void Update()
    {
        Randomizer();
        Spawner();
    }


    private void Randomizer()
    {
        pos = new Vector3(Random.Range(-radius, radius), Random.Range(-radius, radius), 0);

    }
    private void Spawner()
    {
        float elapsed = 0f;
        if (elapsed < duration && slashing)
        {
            elapsed += Time.deltaTime;
            StartCoroutine(Slash());

        }
    }


    private IEnumerator Destruction()
    {
        yield return new WaitForSeconds(duration);
        slashing = false;
        particles.Stop();
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
    private IEnumerator Slash()
    {
        yield return new WaitForSeconds(Random.Range(0.1f, 0.45f));
        Instantiate(shadowslash,transform.position + pos, transform.rotation);

    }
}
