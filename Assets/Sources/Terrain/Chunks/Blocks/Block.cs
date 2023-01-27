using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Представляет блок
/// </summary>
public class Block
{
    /// <summary>
    /// Тип блока
    /// </summary>
    public BlockType Type { get; }
    
    /// <summary>
    /// Позиция блока в чанке
    /// </summary>
    public Vector3Int LocalPosition { get; }

    /// <summary>
    /// Позиция блока в мировых координатах
    /// </summary>
    public Vector3Int WorldPosition => LocalPosition + OwningChunk.WorldPosition.WithZeroHeight();
    
    /// <summary>
    /// Чанк, содержащий текущий блок
    /// </summary>
    public Chunk OwningChunk { get; }

    /// <summary>
    /// Инициализирует <see cref="Block"/>
    /// </summary>
    /// <param name="chunkOwner">Чанк, содержащий текущий блок</param>
    /// <param name="type">Тип блока</param>
    /// <param name="localPosition">Позиция блока в чанке</param>
    public Block(Chunk chunkOwner, BlockType type, Vector3Int localPosition)
    {
        OwningChunk = chunkOwner;
        Type = type;
        LocalPosition = localPosition;
    }
    
    /// <summary>
    /// Проверяет имеет ли текущий блок соседа по указанному направлению
    /// </summary>
    /// <param name="direction">Направление</param>
    public bool HasAdjacentBlock(Vector3Int direction)
        => TryGetAdjacentBlock(direction, out _);

    /// <summary>
    /// Возвращает соседний блок по указанному направлению
    /// </summary>
    /// <param name="direction">Направление</param>
    public bool TryGetAdjacentBlock(Vector3Int direction, out Block block)
    {
        if (direction == Vector3Int.up || direction == Vector3Int.down)
        {
            return OwningChunk.TryGetBlock(LocalPosition + direction, out block);
        }

        return OwningChunk.Terrain.TryGetBlock(WorldPosition + direction, out block);
    }
}
