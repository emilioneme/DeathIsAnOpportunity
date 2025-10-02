using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Health))]
public class HealthEditor : Editor
{
    SerializedProperty maxHealthProp, startAtMaxProp, invulnProp, clampOverhealProp, currentHealthProp;
    SerializedProperty onHealthChangedProp, onDamagedProp, onHealedProp, onDeathProp;

    void OnEnable()
    {
        maxHealthProp        = serializedObject.FindProperty("maxHealth");
        startAtMaxProp       = serializedObject.FindProperty("startAtMax");
        invulnProp           = serializedObject.FindProperty("invulnerable");
        clampOverhealProp    = serializedObject.FindProperty("clampOverheal");
        currentHealthProp    = serializedObject.FindProperty("currentHealth");

        onHealthChangedProp  = serializedObject.FindProperty("OnHealthChanged");
        onDamagedProp        = serializedObject.FindProperty("OnDamaged");
        onHealedProp         = serializedObject.FindProperty("OnHealed");
        onDeathProp          = serializedObject.FindProperty("OnDeath");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(maxHealthProp);
        EditorGUILayout.PropertyField(startAtMaxProp);
        EditorGUILayout.PropertyField(invulnProp, new GUIContent("Invulnerable"));
        EditorGUILayout.PropertyField(clampOverhealProp);

        // === Current Health: Bar + Slider ===
        EditorGUILayout.Space(6);
        float max = Mathf.Max(1f, maxHealthProp.floatValue);
        float current = Mathf.Clamp(currentHealthProp.floatValue, 0f, max);

        Rect rect = GUILayoutUtility.GetRect(18, 20);
        EditorGUI.ProgressBar(rect, current / max, $"Health: {current:0.#} / {max:0.#}");
        EditorGUILayout.Space(4);

        EditorGUI.BeginChangeCheck();
        current = EditorGUILayout.Slider(new GUIContent("Current Health"), current, 0f, max);
        if (EditorGUI.EndChangeCheck())
            currentHealthProp.floatValue = current;

        // Quick test buttons
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("-10")) currentHealthProp.floatValue = Mathf.Max(0f, currentHealthProp.floatValue - 10f);
        if (GUILayout.Button("-1"))  currentHealthProp.floatValue = Mathf.Max(0f, currentHealthProp.floatValue - 1f);
        if (GUILayout.Button("+1"))  currentHealthProp.floatValue = Mathf.Min(max, currentHealthProp.floatValue + 1f);
        if (GUILayout.Button("+10")) currentHealthProp.floatValue = Mathf.Min(max, currentHealthProp.floatValue + 10f);
        if (GUILayout.Button("Full Heal")) currentHealthProp.floatValue = max;
        if (GUILayout.Button("Kill")) currentHealthProp.floatValue = 0f;
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(6);
        EditorGUILayout.PropertyField(onHealthChangedProp);
        EditorGUILayout.PropertyField(onDamagedProp);
        EditorGUILayout.PropertyField(onHealedProp);
        EditorGUILayout.PropertyField(onDeathProp);

        serializedObject.ApplyModifiedProperties();
    }
}

