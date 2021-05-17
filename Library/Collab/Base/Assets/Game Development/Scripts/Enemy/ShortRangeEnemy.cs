using UnityEngine;

public class ShortRangeEnemy : EnemyController
{
    [SerializeField]
    private float _chasingSpeed;
    [SerializeField]
    private float _catchingDistance;

    public override void Update()
    {
        base.Update();

        if (m_currentState != EnemyState.Chasing) return;

        if (!_viewField.playerTarget)
        {
            m_currentState = EnemyState.Patrolling;
            return;
        }

        ChaseAnimal();

        if (Vector3.Distance(_viewField.playerTarget.position, transform.position) <= _catchingDistance)
        {
            //raise event to handle other systems
            m_currentState = EnemyState.Freeze;
        }
    }

    public override void GotTarget()
    {
        m_lastSeenPosition = _viewField.playerTarget.position;
        _agent.speed = _chasingSpeed;
    }

    private void ChaseAnimal()
    {
        m_lastSeenPosition = _viewField.playerTarget.position;
        _agent.SetDestination(m_lastSeenPosition);
    }
}
