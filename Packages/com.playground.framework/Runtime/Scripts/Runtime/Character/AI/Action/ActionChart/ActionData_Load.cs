using System.Collections.Generic;
using UnityEditor;

public partial class ActionData
{
    public static ActionData Load(XmlUtil_Node node)
    {
        ActionData action = new ActionData();

        for (int index = 0; index < node.Attributes.Count; index++)
        {
            System.Xml.XmlAttribute xmlAttr = node.Attributes[index];
            string value = xmlAttr.Value;
            if (xmlAttr.Name == GlobalString.Name)
            {
                action.Name = value;
            }
            else if (xmlAttr.Name == GlobalString.Attribute)
            {
                _LoadActionAttribute(value, ref action);
            }
            else
            {
                node.ErrorPrint($"Wrong Attributes [{xmlAttr.Name}]");
            }
        }

        _LoadActionChildNode(node, ref action);
        return action;
    }

    private static void _LoadActionAttribute(string actionAttr, ref ActionData action)
    {
        if (string.IsNullOrEmpty(actionAttr))
            return;

        var attributes = actionAttr.Split(' ');
        if (attributes == null || attributes.Length <= 0)
            return;
        foreach (var attr in attributes)
        {
            if (string.IsNullOrEmpty(attr))
                continue;

            if (EnumUtil.TryParse<GlobalEnum.eActionAttribute>(attr, out var enumAttr) == false)
            {
                return;
            }

            action.Attr.SetAttr(enumAttr);
        }
    }

    private static void _LoadActionChildNode(XmlUtil_Node node, ref ActionData action)
    {
        var nodeList = new List<XmlUtil_Node>();
        node.GetChildren(ref nodeList);
        foreach (var childNode in nodeList)
        {
            if (childNode.IsName(GlobalString.Animation))
            {
                ActionAnimInfo.Load(childNode, ref action.AnimationInfo.AnimationNameList);
            }
            else if (childNode.IsName(GlobalString.AnimationEvent))
            {
                _LoadActionEvent(childNode, ref action);
            }
            else if (_LoadOtherData(childNode, ref action))
            {
                
            }
            else
            {
                childNode.ErrorPrint($"Wrong Attributes [{childNode.Name}]");
            }
        }
    }

    private static bool _LoadOtherData(XmlUtil_Node node, ref ActionData action)
    {
        if (node.IsName(GlobalString.Move))
        {
            if (action.IsAttribute(GlobalEnum.eActionAttribute.Move) == false)
            {
                DebugManager.LogError($"Move Node But is Not Matching Move Attribute {node.Name}", true);
                return false;
            }

            //Todo; MoveData 추가 필요
            // action.MoveData = new MoveData();
            // for (int index = 0; index < node.Attributes.Count; index++)
            // {
            //     bool error = false;
            //     System.Xml.XmlAttribute xmlAttr = node.Attributes[index];
            //     string value = xmlAttr.Value;
            //     if (xmlAttr.Name == GlobalString.ACTION_CHART_OTHER_MOVE_TYPE)
            //     {
            //         error = !EnumUtil.TryParse(value, out action.MoveData.MoveType);
            //     }
            //     else if (xmlAttr.Name == GlobalString.ACTION_CHART_OTHER_MOVE_SPEED)
            //     {
            //         action.MoveData.Speed = float.Parse(value);
            //         action.MoveData.IsDataSpeed = false;
            //     }
            //     else
            //     {
            //         error = true;
            //     }
            //
            //     if (error)
            //     {
            //         node.ErrorPrint($"Wrong Attribute [{xmlAttr.Name}]");
            //     }
            // }
        }

        return true;
    }

    private static bool _LoadActionEvent(XmlUtil_Node node, ref ActionData action)
    {
        ActionAnimEvent aniEvent = ActionAnimEvent.Load(node);
        if (aniEvent == null)
        {
            DebugManager.LogError($"Fail to Load Action AnimEvent {action.Name}");
            return false;
        }
        
        string key = StringUtil.Append(aniEvent.Type.ToString(), aniEvent.Index.ToString());
        action._animEventDictionary.Add(key, aniEvent);

        return true;
    }
}
