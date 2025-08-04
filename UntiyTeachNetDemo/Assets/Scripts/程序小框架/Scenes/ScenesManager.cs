using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// 场景切换模块
/// 知识点
/// 1.场景异步加载
/// 2.协程
/// 3.委托
/// </summary>
public class ScenesManager : BaseManager<ScenesManager> {
    protected ScenesManager() {

    }
    /// <summary>
    /// 切换场景 同步
    /// </summary>
    /// <param name="name"></param>
    public void LoadScene(string name, UnityAction fun) {
        //场景同步加载
        SceneManager.LoadScene(name);
        //场景加载完成过后才会执行fun
        fun();
    }

    /// <summary>
    /// 提供给外部调用的异步加载的接口方法
    /// </summary>
    /// <param name="name"></param>
    /// <param name="fun"></param>
    public void LoadSceneAsync(string name, UnityAction fun) {
        
        MonoManager.GetInstance().StartCoroutine(ReallyLoadSceneAsyn(name, fun));
    }

    /// <summary>
    /// 协程异步加载场景
    /// </summary>
    /// <param name="name"></param>
    /// <param name="fun"></param>
    /// <returns></returns>
    private IEnumerator ReallyLoadSceneAsyn(string name, UnityAction fun) {
        AsyncOperation ao = SceneManager.LoadSceneAsync(name);
        //可以得到场景加载的一个进度
        while (!ao.isDone) {
            //事件中心 向外分发 进度情况 外面想用就用
            EventCenterMgr.GetInstance().EventTrigger("Loading", ao.progress);
            //希望在这里面更新进度条
            //yield return ao.progress;
        }
        yield return ao;

        //场景加载完成过后才会执行fun
        fun();
    }
}
