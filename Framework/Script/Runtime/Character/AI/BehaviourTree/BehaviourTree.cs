using System.Collections.Generic;
using UnityEngine;

public class BehaviourTree : MonoBehaviour
{
    public Blackboard Blackboard { get { return _blackboard; } }
    
    [SerializeField] private Blackboard _blackboard;
    
    private RootNode _rootNode;
    
    public BehaviourTree(RootNode rootNode)
    {
        _rootNode = rootNode;
    }

    public GlobalEnum.eNodeState Evaluate()
    {
        return _rootNode.Evaluate();
    }

    public void Bind(Context context)
    {
        Traverse(_rootNode, node =>
        {
            node.Bind(context, _blackboard);
        });
    }
    
    
    public static List<Node> GetChildren(Node parent)
    {
        var listNode = new List<Node>();
        parent.GetChild(ref listNode);
        
        return listNode;
    }
    public static void Traverse(Node node, System.Action<Node> visitor)
    {
        if (node == null)
            return;
        visitor.Invoke(node);
        var children = GetChildren(node);
        children.ForEach(n => Traverse(n, visitor));
    }
}
