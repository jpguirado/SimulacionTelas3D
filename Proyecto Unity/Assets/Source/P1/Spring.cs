using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring
{

    #region InEditorVariables

    public float Stiffness;
    public Node nodeA;
    public Node nodeB;

    #endregion

    public MassSpringCloth Manager;

    public float Length0;
    public float Length;
    public Vector3 Pos;
    public float damping1 = 0.01f;
    public float damping2 = 0.01f;



    public Spring(Node node1, Node node2, float Stiffness, float dampingSpring)
    {
        this.nodeA = node1;
        this.nodeB = node2;
        this.Stiffness = Stiffness;
        Length0 = Length = (nodeA.Pos - nodeB.Pos).magnitude;
        this.damping1 = dampingSpring * this.Stiffness;
        this.damping2 = dampingSpring * this.Stiffness;
    }

    public void ComputeForces()
    {
        Vector3 dir = nodeA.Pos - nodeB.Pos;
        Length = dir.magnitude;
        dir = dir * (1.0f / Length);
        Vector3 Force = -Stiffness * (Length - Length0) * dir - damping1 * (nodeA.Vel - nodeB.Vel) - damping2 * Vector3.Dot(dir, (nodeA.Vel - nodeB.Vel)) * dir;
        nodeA.Force += Force;
        nodeB.Force -= Force;
    }

}
