using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Net;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    public float fullHP = 100.0f;
    public float HP = 0.0f;
    public float moveSpeed = 1.0f;
    private Animator anim;
    private readonly int hashMove = Animator.StringToHash("IsMove");
    private readonly int hashFire = Animator.StringToHash("IsFire");
    private readonly int hashDie = Animator.StringToHash("IsDie");
    private bool isDie;
    private bool isArmed;
    public Transform wepPos;
    public Item currEquipment = null;

    private void Awake()
    {
        HP = fullHP;
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (HP <= 0.0f && !isDie)
        {
            isDie = true;
            anim.SetTrigger(hashDie);
            GameManager.Instance.GameOver();
        }

        if(!isDie && !GameManager.Instance.isPaused)
        { 
            Move();
            ChangeInventorySlot();
            RotateByMouse();
            UseWeapon();
        }
    }

    void Move()
    {
        var horizontal = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        var vertical = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        var pos = transform.position;
        
        pos.x = pos.x + horizontal;
        pos.z = pos.z + vertical;
        if(horizontal != 0 || vertical != 0)
            anim.SetBool(hashMove,true);
        else
            anim.SetBool(hashMove,false);
        transform.position = pos;
    }

    void RotateByMouse()
    {
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        Plane GroupPlane = new Plane(Vector3.up, Vector3.zero);

        float rayLength;

        if(GroupPlane.Raycast(cameraRay, out rayLength))

        {		
            Vector3 pointTolook = cameraRay.GetPoint(rayLength);
            transform.LookAt(new Vector3(pointTolook.x, transform.position.y, pointTolook.z));
        }
    }

    public void ChangeInventorySlot()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            ChangeItem(0);
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            ChangeItem(1);
        }
        else if (Input.GetKey(KeyCode.Alpha3))
        {
            ChangeItem(2);
        }
        else if (Input.GetKey(KeyCode.Alpha4))
        {
            ChangeItem(3);
        }
        else if (Input.GetKey(KeyCode.Alpha5))
        {
            ChangeItem(4);
        }
    }

    public void ChangeItem(int i)
    {
        if (InventoryManager.Instance.items.Count < i + 1)
            return;
        
        if (InventoryManager.Instance.items[i] != null && currEquipment != null)
        {
            currEquipment.gameObject.SetActive(false);
            currEquipment = InventoryManager.Instance.items[i];
            currEquipment.gameObject.SetActive(true);
            if (currEquipment.kind == Item.Kind.Weapon)
                isArmed = true;
            if (InventoryManager.Instance.items[i]?.GetComponent<GunCtrl>())
            {
               GameManager.Instance.SetAmmo();
            }
        }
        else if (InventoryManager.Instance.items[i] != null)
        {
            currEquipment = InventoryManager.Instance.items[i];
            currEquipment.gameObject.SetActive(true);
            if (currEquipment.kind == Item.Kind.Weapon)
                isArmed = true;
            if (InventoryManager.Instance.items[i]?.GetComponent<GunCtrl>())
            {
                GameManager.Instance.SetAmmo();
            }
        }
        else
        {
            isArmed = false;
        }
    }

    public void InitItem(Item item)
    {
        item.transform.SetParent(wepPos);
        item.transform.SetPositionAndRotation(wepPos.position,wepPos.rotation);
        item.gameObject.SetActive(false);
    }
    void UseWeapon()
    {
        if (Input.GetMouseButton(0) && isArmed)
        {
            anim.SetBool(hashFire,true);
        }
        else
            anim.SetBool(hashFire,false);
    }

    public void Hit(float damage)
    {
        HP -= damage;
    }
}
