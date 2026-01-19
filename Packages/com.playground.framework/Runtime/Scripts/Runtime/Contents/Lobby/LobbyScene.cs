using UnityEngine;

public class LobbyScene : MonoBehaviour
{
    private void Start()
    {
        UIManager.Instance.MakeUI(GlobalEnum.ePriorityType.Priority_3, GlobalEnum.eUIType.UI_Lobby_Bottom);
    }
}
