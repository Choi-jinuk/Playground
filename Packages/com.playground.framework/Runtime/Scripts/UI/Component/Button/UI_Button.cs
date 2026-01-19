using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public partial class UI_Button : Selectable
{
    [Header("UI Inspecter Data")]
    [SerializeField]
    private string _labelName;
    [SerializeField]
    private string _soundName;

    private Image _icon;
    private TMPro.TextMeshProUGUI _label;
    private Callback_UI _action;
    private UI_Button_Animation_Base _animation;
    
    public Image Icon { get => _icon; }
    public TMPro.TextMeshProUGUI Label { get => _label; }
    
    public string GetInspectorLabelName()
    {
        return _labelName;
    }
    public string GetInspectorSoundName()
    {
        return _soundName;
    }

    protected override void Awake()
    {
        base.Awake();
        _icon = GameObjectUtil.FindChild<Image>(gameObject, StringUtil.Append(gameObject.name, "_Icon"));
        _label = GameObjectUtil.FindChild<TMPro.TextMeshProUGUI>(gameObject, StringUtil.Append(gameObject.name, "_Label"));
    }

    /// <summary> 버튼 안의 텍스트 인스펙터에 설정한 값으로 설정, Builder Pattern 적용 </summary>
    public UI_Button SetLabel_Inspector()
    {
        SetLabel(_labelName);

        return this;
    }
    /// <summary> 버튼 안의 텍스트 설정, Builder Pattern 적용 </summary>
    public UI_Button SetLabel(string key)
    {
        if (_label)
            _label.text = StringTableManager.Instance.GetString(key);

        return this;
    }
    /// <summary> 버튼 안의 아이콘 설정, Builder Pattern 적용 </summary>
    public UI_Button SetIcon(string key)
    {
        if (_icon)
        {
            _icon.sprite = UIManager.Instance.GetAtlasIconSprite(GlobalEnum.eUIAtlasName.Icon_Atlas, key);
            _icon.enabled = _icon.sprite;
        }

        return this;
    }

    public UI_Button BindAnimation(GlobalEnum.eUIAnimation type)
    {
        switch (type)
        {
            case GlobalEnum.eUIAnimation.Scale:
                var scale = gameObject.GetOrAddComponent<UI_Button_Animation_Scale>();
                scale.Init(1.0f, 0.9f, 0.5f);
                _animation = scale;
                break;
            default:
                DebugManager.LogError($"Not Include BindAnimation Type ({type})");
                return this;
        }
        return this;
    }
    protected override void DoStateTransition(SelectionState state, bool instant)
    {
        base.DoStateTransition(state, instant);
        switch (state)
        {
            case SelectionState.Highlighted:
            case SelectionState.Pressed:
            case SelectionState.Selected:
                _animation?.DoEnter();
                break;
            case SelectionState.Normal:
            case SelectionState.Disabled:
            default:
                _animation?.DoExit();
                break;
        }
    }
    /// <summary> 버튼 이벤트 바인딩, Builder Pattern 적용 </summary>
    public UI_Button BindEvent(Callback_UI action, GlobalEnum.eUIEvent type, string soundName)
    {
        UI_Button_Base button;
        switch (type)
        {
            case GlobalEnum.eUIEvent.Click:
            {
                button = gameObject.GetOrAddComponent<UI_Button_Click>();
                break;
            }
            case GlobalEnum.eUIEvent.Drag:
            {
                button = gameObject.GetOrAddComponent<UI_Button_Drag>();
                break;
            }
            case GlobalEnum.eUIEvent.Down:
            {
                button = gameObject.GetOrAddComponent<UI_Button_Down>();
                break;
            }
            case GlobalEnum.eUIEvent.Up:
            {
                button = gameObject.GetOrAddComponent<UI_Button_Up>();
                break;
            }
            default:
                DebugManager.LogError($"Not Include BindEvent Type ({type})");
                return this;
        }

        _action = action;
        _soundName = soundName;
        
        button.onEventHandler -= _OnEvent;
        button.onEventHandler += _OnEvent;

        return this;
    }

    private void _OnEvent(PointerEventData action)
    {
        if (!IsActive() || !IsInteractable())
            return;
        
        _action?.Invoke(action);
        SoundManager.Instance.Play(_soundName);
        
        DoStateTransition(SelectionState.Pressed, false);
        StartCoroutine(OnFinishSubmit());
    }
    private IEnumerator OnFinishSubmit()
    {
        var fadeTime = colors.fadeDuration;
        var elapsedTime = 0f;

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        DoStateTransition(currentSelectionState, false);
    }
}
