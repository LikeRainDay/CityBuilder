using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    // define ui button controller
    //
    public Action OnRoadPlacement;
    public Action OnHousePlacement, OnSpecialPlacment;
    public Button placeRoadButton, placeHouseButton;

    public Color outlineColor;

    private List<Button> _buttonList;

    private void Start()
    {
        _buttonList = new List<Button> { placeRoadButton, placeHouseButton };
        placeRoadButton.onClick.AddListener(() =>
        {
            ResetButtonColor();
            ModifyOutline(placeRoadButton);
            OnRoadPlacement?.Invoke();
        });

        placeHouseButton.onClick.AddListener(() =>
        {
            ResetButtonColor();
            ModifyOutline(placeHouseButton);
            // var houseModel = new HouseModel
            // {
            //     content = "",
            //     des = ""
            // };
            OnHousePlacement?.Invoke();
        });
    }

    private void ModifyOutline(Button button)
    {
        var outline = button.GetComponent<Outline>();
        outline.effectColor = outlineColor;
        outline.enabled = true;
    }

    private void ResetButtonColor()
    {
        foreach (var button in _buttonList)
        {
            button.GetComponent<Outline>().enabled = false;
        }
    }
}
