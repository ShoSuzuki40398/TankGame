using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 砲弾プール
/// </summary>
public class ShellPool : ObjectPool<ShellPool,ShellObject>
{
}

/// <summary>
/// プールされる砲弾オブジェクト
/// </summary>
public class ShellObject : PoolObject<ShellPool, ShellObject>
{
    public Shell shell;

    protected override void SetReferences()
    {
        shell = instance.GetComponent<Shell>();
        shell.m_ShellObject = this;
    }

    public override void WakeUp()
    {
        instance.SetActive(true);
    }

    public override void Sleep()
    {
        instance.SetActive(false);
    }
}