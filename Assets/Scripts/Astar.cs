using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astar {

    public static List<Node> AStarSearch(float[,] noiseMap, Node start, Node end, int mapWidth, int mapHeight){

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();

        Grid grid = new Grid(noiseMap);
        List<Node> river = new List<Node>();

        float[,] fCosts = new float[mapWidth,mapHeight];

        for(int x = 0;x<mapWidth;x++){
            for(int y = 0;y<mapHeight;y++){
                fCosts[x,y] =-1;
            }

        }

        int count = 0;
        int amplitude = 5;
        int repetitions = 5;

        float bValue = 6.28f*repetitions/Mathf.Abs(start.x-end.x);

        openSet.Add(start);
        start.parent = start;

        while (openSet.Count>0){

            count++;

            if(count>1000000){
                break;
            }

            Node current = openSet[0];
            

            for(int i = 1;i<openSet.Count;i++){
                if(openSet[i].fCost() < current.hCost){
                    current = openSet[i];
                }
            }

            if(current.Equals(end)){
                river = retracePath(current, start);
                Debug.Log("Meander Score: "+current.meanderScore);
                break;
            }

            openSet.Remove(current);
            closedSet.Add(current);

            foreach (Node node in grid.getNeighbors(current,mapWidth,mapHeight)){

                float sinRatio = Mathf.Abs(amplitude*Mathf.Sin(bValue*(node.x-start.x))-(node.y-start.y));
                node.meanderScore=current.meanderScore+sinRatio;

                node.gCost = current.gCost+sinRatio-noiseMap[current.x,current.y]+noiseMap[node.x,node.y];

                if(noiseMap[current.x,current.y]<noiseMap[node.x,node.y]){
                    node.hCost = Mathf.Infinity;
                }
                else{
                    node.hCost = Mathf.Sqrt(Mathf.Pow(node.x-end.x,2)+Mathf.Pow(node.y-end.y,2))+sinRatio;
                }
                /**
                if(currentNode.elevation < neighbor.elevation){
                    h-cost = infinity
                }
                else {
                    h-cost = Distance from Node to End + Distance from Node to Sine Curve
                }


                g-cost = Current Node's G-Cost 
                + Distance From Neighbor to Since Curve 
                + Difference in Elevation between Current Node and Neighbor
                **/

                if(fCosts [node.x,node.y] == -1){
                    fCosts [node.x,node.y] = node.fCost();
                    node.parent = current;
                    openSet.Add(node); 
                }
                else if(node.fCost()+0.5<fCosts[node.x,node.y]){
                    fCosts[node.x,node.y] = node.fCost();
                    node.parent = current;
                    openSet.Remove(node);
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
