using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {

    #region InEditorVariables

    public float Mass = 0.5f;
    public bool Fixed = false;

    #endregion

    public  MassSpringCloth Manager;

    public Vector3 Pos;
    public Vector3 Vel;
    public Vector3 Force;
    public float damping = 0.1f;

    public Node(Vector3 posicion, float Mass, float dampingNode)
    {
        this.Pos = posicion;
        this.Mass = Mass;
        this.damping = dampingNode * this.Mass;
    }

    public void ComputeForces ()
    {
        Force += Mass * Manager.Gravity - damping * Vel;
    }
}
