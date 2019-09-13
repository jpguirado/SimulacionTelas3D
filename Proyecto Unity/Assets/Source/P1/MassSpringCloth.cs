using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Basic physics manager capable of simulating a given ISimulable
/// implementation using diverse integration methods: explicit,
/// implicit, Verlet and semi-implicit.
/// </summary>
public class MassSpringCloth : MonoBehaviour
{
    /// <summary>
    /// Default constructor. Zero all. 
    /// </summary>
    public MassSpringCloth()
    {
        Paused = true;
        TimeStep = 0.01f;
        Gravity = new Vector3(0.0f, -9.81f, 0.0f);
        IntegrationMethod = Integration.Explicit;

    }

    /// <summary>
    /// Integration method.
    /// </summary>
    public enum Integration
    {
        Explicit = 0,
        Symplectic = 1,
    };

    #region InEditorVariables

    public bool Paused;
    public float TimeStep;
    public float Stiffness = 200;
    public float StiffnessFlexion = 20;
    public float Mass = 0.5f;
    public float dampingNode = 0.1f;
    public float dampingSpring = 0.01f;
    public Vector3 Gravity;
    public Integration IntegrationMethod;

    #endregion

    #region OtherVariables

    public Node[] nodes;
    public List<Spring> springs;
    Mesh mesh;
    public List<GameObject> fixers;
    Vector3[] vertices;
    public List<Edge> TriangulosOrdenados;

    #endregion

    #region MonoBehaviour

    public void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;
        nodes = new Node[vertices.Length];
        springs = new List<Spring>();
        TriangulosOrdenados = new List<Edge>();
        for (int i = 0; i < vertices.Length; i++)
        {
            nodes[i] = new Node(vertices[i], Mass, dampingNode);

            foreach (GameObject fixer in fixers)
            {
                Collider Fixer_Box = fixer.GetComponent<Collider>();
                if (Fixer_Box.bounds.Contains(transform.TransformPoint(vertices[i])))
                {
                    nodes[i].Fixed = true;
                    break;
                }
            }

        }

        //int[] triangulos = mesh.triangles;
        for (int i = 0; i < mesh.triangles.Length; i = i + 3)
        {
            TriangulosOrdenados.Add(new Edge(mesh.triangles[i], mesh.triangles[i + 1], mesh.triangles[i + 2]));
            TriangulosOrdenados.Add(new Edge(mesh.triangles[i], mesh.triangles[i + 2], mesh.triangles[i + 1]));
            TriangulosOrdenados.Add(new Edge(mesh.triangles[i+1], mesh.triangles[i + 2], mesh.triangles[i]));

        }

        EdgeComparer comparador = new EdgeComparer();
        TriangulosOrdenados.Sort(comparador);
        
        
        for (int i = 0; i<(TriangulosOrdenados.Count-1); i++)
        {
            
            if (TriangulosOrdenados[i].vertexA == TriangulosOrdenados[i+1].vertexA && TriangulosOrdenados[i].vertexB == TriangulosOrdenados[i + 1].vertexB)
            {
                springs.Add(new Spring(nodes[TriangulosOrdenados[i].vertexOther], nodes[TriangulosOrdenados[i + 1].vertexOther], StiffnessFlexion, dampingSpring));
                springs.Add(new Spring(nodes[TriangulosOrdenados[i].vertexOther], nodes[TriangulosOrdenados[i].vertexA], Stiffness, dampingSpring));
                springs.Add(new Spring(nodes[TriangulosOrdenados[i].vertexOther], nodes[TriangulosOrdenados[i].vertexB], Stiffness, dampingSpring));
            }

            else
            {
                springs.Add(new Spring(nodes[TriangulosOrdenados[i].vertexOther], nodes[TriangulosOrdenados[i].vertexA], Stiffness, dampingSpring));
                springs.Add(new Spring(nodes[TriangulosOrdenados[i].vertexOther], nodes[TriangulosOrdenados[i].vertexB], Stiffness, dampingSpring));
                springs.Add(new Spring(nodes[TriangulosOrdenados[i].vertexA], nodes[TriangulosOrdenados[i].vertexB], Stiffness, dampingSpring));
            }
            
        }
          
        foreach (Node node in nodes)
        {
            node.Manager = this; 
        }

        foreach (Spring spring in springs)
        {
           spring.Manager = this;
        }

    }

    public void Update()
{
    if (Input.GetKeyUp(KeyCode.P))
        Paused = !Paused;

}

public void FixedUpdate()
{
    if (Paused)
        return; // Not simulating

        // Select integration method

        for (int i = 0; i < 10; i++)
        { 
            switch (IntegrationMethod)
            {
                case Integration.Explicit: stepExplicit(); break;
                case Integration.Symplectic: stepSymplectic(); break;
                default:
                    throw new System.Exception("[ERROR] Should never happen!");
            }
        }
}

#endregion

/// <summary>
/// Performs a simulation step in 1D using Explicit integration.
/// </summary>
private void stepExplicit()
{

    for (int i = 0; i < nodes.Length; i++)
    {
        nodes[i].Force = Vector3.zero;
        nodes[i].ComputeForces();
    }

    foreach (Spring spring in springs)
    {
        spring.ComputeForces();
    }

    for (int i = 0; i < nodes.Length; i++)
    {
        if (!nodes[i].Fixed)
        {
            nodes[i].Pos += nodes[i].Vel * TimeStep;
            nodes[i].Vel += nodes[i].Force * TimeStep / nodes[i].Mass;
            vertices[i] = nodes[i].Pos;
        }
    }

    mesh.vertices = vertices;
}

/// <summary>
/// Performs a simulation step in 1D using Symplectic integration.
/// </summary>
private void stepSymplectic()
{

    for (int i = 0; i < nodes.Length; i++)
    {
        nodes[i].Force = Vector3.zero;
        nodes[i].ComputeForces();
    }

    foreach (Spring spring in springs)
    {
        spring.ComputeForces();
    }

    for (int i = 0; i < nodes.Length; i++)
    {
        if (!nodes[i].Fixed)
        {
            nodes[i].Vel += nodes[i].Force * TimeStep / nodes[i].Mass;
            nodes[i].Pos += nodes[i].Vel * TimeStep;
            vertices[i] = nodes[i].Pos;
        }
    }

    mesh.vertices = vertices;

}

}
