using UnityEngine;


public class GenericSwordBehaviour : MonoBehaviour
{
    private GameObject _player;
    private CircleCollider2D _collider;
    private float _damage;
    private float _cooldown;
    private string currentstate;
    private Animator animator;
    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("PlayerHitbox");
        _collider = GetComponent<CircleCollider2D>();
        _damage = 10f;
        _cooldown = 0.5f;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        transform.position = new Vector3(_player.transform.position.x, _player.transform.position.y, -0.06f);
        OnAttackCoolDown();
        if (Input.GetKeyDown(KeyCode.Mouse0) && _cooldown <= 0)
        {
            Attack();
            _cooldown = 0.5f;
            
        }
    }

    private void Attack()
    {
        Debug.Log("LightAttack");
    }

    private void OnAttackCoolDown()
    {
        if (_cooldown > 0)
        {
            _cooldown -= Time.deltaTime;
            _cooldown = Mathf.Clamp(_cooldown, 0, 0.5f);
        }
    }
}
