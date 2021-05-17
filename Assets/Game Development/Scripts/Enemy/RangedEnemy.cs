public class RangedEnemy : EnemyController
{
    public override void GotTarget()
    {
        _agent.isStopped = true;
        _agent.updateRotation = false;
        m_currentState = EnemyState.Freeze;

        if(_viewField.playerTarget)
            transform.LookAt(_viewField.playerTarget);
        TriggerPlayerLose();
    }
}
