using System.Collections;
using UnityEngine;

public class CameraSmoother : MonoBehaviour
{
    private GameObject _player;
    
    
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("PlayerRB");
    }

    void Update()
    {
        Vector3 targetPos = new Vector3(_player.transform.position.x, _player.transform.position.y, -4f);
        transform.position = Vector3.Lerp(transform.position, targetPos, 0.1f);
        
    }


    public void CameraShake(float ins, float dur)
    {
        StartCoroutine(Shake(ins, dur));
    }

    public IEnumerator Shake(float intensity, float duration)
    {
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            float x = Random.Range(-1f,1f) * intensity;
            float y = Random.Range(-1f,1f) * intensity;

            transform.position = transform.position + new Vector3(x, y, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }
    
}
