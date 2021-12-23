using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (MapGen))]
public class MapGenTest : Editor
{
    public override void OnInspectorGUI()
    {
        MapGen mapGen = (MapGen)target;
        
        if (DrawDefaultInspector())
        {
            if (mapGen.autoUpdate) mapGen.drawInEditor();
        }

        if (GUILayout.Button("Generate")) mapGen.drawInEditor();
    }
}
