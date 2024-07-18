public class Singleton<T> where T : class, new()
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
