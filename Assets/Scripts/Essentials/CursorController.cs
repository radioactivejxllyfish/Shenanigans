using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    public string Direction = null;
    private GameObject _player;
    private Camera _camera;
    public Vector3 cursorposition;
    void Start()
    {
        _camera = Camera.main;
        _player = GameObject.FindGameObjectWithTag("PlayerRB");
    }

    void Update()
    {
        if (_player != null)
        {
            Vector3 direction = (transform.position - _player.transform.position).normalized;
            Vector3 campos = _camera.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(campos.x, campos.y, 0);
            cursorposition = transform.position;

            if (direction.x > 0)
            {
                Direction = "Right";
            }
            else if (direction.x <= 0)
            {
                Direction = "Left";
            }
            else if (direction.x == 1)
            {
                Direction = "Left";
            }
        }
        else
        {
            _player = GameObject.FindGameObjectWithTag("PlayerRB");
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        
    }
    
    
}
