using SimpleAI;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Action/Proximity")]
public class ProximityAction : Action<ActorAIContext>
{
    public override IEnumerator StartAction(ActorAIContext ctx)
    {
        ctx.Actor.locomotion.faceAimTarget = true;
        ctx.Actor.locomotion.chasePlayer = true;

        Debug.Log("Getting into proximity");

        while (CheckProceduralPreconditions(ctx))
        {
            yield return null;
        }

        Debug.Log("Out of proximity");
    }

    public override bool CheckProceduralPreconditions(ActorAIContext ctx)
    {
        return ctx.Actor.DebugProximity;
    }

    public override void StopAction(ActorAIContext ctx)
    {
        ctx.Actor.locomotion.faceAimTarget = false;
        ctx.Actor.locomotion.chasePlayer = false;

    }
}