using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainAsync : MonoBehaviour
{
    // Start is called before the first frame update
    public InputField input;
    public Button btn1;

    void Start()
    {
        if (NetAsyncMgr.Instance == null) {
            GameObject obj = new GameObject("NetAsyncMgr");
            obj.AddComponent<NetAsyncMgr>();
        }
        NetAsyncMgr.Instance.Connect("127.0.0.1", 8080);
        btn1.onClick.AddListener(() => {
            PlayerMsg ms = new PlayerMsg();
            ms.playerID = 1111;
            ms.playerData = new PlayerData();
            ms.playerData.name = "客户端发送的信息";
            ms.playerData.atk = 22;
            ms.playerData.lev = 10;
            NetAsyncMgr.Instance.Send(ms);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
