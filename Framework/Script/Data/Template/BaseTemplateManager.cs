using SimpleJSON;

public class BaseTemplateManager<T> : BaseManager where T : BaseManager, new()
{    
    private static readonly T _instance = new T();
    public static T Instance
    {
        get
        {
            return _instance;
        }
    }
}

public class BaseManager
{
    /// <returns> True: 로드 성공, False: 로드 실패 </returns>
    public bool LoadData(string textAsset)
    {
        if (string.IsNullOrEmpty(textAsset))
            return false;
        
        var jsonObject = JSON.Parse(textAsset) as JSONObject;
        if (jsonObject == null)
            return false;
        
        return _LoadData(jsonObject);
    }
    /// <returns> True: 로드 성공, False: 로드 실패 </returns>
    protected virtual bool _LoadData(JSONObject jsonObject)
    {
        return false;
    }
}
