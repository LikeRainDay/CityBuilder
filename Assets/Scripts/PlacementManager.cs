using System;
using System.Collections.Generic;
using UnityEngine;

public class PlacementManager: MonoBehaviour
{
    public int width, height;
    private Grid placementGrid;

    private Dictionary<Vector3Int, StructureModel> _temporaryRoadobjects = new();
    private Dictionary<Vector3Int, StructureModel> _structureDictonary = new();

    private void Start()
    {
        placementGrid = new Grid(width, height);
    }

    internal CellType[] GetNeighbourTypesFor(Vector3Int position)
    {
        return placementGrid.GetAllAdjacentCellTypes(position.x, position.y);
    }

    internal bool CheckIfPositionInBound(Vector3Int position)
    {
        if (position.x >= 0 && position.x < width && position.z >= 0 && position.z < height)
        {
            return true;
        }

        return false;
    }

    internal void PlaceObjectOnTheMap(Vector3Int position, GameObject structurePrefab, CellType type)
    {
        placementGrid[position.x, position.y] = type;
        // TODO need to be improved
    }
    
}