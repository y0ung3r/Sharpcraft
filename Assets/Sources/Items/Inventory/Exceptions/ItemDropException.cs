using System;

public class ItemDropException : Exception
{
    public ItemDropException(int amount)
        : base($"Предмет в заданном количестве \"{amount}\" не может быть выброшен")
    { }
}