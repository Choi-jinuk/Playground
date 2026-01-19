using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

public class SkillEffectData
{
    public string Key { get { return _key; } }
    public string EffectPrefab { get { return _effectPrefab; } }
    public string Bone { get { return _bone; } }
    public bool IsFollowBone { get { return _isFollowBone; } }
    public int AddSortingOrder { get { return _addSortingOrder; } }
    public Vector3 Offset { get { return _offSet; } }
    public int MaxPoolSize { get { return _maxPoolSize; } }
    
    private string _key;
    private string _effectPrefab;
    private string _bone;
    private bool _isFollowBone;
    private int _addSortingOrder;
    private Vector3 _offSet;
    private int _maxPoolSize;

    public static SkillEffectData Load(JSONNode node)
    {
        if (node == null)
            return null;

        SkillEffectData data = new SkillEffectData();
        data._key = JsonLoader.GetString(node, GlobalString.Key);
        data._effectPrefab = JsonLoader.GetString(node, GlobalString.EffectPrefab);
        data._bone = JsonLoader.GetString(node, GlobalString.Bone);
        data._isFollowBone = JsonLoader.GetBool(node, GlobalString.IsFollowBone);
        data._addSortingOrder = JsonLoader.GetInteger(node, GlobalString.AddSortingOrder);
        data._offSet = JsonLoader.GetVector3(node, GlobalString.Offset);
        data._maxPoolSize = JsonLoader.GetInteger(node, GlobalString.MaxPoolSize);
        
        return data;
    }
}
