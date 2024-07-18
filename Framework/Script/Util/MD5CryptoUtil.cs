using System.Security.Cryptography;
using System.Text;

public static class MD5CryptoUtil
{
    private const string ENCODE_KEY = "Chocolate";
    
    /// <summary> 암호화 </summary>
    public static string EncodeMD5(string value)
    {        
        if (string.IsNullOrEmpty(value))
            return "";
        
        MD5 md5Hash = new MD5CryptoServiceProvider();
        byte[] secret = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(ENCODE_KEY));
        byte[] bytes = Encoding.UTF8.GetBytes(value);

        TripleDES des = new TripleDESCryptoServiceProvider();
        des.Key = secret;
        des.Mode = CipherMode.ECB;
        ICryptoTransform xform = des.CreateEncryptor();
        byte[] encrypted = xform.TransformFinalBlock(bytes, 0, bytes.Length);

        return System.Convert.ToBase64String(encrypted);
    }

    /// <summary> 복호화 </summary>
    public static string DecodeMD5(string value)
    {
        if (string.IsNullOrEmpty(value))
            return "";

        MD5 md5Hash = new MD5CryptoServiceProvider();
        byte[] secret = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(ENCODE_KEY));

        byte[] bytes = System.Convert.FromBase64String(value);
        TripleDES des = new TripleDESCryptoServiceProvider();
        des.Key = secret;
        des.Mode = CipherMode.ECB;
        ICryptoTransform xform = des.CreateDecryptor();
        byte[] decrypted = xform.TransformFinalBlock(bytes, 0, bytes.Length);

        return Encoding.UTF8.GetString(decrypted);
    }
}
