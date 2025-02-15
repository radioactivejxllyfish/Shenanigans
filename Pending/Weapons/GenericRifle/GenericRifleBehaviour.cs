using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
public class GenericRifleBehaviour : GenericRifle
{
    public Vector3 direction;
    public GameObject bullet;
    public bool canfire;
    public float cooldown = 0f;
    private float reloadtime;
    private bool reloading = true;
    private GameObject bore;
    private CursorController _cursorController;
    private string dir;

    private Animator _anim;
    private string currentState;
    
    private const string IDLE = "Idle";
    private const string FIRE = "Fire";
    private void Start()
    {
        _anim = GetComponentInChildren<Animator>();
        canfire = true;
        reloadtime = 3f;
        damage = 35f;
        firerate = 0.2f;
        magazinecap = 30f;
        ammo = 30;
        player = GameObject.FindGameObjectWithTag("PlayerHitbox"); 
        cursor = GameObject.FindGameObjectWithTag("Cursor");
        bore = GameObject.Find("Bore");
        _cursorController = cursor.GetComponent<CursorController>();

    }

    private void Update()
    {
        if (cooldown > 0f)
        {
            ChangeAnimationState(IDLE);
        }
        dir = _cursorController.Direction;
        cooldown = Mathf.Clamp(cooldown, 0f, firerate);
        fire();
        transformer();

        if (Input.GetKeyDown(KeyCode.R) && ammo < 30)
        {
            StartCoroutine("reload");
        }
        if (!reloading)
        {
            canfire = true;
        }
        
        if (dir == "Left")
        {
            transform.localScale = new Vector3(1f, -1, 1);
        }
        else if (dir == "Right")
        {
            transform.localScale = new Vector3(1f, 1, 1);
        }
    }


    private void fire()
    {

        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }

        if (Input.GetMouseButton(0) && canfire && ammo > 0 && cooldown <= 0)
        {
            ChangeAnimationState(FIRE);
            Instantiate(bullet, bore.transform.position, transform.rotation);
            ammo -= 1;
            cooldown = firerate;
            Debug.Log("fire " + ammo);
        }
        else
        {
            ChangeAnimationState(IDLE);
        }
    }

    private void transformer()
    {
        direction = (cursor.transform.position - transform.position).normalized;
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
        transform.right = direction;

        if (transform.rotation == Quaternion.Euler(0,180, 0))
        {
            transform.rotation = Quaternion.Euler(0, 0, -180);
            transform.localScale = transform.localScale = new Vector3(1f, 1, 1);
            Debug.Log("ROTATION ERROR");
        }
    }

    private IEnumerator reload()
    {
        reloading = true;
        canfire = false;
        Debug.Log("Reload");
        yield return new WaitForSeconds(reloadtime);
        ammo = 30;
        reloading = false;
    }
    private void ChangeAnimationState(string newState)
    {
        // stop the animation from interrupting itself
        if (currentState == newState) return;
        // play animation
        _anim.Play(newState); 
        // reassign current state   
        currentState = newState;
        
    }

}
