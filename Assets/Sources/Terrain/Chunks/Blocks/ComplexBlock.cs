using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Blocks/Assets/Complex")]
public class ComplexBlock : BlockAsset
{
    public override IEnumerable<Vector2> GetUVs(BlockSide side)
        => side.GetUVs();
}