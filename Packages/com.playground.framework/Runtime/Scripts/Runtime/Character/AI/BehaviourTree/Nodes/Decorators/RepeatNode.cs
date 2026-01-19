using System.Collections.Generic;

public class RepeatNode : DecoratorNode
{
    private bool _restartOnSuccess;
    private int _maxRepeats;
    
    private int _iterationCount = 0;
    
    public RepeatNode(bool restartOnSuccess, int maxRepeats)
    {
        _restartOnSuccess = restartOnSuccess;
        _maxRepeats = maxRepeats;
    }
    
    public override void GetChild(ref List<Node> listNode)
    {
        if(Child != null)
            listNode.Add(Child);
    }

    protected override void OnStart()
    {
        _iterationCount = 0;
    }

    protected override GlobalEnum.eNodeState OnUpdate()
    {
        if (Child == null)
            return GlobalEnum.eNodeState.Failure;
        
        switch (Child.Evaluate())
        {
            case GlobalEnum.eNodeState.Running:
                break;
            case GlobalEnum.eNodeState.Failure:
            {
                if (_restartOnSuccess)
                    return GlobalEnum.eNodeState.Failure;

                _iterationCount++;
                if (_iterationCount >= _maxRepeats && _maxRepeats > 0)
                {
                    return GlobalEnum.eNodeState.Failure;
                }
                else
                {
                    return GlobalEnum.eNodeState.Running;
                }
            }
            case GlobalEnum.eNodeState.Success:
            {
                if (_restartOnSuccess == false)
                    return GlobalEnum.eNodeState.Success;
                
                _iterationCount++;
                if (_iterationCount >= _maxRepeats && _maxRepeats > 0)
                {
                    return GlobalEnum.eNodeState.Success;
                }
                else
                {
                    return GlobalEnum.eNodeState.Running;
                }
            }
        }

        return GlobalEnum.eNodeState.Running;
    }
}
