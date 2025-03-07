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

    public bool canSprint = true;
    public Vector3 movement;
    public GameObject player;
    public GameObject cursor;



    public void TakeDamage(float damage)
    {
        health -= damage;
    }
    

}
