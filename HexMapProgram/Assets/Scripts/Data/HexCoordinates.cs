using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HexCoordinates 
{
    [SerializeField]
    private int x, z;

    public int X { get { return x; } }
    
    /// <summary>
    /// X+Y+Z恒等于0
    /// </summary>
    public int Y { get {
            return -X - Z;
        } }

    public int Z { get { return z; } }

    public HexCoordinates(int x,int z)
    {
        this.x = x;
        this.z = z;
    }

    //由普通坐标转化为六边形坐标
    public static HexCoordinates FromOffsetCoordinates(int x,int z)
    {
        return new HexCoordinates(x-z/2, z);
    }

    /// <summary>
    /// 根据提供的坐标，获得当前点选的六边形
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public static HexCoordinates FromPosition(Vector3 position)
    {
        ///x坐标的间隔是两倍内径
        float x = position.x / (HexMertics.innerRadius * 2f);
        float y = -x;

        ///当Z不为0时，x会产生偏移

        float offset = position.z / (HexMertics.outerRadius * 1.5f*2);

        x -= offset;
        y -= offset;


        int iX = Mathf.RoundToInt(x);
        int iY = Mathf.RoundToInt(y);
        int iZ = Mathf.RoundToInt(-x-y);


        if (iX+iY+iZ!=0)
        {
            float dx = Mathf.Abs(x - iX);
            float dy = Mathf.Abs(y - iY);
            float dz = Mathf.Abs(-x-y - iZ);

            if (dx>dy&&dx>dz)
            {
                iX = -iY - iZ;
            }
            else if (dz>dy)
            {
                iZ = -iX - iY;
            }

        }





        return new HexCoordinates(iX, iZ);
    }



    public override string ToString()
    {
        return "{" + X.ToString() + ","+Y.ToString()+"," + Z.ToString() + "}";
    }

    public string ToStringOnSeparateLines()
    {
        return X.ToString() + "\n" +Y.ToString()+"\n"+ Z.ToString();
    }





}
