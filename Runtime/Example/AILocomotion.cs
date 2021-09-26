using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AILocomotion : MonoBehaviour
{
    public Transform target;
    NavMeshAgent agent;

    public bool faceAimTarget = false;
    public Transform aimTarget;
    public bool chasePlayer = false;

    public float turnSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = false;
        agent.updateRotation = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (chasePlayer)
            agent.destination = aimTarget.position;
        else
            agent.destination = target.position;

        transform.position = agent.nextPosition;

        if (faceAimTarget && aimTarget != null)
            FaceTarget(aimTarget.position);
        else
            FaceTarget(agent.steeringTarget);

    }

    void FaceTarget(Vector3 target)
    {
        var turnTowardNavSteeringTarget = target;

        Vector3 direction = (turnTowardNavSteeringTarget - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }
}
