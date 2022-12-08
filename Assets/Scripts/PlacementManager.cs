using System;
using System.Collections.Generic;
using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    public int width, height;
    private Grid _placementGrid;

    private Dictionary<Vector3Int, StructureModel> _temporaryRoadobjects = new();
    private Dictionary<Vector3Int, StructureModel> _structureDictonary = new();

    private void Start()
    {
        _placementGrid = new Grid(width, height);
    }

    internal CellType[] GetNeighbourTypesFor(Vector3Int position)
    {
        return _placementGrid.GetAllAdjacentCellTypes(position.x, position.y);
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
        _placementGrid[position.x, position.y] = type;
        StructureModel structure = CreateANewStructureModel(position, structurePrefab, type);
        _structureDictonary.Add(position, structure);
        DestroyNatureAt(position);
    }

    private void DestroyNatureAt(Vector3Int position)
    {
        RaycastHit[] hits = Physics.BoxCastAll(position + new Vector3(0, 0.5f, 0), new Vector3(0.5f, 0.5f, 0.5f),
            transform.up,
            Quaternion.identity, 1f, 1 << LayerMask.NameToLayer($"Nature"));
        foreach (var raycastHit in hits)
        {
            Destroy(raycastHit.collider.gameObject);
        }
    }

    private StructureModel CreateANewStructureModel(Vector3Int position, GameObject structurePrefab, CellType type)
    {
        var structure = new GameObject(type.ToString());
        structure.transform.SetParent(transform);
        structure.transform.localPosition = position;
        var structureModel = structure.AddComponent<StructureModel>();
        structureModel.CreateModel(structurePrefab);
        return structureModel;
    }

    internal bool CheckIfPositionIsFree(Vector3Int position)
    {
        return CheckIfPositionIsOfType(position, CellType.Empty);
    }

    private bool CheckIfPositionIsOfType(Vector3Int position, CellType type)
    {
        return _placementGrid[position.x, position.y] == type;
    }

    internal void PlacementTemporaryStructure(Vector3Int position, GameObject structurePrefab, CellType type)
    {
        _placementGrid[position.x, position.y] = type;
        StructureModel structure = CreateANewStructureModel(position, structurePrefab, type);
        _temporaryRoadobjects.Add(position, structure);
    }

    internal List<Vector3Int> GetNeighboursOfTypeFor(Vector3Int position, CellType type)
    {
        var adjacentCellOfType = _placementGrid.GetAdjacentCellOfType(position.x, position.z, type);
        var neighbours = new List<Vector3Int>();
        foreach (var point in neighbours)
        {
            neighbours.Add(new Vector3Int(point.x, 0, point.y));
        }

        return neighbours;
    }

    internal List<Vector3Int> GetPathBetween(Vector3Int startPosition, Vector3Int endPosition)
    {
        var resultPath = GridSearch.AStartSearch(_placementGrid, new Point(startPosition.x, startPosition.z),
            new Point(endPosition.x, endPosition.z));
        var path = new List<Vector3Int>();
        foreach (var point in resultPath)
        {
            path.Add(new Vector3Int(point.X, point.Y));
        }

        return path;
    }

    internal void RemoveAllTemporaryStructures()
    {
        foreach (var structure in _temporaryRoadobjects.Values)
        {
            var position = Vector3Int.RoundToInt(structure.transform.position);
            _placementGrid[position.x, position.z] = CellType.Empty;
            Destroy(structure.gameObject);
        }

        _temporaryRoadobjects.Clear();
    }

    internal void AddTemporaryStructuresToStructureDictionary()
    {
        foreach (var structure in _temporaryRoadobjects)
        {
            _structureDictonary.Add(structure.Key, structure.Value);
            DestroyNatureAt(structure.Key);
        }

        _temporaryRoadobjects.Clear();
    }

    public void ModifyStructureModel(Vector3Int position, GameObject newModel, Quaternion rotation)
    {
        if (_temporaryRoadobjects.ContainsKey(position))
        {
            _temporaryRoadobjects[position].SwapModel(newModel, rotation);
        }
        else if (_structureDictonary.ContainsKey(position))
        {
            _structureDictonary[position].SwapModel(newModel, rotation);
        }
    }
}