using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
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
    public CursorController cursorController;
    public PlayerController playerController;
    public AudioSource source;

    public float spread;
    public float damage;
    public float fireRate;
    public int ammoReserve;
    public int ammoCount;
    public int magazineCapacity;
    public int currentAmmo;
    public int idleStyle;
    public string currentState;
    public Vector3 direction;

    public bool isReloading;
    public bool canFire;
    public bool isFiring;

    public AudioClip fireFX;
    public AudioClip reloadFX;
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("PlayerRB");
        cursor = GameObject.FindGameObjectWithTag("Cursor");
        cursorController = cursor.GetComponent<CursorController>();
        playerController = player.GetComponent<PlayerController>();
        animator = GetComponentInChildren<Animator>();
        source = GetComponent<AudioSource>();

        spread = 0.05f;
        fireRate = 0.08f;
        ammoReserve = 120;
        magazineCapacity = 30;

        currentAmmo = 30;
        isReloading = false;
        canFire = true;
    }



    private void FixedUpdate()
    {
        Fire1();
        Transformer();
        if (!isFiring && !isReloading)
        {
            if (idleStyle == 4)
            {
                ChangeAnimationState("Idle");
            }
            else if (idleStyle == 5)
            {
                ChangeAnimationState("Idle2");
            }
            else
            {
                ChangeAnimationState("IdleDefault");
            }
        }
    }

    private void Fire1()
    {
        if (Input.GetMouseButton(0) && currentAmmo > 0 && canFire)
        {
            StartCoroutine(Fire());
        }
        else if (Input.GetKeyDown(KeyCode.R) && currentAmmo < magazineCapacity && ammoReserve >0 && !Input.GetMouseButton(0))
        {
            int deductedAmmo = magazineCapacity - currentAmmo;
            ammoReserve -= deductedAmmo;
            currentAmmo = magazineCapacity;
            StartCoroutine(ReloadIE());
        }
    }


    private void Transformer()
    {
        float x = direction.x;
        Mathf.RoundToInt(direction.x);
        direction = (cursor.transform.position - transform.position).normalized;
        transform.position = player.transform.position;
        transform.right = direction.normalized;
        
        if (cursorController.Direction == "Right" && !playerController.isDashing)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (cursorController.Direction == "Left" && !playerController.isDashing)
        {
            transform.localScale = new Vector3(1f, -1f, 1f);
        }

        if (transform.rotation == Quaternion.Euler(0,180,0))
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }

    }


    private IEnumerator Fire()
    {
        source.clip = fireFX;
        source.Play();
        isFiring = true;
        canFire = false;
        currentAmmo -= 1;
        Instantiate(bullet, bore.transform.position, bore.transform.rotation );
        ChangeAnimationState("Fire");
        yield return new WaitForSeconds(fireRate);
        canFire = true;
        isFiring = false;
    }

    private IEnumerator ReloadIE()
    {
        yield return new WaitForSeconds(0.2f);
        source.clip = reloadFX;
        source.Play();
        canFire = false;
        isReloading = true;
        ChangeAnimationState("Reload");
        yield return new WaitForSeconds(2.35f);
        canFire = true;
        isReloading = false;
        idleStyle = Random.Range(1, 8);
    }
    
    public void ChangeAnimationState(string newState)
    {
        // stop the animation from interrupting itself
        if (currentState == newState) return;
        // play animation
        animator.Play(newState); 
        // reassign current state   
        currentState = newState;
        
    }
}
