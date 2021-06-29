using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorCtrl : MonoBehaviour
{
    void Update()
    {
        transform.position = Input.mousePosition;
    }
}
