using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEventCenterMgr : MonoBehaviour
{
    public int i = 666;
    public int index = 888;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            EventCenterMgr.GetInstance().EventTrigger("Message");
            EventCenterMgr.GetInstance().EventTrigger("Message3_4", index);
        }
    }
    private void OnEnable() {
        
    }
}
