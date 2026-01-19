using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class BaseSkill
{
    private static BaseSkill _CreateSkill(SkillInfo info)
    {
        switch (info.Data.Type)
        {
            case GlobalEnum.eSkillType.Active:
            case GlobalEnum.eSkillType.Passive:
            default:
                return new BaseSkill();
        }
    }
}
