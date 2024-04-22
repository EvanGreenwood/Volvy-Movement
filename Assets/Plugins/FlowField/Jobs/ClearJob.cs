using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

[BurstCompile]
public struct ClearJob : IJobParallelFor
{
    public NativeArray<bool> visitedTiles;

    public void Execute(int index)
    {
        visitedTiles[index] = false;
    }
}