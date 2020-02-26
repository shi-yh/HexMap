using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCell : MonoBehaviour
{
    /// <summary>
    /// 坐标
    /// </summary>
    public HexCoordinates coordinates;
    /// <summary>
    /// 颜色
    /// </summary>
    public Color color = Color.white;
    /// <summary>
    /// 高度
    /// </summary>
    public int elevation;


    [SerializeField]
    private HexCell[] neighbors;


    /// <summary>
    /// 根据方向获取相邻单元格
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public HexCell GetNeighbor(HexDirection direction)
    {
        return neighbors[(int)direction];
    }


    /// <summary>
    /// 设置相邻单元格
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="cell"></param>
    public void SetNeighbor(HexDirection direction,HexCell cell)
    {
       
        neighbors[(int)direction] = cell;
        cell.neighbors[(int)direction.Opposite()]=this;
    }

}
