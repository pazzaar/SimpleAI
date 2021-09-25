using SimpleAI;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Action/Attack")]
public class AttackAction : Action<ActorAIContext>
{
    public override IEnumerator StartAction(ActorAIContext ctx)
    {
        Debug.Log("Start attack");

        while (CheckProceduralPreconditions(ctx))
        {
            Debug.Log("Attacking...");
            yield return null;
        }

        Debug.Log("Done Attacking");
    }

    public override bool CheckProceduralPreconditions(ActorAIContext ctx)
    {
        return ctx.Actor.DebugAttack;
    }
}