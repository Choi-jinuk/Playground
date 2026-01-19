
using System.Collections.Generic;

public class SelectorNode : CompositeNode
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
                case GlobalEnum.eNodeState.Success:
                    return GlobalEnum.eNodeState.Success;
                case GlobalEnum.eNodeState.Failure:
                    continue;
            }
        }

        return GlobalEnum.eNodeState.Failure;
    }
}
