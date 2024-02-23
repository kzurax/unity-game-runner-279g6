

using UnityEngine;

public class PropsPool :  BasePool<Prop>
{
     // protected override Props CreateAction()
     // {
     //     Props chunk = base.CreateAction();
     //     chunk.Init(spawner);
     //     chunk.transform.position = new Vector3(0, 0, 0);
     //     return chunk;
     // }
     //
     // protected override void ReturnAction(Props chunk)
     // {
     //     base.ReturnAction(chunk);
     //     chunk.ResetToDefault();
     //     chunk.Coins.Clear();
     //     foreach (var obstacle in chunk.Obstacles)
     //     {
     //         obstacle.ResetToDefault();
     //     }
     //     chunk.Obstacles.Clear();
     // }
}
