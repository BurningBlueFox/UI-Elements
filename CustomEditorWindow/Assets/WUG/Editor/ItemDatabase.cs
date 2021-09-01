using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemDatabase : EditorWindow
{
    private const string PATH_UXML = "Assets/WUG/Editor/ItemDatabase.uxml";
    private const string PATH_USS = "Assets/WUG/Editor/ItemDatabase.uss";
    private const string PATH_ICON = "Assets/WUG/Sprites/UnknownIcon.png";

    private Sprite defaultIcon;

    [MenuItem("WUG/Item database")]
    public static void Init()
    {
        ItemDatabase wnd = GetWindow<ItemDatabase>();
        wnd.titleContent = new GUIContent("Item Database");

        Vector2Int size = new Vector2Int(800, 475);
        wnd.minSize = size;
    }

    public void CreateGUI()
    {
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(PATH_UXML);
        VisualElement rootFromUxml = visualTree.Instantiate();
        rootVisualElement.Add(rootFromUxml);

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(PATH_USS);
        rootVisualElement.styleSheets.Add(styleSheet);

        defaultIcon = AssetDatabase.LoadAssetAtPath<Sprite>(PATH_ICON);
    }
}
