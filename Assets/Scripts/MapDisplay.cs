using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour{
	public MeshFilter meshFilter;
	public MeshRenderer meshRenderer;
    public Material meshMaterial;

	public void DrawMesh (MeshData meshData){
		meshFilter.sharedMesh = meshData.CreateMesh();
		meshRenderer.material = meshMaterial;
	}
	
}
