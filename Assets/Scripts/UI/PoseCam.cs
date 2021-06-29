using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoseCam : MonoBehaviour
{
    public Transform cam;
    public Transform target;
    public Quaternion rot;
    public Vector3 pos;

    private void Start()
    {
        cam.rotation = rot;
    }

    void Update()
    {
        cam.position = target.position + pos;
    }
}
