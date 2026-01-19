using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillComponent : BaseComponent
{
    public bool IsRunningSkill { get { return _isRunningSkill; } }

    private bool _isRunningSkill = false;
    private List<BaseSkill> _skillList = new List<BaseSkill>();

    public void Init(CharacterInfo info)
    {
        if (info == null)
        {
            return;
        }

        Clear();
        foreach (var skillInfo in info.SkillInfoList)
        {
            if (skillInfo.Level > 0 && skillInfo.IsLock == false)
                _AddSkill(skillInfo);
        }
    }

    void _AddSkill(SkillInfo info)
    {
        BaseSkill skill = BaseSkill.Create(Character, info);

        if (skill == null)
        {
            return;
        }
        
        _skillList.Add(skill);
    }

    public void OnActionChange(bool isSkillAction)
    {
        if (_isRunningSkill && isSkillAction == false)
        {
            _ResetCoolTime();
        }

        _isRunningSkill = isSkillAction;
    }

    void _ResetCoolTime()
    {
        
    }

    public void Clear()
    {
        ClearSkill();
        _skillList.Clear();
    }
    public void ClearSkill()
    {
        foreach (var skill in _skillList)
        {
            skill.Clear();
        }
    }
}
