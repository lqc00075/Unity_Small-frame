using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    public Button btn;
    public InputField input;
    // Start is called before the first frame update
    void Start()
    {
        btn.onClick.AddListener(() => {
            PlayerMsg ms = new PlayerMsg();
            ms.playerID = 1111;
            ms.playerData = new PlayerData();
            ms.playerData.name = "唐老狮客户端发送的信息";
            ms.playerData.atk = 22;
            ms.playerData.lev = 10;
            NetMgr.Instance.Send(ms);
        });

        if (NetMgr.Instance == null)
        {
            GameObject obj = new GameObject("Net");
            obj.AddComponent<NetMgr>();
        }

        NetMgr.Instance.Connect("127.0.0.1", 8080);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
