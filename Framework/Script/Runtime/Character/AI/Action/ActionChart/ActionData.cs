using System.Collections.Generic;

public partial class ActionData
{
    public string Name = string.Empty;
    public AttributeSetter<GlobalEnum.eActionAttribute> Attr = new AttributeSetter<GlobalEnum.eActionAttribute>();
    public ActionAnimInfo AnimationInfo = new ActionAnimInfo();

    //Key = eAnimationEventType + index
    private Dictionary<string, ActionAnimEvent> _animEventDictionary = new Dictionary<string, ActionAnimEvent>();

    public ActionAnimEvent GetAnimationEvent(string key)
    {
        if (_animEventDictionary.TryGetValue(key, out var aniEvent) == false)
        {
            DebugManager.LogError($"Not Include Action [{Name}] AnimationEvent in [{key}]");
            return null;
        }

        return aniEvent;
    }
    public bool IsAttribute(GlobalEnum.eActionAttribute attr)
    {
        return Attr.IsAttr(attr);
    }
}
