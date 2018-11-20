using UnityEngine;
using System.Collections;

using UnityEngine;
using UnityEditor;

public class ShowPopup : EditorWindow
{
    [MenuItem("Example/ShowPopup Example")]
    static void Init()
    {
        ShowPopup window = ScriptableObject.CreateInstance<ShowPopup>();
        window.position = new Rect(Screen.width / 2, Screen.height / 2, 250, 150);
        window.ShowPopup();
    }

    void OnGUI()
    {
        EditorGUILayout.LabelField("This is an example of EditorWindow.ShowPopup", EditorStyles.wordWrappedLabel);
        GUILayout.Space(70);
        if (GUILayout.Button("Agree!")) this.Close();
    }
}
