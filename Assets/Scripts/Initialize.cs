using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initialize : MonoBehaviour {
    public int mapWidth;
    public int mapHeight;
    public int seed;
    public float scale;
    public int octaves;
    public float persistance;
    public float lacunarity;
    public float heightMultiplier;
    public AnimationCurve heightCurve;
    public int levelOfDetail;
    public Vector2 offsets;
    MapDisplay display;

    MeshData meshData;

    Node start;
    Node end;

    float[,] noiseMap;
    
    void Start() {
        display = FindObjectOfType<MapDisplay> ();
        //Create Noise Map
        noiseMap = NoiseMapGenerator.GenerateNoiseMap(mapWidth,mapHeight,seed,scale,octaves,persistance,lacunarity,offsets);

        start = new Node (0,0);
        end = new Node (mapWidth-1,0);

        float highest = -1f;
        

        for (int y = mapHeight/3;y<2*mapHeight/3;y++){
            for(int x = 0;x<mapWidth/5;x++){
                if(noiseMap[x,y]>highest){
                    start.x = x;
                    start.y = y;
                    highest = noiseMap[x,y];
                }
            }
        }

        float lowest = 1000f;
        end.y = start.y;

        for (int x = 3*mapWidth/4;x<mapWidth;x++){
            if(noiseMap[x,start.y]<lowest){
                end.x = x;
                lowest = noiseMap[x,start.y];
            }
        }

        start.gCost = 0;

        //Debug.Log(noiseMap[start.x,start.y]);
        //Debug.Log(noiseMap[end.x,end.y]);

        Debug.Log(start.x+" "+ start.y);
        Debug.Log(end.x+" "+ end.y);
        Debug.Log("Distance:"+Mathf.Sqrt(Mathf.Pow(start.x-end.x,2)+Mathf.Pow(start.y-end.y,2)));

        meshData = MeshGenerator.GenerateTerrainMesh(noiseMap,heightMultiplier,heightCurve,levelOfDetail);

        display.DrawMesh(meshData);

    }

    void Update () {
        if(start == null){
            Debug.Log("No start ur bad");
        }
        if(Input.GetKeyDown(KeyCode.Z)){

            float currentTime = Time.realtimeSinceStartup;
            List<Node> river = BFS.BestFirstSearch(noiseMap,start,end,mapWidth,mapHeight);
            Debug.Log((Time.realtimeSinceStartup-currentTime));

            if(river.Count>0){
                float [,] map1 = NoiseMapGenerator.editNoiseMap(noiseMap,river,mapWidth,mapHeight);

                meshData = MeshGenerator.GenerateTerrainMesh(map1,heightMultiplier,heightCurve,levelOfDetail);

                display.DrawMesh(meshData);
            }
            else {
                Debug.Log("Youtrash boi");
            }
            
        }
        else if(Input.GetKeyDown(KeyCode.X)){
            float currentTime = Time.realtimeSinceStartup;
            List<Node> river = Astar.AStarSearch(noiseMap,start,end,mapWidth,mapHeight);
            Debug.Log((Time.realtimeSinceStartup-currentTime));

            if(river.Count>0){
                float [,] map1 = NoiseMapGenerator.editNoiseMap(noiseMap,river,mapWidth,mapHeight);

                meshData = MeshGenerator.GenerateTerrainMesh(map1,heightMultiplier,heightCurve,levelOfDetail);

                display.DrawMesh(meshData);
            }
            else {
                Debug.Log("Youtrash boi");
            }
        }
    }

}
