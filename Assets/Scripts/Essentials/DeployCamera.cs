using System.Collections;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

public class DeployCamera : MonoBehaviour
{
    public GameObject insertion;
    public Vector3 deployPosition;
    public Vector3 insertionPoint;
    public GameObject bishop;
    public InsertionHellpod insertionHellpod;
    public GameObject LZpref;
    public GameObject LZ;

    
    public float moveSpeed = 5f;
    public float lerpSpeed = 10f;
    private Vector3 targetPosition;
    private bool moving = false;
    
    private bool hasChosenSpawnPoint0;
    private bool hasChosenSpawnpoint = false;
    
    void Start()
    {

        insertionPoint = new Vector3(Random.Range(-30,30),Random.Range(90,250));
        hasChosenSpawnPoint0 = false;
         LZ = Instantiate(LZpref, transform.position, transform.rotation);
    }

    void FixedUpdate()
    {
        LandCheck();
        if (!hasChosenSpawnPoint0)
        {
            if (Input.GetMouseButtonDown(0)) // Left-click sets the target
            {
                targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                targetPosition.z = transform.position.z; // Keep the camera's Z position
                moving = true;
            }

            if (moving)
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, lerpSpeed * Time.deltaTime);

                // Stop moving when close enough
                if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
                {
                    transform.position = targetPosition;
                    moving = false;
                }
            }
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

            transform.position += new Vector3(x, y, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    void ChoseSpawn()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !hasChosenSpawnpoint)
        {
            deployPosition = LZ.transform.position;
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