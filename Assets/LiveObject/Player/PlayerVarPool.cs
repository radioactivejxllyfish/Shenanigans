using UnityEngine;

public abstract class PlayerVarPool : MonoBehaviour
{
    public Rigidbody2D playerRb;
    public float xAxis;
    public float yAxis;
    public float speed;
    public float health;
    public float maxHealth;
    public float stamina = 100;
    public float MAX_STAMINA = 100;
    public float armorCount = 0;
    public float maxArmorCount = 80;


    public bool canSprint = true;
    public Vector3 movement;
    public GameObject player;
    public GameObject cursor;
    public CameraSmoother cameraSmoother;




    public void ApplyKnockback(Vector2 sourcePosition, float knockbackForce)
    {
        Vector2 knockbackDirection = (transform.position - (Vector3)sourcePosition).normalized;

        playerRb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
    }
    

}
