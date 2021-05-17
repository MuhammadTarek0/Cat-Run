using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class CloneAnimalScript : MonoBehaviour
{
    #region Private Serialized Fields
    [Header("Data")]
    [SerializeField] private Material _cloneMat;
    [Space]
    [Header("AI")]
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Transform[] _patrolTransforms;
    [Space]
    [Header("Animations")]
    [SerializeField] private Animator _anim;
    [SerializeField] private string _blendtreeVariable;
    [SerializeField] private string _deathBehaviorString;
    [Space]
    [Header("Effects")]
    [SerializeField] private ParticleSystem _poofParticles;
    [SerializeField] private AudioSource _audioSource;
    #endregion

    #region Module Fields
    private List<Vector3> m_patrolPoints = new List<Vector3>();

    private int m_currentPoint, m_totalPoints,
                m_hashedBlendTree, m_hashedDeathBehavior;

    private Color m_cloneColor;
    #endregion

    #region Unity Callbacks
    private void Start()
    {
        m_currentPoint = 0;
        m_totalPoints = _patrolTransforms.Length;

        m_hashedBlendTree = Animator.StringToHash(_blendtreeVariable);
        m_hashedDeathBehavior = Animator.StringToHash(_deathBehaviorString);

        m_cloneColor = _cloneMat.color;
        m_cloneColor.a = 0.5f;
        _cloneMat.color = m_cloneColor;

        //transform.localScale = Vector3.zero;

        for (int i = 0; i < m_totalPoints; i++)
            m_patrolPoints.Add(_patrolTransforms[i].position);
    }

    private void LateUpdate()
    {
        _anim.SetFloat(m_hashedBlendTree, _agent.velocity.normalized.magnitude);
    }

    private void OnEnable()
    {
        Collectables.RUN_IT_BACK += StartClone;
    }

    private void OnDisable()
    {
        Collectables.RUN_IT_BACK -= StartClone;
    }
    #endregion

    #region Private Methods
    private void SetAgentDestination()
    {
        m_currentPoint++;

        if(m_currentPoint == m_totalPoints)
        {
            _agent.isStopped = true;
            _anim.SetTrigger(m_hashedDeathBehavior);
            _audioSource.Play();
            _poofParticles.Play();
            StartCoroutine(Dissolve());
            Destroy(gameObject, 1f);
        }
        else
        {
            _agent.SetDestination(m_patrolPoints[m_currentPoint]);
        }
    }

    private void StartClone()
    {
        _agent.SetDestination(m_patrolPoints[m_currentPoint]);

        StartCoroutine(CheckIfReachedPoint());
    }
    #endregion

    #region Coroutines
    IEnumerator CheckIfReachedPoint()
    {
        while (!_agent.isStopped)
        {
            if (_agent.pathPending)
                yield return null;

            if (_agent.remainingDistance < 0.1f)
                SetAgentDestination();
            
            yield return null;
        }

    }

    IEnumerator Dissolve()
    {
        float time = 0;

        while(time < 0.8f)
        {
            m_cloneColor.a -= 0.03f;
            _cloneMat.color = m_cloneColor;

            transform.localScale *= 0.96f;

            time += Time.deltaTime;

            yield return null;
        }
    }
    #endregion

}
