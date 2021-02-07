using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryChecker : MonoBehaviour
{
    [SerializeField] private GameObject[] playerEdges;
    [SerializeField] private GameObject[] bulletEdges;


    private Vector3[] playerEdgeNormals;
    private Vector3[] bulletEdgeNormals;


    public static BoundaryChecker instance;

    void Start()
    {
        instance = this;
        playerEdgeNormals = new Vector3[playerEdges.Length];
        bulletEdgeNormals = new Vector3[playerEdges.Length];


        for (int i = 0; i < playerEdges.Length; i++)
        {
            Vector3 normalPlayer = playerEdges[i].GetComponent<MeshFilter>().mesh.normals[0];
            Vector3 normalBullet = bulletEdges[i].GetComponent<MeshFilter>().mesh.normals[0];


            playerEdgeNormals[i] = playerEdges[i].transform.TransformVector(normalPlayer);
            bulletEdgeNormals[i] = bulletEdges[i].transform.TransformVector(normalBullet);

        }
    }

    public static bool withinPlayerEdges(Vector3 worldPosition) 
    {
        bool withinEdges = true;

        for (int i = 0; i < instance.playerEdges.Length; i++)
        {
            Vector3 pointToFence = instance.playerEdges[i].transform.position - worldPosition;
            withinEdges = withinEdges && Vector3.Dot(instance.playerEdgeNormals[i], pointToFence) <= 0;
        }
        return withinEdges;
    }

    public static bool withinBulletEdges(Vector3 worldPosition)
    {
        bool withinEdges = true;

        for (int i = 0; i < instance.playerEdges.Length; i++)
        {
            Vector3 pointToFence = instance.bulletEdges[i].transform.position - worldPosition;
            withinEdges = withinEdges && Vector3.Dot(instance.bulletEdgeNormals[i], pointToFence) <= 0;
        }
        return withinEdges;
    }
}
