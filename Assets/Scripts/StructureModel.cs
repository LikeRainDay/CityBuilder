using UnityEngine;

public class StructureModel : MonoBehaviour
{
    private float yHeight = 0;

    public void CreateModel(GameObject model)
    {
        var structure = Instantiate(model);
        yHeight = structure.transform.position.y;
    }

    public void SwapModel(GameObject model, Quaternion rotation)
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        var structure = Instantiate(model, transform);
        structure.transform.localPosition = new(0, yHeight, 0);
        structure.transform.localRotation = rotation;
    }
}