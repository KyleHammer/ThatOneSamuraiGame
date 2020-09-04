﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{ //script for basic projectile

    GameManager manager;

    int damageAmount = 1;

    float speed = 80f;

    GameObject player;
    public Vector3 direction;
    Rigidbody rb;
    public bool active = true;
    public bool hitEnemies = false;
    public MeshRenderer renderer;
    public Collider collider;

    //initialize
    private void Awake()
    {
        //GameObject managerObject;
        //managerObject = GameObject.FindGameObjectWithTag("Manager");
        //manager = managerObject.GetComponent<GameManager>();
       // player = GameManager.instance.playerController.gameObject;

        rb = GetComponent<Rigidbody>();

        collider = GetComponent<Collider>();
        renderer = GetComponent<MeshRenderer>();
    }

    //launches projectile in target direction
    public void Launch(Vector3 target)
    {
        EnableMethod();
        direction = target.normalized * speed;
        direction.y = 0f;
        rb.velocity = direction;
        StartCoroutine(Die(3f));
    }

    //sees if the object is damagable
    private void OnTriggerEnter(Collider other)
    {
        DamageCheck(other);
    }

    //if the object is damagable, apply damage, if the player parries the projectile it is reflected
    private void DamageCheck(Collider other)
    {
        IDamageable damagable = other.gameObject.GetComponent<IDamageable>();
        if (other.gameObject.tag == "Player")
        {
            if (other.gameObject.GetComponent<PlayerFunctions>().bIsParrying)
            {
                rb.velocity = Vector3.zero;
                rb.velocity = -direction;
               // manager.StartCoroutine("HitPause");
            }
            else
            {
                damagable.OnEntityDamage(damageAmount, this.gameObject, false);
                StartCoroutine(Die(0f));
            }
        }
        else if (other.gameObject.tag == "Attack" && !hitEnemies)
        {
            rb.velocity = Vector3.zero;
            rb.velocity = -direction;
            hitEnemies = true;
            //manager.StartCoroutine("HitPause");
        }
        else if (damagable != null && hitEnemies)
        {

            damagable.OnEntityDamage(damageAmount, this.gameObject, false);
            StartCoroutine(Die(0f));

        }
        else
        {
            StartCoroutine(Die(0f));
        }
    }

    //returns object to pool
    IEnumerator Die(float delay)
    {
        if (delay > 0f)
        {
            yield return new WaitForSeconds(delay);
        }
        DisableMethod();
        ObjectPooler.instance.AddObject("Arrow", gameObject);
       // Destroy(this.gameObject);
    }

    //disable
    public void DisableMethod()
    {
        renderer.enabled = false;
        collider.enabled = false;
        rb.velocity = Vector3.zero;
        hitEnemies = false;
    }

    //enable
    public void EnableMethod()
    {
        renderer.enabled = true;
        collider.enabled = true;
    }

}