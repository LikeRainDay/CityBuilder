using System;
using System.Collections.Generic;


// Introduction to the A* Algorithm [https://www.redblobgames.com/pathfinding/a-star/introduction.html] 
public class GridSearch
{
    public struct SearchResult
    {
        public List<Point> path { get; set; }
    }

    public static List<Point> AStartSearch(Grid grid, Point startPosition, Point endPosition, bool isAgent = false)
    {
        var path = new List<Point>();

        var positionToCheck = new List<Point>();
        var cosDictionary = new Dictionary<Point, float>();
        var priorityDictionary = new Dictionary<Point, float>();
        var parentsDictionary = new Dictionary<Point, Point>();

        positionToCheck.Add(startPosition);
        priorityDictionary.Add(startPosition, 0);
        cosDictionary.Add(startPosition, 0);
        parentsDictionary.Add(startPosition, null);

        while (positionToCheck.Count > 0)
        {
            var current = GetClosestVertex(positionToCheck, priorityDictionary);
            positionToCheck.Remove(current);
            if (current.Equals(endPosition))
            {
                path = GeneratePath(parentsDictionary, current);
                return path;
            }

            foreach (var neighbour in grid.GetAdjacentCells(current, isAgent))
            {
                var newCost = cosDictionary[current] + grid.GetCostOfEnteringCell(neighbour);
                if (cosDictionary.ContainsKey(neighbour) && !(newCost < cosDictionary[neighbour])) continue;
                cosDictionary[neighbour] = newCost;

                var priority = newCost + ManhattanDiscance(endPosition, neighbour);
                positionToCheck.Add(neighbour);
                priorityDictionary[neighbour] = priority;

                parentsDictionary[neighbour] = current;
            }
        }

        return path;
    }

    private static float ManhattanDiscance(Point endPosition, Point neighbour)
    {
        return Math.Abs(endPosition.X - neighbour.X) + Math.Abs(endPosition.Y - neighbour.Y);
    }

    public static Point GetClosestVertex(List<Point> list, Dictionary<Point, float> distanceMap)
    {
        var candidate = list[0];
        foreach (var point in list)
        {
            if (distanceMap[point] < distanceMap[candidate])
            {
                candidate = point;
            }
        }

        return candidate;
    }

    public static List<Point> GeneratePath(Dictionary<Point, Point> parentMap, Point endState)
    {
        var path = new List<Point>();
        Point parent = endState;
        while (parent != null && parentMap.ContainsKey(parent))
        {
            path.Add(parent);
            parent = parentMap[parent];
        }

        return path;
    }
}