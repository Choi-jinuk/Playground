using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionAnimInfo
{
    public List<string> AnimationNameList = new List<string>();

    public static void Load(XmlUtil_Node node, ref List<string> animationName)
    {
        for (int index = 0; index < node.Attributes.Count; index++)
        {
            System.Xml.XmlAttribute xmlAttr = node.Attributes[index];
            string value = xmlAttr.Value;
            if (xmlAttr.Name == GlobalString.Name)
            {
                animationName.Add(value);
            }
            else
            {
                node.ErrorPrint($"Wrong Attributes [{xmlAttr.Name}]");
            }
        }
    }

    public static ActionAnimInfo Load(XmlUtil_Node node)
    {
        List<XmlUtil_Node> aniNodeList = node.GetChildren(GlobalString.Animation);
        if (aniNodeList == null)
        {
            DebugManager.LogError("NullReferenceException: Not Include Action Animation");
            return null;
        }

        ActionAnimInfo info = new ActionAnimInfo();
        foreach (XmlUtil_Node aniNode in aniNodeList)
        {
            string name = string.Empty;
            aniNode.GetAttr(GlobalString.Name, ref name);
            info.AnimationNameList.Add(name);
        }

        return info;
    }

    public string GetAnimation()
    {
        if (AnimationNameList.Count == 0)
        {
            return string.Empty;
        }

        int index = 0;
        if (AnimationNameList.Count > 1)
        {
            index = DataUtil.IntRandom(0, AnimationNameList.Count);
        }

        return AnimationNameList[index];
    }
}
