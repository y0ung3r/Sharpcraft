using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Blocks/Assets/Solid")]
public class SolidBlock : BlockAsset
{
    public override IEnumerable<Vector2> GetUVs(BlockSide side)
    {
        yield return new Vector2(0.0f, 0.0f);
        yield return new Vector2(0.0f, 1.0f);
        yield return new Vector2(1.0f, 0.0f);
        yield return new Vector2(1.0f, 1.0f);
    }
}