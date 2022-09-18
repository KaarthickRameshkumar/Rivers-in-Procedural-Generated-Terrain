using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFS {

    public static List<Node> BestFirstSearch(float[,] noiseMap, Node start, Node end, int mapWidth, int mapHeight){

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();

        Grid grid = new Grid(noiseMap);
        List<Node> river = new List<Node>();

        int count = 0;
        int amplitude = 5;
        int repetitions = 5;

        float bValue = 6.28f*repetitions/Mathf.Abs(start.x-end.x);

        openSet.Add(start);
        start.parent = start;

        while (openSet.Count>0){

            count++;

            if(count>100000){
                break;
            }

            Node current = openSet[0];
            

            for(int i = 1;i<openSet.Count;i++){
                if(openSet[i].hCost < current.hCost){
                    current = openSet[i];
                }
            }

            if(current.Equals(end)){
                river = retracePath(current, start);
                Debug.Log("Meander Score:"+current.meanderScore);
                break;
            }

            openSet.Remove(current);
            closedSet.Add(current);

            foreach (Node node in grid.getNeighbors(current,mapWidth,mapHeight)){

                if(!closedSet.Contains(node)&&!openSet.Contains(node)){

                    float sinRatio = Mathf.Abs(amplitude*Mathf.Sin(bValue*(node.x-start.x))-(node.y-start.y));
                    node.meanderScore+=current.meanderScore+sinRatio;

                    if(noiseMap[current.x,current.y]<noiseMap[node.x,node.y]){
                        node.hCost = Mathf.Infinity;
                    }
                    else{
                        node.hCost = Mathf.Sqrt(Mathf.Pow(node.x-end.x,2)+Mathf.Pow(node.y-end.y,2))+sinRatio;
                    }
                    
                    node.parent = current;
                    openSet.Add(node);
                    
                }
            }
            
        }
       
       return river;
    }

    private static List<Node> retracePath(Node n, Node start) {
        List<Node> river = new List <Node>();
        while (!n.Equals(start)){
           river.Add(n);
           n = n.parent;
        }

        return river;

    }
    

}
