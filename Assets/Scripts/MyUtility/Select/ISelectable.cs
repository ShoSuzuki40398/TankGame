using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �I���C���^�[�t�F�C�X
/// </summary>
public interface ISelectable
{
    void Normal();
    void Highlighted();
    void Decided();
    void Selected();
    void Disabled();
}
