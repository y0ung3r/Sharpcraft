using System;
using UnityEngine;

[Serializable]
public class NoiseOctave
{
	private FastNoiseLite _noiseCreator;

	private bool _isDomainWarp;
	
	[SerializeField]
	private FastNoiseLite.NoiseType _type;

	[SerializeField]
	private float _frequency;
	
	[SerializeField]
	private float _amplitude;

	public float Frequency => _frequency;

	public float Amplitude => _amplitude;

	public NoiseOctave AllowDomainWarp()
	{
		_isDomainWarp = true;

		return this;
	}

	public float GetNoise(float x, float y, int seed)
	{
		_noiseCreator = new FastNoiseLite(seed);
		
		_noiseCreator.SetNoiseType(_type);
		_noiseCreator.SetFrequency(_frequency);
		_noiseCreator.SetDomainWarpAmp(_amplitude);

		if (_isDomainWarp)
		{
			_noiseCreator.SetDomainWarpAmp(_amplitude);
			_noiseCreator.DomainWarp(ref x, ref y);
		}

		return _noiseCreator.GetNoise(x, y);
	}
}