using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    public string Direction = null;
    private GameObject _player;
    private PlayerController _playerController;
    private Camera _camera;
    public SpriteRenderer _spriteRenderer;
    public Vector3 cursorposition;
    void Start()
    {
        
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _player = GameObject.FindGameObjectWithTag("PlayerRB");
        _playerController = _player.GetComponent<PlayerController>();
    }

    void Update()
    {
        if (!_playerController.underUI)
        {
            _spriteRenderer.enabled = false;
            if (_camera == null)
            {
                _camera = Camera.main;
            }
            if (_player != null)
            {
                bool camReset = false;
                if (!camReset)
                {
                    transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    camReset = true;
                }
                Vector3 direction = (transform.position - _player.transform.position).normalized;
                Vector3 campos = _camera.ScreenToWorldPoint(Input.mousePosition);
                transform.position = new Vector3(campos.x, campos.y, 0);
                cursorposition = transform.position;

                if (direction.x > 0)
                {
                    Direction = "Right";
                }
                else if (direction.x < 0)
                {
                    Direction = "Left";
                }
                else if (direction.x == 0)
                {
                    Direction = "Left";
                }
            }
            else
            {
                _spriteRenderer.enabled = true;
                _player = GameObject.FindGameObjectWithTag("PlayerRB");
                transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }

        }

    }
    
    
}
