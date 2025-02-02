using UnityEngine;

public class CameraSmoother : MonoBehaviour
{
    private GameObject _player;
    
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("PlayerHitbox");
    }

    void FixedUpdate()
    {
        Vector3 targetPos = new Vector3(_player.transform.position.x, _player.transform.position.y, -4f);
        transform.position = Vector3.Lerp(transform.position, targetPos, 0.1f);
    }
}
