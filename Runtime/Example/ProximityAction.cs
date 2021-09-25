using SimpleAI;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Action/Proximity")]
public class ProximityAction : Action<ActorAIContext>
{
    public override IEnumerator StartAction(ActorAIContext ctx)
    {
        Debug.Log("Getting into proximity");

        while (CheckProceduralPreconditions(ctx))
        {
            Debug.Log("In Proximity");
            yield return null;
        }

        Debug.Log("Out of proximity");
    }

    public override bool CheckProceduralPreconditions(ActorAIContext ctx)
    {
        return ctx.Actor.DebugProximity;
    }
}