using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

public class CharacterInstanceData
{
    public ulong Uid { get { return _uid; } set { _uid = value; } }
    public string Key { get { return _key; } set { _key = value; } }
    public int Level { get { return _level; } set { _level = value; } }
    
    private ulong _uid;
    private string _key;
    private int _level;

    public static CharacterInstanceData Load(JSONNode node)
    {
        if (node == null)
            return null;

        CharacterInstanceData data = new CharacterInstanceData();
        data._key = JsonLoader.GetString(node, GlobalString.Key);

        return data;
    }

    public stCharacterInfoParam MakeInfoParam()
    {
        return new stCharacterInfoParam()
        {
            Key = _key,
            Level = _level,
            Uid = _uid,
        };
    }
}
