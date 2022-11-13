﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// シングルトンベースクラス
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    
    public static T Instance
    {
        /// <summary>
        /// インスタンスがない時は、空のオブジェクトを作成して
        /// 指定シングルトンコンポーネントをアタッチする。
        /// </summary>
        get
        {
            if(instance == null)
            {
                Type type = typeof(T);

                instance = (T)FindObjectOfType(type);
                if(instance == null)
                {
                    GameObject obj = new GameObject(type.ToString());
                    T singlton = obj.AddComponent<T>();
                    instance = singlton;
                    DontDestroyOnLoad(obj);
                }
            }

            return instance;
        }
    }

    /// <summary>
    /// 起動時処理
    /// 起動時に何か処理を挟みたいときに実装する
    /// </summary>
    protected virtual void ActionInAwake() { }

    protected virtual void Awake()
    {
        ActionInAwake();
        DontDestroyOnLoad(gameObject);
    }

    public void OnApplicationQuit()
    {
        this.Destroy();
    }

    public void Destroy()
    {
        Destroy(this);
    }
}
