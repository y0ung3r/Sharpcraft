using System;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public struct SixSidedUnwrapping
{
	private readonly Size _tileSize;

	private readonly Size _layoutSize;
		
	public SixSidedUnwrapping(Size tileSize, Size layoutSize)
	{
		_tileSize = tileSize;
		_layoutSize = layoutSize;
	}

	public static IEnumerable<Vector2> Left { get; } = new List<Vector2>(capacity: 4)
	{
		new Vector2(16.0f / 64.0f, 16.0f / 48.0f), // Right Top Vertex
		new Vector2(16.0f / 64.0f, 32.0f / 48.0f), // Right Bottom Vertex
		new Vector2(0.0f, 16.0f / 48.0f), // Left Top Vertex
		new Vector2(0.0f, 32.0f / 48.0f), // Left Bottom Vertex
	};

	public static IEnumerable<Vector2> Right { get; } = new List<Vector2>(capacity: 4)
	{
		new Vector2(32.0f / 64.0f, 16.0f / 48.0f), // Left Top Vertex
		new Vector2(48.0f / 64.0f, 16.0f / 48.0f), // Right Top Vertex
		new Vector2(32.0f / 64.0f, 32.0f / 48.0f), // Left Bottom Vertex
		new Vector2(48.0f / 64.0f, 32.0f / 48.0f), // Right Bottom Vertex
	};

	public static IEnumerable<Vector2> Front { get; } = new List<Vector2>(capacity: 4)
	{
		new Vector2(16.0f / 64.0f, 32.0f / 48.0f), // Left Top Vertex 
		new Vector2(32.0f / 64.0f, 32.0f / 48.0f), // Right Top Vertex
		new Vector2(16.0f / 64.0f, 1.0f), // Left Bottom Vertex
		new Vector2(32.0f / 64.0f, 1.0f), // Right Bottom Vertex
	};

	public static IEnumerable<Vector2> Back { get; } = new List<Vector2>(capacity: 4)
	{
		new Vector2(32.0f / 64.0f, 16.0f / 48.0f), // Right Bottom Vertex
		new Vector2(32.0f / 64.0f, 0.0f), // Right Top Vertex
		new Vector2(16.0f / 64.0f, 16.0f / 48.0f), // Left Bottom Vertex
		new Vector2(16.0f / 64.0f, 0.0f), // Left Top Vertex
	};

	public static IEnumerable<Vector2> Upper { get; } = new List<Vector2>(capacity: 4)
	{
		new Vector2(48.0f / 64.0f, 16.0f / 48.0f), // Left Top Vertex
		new Vector2(48.0f / 64.0f, 32.0f / 48.0f), // Left Bottom Vertex
		new Vector2(1.0f, 16.0f / 48.0f), // Right Top Vertex
		new Vector2(1.0f, 32.0f / 48.0f), // Right Bottom Vertex
	};

	public static IEnumerable<Vector2> Lower { get; } = new List<Vector2>(capacity: 4)
	{
		new Vector2(16.0f / 64.0f, 16.0f / 48.0f), // Left Top Vertex
		new Vector2(16.0f / 64.0f, 32.0f / 48.0f), // Left Bottom Vertex
		new Vector2(32.0f / 64.0f, 16.0f / 48.0f), // Right Top Vertex
		new Vector2(32.0f / 64.0f, 32.0f / 48.0f), // Right Bottom Vertex
	};
}