using System.Collections;
using UnityEngine;

public class DeployCamera : MonoBehaviour
{
    private GameObject cursor;
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
        cursor = GameObject.FindGameObjectWithTag("Cursor");
        hasChosenSpawnPoint0 = false;
    }

    void FixedUpdate()
    {
        LandCheck();
        if (!hasChosenSpawnPoint0)
        {
            Vector3 targetPos = new Vector3(cursor.transform.position.x, cursor.transform.position.y, -4f);
            transform.position = Vector3.Lerp(transform.position, targetPos, 0.1f);
        }
        else if (hasChosenSpawnPoint0 && bishop!= null)
        {
            transform.position = new Vector3(bishop.transform.position.x, bishop.transform.position.y, -4f);
        }
        ChoseSpawn();
        Debug.Log(bishop);
    }

    void ChoseSpawn()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !hasChosenSpawnpoint)
        {
            deployPosition = transform.position;
            hasChosenSpawnpoint = true;

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