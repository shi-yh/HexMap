using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour
{
    [Header("网格宽")]
    public int width = 6;
    [Header("网格高")]
    public int height = 6;

    /// <summary>
    /// 用来显示坐标的UI
    /// </summary>
    public Text cellLabelPrefab;

    /// <summary>
    /// 生成的物体
    /// </summary>
    public HexCell cellPrefab;

    private HexCell[] cells;

    private Transform _labelParent;

    private Transform _cellParent;

    private HexMesh _hexMesh;

    private void Awake()
    {
        _labelParent = transform.Find("HexGridCanvas");
        _cellParent = transform.Find("HexGrid");
        _hexMesh = transform.GetComponentInChildren<HexMesh>();

        _hexMesh.Init();

        cells = new HexCell[height * width];

        int idx = 0;

        ///在xoz面上
        for (int z = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                CreateCell(x, z, idx);
                idx++;
            }
        }
    }

    /// <summary>
    /// 创建单元格
    /// </summary>
    /// <param name="x">x轴位置</param>
    /// <param name="z">z轴位置</param>
    /// <param name="idx">是第几个</param>
    private void CreateCell(int x, int z, int idx)
    {
        Vector3 position = new Vector3();

        ///每行的距离是外径的1.5倍
        position.z = z * HexMertics.outerRadius * 1.5f;

        ///每列的间隔是内径的两倍，同时，偶数行需要向正方向偏移内径长度
        position.x = (x + z%2*0.5f) * HexMertics.innerRadius * 2f;

        ///在对应的位置生成单元格
        HexCell cell = cells[idx] = Instantiate(cellPrefab,position,Quaternion.identity,_cellParent);
        ///为单元格赋值游戏内坐标
        cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);

        if (x>0)
        {
            ///每一行的单元格，除了第一个，都有左相邻
            cell.SetNeighbor(HexDirection.W, cells[idx - 1]);
        }

        if (z>0)
        {
            ///偶数的最后一位肯定是0
            if ((z&1)==0)
            {
                ///和下方行同索引位置成东南相邻
                cell.SetNeighbor(HexDirection.SE, cells[idx - width]);

                ///除了每行第一个，都存在西北相邻的格子
                if (x > 0)
                {
                    cell.SetNeighbor(HexDirection.SW, cells[idx - width - 1]);
                }
            }
            else
            {
                ///奇数行全部存在西南相邻
                cell.SetNeighbor(HexDirection.SW, cells[idx - width]);

                ///除了最后一个之外，都存在东南方向的相邻
                if (x<width-1)
                {
                    cell.SetNeighbor(HexDirection.SE, cells[idx - width + 1]);
                }

            }

        }


        Text label = Instantiate(cellLabelPrefab, _labelParent);

        label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);

        label.text =cell.coordinates.ToStringOnSeparateLines();

    }




    // Start is called before the first frame update
    void Start()
    {
        _hexMesh.Triangulate(cells);
    }

    /// <summary>
    /// 根据位置获取
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public HexCell GetCell(Vector3 position)
    {
        position = transform.InverseTransformPoint(position);
        HexCoordinates coorinates = HexCoordinates.FromPosition(position);

        int idx = coorinates.X + coorinates.Z * width + coorinates.Z / 2;

        return cells[idx];
    }

    /// <summary>
    /// 改变颜色
    /// </summary>
    /// <param name="position"></param>
    /// <param name="color"></param>
    public void ColorCell(Vector3 position,Color color)
    {
        position = transform.InverseTransformPoint(position);
        HexCoordinates coordinates = HexCoordinates.FromPosition(position);
        int idx = coordinates.X + coordinates.Z * width + coordinates.Z / 2;

        HexCell cell = cells[idx];

        cell.color = color;

        _hexMesh.Triangulate(cells);

    }
}
