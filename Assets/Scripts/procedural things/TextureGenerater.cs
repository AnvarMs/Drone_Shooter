using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TextureGenerater
{
   public static Texture2D TextureFromeColorMap(Color[] colorMap,int width,int height){

    Texture2D texture = new Texture2D (width ,height);
    texture.SetPixels(colorMap);
    texture.Apply();
    return texture;


   }

   public static Texture2D TextureFromHeightMap(float[,] heightMap){
     int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);



        Color[] colourMap = new Color[width*height];
        for(int y=0;y<height;y++){
            for(int x=0;x<width;x++){
                colourMap[y*width+x] = Color.Lerp(Color.black,Color.white, heightMap[x,y]);
            }
        }
        
        return TextureFromeColorMap(colourMap,width,height);
   }
   
}
