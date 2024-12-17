using UnityEngine;
using UnityEditor;

public class Hotkeys : MonoBehaviour
{
    [MenuItem("Hotkeys/Prefs/Clear Prefs &#c")]
    private static void DeleteSaves() => PlayerPrefs.DeleteAll();

    [MenuItem("Hotkeys/Set/Active Object #a")]
    private static void SelectObjectActive()
    {
        if (Selection.activeObject != null)
            Selection.activeGameObject.SetActive(true);
    }

    [MenuItem("Hotkeys/Set/Disactive Object #q")]
    private static void SelectObjectDisactive()
    {
        if (Selection.activeObject != null)
            Selection.activeGameObject.SetActive(false);
    }
}
