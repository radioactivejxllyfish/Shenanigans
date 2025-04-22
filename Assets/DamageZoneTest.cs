using UnityEngine;

public class DamageZoneTest : MonoBehaviour
{
    private CircleCollider2D _circleCollider2D;
    private Rigidbody2D rb;
    private GameObject player;
    private float DoT;
    private float frequency;
    private float elapsed;

    private void Start()
    {
        DoT = 12f;
        frequency = 0.5f;
        _circleCollider2D = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (player != null) DamageOverTime();
    }


    private void DamageOverTime()
    {
        if (elapsed < frequency) elapsed += Time.deltaTime;
        if (elapsed >= frequency)
        {
            elapsed = 0.0f;
            if (player != null) player.GetComponent<PlayerController>().TakeDamage(DoT, "Energy");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PlayerRB")) player = other.gameObject;
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("PlayerRB")) player = null;
    }
}