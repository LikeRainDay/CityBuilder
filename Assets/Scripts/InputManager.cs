using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    public Action<Vector3Int> OnMouseClick, OnMouseHold;
    public Action OnMouseUp;
    private Vector2 _cameraMovementVector;

    [SerializeField] Camera mainCamera;

    public LayerMask groundMask;

    public Vector2 CameraMovementVector => _cameraMovementVector;

    private void Update()
    {
        CheckClickDownEvent();
        CheckClickUpEvent();
        CheckClickHoldEvent();
        CheckArrowInput();
    }

    private Vector3Int? RaycastGround()
    {
        var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out var hit, Mathf.Infinity, groundMask)) return null;
        var positionInt = Vector3Int.RoundToInt(hit.point);
        return positionInt;
    }


    private void CheckArrowInput()
    {
        _cameraMovementVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void CheckClickHoldEvent()
    {
      if (!Input.GetMouseButton(0) || EventSystem.current.IsPointerOverGameObject() != false) return;
      var position = RaycastGround();
      if (position != null)
      {
        OnMouseHold?.Invoke(position.Value);
      }
    }

    private void CheckClickUpEvent()
    {
        if (Input.GetMouseButton(0) && EventSystem.current.IsPointerOverGameObject() == false)
        {
            OnMouseUp?.Invoke();
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void CheckClickDownEvent()
    {
      if (!Input.GetMouseButton(0) || EventSystem.current.IsPointerOverGameObject()) return;
      var position = RaycastGround();
      if (position != null)
      {
        OnMouseClick?.Invoke(position.Value);
      }
    }
}
