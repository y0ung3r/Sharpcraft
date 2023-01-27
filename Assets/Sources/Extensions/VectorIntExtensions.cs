using System;
using UnityEngine;

public static class VectorIntExtensions
{
	public static Vector2Int DropHeight(this Vector3Int target)
		=> new Vector2Int(target.x, target.z);

    public static Vector3Int InsertHeight(this Vector2Int target, int height)
		=> new Vector3Int(target.x, height, target.y);

	public static Vector3Int WithZeroHeight(this Vector2Int target)
		=> target.InsertHeight(height: 0);

    public static Vector3Int ToWorld(this Vector3Int local, Vector2Int origin)
		=> local + origin.WithZeroHeight();

    public static Vector3Int ToLocal(this Vector3Int world, Vector2Int origin)
		=> world - origin.WithZeroHeight();

    public static Vector2Int GetOrigin(this Vector3Int target, int width, int depth)
	{
		int Calculate(int axis, int length)
		{
			return length * (Math.Clamp(axis % length, min: -1, max: 0) + axis / length);
		}
		
		return new Vector2Int
		(
			x: Calculate(target.x, width),
			y: Calculate(target.z, depth)
		);
	}
}