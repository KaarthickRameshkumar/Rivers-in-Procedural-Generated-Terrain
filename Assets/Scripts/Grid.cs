using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid {

    float [,] noiseMap;
    public Grid (float [,] noiseMap) {
        this.noiseMap = noiseMap;
    }

    public List<Node> getNeighbors (Node currentNode, int mapWidth,int mapHeight) {
        List<Node> neighbors = new List<Node>();

        for(int x = 0;x<=1;x++){
            for(int y = -1;y<=1;y++){
                if(x ==0 && y == 0){
                    continue;
                }

                int checkX = currentNode.x+x;
                int checkY = currentNode.y+y;


                if(checkX>=0 && checkX<mapWidth && checkY>=0 && checkY <mapHeight){
                    neighbors.Add(new Node(checkX,checkY));
                }
            }
        }

        return neighbors;
    }

    /**
    if(noiseMap[node.x,node.y]>0.6){
                        Debug.Log("no meander");
                        node.hCost = 1/(noiseMap[node.x,node.y]-noiseMap[current.x,current.y]+1)*Mathf.Sqrt(Mathf.Pow((node.x - end.x),2) + Mathf.Pow((node.y - end.y),2));
                    }
                    else {
                        if(current.meandering){
                            Debug.Log("continue");

                            node.m = current.m;
                            node.c = current.c;
                            node.meanderStart=current.meanderStart;
                            node.meandering = true;

                            float mP = Mathf.Pow(node.m,-1)*-1;
                            float cP = node.y-mP*node.x;

                            float distX = (node.c-cP)/(mP-node.m);
                            float distY = node.m*distX+node.c;

                            float sinRatio = Mathf.Abs(Mathf.Sqrt(Mathf.Pow((node.x - distX),2) + Mathf.Pow((node.y - distY),2))-10*Mathf.Sin(0.1f*Mathf.Sqrt(Mathf.Pow((node.meanderStart.x - distX),2) + Mathf.Pow((node.meanderStart.y - distY),2)))); 

                            if(sinRatio>0.55){
                                Debug.Log("end meander");
                                node.hCost = 0.1f;
                                node.meandering = false;
                            }
                            else{
                                node.hCost = sinRatio*Mathf.Sqrt(Mathf.Pow((node.x - end.x),2) + Mathf.Pow((node.y - end.y),2));
                            }
                        }
                        else {
                            Debug.Log("meander start");
                            node.meanderStart = current;
                            node.meandering = true;
                            node.m = (current.y-end.y)/(current.x-end.x);
                            node.c = current.y-node.m*current.x;

                            float mP = Mathf.Pow(node.m,-1)*-1;
                            float cP = node.y-mP*node.x;

                            float distX = (node.c-cP)/(mP-node.m);
                            float distY = node.m*distX+node.c;

                            float sinRatio = Mathf.Abs(Mathf.Sqrt(Mathf.Pow((node.x - distX),2) + Mathf.Pow((node.y - distY),2))-10*Mathf.Sin(0.1f*Mathf.Sqrt(Mathf.Pow((node.meanderStart.x - distX),2) + Mathf.Pow((node.meanderStart.y - distY),2))));; 
                            node.hCost = sinRatio*Mathf.Sqrt(Mathf.Pow((node.x - end.x),2) + Mathf.Pow((node.y - end.y),2));

                            //node.hCost = Mathf.Sin(Mathf.Sqrt(Mathf.Pow((node.x - current.x),2) + Mathf.Pow((node.y - current.y),2)));
                        }
                    }

    **/

    /**  
        int count = 0;

        while (start2==null){
            count++;

            if(count>1000){
                break;
            }
            int checkX = start.x+Random.Range(10,20);
            int checkY = start.y+Random.Range(10,20);

            if(checkX>=0 && checkX<mapWidth && checkY>=0 && checkY <mapHeight){
                start2 = new Node (checkX,checkY);
            }
        }

        while (start3==null){
            int checkX = start.x+Random.Range(2,7);
            int checkY = start.y+Random.Range(2,7);

            if(checkX>=0 && checkX<mapWidth && checkY>=0 && checkY <mapHeight){
                start3 = new Node (checkX,checkY);
            }
        }

        **/

        /**
        int basinLength = Random.Range(6,8);
        int basinWidth = Random.Range(4,5);

        int randWidth = Random.Range(-1,1);
        int randLength = Random.Range(0,basinLength);
        start1 = new Node (start.x+randWidth*basinWidth,start.y+randLength*basinLength);

        randWidth = Random.Range(-1,1);
        randLength = Random.Range(0,basinLength);
        start2 = new Node (start.x+randWidth*basinWidth,start.y+randLength*basinLength);

        randWidth = Random.Range(-1,1);
        randLength = Random.Range(0,basinLength);
        start3 = new Node (start.x+randWidth*basinWidth,start.y+randLength*basinLength);

        noiseMap = NoiseMapGenerator.addBasin(basinLength,basinWidth,noiseMap,start,end,mapWidth,mapHeight);
        **/
}
