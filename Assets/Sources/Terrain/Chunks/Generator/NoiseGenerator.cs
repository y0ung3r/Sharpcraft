using System;
using System.Linq;
using UnityEngine;
using Random = System.Random;

[Serializable]
public class NoiseGenerator
{
	private Random _randomizer;
	
	[SerializeField]
	private int _seed;

	[SerializeField]
	private bool _useRandomSeed;
	
	[SerializeField] 
	private float _baseHeight;

	[SerializeField]
	private NoiseOctave _domainWarp;
	
	[SerializeField]
	private NoiseOctave[] _octaves;

	public NoiseGenerator()
	{
		_randomizer = new Random();
	}

	public float GetNoise(float x, float y)
	{
		if (_useRandomSeed)
		{
			_seed = _randomizer.Next(int.MinValue, int.MaxValue);
		}
		
		var commonNoise = _octaves.Aggregate
		(
			_baseHeight,
			(accumulator, octave) => accumulator += octave.GetNoise(x, y, _seed) * octave.Amplitude / 2.0f
		);
		
		var domainWarpNoise = _domainWarp.AllowDomainWarp()
			.GetNoise(x, y, _seed);

		return commonNoise + domainWarpNoise;
	}
}