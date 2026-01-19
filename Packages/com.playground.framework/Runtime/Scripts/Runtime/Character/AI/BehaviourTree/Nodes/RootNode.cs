
using System.Collections.Generic;

public class RootNode : Node
{
    private Node _child;

    public override void GetChild(ref List<Node> listNode)
    {
        listNode.Add(_child);
    }

    protected override void OnStart()
    { }

    protected override GlobalEnum.eNodeState OnUpdate()
    {
        if (_child == null)
            return GlobalEnum.eNodeState.Failure;

        return _child.Evaluate();
    }
}
