using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public partial class SkillEffectManager : BaseTemplateManager<SkillEffectManager>
{
    private Dictionary<string, ObjectPoolUtil<SkillEffectObject>> _effectPools = new Dictionary<string, ObjectPoolUtil<SkillEffectObject>>();
    
    public async UniTaskVoid ProcessAttachEffect(GameCharacter target, SkillEffectData data)
    {
        if (target == null)
            return;
        if (data == null)
            return;

        await _ProcessAttachEffect(target, data);
    }

    public async UniTaskVoid ProcessAttachEffect(GameCharacter target, SkillEffectData data, Callback_SkillEffectObject callback)
    {
        if (target == null)
            return;
        if (data == null)
            return;

        var skillEffectObject = await _ProcessAttachEffect(target, data);
        callback?.Invoke(skillEffectObject);
    }

    private async UniTask<SkillEffectObject> _ProcessAttachEffect(GameCharacter target, SkillEffectData data)
    {
        SkillEffectObject effectObject = null;
        var attachTransform = target.transform;
        if (string.IsNullOrEmpty(data.Bone) == false)
        {
            attachTransform = target.GetBone(data.Bone);
            if (attachTransform != null)
                effectObject = await CreateEffect(data.EffectPrefab, attachTransform, data.AddSortingOrder, data.IsFollowBone, data.MaxPoolSize);
        }

        if (effectObject == null)
        {
            Vector3 pos = target.SelfTransform.position;
            Vector3 offset = data.Offset;
            Quaternion rot = DataUtil.GetApplyRotationPos(target.GetForward(), ref offset);
            pos += offset;

            effectObject = await CreateEffect(data.EffectPrefab, pos, rot, scale: 1f, addLayer:data.AddSortingOrder);
            if (data.IsFollowBone)
            {
                effectObject.SelfTransform.parent = attachTransform;
                effectObject.SelfTransform.localPosition = data.Offset;
            }
        }

        return effectObject;
    }
    async UniTask<SkillEffectObject> CreateEffect(string effectName, Transform attachTransform, int addLayer, bool isFollowBone = false, int maxPoolSize = 10)
    {
        SkillEffectObject effectObject = await CreateEffect(effectName, attachTransform.position, Quaternion.identity, 1f, addLayer);
        if (isFollowBone)
        {
            effectObject.SelfTransform.SetParent(attachTransform);
            effectObject.SelfTransform.localPosition = Vector3.zero;
            effectObject.SelfTransform.localRotation = Quaternion.identity;
        }
        else
        {
            effectObject.SelfTransform.SetParent(GlobalUtil.EffectObjectRoot.transform);
        }

        return effectObject;
    }

    public async UniTask<SkillEffectObject> CreateEffect(string effectName, Vector3 position, Quaternion rotation, float scale, int addLayer, int maxPoolSize = 10)
    {
        if (_effectPools.TryGetValue(effectName, out var effectPool) == false)
        {
            effectPool = new ObjectPoolUtil<SkillEffectObject>();
            await effectPool.Init(GlobalUtil.EffectObjectRoot.transform, effectName, maxPoolSize);
            _effectPools.Add(effectName, effectPool);
        }

        if (effectPool == null)
        {
            DebugManager.LogError($"NullReferenceException: Effect Pool ({effectName}) is Null");
            return null;
        }

        SkillEffectObject effectObject = await effectPool.New();
        effectObject.PoolName = effectName;
        effectObject.AddLayer = addLayer;

        return CreateEffect(effectObject, position, rotation, scale);
    }

    SkillEffectObject CreateEffect(SkillEffectObject effectObject, Vector3 position, Quaternion rotation, float scale = 1.0f)
    {
        if (effectObject == null)
            return null;

        effectObject.SelfTransform.position = position;
        effectObject.SelfTransform.rotation = rotation;
        effectObject.transform.localScale = new Vector3(scale, scale, scale);
        
        effectObject.Create();
        return effectObject;
    }
    
    public static void Remove(SkillEffectObject effectObject)
    {
        if (effectObject != null)
        {
            if (Instance._effectPools.TryGetValue(effectObject.PoolName, out var findPool) == false)
                return;
            
            findPool.Remove(effectObject);
        }
    }
    
    public void Clear()
    {
        _effectPools.Clear();
    }
}
