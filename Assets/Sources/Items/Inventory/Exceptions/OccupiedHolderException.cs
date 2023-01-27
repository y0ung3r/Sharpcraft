using System;

public class OccupiedHolderException : Exception
{
	public OccupiedHolderException()
		: base("Место под предмет уже занято")
	{ }
}