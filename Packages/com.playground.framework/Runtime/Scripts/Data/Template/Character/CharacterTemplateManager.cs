using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class CharacterTemplateManager
{
    public CharacterTemplateData GetTemplate(string key)
    {
        _dataDictionary.TryGetValue(key, out var data);
        return data;
    }
}
