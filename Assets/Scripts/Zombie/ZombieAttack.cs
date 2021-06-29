using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAttack : MonoBehaviour
{
    public ZombieCtrl zombie;
    private float initTime = 0;
    public float delay = 1.0f;
    private bool isAttack = false;
    private void Update()
    {
        if (initTime > delay && zombie.isAttack)
        {
            isAttack = true;
            initTime = 0;
        }
        else
        {
            initTime += Time.deltaTime;
        }

    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (isAttack && other.transform.CompareTag("Player"))
        {
            other.transform.GetComponent<PlayerCtrl>().Hit(zombie.damage);
            isAttack = false;
        }
    }
}
