using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MonoBehaviour
{
    public float heal = 0.0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            var temp = other.transform.GetComponent<PlayerCtrl>();
            if (temp.HP == temp.fullHP)
            {
                return;
            }
            else if (temp.HP + heal > temp.fullHP)
            {
                temp.HP = temp.fullHP;
                Destroy(gameObject);
            }
            else
            {
                temp.HP += heal;
                Destroy(gameObject);
            }
        }
    }
}
