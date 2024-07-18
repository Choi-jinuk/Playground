using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIComponent : BaseComponent
{
    public Blackboard Blackboard { get { return _tree?.Blackboard; } }
    
    private BehaviourTree _tree;
}
