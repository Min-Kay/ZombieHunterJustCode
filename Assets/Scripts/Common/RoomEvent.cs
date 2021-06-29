using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RoomEvent : MonoBehaviour
{
   public UnityEvent enter;
   public bool isClear;
   public int currRoomNum;
   private void OnTriggerEnter(Collider other)
   {
      if (other.transform.CompareTag("Player") && !isClear)
      {
         isClear = true;
         GameManager.Instance.roomNum = currRoomNum;
         enter.Invoke();
      }
   }
}
