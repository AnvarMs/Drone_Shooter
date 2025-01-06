using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerater : MonoBehaviour
{
    public enum DrowMode{
        NoiceMap,
        ColorMap,
        Mesh,
        FolloffMap
    }
    const int mapChunkSize=241;
    [Range(0,6)]
    public int levelOfDetail;
    public DrowMode drowMode;

    public float noiseScale;

    public int octaves;

    [Range(0,1)]
    public float persistence;
    public float lacunarity;

    public int seed;
    public Vector2 offset;

    public float MeshHeightMultyplayer;
    public AnimationCurve meshHeightCurve;
    public bool autoUpdate;

    public TerrainType[] regions;


    public void GenerateMap(){
        float[,] noiseMap= Noice.GenerateNoiseMap(mapChunkSize,mapChunkSize,seed,noiseScale,octaves,persistence,lacunarity,offset);
            Color[] colorMap =new Color[mapChunkSize*mapChunkSize];
            for(int y=0;y<mapChunkSize;y++){
                for(int x=0;x<mapChunkSize;x++){

                    float currentHeight =noiseMap[x,y];
                    for(int i=0;i<regions.Length;i++){
                        if(currentHeight<=regions[i].height){
                                colorMap[y*mapChunkSize+x]= regions[i].color;
                            break;
                        }
                    }
                }
            }

        MapDrower draw = FindAnyObjectByType<MapDrower>();
        if(drowMode==DrowMode.NoiceMap){
        draw.DrawTexture(TextureGenerater.TextureFromHeightMap(noiseMap));
        }else if(drowMode == DrowMode.ColorMap){
            draw.DrawTexture(TextureGenerater.TextureFromeColorMap(colorMap,mapChunkSize,mapChunkSize));
        }else if(drowMode ==DrowMode.Mesh){
            draw.DrawMesh(MeshGeneretor.GenerateTarrainMesh(noiseMap,MeshHeightMultyplayer,meshHeightCurve,levelOfDetail),TextureGenerater.TextureFromeColorMap(colorMap,mapChunkSize,mapChunkSize));
        }else if(drowMode ==DrowMode.FolloffMap){

           // draw.DrawTexture(TextureGenerater.TextureFromHeightMap(FolloOfGeneretor.GenerateFalloffMap(mapChunkSize)))
        }
    }

    void OnValidate(){
        if(lacunarity<1){
            lacunarity=1;
        }
        if(octaves<0){
            octaves=0;
        }
    }

}
[System.Serializable]
public struct TerrainType{
    public string name;
    public float height;
    public Color color;
}
