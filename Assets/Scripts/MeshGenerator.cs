using UnityEngine;
using System.Collections;

public static class MeshGenerator {

	public static MeshData GenerateTerrainMesh(float[,] heightMap, float heightMultiplier, AnimationCurve heightCurve, int levelOfDetail) {
		int width = heightMap.GetLength (0);
		int height = heightMap.GetLength (1);
		float topLeftX = (width - 1) / -2f;
		float topLeftZ = (height - 1) / 2f;

		//Allows you to render meshes at differing levels of detail. You can skip certain increments of vertices for larger triangles
		int meshSimplicationIncrement = (levelOfDetail == 0)?1:levelOfDetail*2;
		int verticesPerLine = (width-1)/meshSimplicationIncrement + 1;

		MeshData meshData = new MeshData (verticesPerLine, verticesPerLine);
		int vertexIndex = 0;

		for (int y = 0; y < height; y+= meshSimplicationIncrement) {
			for (int x = 0; x < width; x+=meshSimplicationIncrement) {

				meshData.vertices [vertexIndex] = new Vector3 (topLeftX + x, heightCurve.Evaluate(heightMap [x, y]) * heightMultiplier, topLeftZ - y);
				meshData.uvs [vertexIndex] = new Vector2 (x / (float)width, y / (float)height);

                //This adds the triangles. Note that if we add triangles between vertices and start at the top left
                //We would never need to create triangles on the rightmost or bottommost columns/rows
				if (x < width - 1 && y < height - 1) {
                    //Imagine a matrix with the top left point being i the vertex index. The next 
                    //point would be i+1, i+2 etc. The next row would be i+w, i+w+1 etc. Width is
					//the vertices per line, which means we use that variable for simplification

					meshData.AddTriangle (vertexIndex, vertexIndex + verticesPerLine + 1, vertexIndex + verticesPerLine);
					meshData.AddTriangle (vertexIndex + verticesPerLine + 1, vertexIndex, vertexIndex + 1);
				}

				vertexIndex++;
			}
		}

		return meshData;

	}
}

public class MeshData {
	public Vector3[] vertices;
	public int[] triangles;
	public Vector2[] uvs;

	int triangleIndex;

	public MeshData(int meshWidth, int meshHeight) {
		vertices = new Vector3[meshWidth * meshHeight];
		uvs = new Vector2[meshWidth * meshHeight];

        //The number of triangles in the mesh is the number of squares (height - 1)(width -1)
        //times 6 because each square has2 triangles with 6 vertices each

		triangles = new int[(meshWidth-1)*(meshHeight-1)*6];
	}

    //adds a triangle to the array based on the current index
	public void AddTriangle(int a, int b, int c) {
		triangles [triangleIndex] = a;
		triangles [triangleIndex + 1] = b;
		triangles [triangleIndex + 2] = c;
		triangleIndex += 3;
	}

	public Mesh CreateMesh() {
		Mesh mesh = new Mesh ();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.uv = uvs;
		mesh.RecalculateNormals ();
		return mesh;
	}

}
