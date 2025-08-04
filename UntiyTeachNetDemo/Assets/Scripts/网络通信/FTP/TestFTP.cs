using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFTP : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //创建一个文件夹
        FtpMgr.Instance.CreateDirectory("刘清城", (result) => {
            print(result ? "创建成功" : "创建失败");
        });

        //上传一个文件
        FtpMgr.Instance.UpLoadFile("刘清城/endeavor.jpg", Application.streamingAssetsPath + "/endeavor.jpg", () => {
            print("上传完毕");
        });

        //获取一个文件的大小
        FtpMgr.Instance.GetFileSize("刘清城/endeavor.jpg", (result) => {
            print($"文件的大小为{result}");
        });

        //下载一个文件
        FtpMgr.Instance.DownLoadFile("刘清城/endeavor.jpg", Application.persistentDataPath + "/endeavor.jpg", () => {
            print("下载完毕");
        });

        //移除一个文件
        FtpMgr.Instance.DeleteFile("刘清城/endeavor.jpg", (result) => {
            print(result ? "创建成功" : "创建失败");
        });

        //获取某一个目录下的所有文件名
        FtpMgr.Instance.GetFileList("刘清城/", (list) => {
            if (list == null) {
                print("获取文件列表失败");
                return;
            }
            for (int i = 0; i < list.Count; i++) {
                print(list[i]);
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
