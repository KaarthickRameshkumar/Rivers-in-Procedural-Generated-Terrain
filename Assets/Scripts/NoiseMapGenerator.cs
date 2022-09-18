using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseMapGenerator : MonoBehaviour {
    
    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistance, float lacunarity,Vector2 offset) {
		//This is the matrix holding all the perlin values
		float[,] noiseMap = new float[mapWidth,mapHeight];

		Vector2[] octaveOffsets = new Vector2[octaves];
		System.Random prng = new System.Random(seed);

		//Adds offsets
		for(int i = 0;i<octaves;i++){
			float offsetX = prng.Next(-100000,100000) + offset.x;
			float offsetY = prng.Next(-100000,100000) - offset.y;
			octaveOffsets[i] = new Vector2(offsetX,offsetY);
		}
		
		if (scale <= 0) {
			scale = 0.0001f;
		}

		float maxNoiseHeight = float.MinValue;
		float minNoiseHeight = float.MaxValue;

		//Creating the Noise or Height Map
		for (int y = 0; y < mapHeight; y++) {
			for (int x = 0; x < mapWidth; x++) {
		
				float amplitude = 1;
				float frequency = 1;
				float noiseHeight = 0;

				for (int i = 0; i < octaves; i++) {
					float sampleX = x / scale * frequency + octaveOffsets[i].x * frequency;
					float sampleY = y / scale * frequency + octaveOffsets[i].y * frequency;

					float perlinValue = Mathf.PerlinNoise (sampleX, sampleY) * 2 - 1;
					noiseHeight += perlinValue * amplitude;

					amplitude *= persistance;
					frequency *= lacunarity;
				}

				if (noiseHeight > maxNoiseHeight) {
					maxNoiseHeight = noiseHeight;
				} else if (noiseHeight < minNoiseHeight) {
					minNoiseHeight = noiseHeight;
				}
				noiseMap [x, y] = noiseHeight;
			}
		}

		//Normalizing values in NoiseMap
		for (int y = 0; y < mapHeight; y++) {
			for (int x = 0; x < mapWidth; x++) {
				noiseMap [x, y] = Mathf.InverseLerp (minNoiseHeight, maxNoiseHeight, noiseMap [x, y]);
			}
		}

		return noiseMap;
		}

	public static float [,] editNoiseMap (float[,] noiseMap, List<Node> nodes, int mapWidth, int mapHeight){
		float [,] newMap = noiseMap;

		int total = nodes.Count;
		int upHill = 0;
		
		Node prev = nodes[0];
		nodes.Remove(prev);

		while(nodes.Count>0){
			Node k = nodes[0];
			nodes.Remove(k);

			if(noiseMap[prev.x,prev.y]>noiseMap[k.x,k.y]){
				upHill++;
			}

			int slope;

			if(Mathf.Abs(prev.x-k.x) == 0){
				slope = 0;
			}
			else {
				slope = (prev.y-k.y)/(prev.x-k.x);
			}

			for(int i = 0;i<Mathf.Abs(prev.x-k.x);i++){

				if(i+prev.x<mapWidth && i*slope+prev.y <mapHeight){
					noiseMap[i+prev.x,i*slope+prev.y]-=0.2f;
				}
				
			}

			prev = k;

		}

		Debug.Log("Uphill Nodes:"+upHill);

		return newMap;
	}

	public static float [,] addBasin (int basinWidth, int basinHeight,float[,] noiseMap, Node start, Node end, int mapWidth, int mapHeight){
		float [,] newMap = noiseMap;
		int down = basinWidth/2;

		for(int x = -basinWidth;x<basinWidth;x++){
			for(int y = 0;y<basinHeight;y++){
				if(x==0){
					int checkX = start.x+x;
					int checkY = start.y+y;
					if(checkX>=0 && checkX<mapWidth && checkY>=0 && checkY <mapHeight){
						newMap[start.x+x,start.y+y] -= 0.01f*Vector2.Distance(new Vector2(start.x,start.y),new Vector2(end.x,end.y))/10;
					}
					
				}
				else {
					int checkX = start.x+x;
					int checkY = start.y+y-down*Mathf.Abs(x);
					if(checkX>=0 && checkX<mapWidth && checkY>=0 && checkY <mapHeight){
						newMap[start.x+x,start.y+y-down*Mathf.Abs(x)] -= 0.01f*Vector2.Distance(new Vector2(start.x,start.y),new Vector2(end.x,end.y))/10;
					}
					
				}
				
			}
		}

		return newMap;
	}

}
