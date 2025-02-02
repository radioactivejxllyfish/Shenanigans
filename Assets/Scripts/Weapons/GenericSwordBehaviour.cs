
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


public class GenericSwordBehaviour : GenericSword
{
    private CircleCollider2D _collider;
    private Animator _animator;
    private float _heavycooldown;
    private GameObject _cursor;
    private GameObject _enemy;
    private string currentState;
    public GameObject _prefabVariant;

    private bool _lightattack = false;
    private bool _heavyattack = false;
    
    const string LAR = "LightAttackRight";
    const string HAR = "HeavyAttackRight";
    const string IDL = "Idle";
    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("PlayerHitbox");
        _collider = GetComponent<CircleCollider2D>();
        _lightdamage = 10f;
        _heavydamage = 25f;
        _cooldown = 0.5f;
        _heavycooldown = 1f;
        _animator = GetComponent<Animator>();
        _cursor = GameObject.FindGameObjectWithTag("Cursor");
        Debug.Log(_collider);
    }

    void Update()
    {
        Throw();
        Transformer();
        OnAttackCoolDown();
        if (Input.GetKeyDown(KeyCode.Mouse0) && _cooldown <= 0)
        {
            LightAttack();
            _cooldown = 0.5f;
            
        }
        else if ( Input.GetKeyDown(KeyCode.Mouse1) && _heavycooldown <= 0 && _cooldown <= 0)
        {
            ChangeAnimationState(HAR);
            HeavyAttack();
            _cooldown = 0.5f;
            _heavycooldown = 1f;
        }

        if (_heavycooldown != 0 && _cooldown != 0)
        {
            ChangeAnimationState(IDL);
        }


    }

    public override void LightAttack()
    {
        ChangeAnimationState(LAR);
        Debug.Log("LightAttack");
        _lightattack = true;
        if (_enemy != null)
        {
            _enemy.GetComponent<GenericEnemyBehaviour>().TakeDamage(_lightdamage);

        }
    }

    public override void HeavyAttack()
    {
        _heavyattack = true;
        Debug.Log("HeavyAttack");
        if (_enemy != null)
        {
            _enemy.GetComponent<GenericEnemyBehaviour>().TakeDamage(_heavydamage);

        }
    }

    private void Transformer()
    {
        transform.position = new Vector3(_player.transform.position.x, _player.transform.position.y -0.7f, -0.06f);
        transform.up = (_cursor.transform.position - transform.position).normalized;
    }
    private void OnAttackCoolDown()
    {
        if (_cooldown > 0)
        {
            _cooldown -= Time.deltaTime;
            _cooldown = Mathf.Clamp(_cooldown, 0, 0.5f);
        }

        if (_heavycooldown > 0)
        {
            _heavycooldown -= Time.deltaTime;
            _heavycooldown = Mathf.Clamp(_heavycooldown, 0, 1f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider != null)
        {
            if (collider.CompareTag("Enemy"))
            {
                Debug.Log("EnemyFound");
                _enemy = collider.gameObject;
            }
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            _enemy = null;
            Debug.Log("EnemyLeft");
        }
            
    }

    private void Throw()
    {
        if (Input.GetKeyDown(KeyCode.G)) 
        {
            Instantiate(_prefabVariant, transform.position, transform.rotation);
            Destroy(gameObject);
            
        }
    }
    
    private void ChangeAnimationState(string newState)
    {
        // stop the animation from interrupting itself
        if (currentState == newState) return;
        // play animation
        _animator.Play(newState); 
        // reassign current state   
        currentState = newState;
    }
}
