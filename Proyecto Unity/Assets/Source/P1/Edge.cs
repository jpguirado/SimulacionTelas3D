using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge {

    public int vertexA;
    public int vertexB;
    public int vertexOther;

    public Edge(int vertexA, int vertexB, int vertexOther)
    {
        if(vertexA < vertexB)
        {
            this.vertexA = vertexA;
            this.vertexB = vertexB;
            this.vertexOther = vertexOther;
        }
        else if (vertexB < vertexA)
        {
            this.vertexA = vertexB;
            this.vertexB = vertexA;
            this.vertexOther = vertexOther;
        }
    }
}

