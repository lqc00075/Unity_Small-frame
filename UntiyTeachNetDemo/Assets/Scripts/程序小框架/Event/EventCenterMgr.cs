using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventCenterMgr : BaseManager<EventCenterMgr> {
    protected EventCenterMgr() {

    }
    //共同的父类 用于里氏替换原则 然后存在字典中
    private class FatherEventInfo {

    }
    //带参数的事件信息
    private class EventInfo<T> : FatherEventInfo  {
        public UnityAction<T> actions;
        public EventInfo(UnityAction<T> action) {
            actions += action;
        }
        public void AddEvent(UnityAction<T> action) {
            actions += action;
        }
        public void RemoveEvent(UnityAction<T> action) {
            actions -= action;
        }
    }
    //不带参数的事件信息
    private class EventInfo : FatherEventInfo {
        public UnityAction actions;
        public EventInfo(UnityAction action) {
            actions += action;
        }
        public void AddEvent(UnityAction action) {
            actions += action;
        }
        public void RemoveEvent(UnityAction action) {
            actions -= action;
        }
    }

    private Dictionary<string, FatherEventInfo> eventDic = new Dictionary<string, FatherEventInfo>();

    //给一个监听对象 添加事件
    public void AddEventListener(string eventName, UnityAction action) {
        if (!eventDic.ContainsKey(eventName)) {
            eventDic.Add(eventName, new EventInfo(action));
        } else {
            (eventDic[eventName] as EventInfo).AddEvent(action);
        }
    }
    //给一个监听对象 移除事件
    public void RemoveEventListener(string eventName, UnityAction action) {
        if (eventDic.ContainsKey(eventName)) {
            (eventDic[eventName] as EventInfo).RemoveEvent(action);
        }
    }
    //触发某一个监听对象的所有事件
    public void EventTrigger(string eventName) {
        if (eventDic.ContainsKey(eventName)) {
            (eventDic[eventName] as EventInfo).actions?.Invoke();
        }
    }
    //清空所有事件
    public void AddEventListener<T>(string eventName, UnityAction<T> action) {
        if (!eventDic.ContainsKey(eventName)) {
            eventDic.Add(eventName, new EventInfo<T>(action));
        } else {
            (eventDic[eventName] as EventInfo<T>).AddEvent(action);
        }
    }
    //带参数的情况
    public void RemoveEventListener<T>(string eventName, UnityAction<T> action) {
        if (eventDic.ContainsKey(eventName)) {
            (eventDic[eventName] as EventInfo<T>).RemoveEvent(action);
        }
    }
    public void EventTrigger<T>(string eventName,T info) {
        if (eventDic.ContainsKey(eventName)) {
            (eventDic[eventName] as EventInfo<T>).actions?.Invoke(info);
        }
    }
    public void Clear() {
        eventDic.Clear();
    }
}
