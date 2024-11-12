using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRoad : MonoBehaviour
{
    public float gamespeed = 5f;

    void Update()
    {
        Debug.Log("gamespeed");
        GameObject newObject = GameObject.Find("road");
        Rigidbody rb = newObject.GetComponent<Rigidbody>();
        Debug.Log("gamespeed");
        rb.velocity = new Vector3(1000, 0, 0);
    }
}
