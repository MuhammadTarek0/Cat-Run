using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Transform[] _patrolTransforms;
    [SerializeField] private Animator _anim;
    [SerializeField] private string _blendtreeVariable;

    private List<Vector3> m_patrolPoints = new List<Vector3>();
    private int m_currentPatrolPointIndex, m_patrolPointsCount, m_animatorHashedBlendTreeVariable;
    private bool m_chasing, m_patrol, m_searching;

    private void Start()
    {
        _agent.isStopped = true;

        m_animatorHashedBlendTreeVariable = Animator.StringToHash(_blendtreeVariable);

        m_patrolPointsCount = _patrolTransforms.Length;
        if (m_patrolPointsCount == 0) return;

        for (int i = 0; i < m_patrolPointsCount; i++)
            m_patrolPoints.Add(_patrolTransforms[i].position);
    }

    private void Update()
    {


        if (m_patrol)
        {
            _anim.SetFloat(m_animatorHashedBlendTreeVariable, _agent.velocity.magnitude / _agent.speed);
        }
        else
        {
            HeadToPoint(m_patrolPoints[m_currentPatrolPointIndex], m_currentPatrolPointIndex);
            m_patrol = true;
        }
    }

    private void HeadToPoint(Vector3 position, int index)
    {
        _agent.isStopped = false;
        _agent.SetDestination(position);
        
        if(index >= 0) StartCoroutine(CheckIfReachedPoint(index));
    }

    private void GetNextPoint(int currentPoint)
    {
        int pointToMoveTo;

        if (currentPoint < m_patrolPointsCount - 1)
            pointToMoveTo = currentPoint + 1;
        
        else
            pointToMoveTo = 0;

        HeadToPoint(m_patrolPoints[pointToMoveTo], pointToMoveTo);
    }

    IEnumerator CheckIfReachedPoint(int currentPointNumber)
    {
        while (!_agent.isStopped)
        {
            if (_agent.pathPending)
                yield return null;

            if (_agent.remainingDistance < 0.1f)
                GetNextPoint(currentPointNumber);

            //Polish agent rotation by making it manualy and shut down the agent auto rotation

            yield return null;
        }

    }
}
