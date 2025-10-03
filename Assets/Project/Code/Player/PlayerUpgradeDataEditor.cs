#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerUpgradeData))]
public class PlayerUpgradeDataEditor : Editor
{
    SerializedProperty moveSpeedProp;
    SerializedProperty maxHorizontalSpeedProp;
    SerializedProperty groundAccelerationProp;
    SerializedProperty airAccelerationProp;
    SerializedProperty groundLinearDragProp;
    SerializedProperty airLinearDragProp;

    SerializedProperty jumpImpulseProp;
    SerializedProperty maxAirJumpsProp;
    SerializedProperty coyoteTimeProp;
    SerializedProperty jumpBufferProp;
    SerializedProperty fallGravityMultiplierProp;
    SerializedProperty lowJumpGravityMultiplierProp;

    SerializedProperty fireRateProp;
    SerializedProperty projectileSizeProp;
    SerializedProperty projectileDamageProp;
    SerializedProperty projectileSpeedProp;
    SerializedProperty projectileLifeProp;
    SerializedProperty projectilesPerShotProp;
    SerializedProperty projectileSpreadRadiusProp;
    SerializedProperty projectileAngleVarianceProp;

    static class Styles
    {
        internal static readonly GUIContent MovementHeader = new GUIContent("Movement");
        internal static readonly GUIContent MoveSpeed = new GUIContent("Move Speed", "Base horizontal move speed.");
        internal static readonly GUIContent MaxHorizontalSpeed = new GUIContent("Max Horizontal Speed", "Derived cap based on move speed.");
        internal static readonly GUIContent GroundAcceleration = new GUIContent("Ground Acceleration");
        internal static readonly GUIContent AirAcceleration = new GUIContent("Air Acceleration");
        internal static readonly GUIContent GroundLinearDrag = new GUIContent("Ground Linear Drag");
        internal static readonly GUIContent AirLinearDrag = new GUIContent("Air Linear Drag");

        internal static readonly GUIContent JumpingHeader = new GUIContent("Jumping");
        internal static readonly GUIContent JumpImpulse = new GUIContent("Jump Impulse", "Initial jump force.");
        internal static readonly GUIContent MaxAirJumps = new GUIContent("Max Air Jumps");
        internal static readonly GUIContent CoyoteTime = new GUIContent("Coyote Time", "Extra time to still jump after leaving ground.");
        internal static readonly GUIContent JumpBuffer = new GUIContent("Jump Buffer", "Window to cache an early jump input.");
        internal static readonly GUIContent FallGravityMultiplier = new GUIContent("Fall Gravity Multiplier");
        internal static readonly GUIContent LowJumpGravityMultiplier = new GUIContent("Low Jump Gravity Multiplier");

        internal static readonly GUIContent CombatHeader = new GUIContent("Combat");
        internal static readonly GUIContent FireRate = new GUIContent("Fire Rate", "Shots per second.");
        internal static readonly GUIContent ProjectileSize = new GUIContent("Projectile Size", "Derived from projectiles per shot.");
        internal static readonly GUIContent ProjectileDamage = new GUIContent("Projectile Damage");
        internal static readonly GUIContent ProjectileSpeed = new GUIContent("Projectile Speed");
        internal static readonly GUIContent ProjectileLife = new GUIContent("Projectile Life", "Lifetime of spawned projectile.");
        internal static readonly GUIContent ProjectilesPerShot = new GUIContent("Projectiles Per Shot");
        internal static readonly GUIContent ProjectileSpreadRadius = new GUIContent("Projectile Spread", "Derived radius based on projectile count.");
        internal static readonly GUIContent ProjectileAngleVariance = new GUIContent("Projectile Angle Variance", "Randomized yaw/pitch range in degrees applied per shot.");

        internal static readonly GUIContent DebugHeader = new GUIContent("Debug / Utilities");
        internal static readonly GUIContent RecalculateButton = new GUIContent("Recalculate Derived Values", "Force recalculation of dependent stats and send change notification.");
    }

    void OnEnable()
    {
        moveSpeedProp = serializedObject.FindProperty("moveSpeed");
        maxHorizontalSpeedProp = serializedObject.FindProperty("maxHorizontalSpeed");
        groundAccelerationProp = serializedObject.FindProperty("groundAcceleration");
        airAccelerationProp = serializedObject.FindProperty("airAcceleration");
        groundLinearDragProp = serializedObject.FindProperty("groundLinearDrag");
        airLinearDragProp = serializedObject.FindProperty("airLinearDrag");

        jumpImpulseProp = serializedObject.FindProperty("jumpImpulse");
        maxAirJumpsProp = serializedObject.FindProperty("maxAirJumps");
        coyoteTimeProp = serializedObject.FindProperty("coyoteTime");
        jumpBufferProp = serializedObject.FindProperty("jumpBuffer");
        fallGravityMultiplierProp = serializedObject.FindProperty("fallGravityMultiplier");
        lowJumpGravityMultiplierProp = serializedObject.FindProperty("lowJumpGravityMultiplier");

        fireRateProp = serializedObject.FindProperty("fireRate");
        projectileSizeProp = serializedObject.FindProperty("projectileSize");
        projectileDamageProp = serializedObject.FindProperty("projectileDamage");
        projectileSpeedProp = serializedObject.FindProperty("projectileSpeed");
        projectileLifeProp = serializedObject.FindProperty("projectileLife");
        projectilesPerShotProp = serializedObject.FindProperty("projectilesPerShot");
        projectileSpreadRadiusProp = serializedObject.FindProperty("projectileSpreadRadius");
        projectileAngleVarianceProp = serializedObject.FindProperty("projectileAngleVariance");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawMovementSection();
        EditorGUILayout.Space();
        DrawJumpingSection();
        EditorGUILayout.Space();
        DrawCombatSection();
        EditorGUILayout.Space();
        DrawDebugSection();

        if (serializedObject.ApplyModifiedProperties())
        {
            NotifyTargetsChanged(recordUndo: false);
            serializedObject.Update();
        }
    }

