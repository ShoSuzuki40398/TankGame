using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 選択インターフェイス
/// </summary>
public interface ISelectable
{
    void Normal();
    void Highlighted();
    void Decided();
    void Selected();
    void Disabled();
}
