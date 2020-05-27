using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Spawner))]
[CanEditMultipleObjects]
public class SpawnerGUI : Editor 
{
    public SerializedProperty waves;
    SerializedProperty enemies;
    SerializedProperty waypoints;
    SerializedProperty bank;
    SerializedProperty gm;
    SerializedProperty ended;
    SerializedProperty index;
    bool[] dropdown;
    
    void OnEnable()
    {
        waves = serializedObject.FindProperty("waves");
        enemies = serializedObject.FindProperty("enemies");
        waypoints = serializedObject.FindProperty("waypoints");
        bank = serializedObject.FindProperty("bank");
        gm = serializedObject.FindProperty("gm");
        ended = serializedObject.FindProperty("waveEnded");
        index = serializedObject.FindProperty("waveIndex");
        dropdown = new bool[waves.arraySize];
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.LabelField("Spawner Properties:", EditorStyles.boldLabel);
        //EditorGUILayout.HelpBox("This is helpful message!", MessageType.Info);
        EditorGUILayout.PropertyField(enemies);
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Wave Properties:", EditorStyles.boldLabel);
        waves.arraySize = EditorGUILayout.DelayedIntField("Number of waves: ", waves.arraySize);
        if(waves.arraySize != dropdown.Length){
            // dropdown = new bool[waves.arraySize];
            bool[] newDropdown = new bool[waves.arraySize];
            for(int i = 0; i < Mathf.Min(waves.arraySize, dropdown.Length); i++){
                newDropdown[i] = dropdown[i];
            }
            dropdown = newDropdown;
        }
        for(int i = 0; i < waves.arraySize; i++){
            var x = waves.GetArrayElementAtIndex(i).GetEnumerator();
            x.MoveNext();

            //Dropdown for wave element
            dropdown[i] = EditorGUILayout.Foldout(dropdown[i], "Wave " + i.ToString(), true);
            if(dropdown[i]){
                EditorGUILayout.BeginVertical();
                x.MoveNext();
                var y = x.Current as SerializedProperty;
                EditorGUILayout.PropertyField(y);
                int enemies = y.intValue;
                for(int j = 0; j < enemies; j++){
                    x.MoveNext();
                    EditorGUILayout.BeginHorizontal();
                    x.MoveNext();
                    y = x.Current as SerializedProperty;
                    EditorGUIUtility.labelWidth = 60;
                    EditorGUILayout.PropertyField(y, new GUIContent("Enemy " + j.ToString()));
                    x.MoveNext();
                    y = x.Current as SerializedProperty;
                    EditorGUILayout.PropertyField(y, new GUIContent("Delay"), GUILayout.MinWidth(0));
                    EditorGUILayout.EndHorizontal();
                }
                x.MoveNext();
                y = x.Current as SerializedProperty;
                EditorGUIUtility.labelWidth = 0;
                EditorGUILayout.PropertyField(y, new GUIContent("Wave Delay"));
                EditorGUILayout.EndVertical();
            }
        }
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(index);
        EditorGUILayout.PropertyField(ended);
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Enemy Properties:", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(waypoints);
        EditorGUILayout.PropertyField(bank);
        EditorGUILayout.PropertyField(gm, new GUIContent("Game Manager"));
        serializedObject.ApplyModifiedProperties();
    }
}
