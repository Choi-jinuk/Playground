using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class SkillEffectObject : BaseObject, IPoolComponent
{
    [HideInInspector]
    public string PoolName = string.Empty;
    [HideInInspector]
    public int AddLayer = 0;

    public float ListTime { get { return _lifeTime; } }
    private float _lifeTime = 0.0f;
    protected EffectSortingOrderProcessor _SortingOrder = new EffectSortingOrderProcessor();

    private bool _isLoop;

    public virtual void Create()
    {
        ParticleSystem[] particles = SelfObject.GetComponentsInChildren<ParticleSystem>();
        if (particles.Length == 0)
            return;

        _lifeTime = 0f;
        foreach (var particle in particles)
        {
            if (particle.main.loop)
            {
                _isLoop = true;
            }

            if (particle.main.duration > _lifeTime)
            {
                _lifeTime = particle.main.duration;
            }
        }
        _lifeTime += 0.3f;

        if (_isLoop == false)
            RemoveWaitForTime().Forget();
        
        _SortingOrder.Init(particles, SelfTransform.position.y, AddLayer);
    }

    private async UniTaskVoid RemoveWaitForTime()
    {
        if (_lifeTime == 0f)
        {
            return;
        }

        await UniTask.WaitForSeconds(_lifeTime);
        Remove();
    }

    void Remove()
    {
        SkillEffectManager.Remove(this);
    }

    public void UpdateSortingOrder(float y)
    {
        _SortingOrder.UpdateSortingOrder(y);
    }

    public void SetData()
    {
        
    }

    public void ReleaseData()
    {
        
    }

    public void ClearData()
    {
        
    }
}
