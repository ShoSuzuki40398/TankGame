using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// シングルトンベースクラス
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class SingletonMonoBehaviour<T> : MonoBehaviourWithInit where T : MonoBehaviourWithInit
{
    private static T instance;

    // 外部参照用プロパティ
    public static T Instance
    {
        /// <summary>
        /// 前提として、事前にシーンに作成してあること
        /// インスタンスがない時は、新規作成する
        /// </summary>
        get
        {
            if (instance == null)
            {
                Type type = typeof(T);

                instance = (T)FindObjectOfType(type);
                if (instance == null)
                {
                    GameObject obj = new GameObject(type.ToString());
                    T singlton = obj.AddComponent<T>();
                    instance = singlton;
                }
                // 初アクセス時に初期化処理をする
                instance.InitIfNeeded();
            }

            return instance;
        }
    }

    protected sealed override void Awake()
    {
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

/// <summary>
/// 初期化メソッドを備えたMonoBehaviour
/// </summary>
public class MonoBehaviourWithInit : MonoBehaviour
{

    //初期化したかどうかのフラグ(一度しか初期化が走らないようにするため)
    private bool isInitialized = false;

    /// <summary>
    /// 必要なら初期化する
    /// </summary>
    public void InitIfNeeded()
    {
        if (isInitialized)
        {
            return;
        }
        DontDestroyOnLoad(gameObject);
        Init();
        isInitialized = true;
    }

    /// <summary>
    /// 初期化(Awake時かその前の初アクセス、どちらかの一度しか行われない)
    /// </summary>
    protected virtual void Init() { }

    //sealed overrideするためにvirtualで作成
    protected virtual void Awake() { }

}