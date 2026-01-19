using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionAnimEvent
{
    public GlobalEnum.eAnimationEventType Type;
    public int Index;
    
    private List<ActionAnimEvent> _subEvents = null;
    private Dictionary<GlobalEnum.eAnimationGroupEventType, List<ActionAnimEvent>> _groupEvents = null;

    public static ActionAnimEvent Create(GlobalEnum.eAnimationEventType type)
    {
        ActionAnimEvent aniEvent = null;
        switch (type)
        {
            case GlobalEnum.eAnimationEventType.Attack: aniEvent = new ActionAnimEvent_Attack(); break;
            case GlobalEnum.eAnimationEventType.Effect: aniEvent = new ActionAnimEvent_Effect(); break;
            case GlobalEnum.eAnimationEventType.Sound: aniEvent = new ActionAnimEvent_Sound(); break;
            case GlobalEnum.eAnimationEventType.CameraShake: aniEvent = new ActionAnimEvent_CameraShake(); break;
            case GlobalEnum.eAnimationEventType.Indicator: aniEvent = new ActionAnimEvent_Indicator(); break;
            default:
            {
                DebugManager.LogError($"Invalid ActionEventType - {type}");
                return null;
            }
        }

        aniEvent.Init(type);
        return aniEvent;
    }

    public static ActionAnimEvent Load(XmlUtil_Node node)
    {
        GlobalEnum.eAnimationEventType type;
        string attr = string.Empty;
        node.GetAttr(GlobalString.Type, ref attr);
        if (EnumUtil.TryParse(attr, out type) == false)
        {
            DebugManager.LogError($"Not include Enum Type {attr} in eAnimationEventType");
            return null;
        }

        ActionAnimEvent aniEvent = Create(type);
        if (aniEvent == null)
        {
            return null;
        }

        aniEvent._Load(node);

        if (aniEvent.LoadAnimationEvent(node, ref aniEvent._subEvents) == false)
        {
            DebugManager.LogError($"Failed to Load Sub AnimationEvent in {type}");
            return null;
        }

        for (int index = 0; index < (int)GlobalEnum.eAnimationGroupEventType.Count; index++)
        {
            var groupType = (GlobalEnum.eAnimationGroupEventType)index;
            XmlUtil_Node groupNode = node.GetChild(groupType.ToString());
            if(groupNode == null)
                continue;
            
            if (aniEvent._groupEvents == null)
            {
                aniEvent._groupEvents = new Dictionary<GlobalEnum.eAnimationGroupEventType, List<ActionAnimEvent>>();
            }

            List<ActionAnimEvent> events = null;
            if (aniEvent._groupEvents.TryGetValue(groupType, out events) == false)
            {
                events = new List<ActionAnimEvent>();
                aniEvent._groupEvents.Add(groupType, events);
            }

            if (aniEvent.LoadAnimationEvent(groupNode, ref events) == false)
            {
                DebugManager.LogError($"Failed to Load TargetEvent in {type}");
                return null;
            }
        }

        if (aniEvent.Type != GlobalEnum.eAnimationEventType.Attack && aniEvent._groupEvents != null)
        {
            DebugManager.LogError("Cannot have group events other than Attack Event");
        }

        return aniEvent;
    }
    
    bool _Load(XmlUtil_Node node)
    {
        for (int index = 0; index < node.Attributes.Count; ++index)
        {
            System.Xml.XmlAttribute xmlAttr = node.Attributes[index];
            string value = xmlAttr.Value;
            if (xmlAttr.Name == GlobalString.Type)
            {
                continue;
            }

            if (xmlAttr.Name == GlobalString.Index)
            {
                Index = int.Parse(value);
            }
            else if (LoadData(xmlAttr.Name, value))
            {
                
            }
            else
            {
                node.ErrorPrint($"Wrong Attributes [{node.FileName}/{xmlAttr.Name}]");
            }
        }

        return true;
    }

    public void Init(GlobalEnum.eAnimationEventType type)
    {
        Type = type;

        _Init();
    }

    bool LoadAnimationEvent(XmlUtil_Node node, ref List<ActionAnimEvent> events)
    {
        List<XmlUtil_Node> listSubNode = node.GetChildren(GlobalString.AnimationEvent);
        if (listSubNode == null)
            return true;
        
        if (events == null)
        {
            events = new List<ActionAnimEvent>();
        }

        foreach (var subNode in listSubNode)
        {
            var subAniEvent = ActionAnimEvent.Load(subNode);
            if (subAniEvent == null)
                return false;
            
            events.Add(subAniEvent);
        }

        return true;
    }

    

    public void Process(GameCharacter owner, stAnimEventProcessParam param)
    {
        _Process(owner, param);

        if (_subEvents == null)
            return;
        param.IsTargetEvent = false;
        foreach (var aniEvent in _subEvents)
        {
            aniEvent.Process(owner, param);
        }
    }

    public void ProcessGroupEvent(GameCharacter owner, GlobalEnum.eAnimationGroupEventType type, stAnimEventProcessParam param)
    {
        if (_groupEvents == null)
        {
            return;
        }

        List<ActionAnimEvent> groupEvents = null;
        if (_groupEvents.TryGetValue(type, out groupEvents) == false)
        {
            return;
        }

        if (groupEvents == null)
        {
            return;
        }

        param.IsTargetEvent = IsTargetEvents(type);
        foreach (var aniEvent in groupEvents)
        {
            aniEvent.Process(owner, param);
        }
    }

    bool IsTargetEvents(GlobalEnum.eAnimationGroupEventType type)
    {
        switch (type)
        {
            case GlobalEnum.eAnimationGroupEventType.OnHitTarget:
            case GlobalEnum.eAnimationGroupEventType.OnEndProjectile:
                return true;
        }

        return false;
    }
    
    
    protected virtual bool LoadData(string name, string value) { return true; }
    protected virtual void _Init() { }
    protected virtual void _Process(GameCharacter owner, stAnimEventProcessParam param) { }
}
