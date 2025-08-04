using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayPush : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnEnable() {
        Invoke("Push", 1.5f);
    }
    private void Push() {
        PoolMgr.GetInstance().Push(this.name,this.gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
