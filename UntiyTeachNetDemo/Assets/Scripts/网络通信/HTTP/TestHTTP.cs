using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class TestHTTP : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //上传一个文件
        //HttpMgr.Instance.UpLoadFile("封装后上传.png", Application.streamingAssetsPath + "/流火之擂.png", (code) => {
        //    if (code == HttpStatusCode.OK)
        //        print("上传指令成功");
        //    else
        //        print("上传指令失败" + code);
        //});
        print(Application.persistentDataPath);
        HttpMgr.Instance.DownLoadFile("封装后上传.png", Application.persistentDataPath + "/HTTP下载.png", (code) => {
            if (code == HttpStatusCode.OK)
                print("下载成功");
            else
                print("下载失败" + code);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
