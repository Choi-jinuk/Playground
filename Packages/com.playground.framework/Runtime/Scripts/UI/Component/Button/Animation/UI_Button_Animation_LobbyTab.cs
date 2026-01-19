using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_Button_Animation_LobbyTab : UI_Button_Animation_Base
{
    private LayoutElement _layout;
    private UI_Button _button;

    private Vector2 _enter;
    private Vector2 _exit;
    private float _duration;
    
    /// <param name="enter"> FlexibleSize 조절, x:Width, y:Height, default 값 -1 </param>
    /// <param name="exit"></param>
    /// <param name="duration"></param>
    public void Init(Vector2 enter, Vector2 exit, float duration)
    {
        _layout = SelfObject.GetOrAddComponent<LayoutElement>();
        _button = SelfObject.GetComponent<UI_Button>();
        _enter = enter;
        _exit = exit;
        _duration = duration;
    }
    public override void DoEnter()
    {
        _layout.DOComplete();
        _layout.DOFlexibleSize(_enter, _duration).SetUpdate(true);

        _SetButtonState(true);
    }

    public override void DoExit()
    {
        _layout.DOComplete();
        _layout.DOFlexibleSize(_exit, _duration).SetUpdate(true);

        _SetButtonState(false);
    }

    private void _SetButtonState(bool enter)
    {
        if (!_button)
            return;
        
        var icon = _button.Icon;
        if (icon)
        {
            if (enter)
                icon.transform.localPosition += Vector3.up * 50;
            else
                icon.transform.localPosition = Vector3.zero;
        }

        var label = _button.Label;
        if (label)
        {
            label.gameObject.SetActive(enter);
        }
    }
}
