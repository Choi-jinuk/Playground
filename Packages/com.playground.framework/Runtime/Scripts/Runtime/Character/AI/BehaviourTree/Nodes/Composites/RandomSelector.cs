
using System.Collections.Generic;
using UnityEngine;

public class RandomSelector : CompositeNode
{
    private int _currentIdx;
    
    public override void GetChild(ref List<Node> listNode)
    {
        if(Children.Count != 0)
            listNode.AddRange(Children);
    }

    protected override void OnStart()
    {
        _currentIdx = Random.Range(0, Children.Count);
    }

    protected override GlobalEnum.eNodeState OnUpdate()
    {
        var child = Children[_currentIdx];
        return child.Evaluate();
    }
}
