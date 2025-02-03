
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Unity.VisualScripting;
using UnityEngine;


public class GenericSwordBehaviour : GenericSword
{
    private PolygonCollider2D _collider;
    private Animator _animator;
    private float _heavycooldown;
    private GameObject _cursor;
    private GameObject _enemy;
    private string currentState;
    public GameObject _prefabVariant;
    private string _direction = null;
    private bool _lightattack = false;
    private bool _heavyattack = false;
    private bool _idle = true;
    private CursorController _cursorController;
    
    const string LAR = "LightAttackRight";
    const string HAR = "HeavyAttackRight";
    const string LAL = "LightAttackLeft";
    const string HAL = "HeavyAttackLeft";
    const string IDL = "Idle";
    

    private List<GameObject> _enemies = new List<GameObject>();
    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("PlayerHitbox");
        _collider = GetComponent<PolygonCollider2D>();
        _lightdamage = 10f;
        _heavydamage = 25f;
        _cooldown = 0.5f;
        _heavycooldown = 1f;
        _animator = GetComponent<Animator>();
        _cursor = GameObject.FindGameObjectWithTag("Cursor");
        _cursorController = _cursor.GetComponent<CursorController>();
        Debug.Log(_collider);
        StartCoroutine("LightAttackRight");
        StartCoroutine("HeavyAttackRight");
        
    }

    void Update()
    {
        
        Throw();
        Transformer();
        OnAttackCoolDown();
        if (Input.GetKeyDown(KeyCode.Mouse0) && _cooldown <= 0 && _heavycooldown <= 0)
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

        if (_idle && _lightattack == false && _heavyattack == false)
        {
            ChangeAnimationState(IDL);
        }
        _direction = _cursorController.Direction;

    }

    public override void LightAttack()
    {
        _lightattack = true;
        if (_enemies != null)
        {
            foreach (GameObject _enemy in _enemies)
            {
                _enemy.GetComponent<GruntEnemyBehaviour>().TakeDamage(_lightdamage);
            }
        }
    }

    public override void HeavyAttack()
    {
        _heavyattack = true;
        if (_enemies != null)
        { 
            foreach (GameObject _enemy in _enemies)
            {
                _enemy.GetComponent<GruntEnemyBehaviour>().TakeDamage(_heavydamage);
            }
            
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
                _enemies.Add(collider.gameObject);
                Debug.Log("EnemyFound");
            }
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            _enemies.Remove(other.gameObject);
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

    private IEnumerator LightAttackRight()
    {
        while (true)
        {
            if (_lightattack)
            {
                _idle = false;
                if (_direction == "Left")
                {
                    ChangeAnimationState(LAL);
                }
                else if (_direction == "Right")
                {
                    ChangeAnimationState(LAR);
                }
                _lightattack = false;
                yield return new WaitForSeconds(_cooldown);
                _idle = true;
            }
            else
            {
                yield return null;
            }
        }
    }

    private IEnumerator HeavyAttackRight()
    {
        while (true)
        {
            if (_heavyattack)
            {
                _idle = false;
                if (_direction == "Left")
                {
                    ChangeAnimationState(HAL);
                }
                else if (_direction == "Right")
                {
                    ChangeAnimationState(HAR);
                }
                _heavyattack = false;
                yield return new WaitForSeconds(_heavycooldown);
                _idle = true;
            }

            yield return null;
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
