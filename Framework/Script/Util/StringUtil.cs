using Cysharp.Text;
public static class StringUtil
{
    public static string Append( params string[] arrStr )
    {
        using var builder = ZString.CreateStringBuilder(true);
        
        if (builder.Length > 0)
            builder.Remove(0, builder.Length);

        var length = arrStr.Length;
        for (int i = 0; i < length; ++i)
        {
            if (string.IsNullOrEmpty(arrStr[i]))
                continue;

            builder.Append(arrStr[i]);
        }

        string str = builder.ToString();
        builder.Remove(0, builder.Length);
        return str;
    }
    public static string AppendLine( params string[] arrStr )
    {
        using var builder = ZString.CreateStringBuilder(true);
        
        if (builder.Length > 0)
            builder.Remove(0, builder.Length);

        var length = arrStr.Length;
        for (int i = 0; i < length; ++i)
        {
            if (string.IsNullOrEmpty(arrStr[i]))
                continue;

            builder.AppendLine(arrStr[i]);
        }

        string str = builder.ToString();
        builder.Remove(0, builder.Length);
        return str;
    }
}