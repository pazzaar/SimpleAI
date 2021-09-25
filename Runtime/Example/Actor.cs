// A context contains all the data that's relevant for the AI to run.
using SimpleAI;
using UnityEngine;

public class Actor : MonoBehaviour
{
    public Intelligence Intelligence;

    ActorAIContext ctx;
    AIAgent<ActorAIContext> ai;

    float nextContextUpdateTime;
    float nextAttackTime;

    public bool something;
    public Transform enemy;

    public bool DebugProximity = false;
    public bool DebugAttack = false;


    void Start()
    {
        ctx = new ActorAIContext() { Actor = this };
        ai = new AIAgent<ActorAIContext>(Intelligence);
    }

    public void Update()
    {
        if (Time.time >= nextContextUpdateTime)
        {
            nextContextUpdateTime = Time.time + 0.1f;

            // Getters, raycasts, Physics.OverlapSphereNonAlloc, etc. to fill the context with valuable information (ctx.SmartObjects for instance)
            //ctx.BestTarget = ...;
            //ctx.BestSmartObject = ai.SelectSmartObject(ctx, ctx.SmartObjects);
            //ctx.IsArmed = ...;
            ctx.AttackSomething = something;
            ctx.enemyPosition = enemy.position;
        }

        ai.Tick(ctx);
    }

    public void Attack()
    {
        if (Time.time < nextAttackTime)
            return;

        nextAttackTime = Time.time + 1;
        Debug.Log($"Attacking {ctx.BestTarget}");
    }
}

