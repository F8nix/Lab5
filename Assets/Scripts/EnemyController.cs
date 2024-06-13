using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{

  public StateMachine stateMachine;
  public List<Transform> patrolWaypoints;
  public Transform player;
  public bool playerInRange = false;
  public float distanceToShoot = 6f;
  public float shootFrequency = 0.2f;
  public float walkSpeed = 2f;
  public float runSpeed = 4f;
  private void Start()
  {
    stateMachine = new();
    var animator = GetComponent<Animator>();
    var agent = GetComponent<NavMeshAgent>();
    var shootSound = GetComponent<AudioSource>();
    var patrol = new EnemyPatrolState(patrolWaypoints, agent, animator, walkSpeed);
    var chase = new EnemyChaseState(ref player, agent, animator, runSpeed);
    var shoot = new EnemyShootState(shootFrequency, player, shootSound, agent, animator);
    patrol.AddTransitionFrom(chase, () => !playerInRange);
    chase
      .AddTransitionFrom(patrol, () => playerInRange)
      .AddTransitionFrom(shoot, () =>
        Vector3.Distance(player.position, transform.position) > distanceToShoot || !PlayerInLineOfSight()
      );
    shoot.AddTransitionFrom(chase, () =>
        Vector3.Distance(player.position, transform.position) < distanceToShoot && PlayerInLineOfSight()
      );
    stateMachine.AddStates(patrol, chase, shoot);
    stateMachine.ChangeState(patrol);
  }

  private bool PlayerInLineOfSight()
  {
    if (player == null) return false;

    for (int x = -1; x <= 1; x++)
    {
      for (int y = -1; y <= 1; y++)
      {
        var offset = new Vector3(x, y, 0);
        var direction = player.position - transform.position + offset;
        var rotatedOffset = Vector3.Project(offset, direction).normalized * 0.4f;
        Physics.Raycast(transform.position, direction + rotatedOffset, out var hit);
        if (hit.transform == null) continue;
        if (hit.transform.CompareTag("Player")) return true;
      }
    }
    return false;
  }

  private void Update()
  {
    stateMachine.Update();
    if (playerInRange)
    {
      Debug.Log(PlayerInLineOfSight());
    }
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.CompareTag("Player"))
    {
      playerInRange = true;
    }
  }

  private void OnTriggerExit(Collider other)
  {
    if (other.gameObject.CompareTag("Player"))
    {
      playerInRange = false;
    }
  }
}
