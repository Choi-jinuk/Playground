
using System;

public class EnumUtil
{
    public static bool TryParse<T>(string value, out T result) where T : struct, Enum
    {
        if (string.IsNullOrEmpty(value))
        {
            result = default;
            return false;
        }
        
        return Enum.TryParse(value, out result);
    }
}
