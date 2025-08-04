using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

public class PoolMgr : BaseManager<PoolMgr> {
    protected PoolMgr() {

    }
    //池子数据类
    public class PoolData {
        //父节点表征 主要是为了便于管理才设置的父节点
        public GameObject fatherObj;
        public List<GameObject> poolList;

        public PoolData(GameObject pushObj, GameObject poolObj) {
            fatherObj = new GameObject(pushObj.name);
            fatherObj.transform.parent = poolObj.transform;
            poolList = new List<GameObject>() { };
            Push(pushObj);
        }

        public void Push(GameObject obj) {
            obj.SetActive(false);
            poolList.Add(obj);
            obj.transform.parent = fatherObj.transform;
        }
        public GameObject Pop() {
            GameObject obj = null;
            obj = poolList[0];
            poolList.RemoveAt(0);

            obj.SetActive(true);
            obj.transform.parent = null;
            return obj;
        }
    }


    //提供两个API
    private Dictionary<string, PoolData> dic = new Dictionary<string, PoolData>();
    private GameObject poolObj;
    //放进来
    public void Push(string name, GameObject obj) {
        if (poolObj == null) {
            poolObj = new GameObject("Pool");
        }
        if (dic.ContainsKey(name)) {
            dic[name].Push(obj);
        } else {
            dic.Add(name, new PoolData(obj,poolObj));
        }
    }
    //拿出去
    public void Pop(string name,UnityAction<GameObject> callback) {
        if (dic.ContainsKey(name) && dic[name].poolList.Count > 0) {
            callback(dic[name].Pop());
        } else {
            ResourceManager.GetInstance().LoadAsync<GameObject>(name, (o) => {
                o.name = name;
                callback(o);//像回调函数穿了一个参数 外部可以使用
            });
        }
    }
    public void Clear() {
        dic.Clear();
        poolObj = null;
    }
   
}
