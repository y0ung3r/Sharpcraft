using System.Collections.Generic;
using UnityEngine;

public class UpperBlockSide : BlockSide
{
	public override Vector3Int Direction 
		=> Vector3Int.up;

	public override IEnumerable<Vector2> GetUVs()
	{
		yield return new Vector2(48.0f / 64.0f, 16.0f / 48.0f);
		yield return new Vector2(48.0f / 64.0f, 32.0f / 48.0f);
		yield return new Vector2(1.0f, 16.0f / 48.0f);
		yield return new Vector2(1.0f, 32.0f / 48.0f);
	}

	protected override IEnumerable<Vector3> GetVertices()
	{
		yield return new Vector3(0, 1, 0);
		yield return new Vector3(0, 1, 1);
		yield return new Vector3(1, 1, 0);
		yield return new Vector3(1, 1, 1);
	}
}
