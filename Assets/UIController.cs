using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField]
    Image bubbleImg;

    [SerializeField]
    Dictionary<items, Image> img; 

    enum items  {FRESH, GROCERY, CLEANING};
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
