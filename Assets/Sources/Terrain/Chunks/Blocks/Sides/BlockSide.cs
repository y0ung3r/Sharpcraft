using System.Collections.Generic;
using UnityEngine;

public abstract class BlockSide
{
	public abstract Vector3Int Direction { get; }

	protected abstract IEnumerable<Vector3> GetVertices();

    public virtual IEnumerable<Vector2> GetUVs()
    {
        yield return new Vector2(0.0f, 0.0f);
        yield return new Vector2(0.0f, 1.0f);
        yield return new Vector2(1.0f, 0.0f);
        yield return new Vector2(1.0f, 1.0f);
    }

    public IEnumerable<Vector3> GetVertices(Vector3Int localPosition)
	{
		var vertices = GetVertices();

		foreach (var vertex in vertices)
		{
			yield return vertex + localPosition;
		}
	}
}
