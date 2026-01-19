using System.Collections.Generic;
using UnityEngine;

public class TimeLimitNode : DecoratorNode
{
    private float _duration;
    
    private float _startTime;

    public TimeLimitNode(float duration)
    {
        _duration = duration;
    }
    
    public override void GetChild(ref List<Node> listNode)
    {
        if(Child != null)
            listNode.Add(Child);
    }

    protected override void OnStart()
    {
        _startTime = Time.time;
    }

    protected override GlobalEnum.eNodeState OnUpdate()
    {
        if (Child == null)
            return GlobalEnum.eNodeState.Failure;

        if (Time.time - _startTime > _duration)
            return GlobalEnum.eNodeState.Failure;

        return Child.Evaluate();
    }
}
