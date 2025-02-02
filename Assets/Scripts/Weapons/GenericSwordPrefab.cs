using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericSwordPrefab : MonoBehaviour
{
    private GameObject _player;
    private CircleCollider2D _circleCollider;
    public GameObject _realVariant;
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("PlayerHitbox");
    }

    void Update()
    {
        if (_player != null && Input.GetKeyDown(KeyCode.G))
        {
            Instantiate(_realVariant, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider != null)
        {
            if (collider.CompareTag("PlayerHitbox"))
            {
                _player = collider.gameObject;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("PlayerHitbox"))
        {
            _player = null;
        }

    }
}
