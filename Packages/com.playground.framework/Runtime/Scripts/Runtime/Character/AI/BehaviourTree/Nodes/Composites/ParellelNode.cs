using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParellelNode : CompositeNode
{
    private List<GlobalEnum.eNodeState> _childrenLeftToExecute = new List<GlobalEnum.eNodeState>();
    public override void GetChild(ref List<Node> listNode)
    {
        if(Children.Count != 0)
            listNode.AddRange(Children);
    }

    protected override void OnStart()
    {
        _childrenLeftToExecute.Clear();
        Children.ForEach(node => _childrenLeftToExecute.Add(GlobalEnum.eNodeState.Running));
    }

    protected override GlobalEnum.eNodeState OnUpdate()
    {
        bool stillRunning = false;
        
        var count = _childrenLeftToExecute.Count;
        for (int i = 0; i < count; i++)
        {
            if (_childrenLeftToExecute[i] != GlobalEnum.eNodeState.Running)
                continue;
            
            var state = Children[i].Evaluate();
            if (state == GlobalEnum.eNodeState.Failure)
            {
                AbortRunningChildren();
                return GlobalEnum.eNodeState.Failure;
            }

            if (state == GlobalEnum.eNodeState.Running)
            {
                stillRunning = true;
            }

            _childrenLeftToExecute[i] = state;
        }

        return stillRunning ? GlobalEnum.eNodeState.Running : GlobalEnum.eNodeState.Success;
    }

    void AbortRunningChildren()
    {
        var count = _childrenLeftToExecute.Count;
        for (int i = 0; i < count; ++i)
        {
            if (_childrenLeftToExecute[i] != GlobalEnum.eNodeState.Running)
                continue;
            
            Children[i].Abort();
        }
    }
}
