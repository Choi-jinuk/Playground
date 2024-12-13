using TMPro;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public partial class UI_Button
{
    #if UNITY_EDITOR
    [UnityEditor.MenuItem("GameObject/UI/UI_Button", false, -10)]
    public static void MakeButton(UnityEditor.MenuCommand menuCommand)
    {
        Transform parent;
        if (UnityEditor.Selection.gameObjects.Length == 0)
        {
            var parentObject = menuCommand.context as GameObject;
            if (parentObject == null || parentObject.GetComponentInParent<Canvas>() == null)
            {
                return;
            }
            parent = parentObject.transform;
        }
        else
            parent = UnityEditor.Selection.gameObjects[0].transform;

        var gameObject = new GameObject("Button");
        gameObject.AddComponent<Image>();
        gameObject.AddComponent<UI_Button>();
        gameObject.transform.SetParent(parent, false);

        var childObject = new GameObject("Text (TMP)");
        var text = childObject.AddComponent<TMPro.TextMeshProUGUI>();
        childObject.transform.SetParent(gameObject.transform, false);
        _TextSetting(text);
        
        UnityEditor.Undo.RegisterCreatedObjectUndo(gameObject, "Create UI_Button");
        UnityEditor.Selection.activeGameObject = gameObject;
    }

    private static void _TextSetting(TMPro.TextMeshProUGUI text)
    {
        text.text = "Text";
        text.color = Color.black;
        text.enableAutoSizing = true;
        text.rectTransform.anchorMax = Vector2.one;
        text.rectTransform.anchorMin = Vector2.zero;
        text.rectTransform.offsetMax = Vector2.zero;
        text.rectTransform.offsetMin = Vector2.zero;

        text.alignment = TextAlignmentOptions.Center;
    }
    #endif
}
