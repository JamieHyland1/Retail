using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testRay : MonoBehaviour
{
    public LayerMask mask;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
          RaycastHit hit;
       Debug.DrawRay(this.transform.position,Vector3.down,Color.red);
                if (Physics.Raycast(this.transform.position, Vector3.down, out hit, 300f, mask)){
                     Debug.Log(hit.collider.gameObject.name);
                    Debug.DrawRay(this.transform.position,Vector3.down*5,Color.green);
                }
                
    }
}
