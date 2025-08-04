using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//单例模式基类模块

//1.C#泛型的知识
//2.设计模式中 单例模式的知识
public class BaseManager<T> where T : class {
    //单例模式
    private static T instance;
    protected BaseManager() { }
    public static T GetInstance() {
        if (instance == null) {
            instance = Activator.CreateInstance(typeof(T), true) as T;
        }
        return instance;
    }
}

