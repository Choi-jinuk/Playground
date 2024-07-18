
using System.Collections.Generic;

public abstract class Node
{
    private Blackboard _blackboard;
    private Context _context;
    
    private bool _started;

    public void Bind(Context context, Blackboard blackboard)
    {
        _context = context;
        _blackboard = blackboard;
    }
    
    public GlobalEnum.eNodeState Evaluate()
    {
        if (_started == false)
        {
            OnStart();
            _started = true;
        }

        return OnUpdate();
    }

    public void Abort()
    {
        
    }

    
    public abstract void GetChild(ref List<Node> listNode);
    protected abstract void OnStart();
    protected abstract GlobalEnum.eNodeState OnUpdate();
}
