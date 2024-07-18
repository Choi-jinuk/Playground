using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfo
{
    public string Key { get { return _infoParam.Key; } }
    public bool IsDead { get { return _currentHp <= 0; } }
    public List<SkillInfo> SkillInfoList { get { return _skillInfoList; } }
    
    private stCharacterInfoParam _infoParam;
    private CharacterTemplateData _data;
    private List<SkillInfo> _skillInfoList = new List<SkillInfo>();

    private double _currentHp;
    public void SetData(CharacterInstanceData instanceData)
    {
        _data = CharacterTemplateManager.Instance.GetTemplate(instanceData.Key);
        _infoParam = instanceData.MakeInfoParam();
        
        
    }

    public void SetSkillList()
    {
        
    }
}
