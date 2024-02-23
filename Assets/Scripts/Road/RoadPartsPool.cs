using UnityEngine;


public class RoadPartsPool // : BasePool<RoadPart>
{
    // [SerializeField] private ChunkSpawner spawner;
    //
    // protected override RoadPart CreateAction()
    // {
    //     RoadPart chunk = base.CreateAction();
    //     chunk.Init(spawner);
    //     chunk.transform.position = new Vector3(0, 0, 0);
    //     return chunk;
    // }
    //
    // protected override void ReturnAction(RoadPart chunk)
    // {
    //     base.ReturnAction(chunk);
    //     chunk.ResetToDefault();
    //     chunk.Coins.Clear();
    //     foreach (var obstacle in chunk.Obstacles)
    //     {
    //         obstacle.ResetToDefault();
    //     }
    //
    //     chunk.Obstacles.Clear();
    // }
}
