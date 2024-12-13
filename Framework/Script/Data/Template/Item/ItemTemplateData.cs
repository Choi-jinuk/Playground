using SimpleJSON;
using UnityEngine;

public class ItemTemplateData
{
    public string Key { get => _key; }
    
    private string _key;

    public static ItemTemplateData Load(JSONNode node)
    {
        if (node == null)
            return null;

        var data = new ItemTemplateData();
        data._key = JsonLoader.GetString(node, GlobalString.Key);

        return data;
    }
}
