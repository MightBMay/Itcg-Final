using UnityEngine;
using UnityEditor;

public class LinePlacer : EditorWindow
{
    private GameObject selectedObject;
    private int objectCount = 10;
    private float spacing = 2f;
    private Vector3 direction = Vector3.right;

    [MenuItem("Tools/Line Placer")]
    public static void ShowWindow()
    {
        GetWindow<LinePlacer>("Line Placer");
    }

    private void OnGUI()
    {
        GUILayout.Label("Place Objects in a Line", EditorStyles.boldLabel);

        selectedObject = (GameObject)EditorGUILayout.ObjectField("Selected Object", selectedObject, typeof(GameObject), true);
        objectCount = EditorGUILayout.IntField("Object Count", objectCount);
        spacing = EditorGUILayout.FloatField("Spacing", spacing);
        direction = EditorGUILayout.Vector3Field("Direction", direction);

        if (GUILayout.Button("Place Objects"))
        {
            PlaceObjects();
        }
    }

    private void PlaceObjects()
    {
        if (selectedObject == null)
        {
            Debug.LogError("No object selected!");
            return;
        }

        Vector3 startPosition = selectedObject.transform.position;

        for (int i = 1; i < objectCount; i++)
        {
            Vector3 position = startPosition + direction.normalized * spacing * i;
            GameObject clone = Instantiate(selectedObject, position, selectedObject.transform.rotation);
            clone.name = selectedObject.name + "_" + i;
            Undo.RegisterCreatedObjectUndo(clone, "Placed Objects");
        }
    }
}