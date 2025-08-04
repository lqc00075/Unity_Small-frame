using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUIMgr : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UIManager.GetInstance().ShowPanel<Panel_1>("Panel1");
        //Invoke("Hide", 5f);
    }
    private void Hide() {
        UIManager.GetInstance().HidePanel("Panel1");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
