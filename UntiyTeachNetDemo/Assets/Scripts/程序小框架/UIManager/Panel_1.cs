using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Panel_1 : BasePanel {
    // Start is called before the first frame update
    void Start() {
        PrintControlsDic();
        //对于Button按键 在内部封装了点击事件的回调函数 当然你在外面直接添加回调函数也可以
        //GetControl<Button>("StartBtn").onClick.AddListener(() => {
        //    Debug.Log("你点击了一次开始按钮");
        //});
        //GetControl<Button>("EndBtn").onClick.AddListener(() => {
        //    Debug.Log("你点击了一次结束按钮");
        //});
        UIManager.AddCustomEventListener(GetControl<Button>("StartBtn"), EventTriggerType.PointerEnter, (data) => {
            Debug.Log("进去");
        });
        UIManager.AddCustomEventListener(GetControl<Button>("EndBtn"), EventTriggerType.PointerEnter, (data) => {
            Debug.Log("离开");
        });
    }
    protected override void OnClick(string btnName) {
        switch (btnName) {
            case "StartBtn":
                Debug.Log("你点击了一次开始按钮");
                break;
            case "EndBtn":
                Debug.Log("你点击了一次结束按钮");
                break;
            default:
                break;
        }
    }
    protected override void OnValueChanged(string toggleName, bool value) {
        switch (toggleName) {
            case "Toggle":
                value = false;
                break;
            default:
                break;
        }
    }
    public override void ShowMe() {
        base.ShowMe();
        //显示面板时 想要执行的逻辑 这个函数 在UI管理器中会自动帮我们调用
        //所以我们如果想要在显示面板之后想要执行什么逻辑 
        //我们只需要在面板脚本中修改就行 不需要关心其他地方
    }
    private void Update() {
        Debug.Log(GetControl<InputField>("Input") == null);
        if (GetControl<InputField>("Input")!=null ) {
            GetControl<Text>("Text").text = GetControl<InputField>("Input").text;
            
        }
    }
}
