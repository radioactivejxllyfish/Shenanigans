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
        _player = GameObject.FindGameObjectWithTag("PlayerHitbox");
        _camera = Camera.main;
    }

    void Update()
    {
        Vector3 direction = transform.position - _player.transform.position;
        float dot = Vector3.Dot(_player.transform.right, direction);
        Vector3 campos = _camera.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(campos.x, campos.y, 0);
        cursorposition = transform.position;

        if (dot > 0)
        {
            Direction = "Right";
        }
        else
        {
            Direction = "Left";
        }
    }

    public void ChangeDirection(string _direction)
    {
        _direction = Direction;
    }
}
