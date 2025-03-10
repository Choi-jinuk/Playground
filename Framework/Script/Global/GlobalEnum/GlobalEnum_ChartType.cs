public static partial class GlobalEnum
{
    /// <summary> ActionChartData 진행에 필요한 데이터 타입  </summary>
    public enum eActionAttribute
    {
        None = 0,
        Move = 1 << 0,          //이동 액션
        Attack = 1<<1,          //공격 액션
        Skill = 1<<2,           //스킬 액션
    }

    /// <summary> 애니메이션 이벤트 키 </summary>
    public enum eAnimationEventType
    {
        Attack,
        Effect,
        CameraShake,
        Sound,
        Indicator,  //공격 범위 표시
    }
    /// <summary> 애니메이션 이벤트 그룹 키 </summary>
    public enum eAnimationGroupEventType
    {
        OnHit,
        OnHitTarget,
        OnEndProjectile,
        
        Count,
    }

    public enum eActionChangeResult
    {
        Success,
        Loop,
        Failed,
    }
}