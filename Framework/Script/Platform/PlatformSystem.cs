using UnityEngine;

public class PlatformSystem : Singleton<PlatformSystem>
{
    public GlobalEnum.eCountry Country = GlobalEnum.eCountry.Count;

    public void InitCountry()
    {
        var uiCulture = System.Globalization.CultureInfo.CurrentUICulture;
        switch (uiCulture.TwoLetterISOLanguageName)
        {
            case "ko":  //한국
                Country = GlobalEnum.eCountry.Korea; return;
            case "ja":  //일본
            case "zh":  //중국
            case "de":  //독일
            case "fr":  //프랑스
            case "ar":  //아랍
            case "es":  //스페인
            case "pt":  //포르투갈
            case "vi":  //베트남
            case "th":  //태국
            case "ru":  //러시아
            case "en":  //영어
            default:
                Country = GlobalEnum.eCountry.English; return;
        }
    }
}
