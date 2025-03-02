using System.Collections;
using UnityEngine;

public class DeployCamera : MonoBehaviour
{
    public GameObject insertion;
    public Vector3 deployPosition;
    public Vector3 insertionPoint;
    public GameObject bishop;
    public InsertionHellpod insertionHellpod;

    private bool hasChosenSpawnPoint0;
    private bool hasChosenSpawnpoint = false;
    
    void Start()
    {
        insertionPoint = new Vector3(Random.Range(-30,30),Random.Range(90,250));
        hasChosenSpawnPoint0 = false;
    }

    void FixedUpdate()
    {
        Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        LandCheck();
        if (!hasChosenSpawnPoint0)
        {
            transform.position = new Vector3(target.x * 0.75f, target.y * 0.75f, -4f);
        }
        else if (hasChosenSpawnPoint0 && bishop!= null)
        {
            transform.position = new Vector3(bishop.transform.position.x, bishop.transform.position.y, -4f);
        }
        ChoseSpawn();
    }
    
    public IEnumerator Shake(float intensity, float duration)
    {
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            float x = Random.Range(-1f,1f) * intensity;
            float y = Random.Range(-1f,1f) * intensity;

            transform.position = transform.position + new Vector3(x, y, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    void ChoseSpawn()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !hasChosenSpawnpoint)
        {
            deployPosition = transform.position;
            hasChosenSpawnpoint = true;
            StartCoroutine(Shake(0.1f, 15f));

            StartCoroutine(DeploySequence());
        }
    }

    void LandCheck()
    {
        
        if (bishop != null && insertionHellpod != null)
        {
            if (insertionHellpod.hasLanded)
            {
                Destroy(gameObject);
            }

        }

    }

    private IEnumerator DeploySequence()
    {
        bishop = Instantiate(insertion, insertionPoint, Quaternion.identity);
        insertionHellpod = bishop.GetComponent<InsertionHellpod>();
        hasChosenSpawnPoint0 = true;
        yield return null;
    }
    
}