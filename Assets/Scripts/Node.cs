using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {
    
    public int x;
    public int y;
    public Node parent;
    public float gCost;
    public float hCost;

    public float meanderScore;

    public Node (int x,int y){
        this.x = x;
        this.y = y;
        gCost = 0;
        meanderScore = 0;
    }

    public float fCost (){
        return gCost+hCost;
    }

    public override bool Equals(object obj){
        if ((obj == null) || ! this.GetType().Equals(obj.GetType())){
            return false;
        }
        else {
            Node node = (Node)obj;
            return (node.x == this.x) && (node.y == this.y);
        }
    }

    public override int GetHashCode(){
        return (x << 2) ^ y;
    }

    
    

}
