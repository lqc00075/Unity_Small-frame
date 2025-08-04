using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task_4 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EventCenterMgr.GetInstance().AddEventListener<int>("Message3_4", (obj) => {
            Debug.Log(obj);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
