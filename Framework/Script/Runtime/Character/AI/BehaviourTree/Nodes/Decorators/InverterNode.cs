using System.Collections.Generic;

public class InverterNode : DecoratorNode
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

        switch (Child.Evaluate())
        {
            case GlobalEnum.eNodeState.Running:
                return GlobalEnum.eNodeState.Running;
            case GlobalEnum.eNodeState.Failure:
                return GlobalEnum.eNodeState.Success;
            case GlobalEnum.eNodeState.Success:
                return GlobalEnum.eNodeState.Failure;
        }

        return GlobalEnum.eNodeState.Failure;
    }
}
