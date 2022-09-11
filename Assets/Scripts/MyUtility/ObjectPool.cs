using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectPool<TPool,TObject> : MonoBehaviour
    where TPool : ObjectPool<TPool,TObject>
    where TObject : PoolObject<TPool,TObject>, new()
{
    // ��������v���n�u
    // �v�[�������I�u�W�F�N�g�̃L�[�ɂ��Ȃ�
    public GameObject prefab;

    // �����v�[����
    public int initialPoolCount = 10;

    // �v���n�u������
    // �ݒ肳��Ă��Ȃ���Pool���A�^�b�`����Ă���GameObject�ɂ���
    public Transform parent = null;

    // ���ۂ̃I�u�W�F�N�g
    [HideInInspector]
    public List<TObject> poolingObjectList = new List<TObject>();

    private void Start()
    {
        // �V�[���J�n���Ɏw��̐������I�u�W�F�N�g�𐶐����z��ŊǗ�����
        // �������������ƁA�V�[���J�n���ɏd���Ȃ�̂ŁA�d������ꍇ�͔񓯊���
        // ��������`���ɂ��邩�A�����������炷
        for (int i = 0; i < initialPoolCount; i++)
        {
            TObject newPoolObject = CreateNewPoolObject();
            poolingObjectList.Add(newPoolObject);
        }
    }

    /// <summary>
    /// TObject���p�������I�u�W�F�N�g�𐶐���
    /// ���������I�u�W�F�N�g�̏����ݒ������B
    /// �x����Ԃ̃I�u�W�F�N�g�Ƃ��ăv�[���Ɏ��g��o�^����B
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
    /// �v�[�������疢�g�p�̃I�u�W�F�N�g�����o���B
    /// �S�Ďg�p���̏ꍇ�͐V�����I�u�W�F�N�g�𐶐���
    /// �v�[���ɒǉ�������ŁA���̐��������V�K�I�u�W�F�N�g��Ԃ��B
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
    /// �I�u�W�F�N�g���v�[���ɖ߂�
    /// </summary>
    /// <param name="poolObject"></param>
    public virtual void Push(TObject poolObject)
    {
        poolObject.inPool = true;
        poolObject.Sleep();
    }
}

/// <summary>
/// ObjectPool�ɓ����I�u�W�F�N�g���ۃN���X
/// ���̃N���X���p�������T�u�N���X���`����
/// ObjectPool�ɓ����
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