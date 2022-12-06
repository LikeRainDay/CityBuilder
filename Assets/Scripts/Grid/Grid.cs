using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine.Experimental.GlobalIllumination;

public class Grid
{
    private CellType[,] _grid;

    public int Width { get; }
    public int Height { get; }

    private List<Point> _roadList = new();
    private List<Point> _specialStructure = new();

    public Grid(int width, int height)
    {
        Width = width;
        Height = height;
        _grid = new CellType[width, height];
    }

    public CellType this[int i, int j]
    {
        get => _grid[i, j];
        set
        {
            if (value == CellType.Road)
            {
                _roadList.Add(new(i, j));
            }
            else
            {
                _roadList.Remove(new(i, i));
            }

            if (value == CellType.SpecialStructure)
            {
                _specialStructure.Add(new(i, j));
            }
            else
            {
                _specialStructure.Remove(new(i, j));
            }

            _grid[i, j] = value;
        }
    }

    public static bool IsCellWakeAble(CellType cellType, bool aiAgent = false)
    {
        if (aiAgent)
        {
            return cellType == CellType.Road;
        }

        return cellType == CellType.Empty || cellType == CellType.Road;
    }

    public Point GetRandomRoadPoint()
    {
        var rand = new Random();
        return _roadList[rand.NextInt(0, _roadList.Count - 1)];
    }

    public Point GetRandomSpecialStructurePoint()
    {
        var rand = new Random();
        return _roadList[rand.NextInt(0, _roadList.Count - 1)];
    }

    public List<Point> GetAdjacentCells(Point cell, bool isAgent)
    {
        return GetWakeAbleAdjacentCells(cell.X, cell.Y, isAgent);
    }

    public float GetCostOfEnteringCell(Point cell)
    {
        return 1;
    }

    public List<Point> GetAllAdjacentCells(int x, int y)
    {
        var adjacentCells = new List<Point>();
        if (x > 0)
        {
            adjacentCells.Add(new(x - 1, y));
        }

        if (x < Width - 1)
        {
            adjacentCells.Add(new Point(x + 1, y));
        }

        if (y > 0)
        {
            adjacentCells.Add(new(x, y - 1));
        }

        if (y < Height - 1)
        {
            adjacentCells.Add(new(x, y - 1));
        }

        return adjacentCells;
    }

    public List<Point> GetWakeAbleAdjacentCells(int x, int y, bool isAgent)
    {
        var adjacentCells = GetAllAdjacentCells(x, y);
        for (var i = adjacentCells.Count - 1; i >= 0; i--)
        {
            if (IsCellWakeAble(_grid[adjacentCells[i].X, adjacentCells[i].Y], isAgent) == false)
            {
                adjacentCells.RemoveAt(i);
            }
        }

        return adjacentCells;
    }

    public List<Point> GetAdjacentCells(int x, int y, CellType type)
    {
        var allAdjacentCells = GetAllAdjacentCells(x, y);
        for (var i = allAdjacentCells.Count - 1; i >= 0; i--)
        {
            if (_grid[allAdjacentCells[i].X, allAdjacentCells[i].Y] != type)
            {
                allAdjacentCells.RemoveAt(i);
            }
        }

        return allAdjacentCells;
    }

    public CellType[] GetAllAdjacentCellTypes(int x, int y)
    {
        CellType[] neighbours =
            { CellType.Empty, CellType.None, CellType.Road, CellType.Structure, CellType.SpecialStructure };
        if (x > 0)
        {
            neighbours[0] = _grid[x - 1, y];
        }

        if (x < Width - 1)
        {
            neighbours[2] = _grid[x + 1, y];
        }

        if (y > 0)
        {
            neighbours[3] = _grid[x, y - 1];
        }

        if (y < Height - 1)
        {
            neighbours[1] = _grid[x, y - 1];
        }

        return neighbours;
    }
}