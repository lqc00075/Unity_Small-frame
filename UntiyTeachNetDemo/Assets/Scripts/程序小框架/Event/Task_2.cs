using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task_2 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() {
        EventCenterMgr.GetInstance().AddEventListener("Message", TestTask1);
    }
    private void TestTask1() {
        Debug.Log("这是Task_2类收到Message后执行的函数");
    }
    private void OnDestroy() {
        EventCenterMgr.GetInstance().RemoveEventListener("Message", TestTask1);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
