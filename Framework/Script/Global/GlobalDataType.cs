
//======================= Delegate =======================//
public delegate void Callback();
public delegate void Callback_string(string s1);
public delegate void Callback_int(int i1);
public delegate void Callback_float(float f1);
public delegate void Callback_param1(System.Object p1);
public delegate void Callback_SkillEffectObject(SkillEffectObject eo);

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