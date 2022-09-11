using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectPool<TPool,TObject> : MonoBehaviour
    where TPool : ObjectPool<TPool,TObject>
    where TObject : PoolObject<TPool,TObject>, new()
{
    // 生成するプレハブ
    // プールされるオブジェクトのキーにもなる
    public GameObject prefab;

    // 初期プール数
    public int initialPoolCount = 10;

    // プレハブ生成先
    // 設定されていなけばPoolがアタッチされているGameObjectにする
    public Transform parent = null;

    // 実際のオブジェクト
    [HideInInspector]
    public List<TObject> poolingObjectList = new List<TObject>();

    private void Start()
    {
        // シーン開始時に指定の数だけオブジェクトを生成し配列で管理する
        // 生成数が多いと、シーン開始時に重くなるので、重すぎる場合は非同期で
        // 生成する形式にするか、生成数を減らす
        for (int i = 0; i < initialPoolCount; i++)
        {
            TObject newPoolObject = CreateNewPoolObject();
            poolingObjectList.Add(newPoolObject);
        }
    }

    /// <summary>
    /// TObjectを継承したオブジェクトを生成し
    /// 生成したオブジェクトの初期設定をする。
    /// 休眠状態のオブジェクトとしてプールに自身を登録する。
    /// </summary>
    /// <returns></returns>
    protected TObject CreateNewPoolObject()
    {
        TObject newPoolObject = new TObject();
        newPoolObject.instance = Instantiate(prefab);
        if (parent == null)
            newPoolObject.instance.transform.SetParent(transform);
        else
            newPoolObject.instance.transform.SetParent(parent);

        newPoolObject.inPool = true;
        newPoolObject.SetReferences(this as TPool);
        newPoolObject.Sleep();
        return newPoolObject;
    }

    /// <summary>
    /// プール内から未使用のオブジェクトを取り出す。
    /// 全て使用中の場合は新しくオブジェクトを生成し
    /// プールに追加した上で、その生成した新規オブジェクトを返す。
    /// </summary>
    /// <returns></returns>
    public virtual TObject Pop()
    {
        for (int i = 0; i < poolingObjectList.Count; i++)
        {
            if (poolingObjectList[i].inPool)
            {
                poolingObjectList[i].inPool = false;
                poolingObjectList[i].WakeUp();
                return poolingObjectList[i];
            }
        }

        TObject newPoolObject = CreateNewPoolObject();
        poolingObjectList.Add(newPoolObject);
        newPoolObject.inPool = false;
        newPoolObject.WakeUp();
        return newPoolObject;
    }

    /// <summary>
    /// オブジェクトをプールに戻す
    /// </summary>
    /// <param name="poolObject"></param>
    public virtual void Push(TObject poolObject)
    {
        poolObject.inPool = true;
        poolObject.Sleep();
    }
}

/// <summary>
/// ObjectPoolに入れるオブジェクト抽象クラス
/// このクラスを継承したサブクラスを定義して
/// ObjectPoolに入れる
/// </summary>
/// <typeparam name="TPool"></typeparam>
/// <typeparam name="TObject"></typeparam>
public abstract class PoolObject<TPool,TObject>
    where TPool : ObjectPool<TPool,TObject>
    where TObject : PoolObject<TPool,TObject>,new()
{
    public bool inPool;
    public GameObject instance;
    public TPool objectPool;

    public void SetReferences(TPool pool)
    {
        objectPool = pool;
        SetReferences();
    }

    protected virtual void SetReferences()
    { }

    public virtual void WakeUp()
    { }

    public virtual void Sleep()
    { }

    public virtual void ReturnToPool()
    {
        TObject thisObject = this as TObject;
        objectPool.Push(thisObject);
    }

}