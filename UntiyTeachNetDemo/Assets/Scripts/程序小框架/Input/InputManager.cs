using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 1.Input类
/// 2.实践中心模块
/// 3.公共Mono模块的使用
/// </summary>
public class InputManager : BaseManager<InputManager> {
    private Dictionary<string, KeyCode> keyCodeDic = new Dictionary<string, KeyCode>();
    private bool isStart = false;
    protected InputManager() {
        MonoManager.GetInstance().AddUpdateListener(MyUpdate);
    }

    public void StartOrEndCheck(bool isOpen) {
        isStart = isOpen;
    }

    /// <summary>
    /// 用来检测按键抬起按下 分发事件的
    /// </summary>
    /// <param name="key"></param>
    private void CheckKeyCode(KeyCode key) {
        if (Input.GetKeyDown(key)) {
            //事件中心模块 分发按下抬起事件
            EventCenterMgr.GetInstance().EventTrigger("某键按下", key);
            if (!keyCodeDic.ContainsKey("某键按下")) {
                keyCodeDic.Add("某键按下", key);
            } else {
                keyCodeDic["某键按下"] = key;
            }
        }
        if (Input.GetKeyUp(key)) {
            //事件中心模块 分发按下抬起事件
            EventCenterMgr.GetInstance().EventTrigger("某键抬起", key);
            if (!keyCodeDic.ContainsKey("某键抬起")) {
                keyCodeDic.Add("某键抬起", key);
            } else {
                keyCodeDic["某键抬起"] = key;
            }
        }
    }
    private void MyUpdate() {
        //没有开启输入检测就不去检测 直接return
        if (!isStart)
            return;
        CheckKeyCode(KeyCode.W);
        CheckKeyCode(KeyCode.S);
        CheckKeyCode(KeyCode.A);
        CheckKeyCode(KeyCode.D);
    }
}
