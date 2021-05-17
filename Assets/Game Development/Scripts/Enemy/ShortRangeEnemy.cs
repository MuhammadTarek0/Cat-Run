using UnityEngine;

public class ShortRangeEnemy : EnemyController
{
    #region Private Serialized Fields
    [Header("Short Range Behavior")]
    [SerializeField] private float _chasingSpeed;
    [SerializeField] private float _catchingDistance;
    #endregion

    #region Unity Callbacks
    public override void Update()
    {
        base.Update();

        if (m_currentState != EnemyState.Chasing) return;

        if (!_viewField.playerTarget)
        {
            m_currentState = EnemyState.Patrolling;
            _disappointedParticles.Play();
            return;
        }

        ChaseAnimal();

        if (Vector3.Distance(_viewField.playerTarget.position, transform.position) <= _catchingDistance)
        {
            TriggerPlayerLose();
            m_currentState = EnemyState.Freeze;
        }
    }
    #endregion

    #region Public Methods
    public override void GotTarget()
    {
        m_lastSeenPosition = _viewField.playerTarget.position;
        _agent.speed = _chasingSpeed;
    }
    #endregion

    #region Private Methods
    private void ChaseAnimal()
    {
        m_lastSeenPosition = _viewField.playerTarget.position;
        _agent.SetDestination(m_lastSeenPosition);
    }
    #endregion
}
