using UnityEngine;
using UnityEngine.AI;

public class EnemyChaseState : State
{

  private Transform player;
  private NavMeshAgent agent;
  private Animator animator;
  private float speed;
  public EnemyChaseState(ref Transform player, NavMeshAgent agent, Animator animator, float speed)
  {
    this.player = player;
    this.agent = agent;
    this.animator = animator;
    this.speed = speed;
  }

  public override void Enter()
  {
    agent.SetDestination(player.position);
    agent.speed = speed;
    animator.SetBool("isRunning", true);
  }
  public override void Update()
  {
    agent.SetDestination(player.position);
  }
  public override void Exit()
  {
    animator.SetBool("isRunning", false);
  }
}