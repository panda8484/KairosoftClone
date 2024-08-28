using System;
using System.Collections.Generic;
using UnityEngine;
public class Graph
{
    // Dictionary<string, Node> nodes = new Dictionary<string, Node>();
    Dictionary<(int, int), Node> nodes = new Dictionary<(int, int), Node>();
    public int xAxisNum;
    public int zAxisNum;
    public int length;

    public Graph(int xAxisNum, int zAxisNum, int length)
    {
        this.xAxisNum = xAxisNum;
        this.zAxisNum = zAxisNum;
        this.length = length;

        int minXAxisPosition = -xAxisNum / 2 * length;
        int minZAxisPosition = -zAxisNum / 2 * length;

        for (int i = 0; i < xAxisNum; i++)
        {
            for (int j = 0; j < zAxisNum; j++)
            {
                int xPosition = minXAxisPosition + length * i;
                int zPosition = minZAxisPosition + length * j;
                Node node = new Node(xPosition, zPosition);
                nodes[node.Key] = node; // add node to dict
            }
        }
        ConnectNodes();
    }

    void ConnectNodes()
    {
        int[] dx = {0, 1, 0, -1}; // x axis vector
        int[] dz = {1, 0, -1, 0}; // z axis vector

        foreach (var node in nodes.Values)
        {
            for (int d = 0; d < 4; d++)
            {
                int neighbourX = node.XPosition + dx[d] * length;
                int neighbourZ = node.ZPosition + dz[d] * length;
                // string neighbourKey = $"{neighbourX},{neighbourZ}";
                (int, int) neighbourKey = (neighbourX, neighbourZ);

                if (nodes.ContainsKey(neighbourKey))
                {
                    Node neighbourNode = nodes[neighbourKey];
                    node.AdjNodes[neighbourKey] = neighbourNode;
                }
            }
        }
    }

    public void ChangeNodeOccupiedState(List<(int,int)> nodeKeys, bool occupied)
    {   
        foreach (var nodeKey in nodeKeys)
        {
            if (nodes.ContainsKey(nodeKey))
            {
                if (occupied)
                    nodes[nodeKey].BuildOnNode();
                else
                    nodes[nodeKey].RemoveBuildingOnNode();
            }
        }
        
    }

    public bool isOccupied((int,int) nodeKey)
    {
        if (nodes.ContainsKey(nodeKey))
        {
            return nodes[nodeKey].isOccupied;
        }
        else
        {   
            Debug.Log("key does not exists in graph");
            return false;
        }
    }


    public List<Vector3> GetShortestPath((int,int) startNodeKey, (int,int) endNodeKey)
    {   
        // astar algorithm
        Node startNode = nodes[startNodeKey];
        Node endNode = nodes[endNodeKey];
        var openSet = new PriorityQueue<Node>(); // gpt custom class
        var cameFrom = new Dictionary<Node, Node>();
        var gScore = new Dictionary<Node, float>();
        var fScore = new Dictionary<Node, float>();

        foreach (var node in nodes.Values)
        {
            gScore[node] = float.MaxValue;
            fScore[node] = float.MaxValue;
        }

        gScore[startNode] = 0;
        fScore[startNode] = Heuristic(startNode, endNode);
        openSet.Enqueue(startNode, fScore[startNode]);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet.Dequeue();
            if (currentNode == endNode)
            {
                return ReconstructPath(cameFrom, currentNode);
            }

            foreach (var neighbour in currentNode.AdjNodes.Values)
            {
                if (neighbour.isOccupied) continue;

                float tentativeGScore = gScore[currentNode] + Vector3.Distance(new Vector3(currentNode.XPosition, 0, currentNode.ZPosition),
                                                                               new Vector3(neighbour.XPosition, 0, neighbour.ZPosition));

                if (tentativeGScore < gScore[neighbour])
                {
                    cameFrom[neighbour] = currentNode;
                    gScore[neighbour] = tentativeGScore;
                    fScore[neighbour] = gScore[neighbour] + Heuristic(neighbour, endNode);
                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Enqueue(neighbour, fScore[neighbour]);
                    }
                }
            }
        }

        return new List<Vector3>(); // Return an empty path if no path is found
    }

    private List<Vector3> ReconstructPath(Dictionary<Node, Node> cameFrom, Node currentNode)
    {
        List<Vector3> path = new List<Vector3>();
        while (cameFrom.ContainsKey(currentNode))
        {
            path.Add(new Vector3(currentNode.XPosition, 0, currentNode.ZPosition));
            currentNode = cameFrom[currentNode];
        }
        path.Add(new Vector3(currentNode.XPosition, 0, currentNode.ZPosition));
        path.Reverse();
        return path;
    }

    private float Heuristic(Node a, Node b)
    {
        return Vector3.Distance(new Vector3(a.XPosition, 0, a.ZPosition), new Vector3(b.XPosition, 0, b.ZPosition));
    }
}

public class Node
{
    public int XPosition { get; }
    public int ZPosition { get; }
    public bool isOccupied = false; // This boolean indicates whether a building can be placed on this tile
    // public Dictionary<string, Node> AdjNodes { get; } = new Dictionary<string, Node>();
    // public string Key => $"{XPosition},{ZPosition}"; // generate node key

    public Dictionary<(int, int), Node> AdjNodes { get; } = new Dictionary<(int, int), Node>();
    public (int, int) Key => (XPosition, ZPosition);

    public Node(int xPosition, int zPosition)
    {
        XPosition = xPosition;
        ZPosition = zPosition;
    }

    public void BuildOnNode()
    {
        isOccupied = true;
    }

    public void RemoveBuildingOnNode()
    {
        isOccupied = false;
    }
}
