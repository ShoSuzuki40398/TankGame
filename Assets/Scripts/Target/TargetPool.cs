using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPool : ObjectPool<TargetPool, TargetObject>
{
}

public class TargetObject : PoolObject<TargetPool, TargetObject>
{
    public Target target;

    protected override void SetReferences()
    {
        target = instance.GetComponent<Target>();
        target.m_TargetObject = this;
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

