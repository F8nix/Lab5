using UnityEngine;
using UnityEngine.AI;

public class EnemyShootState : State
{
  private AudioSource shootSound;
  private NavMeshAgent agent;
  private Animator animator;
  private float shootDelay;
  private float lastShotTime = 0f;
  private Transform player;
  public EnemyShootState(float shootDelay, Transform player, AudioSource shootSound, NavMeshAgent agent, Animator animator)
  {
    this.player = player;
    this.shootSound = shootSound;
    this.agent = agent;
    this.shootDelay = shootDelay;
    this.animator = animator;
  }

  public override void Enter()
  {
    animator.SetBool("isShooting", true);
    agent.SetDestination(agent.transform.position);

  }

  public override void Update()
  {
    var playerPosition = player.position;
    playerPosition.y = agent.transform.position.y;
    agent.transform.LookAt(playerPosition);
    if (Time.time - lastShotTime >= shootDelay)
    {
      lastShotTime = Time.time;
      shootSound.Play();
    }
  }
  public override void Exit()
  {
    animator.SetBool("isShooting", false);
  }
}