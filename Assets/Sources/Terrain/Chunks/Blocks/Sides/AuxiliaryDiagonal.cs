using System.Collections.Generic;
using UnityEngine;

public class AuxiliaryDiagonal : BlockSide
{
    public override Vector3Int Direction
        => Vector3Int.left + Vector3Int.forward;

    protected override IEnumerable<Vector3> GetVertices()
    {
        yield return new Vector3(1, 0, 0);
        yield return new Vector3(1, 1, 0);
        yield return new Vector3(0, 0, 1);
        yield return new Vector3(0, 1, 1);
    }
}