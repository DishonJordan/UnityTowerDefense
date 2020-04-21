using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Spawner))]
[CanEditMultipleObjects]
public class SpawnerGUI : Editor 
{
    public SerializedProperty waves;
    SerializedProperty enemies;
    SerializedProperty timeBetweenWaves;
    SerializedProperty timeBetweenSpawns;
    SerializedProperty waypoints;
    SerializedProperty bank;
    SerializedProperty gm;
    SpawnerWindow window;
    
    void OnEnable()
    {
        waves = serializedObject.FindProperty("waves");
        enemies = serializedObject.FindProperty("enemies");
        timeBetweenWaves = serializedObject.FindProperty("timeBetweenWaves");
        timeBetweenSpawns = serializedObject.FindProperty("timeBetweenSpawns");
        waypoints = serializedObject.FindProperty("waypoints");
        bank = serializedObject.FindProperty("bank");
        gm = serializedObject.FindProperty("gm");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.LabelField("Spawner Properties:", EditorStyles.boldLabel);
        //EditorGUILayout.HelpBox("This is helpful message!", MessageType.Info);
        EditorGUILayout.PropertyField(enemies);
        EditorGUILayout.PropertyField(timeBetweenWaves);
        EditorGUILayout.PropertyField(timeBetweenSpawns);
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Wave Properties:", EditorStyles.boldLabel);
        waves.arraySize = EditorGUILayout.IntField("Number of waves: ", waves.arraySize);
        for(int i = 0; i < waves.arraySize; i++){
            var x = waves.GetArrayElementAtIndex(i).GetEnumerator();
            x.MoveNext();
            EditorGUILayout.PropertyField(x.Current as SerializedProperty, new GUIContent("Wave " + i.ToString()));
        }
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Enemy Properties:", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(waypoints);
        EditorGUILayout.PropertyField(bank);
        EditorGUILayout.PropertyField(gm, new GUIContent("Game Manager"));
        serializedObject.ApplyModifiedProperties();
    }
}