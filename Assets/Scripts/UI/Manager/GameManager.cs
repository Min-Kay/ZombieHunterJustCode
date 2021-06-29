using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Shop")] 
    public Camera shopCam;
    public PlayerCtrl player;
    public bool isPaused;
    [Header("Info")] 
    public Canvas invenCanvas;
    public Image hpBar;
    public Text goldText;
    public Text zombieCount;
    public Text currAmmo;
    public Text fullAmmo;
    public CanvasGroup roundBackground;
    public Text roundText;
    private int currIndex = 1;
    [Header("Gold")] 
    public int initGold = 1000;
    public int gold = 0;
    [Header("Game")]
    public bool roundClear;
    public bool gameClear;
    private static GameManager instance = null;
    public Transform[] leftDoor;
    public Transform[] rightDoor;

    public int roomNum = 0;
    public int clearRound = 0;
    
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
                return null;

            else return instance;
        }

    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);
        gold = initGold;
        OpenDoor();
        ShopPhase();
    }

    private void Update()
    {
        hpBar.fillAmount = player.HP / player.fullHP;
        goldText.text = gold.ToString();
        zombieCount.text = ZombieSpawner.Instance.zombies.Count > 0
                ? ZombieSpawner.Instance.zombies.Count.ToString()
                : "";
        if (roundClear)
        {
            StartCoroutine(RoundClear());
        }
    }

    public void InitRound()
    {
        if (InventoryManager.Instance.items.Count <= 0)
        {
            return;
        }

        Cursor.visible = false;
        shopCam.enabled = false;
        invenCanvas.enabled = true;
        InventoryManager.Instance.InitInventory();
        isPaused = false;
        if (clearRound == 0)
        {
            StartRound();
        }
        clearRound++;
           
    }

    public void ShopPhase()
    {
        isPaused = true;
        invenCanvas.enabled = false;
        shopCam.enabled = true;
        if(ShopManager.Instance)
            ShopManager.Instance.gold.text = gold.ToString(); 
    }

    public void GameOver()
    {
        StartCoroutine(GameOverText());
    }
    

    IEnumerator ShowMainText(int i = 0)
    {
        if (i == 0)
        {
            roundText.text = "Round " + currIndex.ToString();
            currIndex++;
        }
        else if (i == 1)
        {
            roundText.text = "Clear";
        }
        else if (i == 2)
        {
            roundText.text = "Game Over";
        }

        roundBackground.alpha = 1.0f;
        yield return new WaitForSeconds(3.0f);
        roundBackground.alpha = 0.0f;
    }
    
    public void OpenDoor()
    {

        foreach (var i in leftDoor)
        {
            i.Rotate(0,90,0);
        }
        
        foreach (var i in rightDoor)
        {
            i.Rotate(0,-90,0);
        }
        
    }

    public void CloseDoor()
    {
        foreach (var i in leftDoor)
        {
            i.Rotate(0,-90,0);
        }
        
        foreach (var i in rightDoor)
        {
            i.Rotate(0,90,0);
        }
    }
    
    public void SetAmmo()
    {
        fullAmmo.text = "/" + player.currEquipment?.GetComponent<GunCtrl>().fullAmmo.ToString();
    }

    public void StartRound()
    {
        StartCoroutine(RoundStart());
    }
    
    IEnumerator RoundStart()
    {
        CloseDoor();
        yield return StartCoroutine(ShowMainText(0));
        ZombieSpawner.Instance.Spawn();
    }
    
    IEnumerator RoundClear()
    {
        OpenDoor();
        roundClear = false;
        yield return StartCoroutine(ShowMainText(1));
        Cursor.visible = true; 
        ShopPhase();
    }

    IEnumerator GameOverText()
    {
        yield return StartCoroutine(ShowMainText(2));
    }

}
