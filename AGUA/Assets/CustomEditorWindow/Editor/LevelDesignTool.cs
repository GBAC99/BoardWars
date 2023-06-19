using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LevelDesignTool : EditorWindow
{

    GameObject[] enemyPieces;

    bool[,] gridPosition;
     


    public enum EnemyPieces
    {
        TRIANGLE = 0,
        DIAMOND = 1,
        CUBE = 2
    }

    public EnemyPieces ep;

    [MenuItem("MY Tools/LevelDesignTool")]

    public static void ShowWindow()
    {
        GetWindow(typeof(LevelDesignTool));
    }

    private void OnGUI()
    {
        GUILayout.Label("Enemy Piece setting Up", EditorStyles.boldLabel);
        ep = (EnemyPieces)EditorGUILayout.EnumPopup("Select an enemy to place", ep);
         

    }

    /*private void DrawHexTable()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("x  |  z");
        for (int i = 0; i < gridPosition.GetLength(0); i++)
        {
            GUILayout.Label(i.ToString());
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginVertical();
        for (int ir = 0; ir < gridPosition.GetLength(1); ir++)
        {
            EditorGUILayout.BeginHorizontal();
            //EditorGUILayout.LabelField((target.rows - (ir + 1)).ToString());
            for (int ic = 0; ic < gridPosition.GetLength(0); ic++)
            {
                gridPosition[ic, ir] = EditorGUILayout.Toggle(gridPosition[ic, ir]);
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
    }*/

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
