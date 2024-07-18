using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class AniEventDataElement : IPoolComponent
{
    public bool Done { get { return _done; } set { _done = value; } }
    public GlobalEnum.eAnimationEventType Type { get { return _type; } set { _type = value; } }

    public float Time;
    public string EventType;
    public int Index = 0;

    private bool _done = false;
    private GlobalEnum.eAnimationEventType _type = 0;
    
    public void SetData() { }
    public void ReleaseData() { }
    public void ClearData() { }
}

public class AniEventData
{
    public string Animation;
    public List<AniEventDataElement> Events;

    public void Clear()
    {
        Animation = null;
        Events = null;
    }
}

public class CharacterAnimEventData
{
    public string Key;
    public List<AniEventData> DataList;
}
public class ActionManager : Singleton<ActionManager>
{
    public Dictionary<string, List<AniEventDataElement>> CommonAniEvents { get { return _commonAniEvents; } }
    public Dictionary<string, CharacterAnimEventData> AniEventDictionary { get { return _aniEventDictionary; } }

    private Dictionary<string, List<AniEventDataElement>> _commonAniEvents = new Dictionary<string, List<AniEventDataElement>>();
    private Dictionary<string, CharacterAnimEventData> _aniEventDictionary = new Dictionary<string, CharacterAnimEventData>();
    private Dictionary<string, ActionChartData> _actionChartDictionary = new Dictionary<string, ActionChartData>();

    public async UniTaskVoid Init()
    {
        _aniEventDictionary.Clear();
        _actionChartDictionary.Clear();
        _commonAniEvents.Clear();

        _actionChartDictionary = await ActionXMLLoader.LoadAction(GlobalString.ACTION_CHART_PATH, _actionChartDictionary);
        var result = await ActionXMLLoader.LoadAniEvents(GlobalString.ANI_EVENT_PATH, 
            new stLoadAnimEventParam() { AniEventDictionary = _aniEventDictionary, CommonEvents = _commonAniEvents });

        _aniEventDictionary = result.AniEventDictionary;
        _commonAniEvents = result.CommonEvents;
    }

    public ActionChartData GetActionChartData(string chartName)
    {
        if (string.IsNullOrEmpty(chartName))
            return null;
        
        if (_actionChartDictionary.TryGetValue(chartName.ToLower(), out var data) == false)
        {
            DebugManager.LogError($"Not Exist Action Chart - {chartName}");
            return null;
        }

        return data;
    }
}
