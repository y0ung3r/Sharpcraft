using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Blocks/Assets/Cruciform")]
public class CruciformBlock : SolidBlock
{
    public override IEnumerable<BlockSide> GetSides(Block block) => new BlockSide[4]
    {
        new MainDiagonal(),
        new AuxiliaryDiagonal(),
        new BackMainDiagonal(),
        new BackAuxiliaryDiagonal()
    };
}