using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TestM {
    public void Update() {
        Debug.Log("全文背诵");
    }
}
public class TestMonoMgr : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //这样就可以在没有继承Mono的类中 使用Mono的函数了 目前只实现了 帧更新 以及 协程
        TestM tm = new TestM();
        MonoManager.GetInstance().AddUpdateListener(tm.Update);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
