using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowSlash : MonoBehaviour
{
    public TrailRenderer trail;
    public List<GameObject> enemies;
    public SpriteRenderer spriteRenderer;
    public PolygonCollider2D polygonCollider;
    public Rigidbody2D rigidBody;
    public GameObject header;
    public GameObject ShadowStrike;

    public float speed;
    private Vector3 direction;
    private float z;
    
    void Start()
    {
        trail = GetComponent<TrailRenderer>();
        z = Random.Range(-80f, -180f);
        trail.enabled = true;
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        polygonCollider = GetComponent<PolygonCollider2D>();
        speed = 45f;
        Destroy(gameObject, 0.35f);
        transform.rotation = Quaternion.Euler(0, 0, z);

    }

    void Update()
    {
        Slash();
        Sizer();
    }


    private void Slash()
    {
        direction = (header.transform.position - transform.position).normalized;
        rigidBody.velocity = direction * speed;
    }

    private void Sizer()
    {
        float x = Random.Range(0.75f, 1f);
        float y = Random.Range(0.75f, 1.35f);
        transform.localScale = new Vector3(x, y, 1);
    }
    
    
    private void CauseDMG(float dmg)
    {
        if (enemies != null)
        {
            foreach (GameObject _enemy in enemies)
            {
                _enemy.GetComponent<BasicEnemy>().TakeDamage(dmg);
            }
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider != null)
        {
            if (collider.gameObject.CompareTag("Enemy"))
            {
                enemies.Add(collider.gameObject);
                CauseDMG(25);
            }
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemies.Remove(other.gameObject);
        }
            
    }
}
