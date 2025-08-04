using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUdp : MonoBehaviour
{
    public Button btnSend;
    // Start is called before the first frame update
    void Start()
    {
        btnSend.onClick.AddListener(() => {
            PlayerMsg msg = new PlayerMsg();
            msg.playerData = new PlayerData();
            msg.playerID = 1;
            msg.playerData.name = "UDP同步客户端发的消息";
            msg.playerData.atk = 888;
            msg.playerData.lev = 666;
            UdpNetMgr.Instance.Send(msg);
        });

        if (UdpNetMgr.Instance == null)
        {
            GameObject obj = new GameObject("UdpNet");
            obj.AddComponent<UdpNetMgr>();
        }

        UdpNetMgr.Instance.StartClient("127.0.0.1", 8080);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
