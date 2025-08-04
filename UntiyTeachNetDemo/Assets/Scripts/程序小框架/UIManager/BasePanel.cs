using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


/// <summary>
/// 面板基类
/// 帮助我们通过代码快速找到左右的子控件
/// 方便我们在子类中处理逻辑
/// 节约找控件的工作量
/// 找到自己面板下的控件对象
/// 他应该提供显示 或者隐藏的函数（方法）
/// </summary>
public class BasePanel : MonoBehaviour {

    //通过里氏替换原则 来存储所有的组件
    private Dictionary<string, List<UIBehaviour>> controllerDic = new Dictionary<string, List<UIBehaviour>>();
    // Start is called before the first frame update
    protected virtual void Awake() {
        // 查找Button控件
        FindChildControl<Button>();
        // 查找Image控件
        FindChildControl<Image>();
        // 查找Text控件
        FindChildControl<Text>();
        // 查找Toggle控件切换按钮
        FindChildControl<Toggle>();
        // 查找Scrollbar控件滚动条
        FindChildControl<Scrollbar>();
        // 查找Slider控件
        FindChildControl<Slider>();
        // 查找ScrollRect控件 滚动视图组件
        FindChildControl<ScrollRect>();
        // 查找InputField控件 输入框
        FindChildControl<InputField>();

    }

    /// <summary>
    /// 显示自己
    /// </summary>
    public virtual void ShowMe() {

    }
    /// <summary>
    /// 隐藏自己
    /// </summary>
    public virtual void HideMe() {

    }
    protected virtual void OnClick(string btnName) {

    }
    protected virtual void OnValueChanged(string toggleName, bool value) {

    }
    /// <summary>
    /// 得到对应名字的对应控件脚本
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="controlName"></param>
    /// <returns></returns>
    protected T GetControl<T>(string controlName) where T : UIBehaviour {
        if (controllerDic.ContainsKey(controlName)) {
            for (int i = 0; i < controllerDic[controlName].Count; i++) {
                if (controllerDic[controlName][i] is T)
                    return controllerDic[controlName][i] as T;
            }
        }
        return null;
    }
    /// <summary>
    /// 获取子对象的对应控件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    private void FindChildControl<T>() where T : UIBehaviour {
        T[] controlls = this.GetComponentsInChildren<T>();
        for (int i = 0; i < controlls.Length; i++) {
            string objName = controlls[i].gameObject.name;
            if (controllerDic.ContainsKey(objName))
                controllerDic[objName].Add(controlls[i]);
            else
                controllerDic.Add(objName, new List<UIBehaviour>() { controlls[i] });

            if (controlls[i] is Button) {
                (controlls[i] as Button).onClick.AddListener(() => {
                    OnClick(objName);
                });
            }
            //如果是单选框或者多选框
            //true就是选中 false就是未选中 传入的是一个bool值
            else if (controlls[i] is Toggle) {
                //选中默认传入一个true
                (controlls[i] as Toggle).onValueChanged.AddListener((value) => {
                    OnValueChanged(objName, value);
                });
            }
        }
    }
    protected void PrintControlsDic() {
        foreach (var item in controllerDic) {
            Debug.Log($"这是{item.Key}的组件们");
            for (int i = 0; i < item.Value.Count; i++) {
                Debug.Log(item.Value[i].name);
            }
        }
    }
}
