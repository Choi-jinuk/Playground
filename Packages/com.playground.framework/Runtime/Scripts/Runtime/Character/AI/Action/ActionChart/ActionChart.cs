

public class ActionChart
{
    public ActionData PrevAction { get { return _prevAction; } }
    public ActionData CurrentAction { get { return _currentAction; } }
    
    public ActionChartData Data { get { return _data; } }
    public ActionChartData IncludeData { get { return _includeData; } }
    
    private ActionChartData _data;
    private ActionChartData _includeData;

    private GameCharacter _owner;
    private ActionData _prevAction;
    private ActionData _currentAction;

    public ActionChart(GameCharacter owner, ActionChartData data)
    {
        _owner = owner;
        _data = data;

        if (data.IsIncludeData)
        {
            _includeData = new ActionChartData();
            data.GetIncludeData(data.IncludeAction, ref _includeData);
        }
    }

    public GlobalEnum.eActionChangeResult ChangeAction(string actionName, ActionComponent component)
    {
        if (_data == null)
        {
            DebugManager.LogError("Invalid ActionChartData");
            return GlobalEnum.eActionChangeResult.Failed;
        }

        if (_owner == null)
        {
            DebugManager.LogError($"Invalid Owner [{_data.Name}]");
            return GlobalEnum.eActionChangeResult.Failed;
        }

        ActionData action = GetActionData(actionName);
        if (action == null)
        {
            DebugManager.LogError($"{actionName} is Not Include in {_owner.name}");
            return GlobalEnum.eActionChangeResult.Failed;
        }

        _prevAction = _currentAction;
        _currentAction = action;

        if (component != null)
        {
            if (_currentAction.AnimationInfo.AnimationNameList == null)
            {
                DebugManager.LogError($"Invalid [{_currentAction.Name}] Action AnimationInfo");
                return GlobalEnum.eActionChangeResult.Failed;
            }

            string animation = _currentAction.AnimationInfo.GetAnimation();
            component.ChangeAnimationState(animation);
        }

        return GlobalEnum.eActionChangeResult.Success;

    }

    public ActionData GetActionData(string name)
    {
        ActionData action = null;
        if (_data.ActionDictionary.TryGetValue(name, out action) == false)
        {
            if (_includeData != null && _includeData.ActionDictionary.TryGetValue(name, out action) == false)
            {
                return null;
            }
        }

        return action;
    }
}
