// A context contains all the data that's relevant for the AI to run.
using SimpleAI;
using UnityEngine;

public class Actor : MonoBehaviour
{
    public Intelligence Intelligence;

    ActorAIContext ctx;
    AIAgent<ActorAIContext> ai;

    public AILocomotion locomotion;

    float nextContextUpdateTime;

    public bool something;
    public Transform enemy;

    public bool DebugProximity = false;
    public bool DebugAttack = false;


    void Start()
    {
        ctx = new ActorAIContext() { Actor = this };
        ai = new AIAgent<ActorAIContext>(Intelligence);
        locomotion = GetComponent<AILocomotion>();
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

            Debug.Log("ticking...");
        }

        ai.Tick(ctx);
    }

}