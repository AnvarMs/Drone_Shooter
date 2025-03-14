using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FolloOfGeneretor 
{
    public static float[,] GenerateFalloffMap(int size){

        float[,] map = new float[size,size];

        for(int i=0;i<size;i++){
            for(int j=0;j<size;j++){

                float x = i/(float)size*2-1;
                float y =j/(float) size*2-1;

                float value =Math.Max (Mathf.Abs(x),Mathf.Abs(y));
                map [i,j] =value;
            }
        }
        return map;
    }
}
