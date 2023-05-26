// Here is a basic implementation of the A* algorithm in C#:

using System;
using System.Collections.Generic;
using UnityEngine;

public class AStarNode:MonoBehaviour
{
    public int x;
    public int y;
    public bool isObstacle;
    public List<AStarNode> neighbors;
    public AStarNode parent;
    public int gCost;
    public int hCost;
    public int fCost { get { return gCost + hCost; } }

    public AStarNode(int x, int y, bool isObstacle)
    {
        this.x = x;
        this.y = y;
        this.isObstacle = isObstacle;
        neighbors = new List<AStarNode>();
    }

    public void Start()
    {
        // Create a grid of AStarNodes
        AStarNode[,] grid = new AStarNode[10, 10];
        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                bool isObstacle = false; // Set this to true if this node is an obstacle
                grid[x, y] = new AStarNode(x, y, isObstacle);
            }
        }

        // Create an instance of the AStar class
        AStar aStar = new AStar(grid);

        // Find the path from (0, 0) to (9, 9)
        AStarNode startNode = grid[0, 0];
        AStarNode endNode = grid[9, 9];
        List<AStarNode> path = aStar.FindPath(startNode, endNode);

        // Print the path
        foreach (AStarNode node in path)
        {
            Console.WriteLine("(" + node.x + ", " + node.y + ")");
        }
    }



    //public void CalculateFCost()
    //{
    //    fCost = gCost + hCost;
    //}
}

public class AStar
{
    private AStarNode[,] grid;
    private List<AStarNode> openList;
    private List<AStarNode> closedList;

    public AStar(AStarNode[,] grid)
    {
        this.grid = grid;
        openList = new List<AStarNode>();
        closedList = new List<AStarNode>();
    }

    public List<AStarNode> FindPath(AStarNode startNode, AStarNode endNode)
    {
        openList.Add(startNode);

        while (openList.Count > 0)
        {
            AStarNode currentNode = openList[0];

            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].fCost < currentNode.fCost || openList[i].fCost == currentNode.fCost && openList[i].hCost < currentNode.hCost)
                {
                    currentNode = openList[i];
                }
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (currentNode == endNode)
            {
                return GetPath(startNode, endNode);
            }

            foreach (AStarNode neighbor in currentNode.neighbors)
            {
                if (neighbor.isObstacle || closedList.Contains(neighbor))
                {
                    continue;
                }

                int newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);
                if (newMovementCostToNeighbor < neighbor.gCost || !openList.Contains(neighbor))
                {
                    neighbor.gCost = newMovementCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, endNode);
                    neighbor.parent = currentNode;

                    if (!openList.Contains(neighbor))
                    {
                        openList.Add(neighbor);
                    }
                }
            }
        }

        return null;
    }

    private List<AStarNode> GetPath(AStarNode startNode, AStarNode endNode)
    {
        List<AStarNode> path = new List<AStarNode>();
        AStarNode currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();
        return path;
    }

    private int GetDistance(AStarNode nodeA, AStarNode nodeB)
    {
        int dstX = Mathf.Abs(nodeA.x - nodeB.x);
        int dstY = Mathf.Abs(nodeA.y - nodeB.y);

        if (dstX > dstY)
        {
            return 14 * dstY + 10 * (dstX - dstY);
        }
        else
        {
            return 14 * dstX + 10 * (dstY - dstX);
        }
    }


}

// To use this implementation, you would first need to create a grid of AStarNodes, where each node represents a point on the map and has a boolean value indicating whether it is an obstacle or not. You would then create an instance of the AStar class and call its FindPath method, passing in the start and end nodes. The method will return a list of nodes representing the path from the start to the end.