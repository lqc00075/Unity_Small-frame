using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPoolMgr : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            PoolMgr.GetInstance().Pop("TestPrefab/Cube", (obj) => {
                obj.transform.localScale *= 20;
            });
        }
        if (Input.GetKeyDown(KeyCode.Mouse1)) {
            PoolMgr.GetInstance().Pop("TestPrefab/Sphere", (obj) => {
                obj.transform.localScale *= 20;
            });
        }
    }
}
