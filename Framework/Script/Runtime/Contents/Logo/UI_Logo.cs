using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_Logo : BaseUI
{
    private enum Images
    {
        LogoTitle,
    }

    public override void Init()
    {
        Bind<Image>(typeof(Images));
    }

    IEnumerator Start()
    {
        var logoImage = GetImage((int)Images.LogoTitle);
        if (logoImage != null)
        {   //Splash Image
            var color = logoImage.color;
            while (logoImage.color.a < 1f)
            {
                color.a -= Time.deltaTime;
                logoImage.color = color;
                yield return null;
            }
        }
        
        
    }

    public override void OnBack()
    {
        
    }
}
