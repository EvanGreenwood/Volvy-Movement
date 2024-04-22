using Unity.Mathematics;

public struct Tile
{
    public int row;
    public int col;
    public TileType type;
    public float2 flow;

    public readonly bool IsTraversable
    {
        get => type == TileType.Empty;
    }

    public Tile(Tile tile)
    {
        row = tile.row;
        col = tile.col;
        type = tile.type;
        flow = tile.flow;
    }
}