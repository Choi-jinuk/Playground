
//======================= Delegate =======================//

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public delegate void Callback();
public delegate void Callback_string(string s1);
public delegate void Callback_int(int i1);
public delegate void Callback_float(float f1);
public delegate void Callback_param1(System.Object p1);
public delegate void Callback_SkillEffectObject(SkillEffectObject eo);
public delegate void Callback_UI(PointerEventData action);

/*  Server  */




//======================= Struct =======================//

public struct stAnimEventProcessParam
{
    public GameCharacter Target;
    public bool IsTargetEvent;
    public UnityEngine.Vector3 Position;
}

public struct stLoadAnimEventParam
{
    public System.Collections.Generic.Dictionary<string, CharacterAnimEventData> AniEventDictionary;
    public System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<AniEventDataElement>> CommonEvents;
}

public struct stDownloadData
{
    public string Label;
    public long Size;
}

public class SoundData : IPoolComponent
{
    public AudioClip Clip;
    public string Key;
    public float Time;

    public void SetData() => Clear();
    public void ReleaseData() => Clear();
    public void ClearData() => Clear();

    private void Clear()
    {
        Clip = null;
        Key = string.Empty;
        Time = 0f;
    }
}

/* Runtime */
public struct stCharacterInfoParam
{
    public string Key { get { return _key; } set { _key = value; } }
    public int Level { get { return _level; } set { _level = value; } }
    public ulong Uid { get { return _uid; } set { _uid = value; } }

    private string _key;
    private int _level;
    private ulong _uid;
}

public struct stSkillInfoParam
{
    public string Key { get { return _key; } set { _key = value; } }
    public int Level { get { return _level; } set { _level = value; } }
    public ulong Uid { get { return _uid; } set { _uid = value; } }

    private string _key;
    private int _level;
    private ulong _uid;
}

/* Stat */
public struct stStatLevel
{
    public GlobalEnum.eStatType Type { get { return _type; } set { _type = value; } }
    public int Level { get { return _level; } set { _level = value; } }
    private GlobalEnum.eStatType _type;
    private int _level;
}