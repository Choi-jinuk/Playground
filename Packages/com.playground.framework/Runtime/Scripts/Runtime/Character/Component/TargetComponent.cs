using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetComponent : BaseComponent
{
    public GameCharacter Target { get { _CheckEnableTarget(); return _target; } }

    private GameCharacter _target;


    void _CheckEnableTarget()
    {
        if (Character.AIComponent.Blackboard.IsData(stBoardMeta.META.Target))
        {
            ulong uid = Character.AIComponent.Blackboard.GetValueULong(stBoardMeta.META.Target);
            //Todo; Ingame Character를 관리하는 Manager에서 uid로 비교해서 target으로 설정된 Character 가져오기
        }

        if (_target == null)
            return;
        if (_target.Info?.IsDead ?? true)
            SetTarget(null);
    }

    public bool SetTarget(GameCharacter target)
    {
        _target = target;

        return _target != null;
    }
}
