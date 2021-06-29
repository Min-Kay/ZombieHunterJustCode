using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    public float damage = 0;
    public Transform attacker = null;
    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Zombie"))
        {
            other.transform.GetComponent<ZombieCtrl>().Hit(damage);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
