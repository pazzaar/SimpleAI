using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleAI;

[CreateAssetMenu(menuName = "AI/Checks/Test")]
public class TestCheck : Check<ActorAIContext>
{
    public override bool Evaluate(ActorAIContext ctx)
    {
        return true;
    }
}
