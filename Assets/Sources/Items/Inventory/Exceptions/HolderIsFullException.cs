using System;

public class HolderIsFullException : Exception
{
	public HolderIsFullException()
		: base("Место под предмет переполнено")
	{ }
}