using Unity.VisualScripting;
using UnityEngine;

public class UI_Tab_Button : MonoBehaviour
{
    private UI_Button_Animation_Base _animation;

    public UI_Tab_Button BindAnimation(GlobalEnum.eUIAnimation type)
    {
        switch (type)
        {
            case GlobalEnum.eUIAnimation.Lobby_Tab:
                var tabLayout = gameObject.GetOrAddComponent<UI_Button_Animation_LobbyTab>();
                tabLayout.Init(new Vector2(2.2f, -1.0f), new Vector2(1.0f, -1.0f), 0.2f);
                _animation = tabLayout;
                break;
            default:
                DebugManager.LogError($"Not Include BindAnimation Type ({type})");
                return this;
        }
        return this;
    }
    public void SetState(bool on)
    {
        if(on)
            _animation.DoEnter();
        else
            _animation.DoExit();
    }
}
