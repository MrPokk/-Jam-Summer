#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AISetting))]
public class AISettingEditor : Editor
{
    private SerializedProperty _behaviorsProperty;

    private void OnEnable()
    {
        _behaviorsProperty = serializedObject.FindProperty("Behaviors");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Рисуем стандартные свойства (кроме Behaviors)
        DrawPropertiesExcluding(serializedObject, "Behaviors");

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("AI Behaviors", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox("Behaviors are executed from top to bottom. Use buttons to reorder.", MessageType.Info);

        // Отрисовка списка Behaviors
        for (int i = 0; i < _behaviorsProperty.arraySize; i++)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            // Заголовок с кнопками перемещения
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField($"Behavior {i + 1}", EditorStyles.boldLabel, GUILayout.ExpandWidth(false));

            // Кнопка перемещения вверх
            GUI.enabled = i > 0;
            if (GUILayout.Button("↑", GUILayout.Width(20)))
            {
                _behaviorsProperty.MoveArrayElement(i, i - 1);
                GUI.FocusControl(null); // Снимаем фокус, чтобы избежать артефактов
            }

            // Кнопка перемещения вниз
            GUI.enabled = i < _behaviorsProperty.arraySize - 1;
            if (GUILayout.Button("↓", GUILayout.Width(20)))
            {
                _behaviorsProperty.MoveArrayElement(i, i + 1);
                GUI.FocusControl(null);
            }
            GUI.enabled = true;

            // Кнопка удаления
            if (GUILayout.Button("Remove", GUILayout.Width(80)))
            {
                _behaviorsProperty.DeleteArrayElementAtIndex(i);
                break;
            }

            EditorGUILayout.EndHorizontal();

            // Отображение Behavior
            DrawBehavior(_behaviorsProperty.GetArrayElementAtIndex(i), i);

            EditorGUILayout.EndVertical();
        }

        // Кнопка добавления нового Behavior
        if (GUILayout.Button("Add New Behavior", GUILayout.Width(150)))
        {
            AddNewEmptyBehavior();
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawBehavior(SerializedProperty behaviorProperty, int index)
    {
        SerializedProperty conditions = behaviorProperty.FindPropertyRelative("Conditions");
        SerializedProperty actions = behaviorProperty.FindPropertyRelative("Actions");
        SerializedProperty exit = behaviorProperty.FindPropertyRelative("ExitWhenExecuting");

        // Условия
        EditorGUILayout.LabelField("Conditions", EditorStyles.miniBoldLabel);
        DrawConditionActionsList(conditions, "Add Condition", AddConditionMenu);

        // Действия
        EditorGUILayout.LabelField("Actions", EditorStyles.miniBoldLabel);
        DrawConditionActionsList(actions, "Add Action", AddActionMenu);

        // Exit When Executing
        EditorGUILayout.PropertyField(exit);
    }

    private void DrawConditionActionsList(SerializedProperty list, string addButtonLabel, Action<SerializedProperty> addMenuAction)
    {
        EditorGUI.indentLevel++;

        for (int i = 0; i < list.arraySize; i++)
        {
            EditorGUILayout.BeginHorizontal();

            // Отображаем элемент списка
            EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i), GUIContent.none, true);

            // Кнопка удаления
            if (GUILayout.Button("Remove", GUILayout.Width(80)))
            {
                list.DeleteArrayElementAtIndex(i);
                break;
            }

            EditorGUILayout.EndHorizontal();
        }

        // Кнопка добавления
        if (GUILayout.Button(addButtonLabel, GUILayout.Width(150)))
        {
            addMenuAction(list);
        }

        EditorGUI.indentLevel--;
    }

    private void AddNewEmptyBehavior()
    {
        int index = _behaviorsProperty.arraySize;
        _behaviorsProperty.arraySize++;
        SerializedProperty newBehavior = _behaviorsProperty.GetArrayElementAtIndex(index);

        // Инициализируем пустые списки
        SerializedProperty conditions = newBehavior.FindPropertyRelative("Conditions");
        conditions.ClearArray();

        SerializedProperty actions = newBehavior.FindPropertyRelative("Actions");
        actions.ClearArray();

        SerializedProperty waitAfter = newBehavior.FindPropertyRelative("ExitWhenExecuting");
        waitAfter.boolValue = false;

        serializedObject.ApplyModifiedProperties();
    }

    private void AddConditionMenu(SerializedProperty conditions)
    {
        GenericMenu menu = new GenericMenu();

        menu.AddItem(new GUIContent("Has Enough Money"), false, () => AddCondition<HasEnoughMoneyCondition>(conditions));
        menu.AddItem(new GUIContent("Has Enough Money Buy Card"), false, () => AddCondition<HasMoneyEntityCondition>(conditions));
        menu.AddItem(new GUIContent("Has Money Build Buy Card"), false, () => AddCondition<HasMoneyBuildCondition>(conditions));
        menu.AddItem(new GUIContent("Count Card Type"), false, () => AddCondition<CountCardTypeCondition>(conditions));
        menu.AddItem(new GUIContent("Count Card In Square"), false, () => AddCondition<CountCardInSquareCondition>(conditions));

        

        menu.ShowAsContext();
    }

    private void AddActionMenu(SerializedProperty actions)
    {
        GenericMenu menu = new GenericMenu();

        menu.AddItem(new GUIContent("Spawn Card"), false, () => AddAction<SpawnCardAction>(actions));

        menu.ShowAsContext();
    }

    private void AddCondition<T>(SerializedProperty conditions) where T : AICondition, new()
    {
        int index = conditions.arraySize;
        conditions.arraySize++;
        SerializedProperty element = conditions.GetArrayElementAtIndex(index);
        element.managedReferenceValue = new T();
        conditions.serializedObject.ApplyModifiedProperties();
    }

    private void AddAction<T>(SerializedProperty actions) where T : AIAction, new()
    {
        int index = actions.arraySize;
        actions.arraySize++;
        SerializedProperty element = actions.GetArrayElementAtIndex(index);
        element.managedReferenceValue = new T();
        actions.serializedObject.ApplyModifiedProperties();
    }
}

[CustomPropertyDrawer(typeof(AICondition), true)]
[CustomPropertyDrawer(typeof(AIAction), true)]
public class ConditionActionDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.PropertyField(position, property, label, true);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }
}
#endif
