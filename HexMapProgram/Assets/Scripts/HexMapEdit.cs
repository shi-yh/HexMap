using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HexMapEdit : MonoBehaviour
{
    public Color[] colors;

    public HexGrid hexGrid;

    private Color _activeColor;

    private void Awake()
    {
        SelectColor(0);
    }

    public void SelectColor(int v)
    {
        _activeColor = colors[v];
    }


   
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0)&&!EventSystem.current.IsPointerOverGameObject())
        {
            HandleInput();
        }
    }

    private void HandleInput()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(inputRay,out hit))
        {
            hexGrid.ColorCell(hit.point, _activeColor);
        }
    }
}
