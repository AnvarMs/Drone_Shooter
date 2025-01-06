using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDrower : MonoBehaviour
{
   public Renderer texturRender;
   public MeshFilter meshFilter;
   public MeshRenderer meshRenderer;

   public void DrawTexture(Texture2D texture){

       

        texturRender.sharedMaterial.mainTexture =texture;
        texturRender.transform.localScale =new Vector3(texture.width,1,texture.height);

   }

   public void DrawMesh(MeshData meshData,Texture2D texture){
      meshFilter.sharedMesh =meshData.CreateMesh();
      meshRenderer.sharedMaterial.mainTexture =texture;
   }
}
