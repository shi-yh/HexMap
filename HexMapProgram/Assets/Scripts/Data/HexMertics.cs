using UnityEngine;

/// <summary>
/// 定义六边形内外径的长度
/// </summary>
public static class HexMertics
{
    /// <summary>
    /// 六边形外径
    /// </summary>
    public const float outerRadius = 10f;
    /// <summary>
    /// 六边形内径
    /// </summary>
    public const float innerRadius = outerRadius * 0.866025404f;
    /// <summary>
    /// 固定颜色的区域
    /// </summary>
    public const float solidFactor = 0.75f;
    /// <summary>
    /// 混合颜色的区域
    /// </summary>
    public const float blendFactor = 1 - solidFactor;

    /// <summary>
    /// 每层的高度值
    /// </summary>
    public const float elevationStep = 5f;


    private static Vector3[] corners =
    {
        ///最上方顶点
        new Vector3(0,0,outerRadius),
        new Vector3(innerRadius,0,0.5f*outerRadius),
        new Vector3(innerRadius,0,-0.5f*outerRadius),
        new Vector3(0,0,-outerRadius),
        new Vector3(-innerRadius,0,-0.5f*outerRadius),
        new Vector3(-innerRadius,0,0.5f*outerRadius),
        new Vector3(0,0,outerRadius),

    };

    /// <summary>
    /// 获取某方向的第一个顶点（由两个顶点组成的一个线的法向决定方向）
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public static Vector3 GetFirstCornor(HexDirection direction)
    {
        return corners[(int)direction];
    }

    /// <summary>
    /// 获取某方向的第二个顶点
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public static Vector3 GetSecondCornor(HexDirection direction)
    {
        return corners[(int)direction+1];
    }

    public static Vector3 GetFirstSolidCornor(HexDirection direction)
    {
        return corners[(int)direction] * solidFactor;
    }

    public static Vector3 GetSecondSolidCornor(HexDirection direction)
    {
        return corners[(int)direction + 1] * solidFactor;
    }

    /// <summary>
    /// 算出Z轴偏移量
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public static Vector3 GetBridge(HexDirection direction)
    {
        return (corners[(int)direction] + corners[(int)direction + 1]) * blendFactor;
    }

}


public enum HexDirection
{
    /// <summary>
    /// 东北
    /// </summary>
    NE=0,
    /// <summary>
    /// 东
    /// </summary>
    E,
    /// <summary>
    /// 东南
    /// </summary>
    SE,
    /// <summary>
    /// 西南
    /// </summary>
    SW,
    /// <summary>
    /// 西
    /// </summary>
    W,
    /// <summary>
    /// 西北
    /// </summary>
    NW
}

/// <summary>
/// 扩展方法是一个静态类中的静态方法，但使用起来像是某些类型的实例方法
/// 扩展方法的第一个参数之前必须有this关键字，它定义方法将操作类型和实例的值
/// </summary>
public static class HexDirectionExtensions
{
    public static HexDirection Opposite(this HexDirection direction)
    {
        return (int)direction < 3 ? (direction + 3) : (direction - 3);
    }

    /// <summary>
    /// 跳转到上一个方向
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public static HexDirection Previous(this HexDirection direction)
    {
        return direction == HexDirection.NE ? HexDirection.NW : (direction - 1);
    }

    public static HexDirection Next(this HexDirection direction)
    {
        return direction==HexDirection.NW?HexDirection.NE:(direction + 1);
    }


}
