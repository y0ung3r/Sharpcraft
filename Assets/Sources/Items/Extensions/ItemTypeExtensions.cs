public static class ItemTypeExtensions
{
    public static BlockType? ToBlockType(this ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.GrassBlock:
                return BlockType.Grass;

            case ItemType.GlassBlock:
                return BlockType.Glass;

            case ItemType.StoneBlock:
                return BlockType.Stone;
        }

        return default;
    }
}