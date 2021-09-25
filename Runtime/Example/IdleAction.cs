using SimpleAI;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Action/Idle")]
public class IdleAction : Action<ActorAIContext>
{
    public override IEnumerator StartAction(ActorAIContext ctx)
    {
        Debug.Log("Start idle");

        while (CheckProceduralPreconditions(ctx))
        {
            Debug.Log("idling...");
            yield return null;
        }

        Debug.Log("Done idle");
    }

    public override bool CheckProceduralPreconditions(ActorAIContext ctx)
    {
        return true;
        return ctx.BestTarget != null;
    }
}