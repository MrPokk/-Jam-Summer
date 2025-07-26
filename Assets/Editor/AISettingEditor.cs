#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AISetting))]
public class AISettingEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Отрисовка стандартных полей
        DrawDefaultInspector();

        AISetting setting = (AISetting)target;
        GUILayout.Space(10);

        // Кнопка синхронизации
        if (GUILayout.Button("Sync Card Weights", GUILayout.Height(30)))
        {
            setting.SyncCardWeights();
        }

        // Подсказка
        if (setting.CardList == null || setting.KeepProportions == null)
        {
            EditorGUILayout.HelpBox(
                "Assign CardList and KeepProportions first!",
                MessageType.Warning
            );
        }
    }
}
#endif
