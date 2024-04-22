using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

[BurstCompile]
public struct FlowJob : IJobParallelFor
{
    [ReadOnly] public int numRows;
    [ReadOnly] public int numCols;
    [ReadOnly] public NativeArray<Tile> tiles;
    [ReadOnly] public NativeArray<float> distancesToTarget;
    public NativeArray<float2> flowField;

    public void Execute(int index)
    {
        Tile tile = tiles[index];
        int row = tile.row;
        int col = tile.col;

        if (IsTraversable(row, col))
        {
            float currentDistance = distancesToTarget[index] + 1;

            float x = GetDistance(row, col - 1, currentDistance) - GetDistance(row, col + 1, currentDistance);
            float y = GetDistance(row + 1, col, currentDistance) - GetDistance(row - 1, col, currentDistance);

            float2 flow = math.normalize(new float2(x, -y));
            flowField[index] = flow;
        }
        else
        {
            flowField[index] = tile.flow;
        }
    }

    public float GetDistance(int y, int x, float fallback)
    {
        if (IsOutOfBounds(y, x) || !IsTraversable(y, x))
        {
            return fallback;
        }
        int index = y * numCols + x;
        return distancesToTarget[index];
    }
    public bool IsOutOfBounds(int y, int x)
    {
        return (x < 0 || y < 0 || x > numCols - 1 || y > numRows - 1);
    }
    public bool IsTraversable(int y, int x)
    {
        int index = y * numCols + x;
        return tiles[index].IsTraversable;
    }
}