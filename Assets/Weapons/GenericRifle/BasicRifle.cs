using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicRifle : MonoBehaviour
{
    public GameObject bore;
    public GameObject shell;
    public GameObject bullet;
    public GameObject player;
    public GameObject cursor;
    public GameObject cameraObject;
    public Animator animator;
    public ParticleSystem shellEject;
    public CursorController cursorController;

    public float spread;
    public float damage;
    public float fireRate;
    public int ammoReserve;
    public int ammoCount;
    public int magazineCapacity;
    public int currentAmmo;
    public Vector3 direction;

    public bool isReloading;
    public bool canFire;

    
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("PlayerRB");
        cursor = GameObject.FindGameObjectWithTag("Cursor");
        shellEject = GetComponentInChildren<ParticleSystem>();
        cursorController = cursor.GetComponent<CursorController>();

        spread = 0.05f;
        fireRate = 0.15f;
        ammoReserve = 90;
        magazineCapacity = 30;

        currentAmmo = 30;
        isReloading = false;
        canFire = true;
    }

    void Update()
    {
        Transformer();
        Fire1();
    }

    private void Fire1()
    {
        if (Input.GetMouseButton(0) && currentAmmo > 0 && canFire)
        {
            StartCoroutine(Fire());
        }
    }

    private void Transformer()
    {
        float x = direction.x;
        Mathf.RoundToInt(direction.x);
        direction = (cursor.transform.position - transform.position).normalized;
        transform.position = player.transform.position;
        transform.right = direction.normalized;
        
        if (cursorController.Direction == "Right")
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (cursorController.Direction == "Left")
        {
            transform.localScale = new Vector3(1f, -1f, 1f);
        }

        if (x == -1)
        {
            transform.localScale = new Vector3(1f, -1f, 1f);
        }

    }


    private IEnumerator Fire()
    {
        canFire = false;
        shellEject.Play();
        currentAmmo -= 1;
        Instantiate(bullet, bore.transform.position, bore.transform.rotation );
        yield return new WaitForSeconds(fireRate);
        canFire = true;
        shellEject.Stop();
    }
}
