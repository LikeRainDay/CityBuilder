using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public SVS.CameraMovement cameraMovement;
    public RoadManager roadManager;
    public InputManager inputManager;

    public UIController uiController;

    public StructureManager structureManager;

    private void Start()
    {
        uiController.OnRoadPlacement += RoadPlacementHandler;
        uiController.OnHousePlacement += HousePlacementHandler;
        uiController.OnRoadPlacement += SpecialPlacementHandler;
    }

    private void SpecialPlacementHandler()
    {
        ClearInputAction();
        inputManager.OnMouseClick += structureManager.PlaceSpecial;
    }

    private void HousePlacementHandler()
    {
        ClearInputAction();
        inputManager.OnMouseClick += structureManager.PlaceHouse;
    }

    private void RoadPlacementHandler()
    {
        ClearInputAction();

        inputManager.OnMouseClick += roadManager.PlaceRoad;
        inputManager.OnMouseHold += roadManager.PlaceRoad;
        inputManager.OnMouseUp += roadManager.FinishPlacingRoad;
    }

    private void ClearInputAction()
    {
        inputManager.OnMouseClick = null;
        inputManager.OnMouseHold = null;
        inputManager.OnMouseUp = null;
    }

    private void Update()
    {
        cameraMovement.MoveCamera(new Vector3(inputManager.CameraMovementVector.x, 0,
            inputManager.CameraMovementVector.y));
    }
}