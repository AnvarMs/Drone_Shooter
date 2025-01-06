using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapGenerater))]
public class MapGeneratoreEditer : Editor
{
    public override void OnInspectorGUI()
    {
       MapGenerater mapGen = (MapGenerater)target;

       if(DrawDefaultInspector()){
        if(mapGen.autoUpdate){
            mapGen.GenerateMap();
        }
       }
       if(GUILayout.Button ("Gererate")){
        mapGen.GenerateMap();
       }
    }
}
