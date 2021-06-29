using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    private List<Transform[]> spawnPoint;
    public Transform[] spawnPoints1;
    public Transform[] spawnPoints2;
    public Transform[] spawnPoints3;
    public Transform[] spawnPoints4;
    public ZombieCtrl prefab;
    public List<ZombieCtrl> zombies;
    private bool isSpawned = false;

    private static ZombieSpawner instance = null;

    public static ZombieSpawner Instance
    {
        get
        {
            if (instance == null)
                return null;

            else return instance;
        }
    }

    public void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);

        zombies = new List<ZombieCtrl>();
        spawnPoint = new List<Transform[]>();
        
        spawnPoint.Add(spawnPoints1);
        spawnPoint.Add(spawnPoints2);
        spawnPoint.Add(spawnPoints3);
        spawnPoint.Add(spawnPoints4);
    }

    private void Update()
    {
        if (isSpawned && zombies.Count == 0)
        {
            GameManager.Instance.roundClear = true;
            isSpawned = false;
        }
    }
    
    public void Spawn()
    {
        isSpawned = true;

        foreach (var i in spawnPoint[GameManager.Instance.roomNum])
        {
            var zombie = Instantiate(prefab, i.position, i.rotation);
            zombies.Add(zombie);
        }
    }
}