    void DrawMovementSection()
    {
        EditorGUILayout.LabelField(Styles.MovementHeader, EditorStyles.boldLabel);
        using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
        {
            EditorGUILayout.Slider(moveSpeedProp, 7.5f, 50f, Styles.MoveSpeed); 
            using (new EditorGUI.DisabledScope(true))
            {
                EditorGUILayout.PropertyField(maxHorizontalSpeedProp, Styles.MaxHorizontalSpeed);
            }
            EditorGUILayout.Slider(groundAccelerationProp, 40f, 100f, Styles.GroundAcceleration);
            EditorGUILayout.Slider(airAccelerationProp, 10f, 100f, Styles.AirAcceleration);
            //EditorGUILayout.Slider(groundLinearDragProp, 1f, 1f, Styles.GroundLinearDrag);
            //EditorGUILayout.Slider(airLinearDragProp, .5f, .5f, Styles.AirLinearDrag);
        }
    }

    void DrawJumpingSection()
    {
        EditorGUILayout.LabelField(Styles.JumpingHeader, EditorStyles.boldLabel);
        using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
        {
            EditorGUILayout.Slider(jumpImpulseProp, 5f, 10f, Styles.JumpImpulse);
            EditorGUILayout.IntSlider(maxAirJumpsProp, 0, 3, Styles.MaxAirJumps);
            //EditorGUILayout.Slider(coyoteTimeProp, .1f, 0.1f, Styles.CoyoteTime);
            //EditorGUILayout.Slider(jumpBufferProp, .1f, 0.1f, Styles.JumpBuffer);
            EditorGUILayout.Slider(fallGravityMultiplierProp, 2.0f, 10f, Styles.FallGravityMultiplier);
            EditorGUILayout.Slider(lowJumpGravityMultiplierProp, 2.5f, 15f, Styles.LowJumpGravityMultiplier);
        }
    }

    void DrawCombatSection()
    {
        EditorGUILayout.LabelField(Styles.CombatHeader, EditorStyles.boldLabel);
        using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
        {
            EditorGUILayout.Slider(fireRateProp, 0.5f, 20f, Styles.FireRate);
            EditorGUILayout.Slider(projectileDamageProp, 1f, 100f, Styles.ProjectileDamage);
            EditorGUILayout.Slider(projectileSpeedProp, 10f, 100f, Styles.ProjectileSpeed);
            EditorGUILayout.Slider(projectileLifeProp, 2f, 10f, Styles.ProjectileLife);

            EditorGUILayout.IntSlider(projectilesPerShotProp, 1, 12, Styles.ProjectilesPerShot);
            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("-1"))
                    projectilesPerShotProp.intValue = Mathf.Max(1, projectilesPerShotProp.intValue - 1);
                if (GUILayout.Button("+1"))
                    projectilesPerShotProp.intValue = Mathf.Min(12, projectilesPerShotProp.intValue + 1);
                if (GUILayout.Button("Single"))
                    projectilesPerShotProp.intValue = 1;
                if (GUILayout.Button("Burst 3"))
                    projectilesPerShotProp.intValue = 3;
                if (GUILayout.Button("Spread 5"))
                    projectilesPerShotProp.intValue = 5;
            }

            using (new EditorGUI.DisabledScope(true))
            {
                EditorGUILayout.PropertyField(projectileSizeProp, Styles.ProjectileSize);
                EditorGUILayout.PropertyField(projectileSpreadRadiusProp, Styles.ProjectileSpreadRadius);
            }

            EditorGUILayout.Slider(projectileAngleVarianceProp, 10f, 0f, Styles.ProjectileAngleVariance);
        }
    }

    void DrawDebugSection()
    {
        EditorGUILayout.LabelField(Styles.DebugHeader, EditorStyles.boldLabel);
        using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
        {
            EditorGUILayout.HelpBox("Use this to resync any derived values if they fall out of sync during tweaks.", MessageType.Info);
            if (GUILayout.Button(Styles.RecalculateButton))
            {
                serializedObject.ApplyModifiedProperties();
                NotifyTargetsChanged(recordUndo: true);
                serializedObject.Update();
            }
        }
    }

    void NotifyTargetsChanged(bool recordUndo)
    {
        foreach (Object obj in targets)
        {
            if (obj is PlayerUpgradeData data)
            {
                if (recordUndo)
                    Undo.RecordObject(data, "Recalculate Player Upgrade Data");

                data.NotifyChanged();
                EditorUtility.SetDirty(data);
            }
        }
    }
}
#endif
