using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public partial class ActionComponent : BaseComponent
{
    private ActionChart _actionChart;
    
    private Animator _animator;
    private Dictionary<string, AnimationClip> _animationClips = new Dictionary<string, AnimationClip>();

    private CancellationTokenSource _changeAnimationCancel;

    private float _statAttackTime;
    private float _orgSpeed;

    private string _prevActionSkillKey;
    private void Awake()
    {
        _animator = SelfObject.GetComponent<Animator>();
        
        foreach (var clip in _animator.runtimeAnimatorController.animationClips)
        {
            _animationClips[clip.name] = clip;
        }
    }

    public void Init(string actionChartName)
    {
        ActionChartData data = ActionManager.Instance.GetActionChartData(actionChartName);
        if (data == null)
        {
            return;
        }

        _actionChart = new ActionChart(Character, data);

        _aniEventData.Events = new List<AniEventDataElement>();
        _eventPool.Init(5);
        _orgSpeed = _animator.speed;
    }

    public void ChangeAction(string actionName)
    {
        var result = _actionChart.ChangeAction(actionName, this);
        if (result == GlobalEnum.eActionChangeResult.Success)
        {
            _SetMove();
        }
    }

    public void ChangeAnimationState(string stateName)
    {
        if (_changeAnimationCancel != null)
        {
            _changeAnimationCancel.Cancel();
            _changeAnimationCancel.Dispose();
            _changeAnimationCancel = null;
        }

        _changeAnimationCancel = new CancellationTokenSource();
        _ChangeAnimationState(stateName, _changeAnimationCancel.Token).Forget();
    }

    async UniTaskVoid _ChangeAnimationState(string stateNam, CancellationToken token)
    {
        try
        {
            _aniEventData.Clear();
            _statAttackTime = 0f;
            if (Character.SkillComponent)
            {
                Character.SkillComponent.OnActionChange(IsCurrentActionAttribute(GlobalEnum.eActionAttribute.Skill));
            }

            if (_IsAttackAction())
            {
                if (Character.TargetComponent.Target != null)
                {
                    Character.RotateComponent?.RotationToTarget(Character.TargetComponent.Target);
                }

                if (IsCurrentActionAttribute(GlobalEnum.eActionAttribute.Attack))
                {
                    //_statAttackTime = Character.Info.GetStat()
                }
            }
        }
        catch (OperationCanceledException e)
        {
            DebugManager.LogError(e.Message);
        }
    }

    public bool IsCurrentActionAttribute(GlobalEnum.eActionAttribute attr)
    {
        if (_actionChart.CurrentAction == null)
        {
            return false;
        }

        return _actionChart.CurrentAction.IsAttribute(attr);
    }

    bool _IsAttackAction()
    {
        return IsCurrentActionAttribute(GlobalEnum.eActionAttribute.Attack | GlobalEnum.eActionAttribute.Skill);
    }

    private void _SetMove()
    {
        
    }
}
