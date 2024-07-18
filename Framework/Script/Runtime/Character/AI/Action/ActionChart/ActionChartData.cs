using System.Collections.Generic;
using Unity.VisualScripting;

public class ActionChartData
{
    public string Name { get { return _name; } }
    public Dictionary<string, ActionData> ActionDictionary { get { return _actionDictionary; } }
    
    public bool IsIncludeData { get { return _includeAction != null && _includeAction.Count > 0; } }
    public List<string> IncludeAction { get { return _includeAction; } }
    
    private string _name;
    private string _animationEventKey;
    private Dictionary<string, ActionData> _actionDictionary = new Dictionary<string, ActionData>();

    private List<string> _includeAction = null;
    public static ActionChartData Load(XmlUtil_Node node)
    {
        ActionChartData data = new ActionChartData();
        for (int index = 0; index < node.Attributes.Count; index++)
        {
            System.Xml.XmlAttribute xmlAttr = node.Attributes[index];
            string value = xmlAttr.Value;
            if (xmlAttr.Name == GlobalString.Name)
            {
                data._name = value;
            }
            else
            {
                node.ErrorPrint($"Wrong Attributes [{xmlAttr.Name}]");
            }
        }

        var nodeList = new List<XmlUtil_Node>();
        node.GetChildren(ref nodeList);
        foreach (var childNode in nodeList)
        {
            if (childNode.IsName(GlobalString.Include))
            {
                if (data._includeAction == null)
                {
                    data._includeAction = new List<string>();
                }
                
                _LoadIncludeAttributes(childNode, ref data._includeAction);
            }
            else if (childNode.IsName(GlobalString.Action))
            {
                var action = ActionData.Load(childNode);
                if (action != null)
                {
                    data._actionDictionary.Add(action.Name, action);
                }
            }
            else if (childNode.IsName(GlobalString.AnimationEventKey))
            {
                _LoadAnimationEventKeyAttributes(childNode, ref data._animationEventKey);
            }
            else
            {
                childNode.ErrorPrint($"Wrong Attributes [{childNode.Name}]");
            }
        }

        return data;
    }

    private static void _LoadIncludeAttributes(XmlUtil_Node node, ref List<string> includeActionList)
    {
        for (int index = 0; index < node.Attributes.Count; index++)
        {
            System.Xml.XmlAttribute xmlAttr = node.Attributes[index];
            string value = xmlAttr.Value;
            if (xmlAttr.Name == GlobalString.ActionChart)
            {
                includeActionList.Add(value);
            }
            else
            {
                node.ErrorPrint($"Wrong Attributes [{xmlAttr.Name}]");
            }
        }
    }

    private static void _LoadAnimationEventKeyAttributes(XmlUtil_Node node, ref string key)
    {
        for (int index = 0; index < node.Attributes.Count; index++)
        {
            System.Xml.XmlAttribute xmlAttr = node.Attributes[index];
            string value = xmlAttr.Value;
            if (xmlAttr.Name == GlobalString.Name)
            {
                key = value;
            }
            else
            {
                node.ErrorPrint($"Wrong Attributes [{xmlAttr.Name}]");
            }
        }
    }

    public void GetIncludeData(List<string> includeAction, ref ActionChartData includeData)
    {
        if (includeAction == null)
        {
            return;
        }

        foreach (var name in includeAction)
        {
            var data = ActionManager.Instance.GetActionChartData(name);
            if (data == null)
            {
                DebugManager.LogError($"NullReferenceException: Include ActionChart [{name}] is Null");
                return;
            }

            foreach (var keyValuePair in data._actionDictionary)
            {
                includeData.AddActionData(keyValuePair.Key, keyValuePair.Value);
            }
            
            GetIncludeData(data._includeAction, ref includeData);
        }
    }

    bool AddActionData(string name, ActionData data)
    {
        if (_actionDictionary.ContainsKey(name))
        {
            return false;
        }

        _actionDictionary.Add(name, data);
        return true;
    }
}
