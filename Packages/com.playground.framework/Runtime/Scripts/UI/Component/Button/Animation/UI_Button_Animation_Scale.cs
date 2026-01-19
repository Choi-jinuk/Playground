using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class UI_Button_Animation_Scale : UI_Button_Animation_Base
{
    private CancellationTokenSource _token;

    private float _from;    //시작지점
    private float _to;      //목표지점
    private float _duration;//시간

    private float _originalScale;
    
    /// <param name="from">시작 지점 (%)</param>
    /// <param name="to">목표 지점 (%)</param>
    /// <param name="duration">시간 (s)</param>
    public void Init(float from, float to, float duration)
    {
        _from = from;
        _to = to;
        _duration = duration;
        _originalScale = SelfTransform.localScale.x;    //x,y,z 를 같은 값으로 유지
    }
    public override void DoEnter()
    {
        if (_IsEndValue(_to))
            return;
        
        _token?.Cancel();
        _token = new CancellationTokenSource();

        SelfTransform.DOComplete();
        SelfTransform.DOScale(_to, _duration).SetUpdate(true);
    }

    public override void DoExit()
    {
        if (_IsEndValue(_from))
            return;
        
        _token?.Cancel();
        _token = new CancellationTokenSource();

        SelfTransform.DOComplete();
        SelfTransform.DOScale(_from, _duration).SetUpdate(true);
    }
    /// <summary> 현재 값이 목표 값과 같은지 비교 </summary>
    private bool _IsEndValue(float endValue)
    {
        var endScale = _originalScale * endValue;
        return Mathf.Approximately(SelfTransform.localScale.x, endScale);
    }
}
