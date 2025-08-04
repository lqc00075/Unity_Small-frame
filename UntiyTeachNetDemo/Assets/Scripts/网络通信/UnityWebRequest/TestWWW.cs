using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TestWWW : MonoBehaviour
{
    public RawImage image;
    // Start is called before the first frame update
    void Start() {
        //只要保证一运行时 进行该判断 进行动态创建
        if (NetWWWMgr.Instance == null) {
            GameObject obj = new GameObject("WWW");
            obj.AddComponent<NetWWWMgr>();
        }

        //在任何地方使用NetWWWMgr都没有问题

        NetWWWMgr.Instance.LoadRes<Texture>("http://10.20.106.239:8000/HTTPServer/阴阳师.png", (obj) => {
            //使用加载结束的资源
            image.texture = obj;
        });

        NetWWWMgr.Instance.LoadRes<byte[]>("http://10.20.106.239:8000/HTTPServer/阴阳师.png", (obj) => {
            //使用加载结束的资源
            //把得到的字节数组存储到本地
            print(Application.persistentDataPath);
            File.WriteAllBytes(Application.streamingAssetsPath + "/www图片.png", obj);
        });

        NetWWWMgr.Instance.LoadRes<string>("http://10.20.106.239:8000/HTTPServer/test.txt", (str) => {
            print(str);
        });

        NetWWWMgr.Instance.UploadFile("练习题上传.jpg", Application.streamingAssetsPath + "/流火之擂.png", (code) => {
            if (code == UnityWebRequest.Result.Success) {
                print("上传成功");
            } else
                print("上传失败" + code);
        });

        //NetWWWMgr.Instance.UnityWebRequestLoad<Texture>("http://10.20.106.239:8000/HTTPServer/流火之擂.png",
        //                                                (tex) =>
        //                                                {
        //                                                    image.texture = tex;
        //                                                });

        //NetWWWMgr.Instance.UnityWebRequestLoad<byte[]>("http://10.20.106.239:8000/HTTPServer/流火之擂.png",
        //                                                (bytes) => {
        //                                                    print("图片字节数" + bytes.Length);
        //                                                });

        //print(Application.persistentDataPath);
        //NetWWWMgr.Instance.UnityWebRequestLoad<object>("http://10.20.106.239:8000/HTTPServer/流火之擂.png,
        //                                                (obj) => {
        //                                                    print("保存本地成功");
        //                                                }, Application.persistentDataPath + "/流火之擂.png");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
