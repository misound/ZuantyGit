using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacLoadTester : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SSItem ssItem = (SSItem)Factory.reset("SS");
        
        ssItem.saysomething();
        
        S2Item s2Item = (S2Item)Factory.reset("S2");
        
        s2Item.saysomething();
        
        S3Item s3Item = (S3Item)Factory.reset("S3");
        
        s3Item.saysomething();
        
        NewSItem Item = (NewSItem)Factory.reset("NS");
        
        Item.saysomething();
        Item.FakeData1();
    }

    // Update is called once per frame
    void Update()
    {
    }
}