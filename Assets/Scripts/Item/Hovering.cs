using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hovering : MonoBehaviour
{
   public float rotSpeed = 10.0f;
   private void Update()
   {
      Hover();
   }

   void Hover()
   {
      transform.Rotate(0,rotSpeed*Time.deltaTime,0);
   }
   
}
