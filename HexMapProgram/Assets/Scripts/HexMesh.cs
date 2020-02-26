using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class HexMesh : MonoBehaviour
{
    private Mesh _hexMesh;

    private List<Vector3> _vertices;

    private List<int> _triangles;

    private List<Color> _colors;

    public void Init()
    {
        GetComponent<MeshFilter>().mesh = _hexMesh = new Mesh();
        _hexMesh.name = "Hex Mesh";

        _vertices = new List<Vector3>();
        _triangles = new List<int>();
        _colors = new List<Color>();
    }

    public void Triangulate(HexCell[] cells)
    {
        _hexMesh.Clear();
        _vertices.Clear();
        _triangles.Clear();
        _colors.Clear();

        for (int i = 0; i < cells.Length; i++)
        {
            Triangulate(cells[i]);
        }

        _hexMesh.vertices = _vertices.ToArray();
        _hexMesh.triangles = _triangles.ToArray(); ;
        _hexMesh.RecalculateNormals();
        _hexMesh.colors = _colors.ToArray();
        GetComponent<MeshCollider>().sharedMesh = _hexMesh;

    }

    private void Triangulate(HexCell cell)
    {
        Vector3 center = cell.transform.localPosition;

        for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++)
        {
            Triangulate(d, cell);
        }

    }

    private void Triangulate(HexDirection dir, HexCell cell)
    {
        Vector3 center = cell.transform.localPosition;

        Vector3 v1 = center + HexMertics.GetFirstSolidCornor(dir);

        Vector3 v2 = center + HexMertics.GetSecondSolidCornor(dir);

        ///第一个顶点的位置为cell正中心，其余为cell的顶点
        AddTriangle(center,v1,v2);
        AddTriangleColor(cell.color);


     
        ///根据方向获取相邻的节点
        HexCell neighbor = cell.GetNeighbor(dir) ?? cell;
        HexCell preNeighbor = cell.GetNeighbor(dir.Previous()) ?? cell;
        HexCell nextNeighbor = cell.GetNeighbor(dir.Next()) ?? cell;

        if (dir<=HexDirection.SE)
        {
            TriangulateConnection(dir, cell, v1, v2);
        }


        //AddTriangle(v1, center + HexMertics.GetFirstCornor(dir),v3);
        //AddTriangleColor(cell.color, (cell.color + preNeighbor.color + neighbor.color) / 3, bridgeColor);

        //AddTriangle(v2, v4, center + HexMertics.GetSecondCornor(dir));
        //AddTriangleColor(cell.color, bridgeColor, (cell.color + neighbor.color + nextNeighbor.color) / 3);
    }

    /// <summary>
    /// 绘制连接处的三角
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="cell"></param>
    /// <param name="v1"></param>
    /// <param name="v2"></param>
    private void TriangulateConnection(HexDirection direction,HexCell cell,Vector3 v1,Vector3 v2)
    {
        HexCell neighbor = cell.GetNeighbor(direction);

        if (neighbor==null)
        {
            return;
        }


        Vector3 bridge = HexMertics.GetBridge(direction);

        Vector3 v3 = v1 + bridge;

        Vector3 v4 = v2 + bridge;

        AddQuad(v1, v2, v3, v4);
        AddQuadColor(cell.color, neighbor.color);


        HexCell nextNeighbor = cell.GetNeighbor(direction.Next());

        if (direction<=HexDirection.E&&nextNeighbor!=null)
        {
            AddTriangle(v2, v4, v2+HexMertics.GetBridge(direction.Next()));
            AddTriangleColor(cell.color, neighbor.color, nextNeighbor.color);
        }

    }





    /// <summary>
    /// 为三角形着色
    /// </summary>
    /// <param name="c1"></param>
    /// <param name="c2"></param>
    /// <param name="c3"></param>
    private void AddTriangleColor(Color c1)
    {
        _colors.Add(c1);
        _colors.Add(c1);
        _colors.Add(c1);
    }

    /// <summary>
    /// 三角形着色
    /// </summary>
    /// <param name="c1"></param>
    /// <param name="c2"></param>
    /// <param name="c3"></param>
    private void AddTriangleColor(Color c1,Color c2,Color c3)
    {
        _colors.Add(c1);
        _colors.Add(c2);
        _colors.Add(c3);
    }


    private void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        int vertexIdx = _vertices.Count;

        _vertices.Add(v1);
        _vertices.Add(v2);
        _vertices.Add(v3);

        _triangles.Add(vertexIdx);
        _triangles.Add(vertexIdx + 1);
        _triangles.Add(vertexIdx + 2);

    }
    /// <summary>
    /// 生成混合区域面片（梯形）
    /// </summary>
    /// <param name="v1"></param>
    /// <param name="v2"></param>
    /// <param name="v3"></param>
    /// <param name="v4"></param>
    private void AddQuad(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4)
    {
        int vertexIndex = _vertices.Count;

        _vertices.Add(v1);
        _vertices.Add(v2);
        _vertices.Add(v3);
        _vertices.Add(v4);

        ///顺时针画才能在y轴正反向看到
        _triangles.Add(vertexIndex);
        _triangles.Add(vertexIndex + 2);
        _triangles.Add(vertexIndex + 1);

        _triangles.Add(vertexIndex + 1);
        _triangles.Add(vertexIndex + 2);
        _triangles.Add(vertexIndex + 3);
    }

    private void AddQuadColor(Color c1, Color c2)
    {
        _colors.Add(c1);
        _colors.Add(c1);
        _colors.Add(c2);
        _colors.Add(c2);
    }



}
