using System;
using System.Linq;
using SVS;
using UnityEngine;

public class StructureManager : MonoBehaviour
{
  public StructurePrefabWeighted[] housesPrefab, specialPrefabs;
  public PlacementManager placementManager;

  private float[] houseWeights, specialWeight;

  private void Start()
  {
    houseWeights = housesPrefab.Select(prefabStats => prefabStats.weight).ToArray();
    specialWeight = specialPrefabs.Select(prefabStats => prefabStats.weight).ToArray();
  }

  // ReSharper disable Unity.PerformanceAnalysis
  public void PlaceHouse(Vector3Int position)
  {
    if (!CheckPositionBeforePlacement(position)) return;
    var randomIndex = GetRandomWeightedIndex(houseWeights);
    placementManager.PlaceObjectOnTheMap(position, housesPrefab[randomIndex].prefab, CellType.Structure);
    AudioPlayer._instance.PlayPlacementSound();
  }

  // ReSharper disable Unity.PerformanceAnalysis
  public void PlaceSpecial(Vector3Int position)
  {
    if (!CheckPositionBeforePlacement(position)) return;
    var randomIndex = GetRandomWeightedIndex(specialWeight);
    placementManager.PlaceObjectOnTheMap(position, housesPrefab[randomIndex].prefab, CellType.Structure);
    AudioPlayer._instance.PlayPlacementSound();
  }

  // ReSharper disable Unity.PerformanceAnalysis
  private bool CheckPositionBeforePlacement(Vector3Int position)
  {
    if (placementManager.CheckIfPositionInBound(position) == false)
    {
      Debug.Log("This position is out of bound");
      return false;
    }

    if (placementManager.CheckIfPositionInBound(position) == false)
    {
      Debug.Log("This position is not EMPTY");
      return false;
    }

    if (placementManager.GetNeighboursOfTypeFor(position, CellType.Road).Count > 0) return true;
    Debug.Log("Must be placed near a road");
    return false;
  }

  private int GetRandomWeightedIndex(float[] weights)
  {
    var sum = 0f;
    for (var i = weights.Length - 1; i >= 0; i--)
    {
      sum += weights[i];
    }

    var randomValue = UnityEngine.Random.Range(0, sum);
    float tempSum = 0;
    for (var i = weights.Length - 1; i >= 0; i--)
    {
      if (randomValue >= tempSum && randomValue < tempSum + weights[i])
      {
        return i;
      }

      tempSum += weights[i];
    }

    return 0;
  }
}

[Serializable]
public struct StructurePrefabWeighted
{
  public GameObject prefab;
  [Range(0, 1)] public float weight;
}
