using SimpleAI;
using System;
using UnityEngine;

public class ActorAIContext : IContext
{
    public MonoBehaviour CoroutineTarget => Actor;

    /// The Pawn this AIAgent controls
    public Actor Actor;

    public Actor BestTarget;
    public SmartObjectBase BestSmartObject;
    public bool AttackSomething;
    public Vector3 enemyPosition;

    public bool IsArmed = false;

    // The next 2 methods are required for the inspector to display "considerations" for an action
    public float GetCurrentConsiderationScore(int considerationIdx)
        => considerationIdx switch
        {
            0 => 1,
            1 => BestTarget != null ? 1 : 0,
            2 => IsArmed ? 1 : 0,
            3 => AttackSomething ? .5f : 0,
            4 => CheckProximity(),
            5 => .4f,
            _ => throw new Exception("case missing")
        };

    public string[] GetConsiderationDescriptions()
        => new string[] {
            "Constant",
            "Has Target",
            "Is Armed",
            "AttackSomething",
            "Proximity",
            "NothingToDo (Idle)",
        };

    public float CheckProximity()
    {
        Vector3 vDist = Actor.transform.position - enemyPosition;

        if (vDist.magnitude < 5) 
            return 1.0f;

        return 0.0f;
    }

}