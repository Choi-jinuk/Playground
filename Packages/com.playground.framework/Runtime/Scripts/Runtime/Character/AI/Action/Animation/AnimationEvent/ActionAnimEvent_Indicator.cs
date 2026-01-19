using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionAnimEvent_Indicator : ActionAnimEvent
{
    protected override bool LoadData(string name, string value)
    {
        return true;
    }

    protected override void _Process(GameCharacter owner, stAnimEventProcessParam param)
    {
        
    }
}
