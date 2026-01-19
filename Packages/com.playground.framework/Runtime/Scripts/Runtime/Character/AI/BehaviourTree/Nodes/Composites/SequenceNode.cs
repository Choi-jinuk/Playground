
using System.Collections.Generic;

public class SequenceNode : CompositeNode
{
    private int _currentIdx;
    public override void GetChild(ref List<Node> listNode)
    {
        if(Children.Count != 0)
            listNode.AddRange(Children);
    }

    protected override void OnStart()
    {
        _currentIdx = 0;
    }

    protected override GlobalEnum.eNodeState OnUpdate()
    {
        var count = Children.Count;
        for (int i = _currentIdx; i < count; i++)
        {
            _currentIdx = i;
            var child = Children[_currentIdx];

            switch (child.Evaluate())
            {
                case GlobalEnum.eNodeState.Running:
                    return GlobalEnum.eNodeState.Running;
                case GlobalEnum.eNodeState.Failure:
                    return GlobalEnum.eNodeState.Failure;
                case GlobalEnum.eNodeState.Success:
                    continue;
            }
        }

        return GlobalEnum.eNodeState.Success;
    }
}
