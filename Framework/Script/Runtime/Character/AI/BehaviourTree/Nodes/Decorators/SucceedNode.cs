using System.Collections.Generic;

public class SucceedNode : DecoratorNode
{
    public override void GetChild(ref List<Node> listNode)
    {
        if(Child != null)
            listNode.Add(Child);
    }

    protected override void OnStart() { }

    protected override GlobalEnum.eNodeState OnUpdate()
    {
        if (Child == null)
            return GlobalEnum.eNodeState.Failure;

        var state = Child.Evaluate();
        if (state == GlobalEnum.eNodeState.Failure)
            return GlobalEnum.eNodeState.Success;
        
        return state;
    }
}
