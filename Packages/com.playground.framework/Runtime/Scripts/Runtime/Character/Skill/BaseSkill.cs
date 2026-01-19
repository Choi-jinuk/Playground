using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class BaseSkill
{
    protected GameCharacter _owner;
    protected SkillInfo _info;

    protected float _accCoolTime;
    protected float _coolTime;
    protected float _coolRate;

    public static BaseSkill Create(GameCharacter owner, SkillInfo info)
    {
        if (owner == null)
        {
            DebugManager.LogError("GameCharacter is Null");
            return null;
        }
        if (info == null)
        {
            DebugManager.LogError($"SkillInfo is Null in {owner.Key}");
            return null;
        }

        BaseSkill skill = _CreateSkill(info);
        if (skill == null)
        {
            DebugManager.LogError($"Invalid SkillType {owner.Key}_{info.Key}");
            return null;
        }
        
        skill.Init(owner, info);
        return skill;
    }

    private void Init(GameCharacter owner, SkillInfo info)
    {
        _owner = owner;
        _info = info;
        
        _Init();
    }
    
    protected virtual void _Init() { }
    public virtual void Clear() { }
}
