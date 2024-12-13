using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Lobby_Bottom : BaseUI
{
    enum eUITab
    {
        Bottom,
    }
    enum eUIButton
    {
        BottomButton_1,
        BottomButton_2,
        BottomButton_3,
        BottomButton_4,
        BottomButton_5
    }

    private readonly int _defaultIndex = 2; //디폴트로 선택되는 탭 인덱스
    public override void Init()
    {
        Bind<UI_Button>(typeof(eUIButton));
        Bind<UI_Tab_Group>(typeof(eUITab));

        _InitButton();
        _Refresh();
    }

    private void _InitButton()
    {
        var tab = Get<UI_Tab_Group>((int)eUITab.Bottom);
        if (!tab)
            return;

        foreach (eUIButton buttonType in Enum.GetValues(typeof(eUIButton)))
        {
            var button = Get<UI_Button>((int)buttonType);
            if (!button)
                continue;

            button.SetLabel_Inspector().SetIcon($"Icon_Lobby{buttonType.ToString()}");
            tab.AddButton(button);
        }
        
        tab.BindEvent(_OnClickTab);
        tab.BindAnimation(GlobalEnum.eUIAnimation.Lobby_Tab);
        tab.Init(_defaultIndex);
    }

    private void _Refresh()
    {
        
    }

    private void _OnClickTab(PointerEventData action)
    {
        _Refresh();
    }

    public override void OnBack()
    {   //뒤로가기 누를 경우 디폴트 탭으로 변경
        var tab = Get<UI_Tab_Group>((int)eUITab.Bottom);
        if (!tab || tab.selectedIndex == _defaultIndex)
            return;

        tab.Init(_defaultIndex);
        _Refresh();
    }
}
