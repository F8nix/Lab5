using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrolState : State
{
  private List<Transform> waypoints;
  private NavMeshAgent agent;
  private int currentWaypoint;
  private Animator animator;
  private float speed;
  public EnemyPatrolState(List<Transform> waypoints, NavMeshAgent agent, Animator animator, float speed)
  {
    this.animator = animator;
    this.waypoints = waypoints;
    this.agent = agent;
    this.speed = speed;
    currentWaypoint = 0;
  }

  public override void Enter()
  {
    agent.speed = speed;
    var closestWaypoint = waypoints[0];
    var closestWaypointDistance = Vector3.Distance(waypoints[0].position, agent.transform.position);
    for (var i = 0; i < waypoints.Count; i++)
    {
      if (i == currentWaypoint) continue;
      var waypoint = waypoints[i];
      var distance = Vector3.Distance(waypoint.position, agent.transform.position);
      if (distance < closestWaypointDistance)
      {
        closestWaypoint = waypoint;
        closestWaypointDistance = distance;
        currentWaypoint = i;
      }
    }

    agent.destination = closestWaypoint.position;
    animator.SetBool("isWalking", true);
  }

  public override void Update()
  {
    if (agent.remainingDistance <= agent.stoppingDistance)
    {
      currentWaypoint = (currentWaypoint + 1) % waypoints.Count;
      agent.destination = waypoints[currentWaypoint].position;
    }
  }

  public override void Exit()
  {
    animator.SetBool("isWalking", false);
  }
}