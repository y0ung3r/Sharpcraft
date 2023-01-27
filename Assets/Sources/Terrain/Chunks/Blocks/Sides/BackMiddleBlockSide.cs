using System.Collections.Generic;
using UnityEngine;

internal class BackMiddleBlockSide : BlockSide
{
    public override Vector3Int Direction
        => Vector3Int.back;

    protected override IEnumerable<Vector3> GetVertices()
    {
        yield return new Vector3(0.5f, 0, 0.5f);
        yield return new Vector3(0.5f, 0.5f, 0.5f);
        yield return new Vector3(0, 0, 0.5f);
        yield return new Vector3(0, 0.5f, 0.5f);
    }
}