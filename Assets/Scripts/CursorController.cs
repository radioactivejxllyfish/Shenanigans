using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    private Camera _camera;
    public Vector3 cursorposition;
    void Start()
    {
        _camera = Camera.main;
    }

    void Update()
    {
        Vector3 campos = _camera.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(campos.x, campos.y, 0);
        cursorposition = transform.position;
    }
}
