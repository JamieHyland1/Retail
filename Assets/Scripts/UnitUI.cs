using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UnitUI : MonoBehaviour
{
    
    [SerializeField]
    Image image;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        image.rectTransform.LookAt(Camera.main.transform.position);
        image.rectTransform.anchoredPosition3D = new Vector3(transform.position.x,transform.position.y+1.6f,transform.position.z);
    }
}
