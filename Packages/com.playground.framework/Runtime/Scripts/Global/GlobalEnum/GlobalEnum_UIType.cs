public static partial class GlobalEnum
{
    /// <summary> 로드할 Atlas 이름 </summary>
    public enum eUIAtlasName
    {
        Icon_Atlas,
        Count,
    }
    /// <summary> UI 이름 등록 </summary>
    public enum eUIType
    {
        UI_Lobby_Bottom,
    }
    /// <summary> 각 UI Root Priority 타입 </summary>
    public enum ePriorityType
    {
        Priority_1,
        Priority_2,
        Priority_3,
        Priority_4,
        Priority_5,
        Priority_6,
        Count,
    }
    /// <summary> UI Button 이벤트 바인딩 종류 </summary>
    public enum eUIEvent
    {
        Click,
        Down,
        Up,
        Drag,
    }

    public enum eUIAnimation
    {
        Scale,
        Lobby_Tab,
    }
}