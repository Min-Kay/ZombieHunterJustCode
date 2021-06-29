using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Rendering;

public class GunCtrl : MonoBehaviour
{
    public enum Gun
    {
        Rifle,
        SMG,
        LMG,
        Shotgun,
        Pistol,
        SniperRifle
    }
    [Header("Gun Info")]
    public Gun type = Gun.Rifle;
    public int fullAmmo = 0;
    public int currAmmo = 0;
    private float initTime = 0;
    public float reloadTime = 0;
    public float decreaseSpeed = 0;
    private float originSpeed = 0;
    public bool canCast = true;
    private bool isReload;
    private WeaponItem item;
    
    [Header("Fire Effect")]
    public Transform firePos;
    public GameObject bullet;
    public ParticleSystem fireFx;
    public ParticleSystem castFx;
    private WaitForSeconds ws;

    [Header("Additional Options")] 
    [Tooltip("For Shotgun Shot ")]public int shotCount = 0;
    [Tooltip("To Set Accuracy")] public float accuracy = 0.0f;
    void Start()
    {
        item = GetComponent<WeaponItem>();
        currAmmo = fullAmmo;
        initTime = Time.deltaTime;
        ws = new WaitForSeconds(reloadTime);
        fireFx.Stop();
        if(canCast)
            castFx.Stop();
        originSpeed = GameManager.Instance.player.moveSpeed;
    }
    void Update()
    {
        if(!GameManager.Instance.isPaused)
        { 
            if (Input.GetMouseButton(0))
            {
                GameManager.Instance.player.moveSpeed = decreaseSpeed;
                if (type == Gun.SMG || type == Gun.LMG)
                {
                    AccuracyFire(accuracy);
                }
                else if(type == Gun.Shotgun)
                {
                    SplitFire(shotCount, accuracy);
                }
                else
                {
                    Fire();
                }
            }
            else
            {
                GameManager.Instance.player.moveSpeed = originSpeed;
                fireFx.Stop();
                if(canCast)
                    castFx.Stop();
                initTime = item.speed/2.0f;
            }

            if (Input.GetKeyDown(KeyCode.R) && currAmmo < fullAmmo)
            {
                Reload();
            }
            GameManager.Instance.currAmmo.text = isReload?"RELOAD":currAmmo.ToString();
            
        }
    }

    void Fire()
    {
        if (initTime >= item.speed && currAmmo > 0 && !isReload)
        {
            var temp = Instantiate(bullet, firePos.position, firePos.rotation);
            temp.GetComponent<Rigidbody>().AddForce(temp.transform.forward * 50.0f,ForceMode.Impulse);
            temp.GetComponent<BulletCtrl>().damage = item.damage;
            fireFx.Play();
            if(canCast)
                castFx.Play();
            Destroy(temp,item.distance/10.0f);
            currAmmo--;
            initTime = 0;
        }
        else if (currAmmo <= 0 && !isReload)
        {
            Reload();
        }
        else
        {
            initTime += Time.deltaTime;
            fireFx.Stop();
            if(canCast)
                castFx.Stop();
        }
    }

    public void AccuracyFire(float accuracy)
    {
        if (initTime >= item.speed && currAmmo > 0 && !isReload)
        {
            var rot = Quaternion.Euler(0,Random.Range(-accuracy,accuracy),0);
            var temp = Instantiate(bullet, firePos.position , firePos.rotation * rot);
            temp.GetComponent<Rigidbody>().AddForce(temp.transform.forward * 50.0f ,ForceMode.Impulse);
            temp.GetComponent<BulletCtrl>().damage = item.damage;
            Destroy(temp,item.distance/10.0f);
            fireFx.Play();
            if(canCast)
                castFx.Play();
            currAmmo--;
            initTime = 0;
        }
        else if (currAmmo <= 0 && !isReload)
        {
            Reload();
        }
        else
        {
            initTime += Time.deltaTime;
            fireFx.Stop();
            if(canCast)
                castFx.Stop();
        }
    }
    
    
    public void SplitFire(int shotCount, float accuracy)
    {
        if (initTime >= item.speed && currAmmo > 0 && !isReload)
        {
            for (int i = 1; i <= shotCount; ++i)
            {
                var rot = Quaternion.Euler(0,Random.Range(-accuracy,accuracy),0);
                var temp = Instantiate(bullet, firePos.position , firePos.rotation * rot);
                temp.GetComponent<Rigidbody>().AddForce(temp.transform.forward * 50.0f ,ForceMode.Impulse);
                temp.GetComponent<BulletCtrl>().damage = item.damage;
                Destroy(temp,item.distance/10.0f);
            }
            fireFx.Play();
            currAmmo--;
            initTime = 0;
        }
        else if (currAmmo <= 0 && !isReload)
        {
            Reload();
        }
        else
        {
            initTime += Time.deltaTime;
            fireFx.Stop();
        }
    }

    void Reload()
    {
        StartCoroutine(Reloading());
    }

    IEnumerator Reloading()
    {
        fireFx.Stop();
        if(canCast)
            castFx.Stop();
        isReload = true;
        yield return ws;
        currAmmo = fullAmmo;
        isReload = false;
    }

}
