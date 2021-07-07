using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testRotate : MonoBehaviour
{
    GameObject focalPoint;

    [SerializeField]
    float speed = 5;
    // Start is called before the first frame update
    void Start()
    {
        focalPoint = GameObject.FindGameObjectWithTag("Floor");
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.RotateAround(focalPoint.transform.position,Vector3.up,speed*Time.deltaTime);
    }
}
