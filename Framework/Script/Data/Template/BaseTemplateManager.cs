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
    public bool LoadData(string fileName, JSONObject jsonObject)
    {
        return _LoadData(fileName, jsonObject);
    }
    /// <returns> True: 로드 성공, False: 로드 실패 </returns>
    protected virtual bool _LoadData(string fileName, JSONObject jsonObject)
    {
        return false;
    }
}
