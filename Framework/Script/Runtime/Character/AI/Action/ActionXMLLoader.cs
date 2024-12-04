using System.Collections.Generic;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using UnityEngine;


public class ActionXMLLoader
{
    public static async UniTask<Dictionary<string, ActionChartData>> LoadAction(string path, Dictionary<string, ActionChartData> actionChartDictionary)
    {
        XmlUtil xml = new XmlUtil();
        
        var assetList = await AddressableManager.LoadAssetList(GlobalString.ACTION_CHART_LIST, false);
        foreach (var assetName in assetList.assets)
        {
            if (false == await xml.LoadAsset(assetName))
            {
                return actionChartDictionary;
            }

            var rootNode = xml.GetRootChild(GlobalString.Root);
            if (rootNode.InnerXml.Length == 0)
            {
                return actionChartDictionary;
            }

            var nodeList = new List<XmlUtil_Node>();
            rootNode.GetChildren(ref nodeList);
            foreach (var node in nodeList)
            {
                if (node.IsName(GlobalString.ActionChart) == false)
                {
                    return actionChartDictionary;
                }

                var data = ActionChartData.Load(node);
                var name = data.Name.ToLower();
                if (actionChartDictionary.ContainsKey(name))
                {
                    return actionChartDictionary;
                }
                actionChartDictionary.Add(name, data);
            }
        }

        return actionChartDictionary;
    }

    public static async UniTask<stLoadAnimEventParam> LoadAniEvents(string path, stLoadAnimEventParam param)
    {
        string fileName = System.IO.Path.GetFileNameWithoutExtension(path);
        XmlUtil xml = new XmlUtil();
        if (false == await xml.LoadAsset(fileName))
        {
            DebugManager.LogError($"Failed To Load LoadAniEvents XML [{fileName}]");
            return default;
        }

        var aniEventDictionary = param.AniEventDictionary;
        var commonEvents = param.CommonEvents;
        
        XmlUtil_Node rootNode = xml.GetRootChild(GlobalString.Root);
        List<XmlUtil_Node> nodeList = new List<XmlUtil_Node>();
        rootNode.GetChildren(ref nodeList);
        foreach (var node in nodeList)
        {
            if (node.Name == GlobalString.CommonEvents)
            {
                List<XmlUtil_Node> commonEventNode = new List<XmlUtil_Node>();
                node.GetChildren(ref commonEventNode);
                foreach (var common in commonEventNode)
                {
                    string key = common.Name;
                    List<XmlUtil_Node> eventNodeList = new List<XmlUtil_Node>();
                    common.GetChildren(ref eventNodeList);
                    List<AniEventDataElement> events = new List<AniEventDataElement>();
                    if (eventNodeList.Count > 0)
                    {
                        foreach (var elementNode in eventNodeList)
                        {
                            var element = LoadElement(elementNode);
                            events.Add(element);
                        }
                    }

                    commonEvents.Add(key, events);
                }
            }
            else if (node.Name == GlobalString.CharacterEvents)
            {
                List<XmlUtil_Node> charEventNodes = new List<XmlUtil_Node>();
                node.GetChildren(ref charEventNodes);
                foreach (var charNode in charEventNodes)
                {
                    var charData = new CharacterAnimEventData();
                    charData.Key = charNode.Name;
                    charData.DataList = new List<AniEventData>();
                    aniEventDictionary.Add(charData.Key, charData);

                    List<XmlUtil_Node> eventNodes = new List<XmlUtil_Node>();
                    charNode.GetChildren(ref eventNodes);
                    foreach (var dataNode in eventNodes)
                    {
                        var data = new AniEventData();
                        data.Animation = dataNode.Name;
                        
                        charData.DataList.Add(data);

                        string eventKey = string.Empty;
                        if (dataNode.GetAttr(GlobalString.EventKey, ref eventKey))
                        {
                            if (commonEvents.TryGetValue(eventKey, out data.Events) == false)
                            {
                                dataNode.ErrorPrint($"Not Exist AnimationEvent Key {eventKey} in [{charNode.Name}]");
                                return default;
                            }
                        }
                        else
                        {
                            data.Events = new List<AniEventDataElement>();
                            List<XmlUtil_Node> eventNodeList = new List<XmlUtil_Node>();
                            dataNode.GetChildren(ref eventNodeList);
                            foreach (var elementNode in eventNodeList)
                            {
                                var element = LoadElement(elementNode);
                                data.Events.Add(element);
                            }
                        }
                    }
                }
            }
        }

        param.AniEventDictionary = aniEventDictionary;
        param.CommonEvents = commonEvents;
        return param;
    }

    private static AniEventDataElement LoadElement(XmlUtil_Node node)
    {
        string attrValue = string.Empty;
        node.GetAttr(GlobalString.Value, ref attrValue);
        AniEventDataElement element = JsonUtility.FromJson<AniEventDataElement>(attrValue);
        EnumUtil.TryParse<GlobalEnum.eAnimationEventType>(element.EventType, out var type);
        element.Type = type;
        return element;
    }
}
