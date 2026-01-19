using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Tab_Group : MonoBehaviour
{
    public int selectedIndex = -1;
    // Dictionary<버튼 이름, 인덱스>
    private List<UI_Tab_Button> _listTabButton = new List<UI_Tab_Button>();

    private Callback_UI _callbackClick;
    public void AddButton(UI_Button button)
    {
        if (!button)
            return;

        button.BindEvent(_OnClickTab, GlobalEnum.eUIEvent.Click, GlobalEnum.eSound.Sound_Button_Tab.ToString());
        var tab = button.GetOrAddComponent<UI_Tab_Button>();
        _listTabButton.Add(tab);
    }

    /// <summary> 최초 선택되어 있는 탭 Index 설정 및 상태 설정, 별도의 콜백은 호출하지 않음 </summary>
    public void Init(int defaultIndex)
    {       
        if (selectedIndex == defaultIndex)
            return;
        
        selectedIndex = defaultIndex;
        _RefreshTabButton();
    }

    public void BindAnimation(GlobalEnum.eUIAnimation type)
    {
        foreach (var button in _listTabButton)
        {
            if (button)
                button.BindAnimation(type);
        }
    }

    public void BindEvent(Callback_UI callback)
    {
        _callbackClick = callback;
    }

    private void _OnClickTab(PointerEventData action)
    {
        var clickName = action.pointerClick.name;
        var index = _listTabButton.FindIndex(x=>clickName.Equals(x.name));
        if (index < 0)
            return;
        
        //선택된 탭 인덱스를 Group이 저장하여 사용
        selectedIndex = index;
        _callbackClick?.Invoke(action);
        _RefreshTabButton();
    }

    private void _RefreshTabButton()
    {
        for (int i = 0; i < _listTabButton.Count; i++)
        {
            var tabButton = _listTabButton[i];
            if (!tabButton)
                continue;

            tabButton.SetState(i == selectedIndex);
        }
    }
}
