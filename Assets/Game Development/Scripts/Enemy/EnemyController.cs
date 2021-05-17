using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    #region Private Serialized Fields
    [Header("Data")]
    [SerializeField] internal FieldOfView _viewField;
    [Space]
    [Header("AI")]
    [SerializeField] internal NavMeshAgent _agent;
    [SerializeField] internal Transform[] _patrolTransforms;
    [SerializeField] private float _timeDelayBetweenPoints;
    [SerializeField] private bool _staticEnemy;
    [Space]
    [Header("Animations")]
    [SerializeField] private Animator _anim;
    [SerializeField] private string _blendtreeVariable;
    [SerializeField] private string _deathBehaviorString;
    [Space]
    [Header("Effects")]
    [SerializeField] internal ParticleSystem _disappointedParticles;
    [SerializeField] internal ParticleSystem _idleParticles;
    #endregion

    #region Internal Fields
    internal Vector3 m_lastSeenPosition = Vector3.zero;
    #endregion

    #region Module Fields
    private List<Vector3> m_patrolPoints = new List<Vector3>();

    private int m_currentPatrolPointIndex, m_patrolPointsCount,
                m_animatorHashedBlendTreeVariable, m_animatorHashedDeathBehavior;
    private float m_patrolSpeed;

    private bool  m_hasDestination, m_died;

    private Collider m_collider;

    private AudioSource m_audioSource;
    #endregion

    #region Enums
    internal enum EnemyState { Freeze, Patrolling, Chasing}
    [SerializeField] internal EnemyState m_currentState = EnemyState.Freeze;
    #endregion

    #region Public Events
    public delegate void OnNullEventTrigger();
    public static event OnNullEventTrigger PLAYER_LOSE = delegate { };
    #endregion

    #region Unity Callbacks
    public virtual void Start()
    {
        m_patrolSpeed = _agent.speed;

        m_animatorHashedBlendTreeVariable = Animator.StringToHash(_blendtreeVariable);
        m_animatorHashedDeathBehavior = Animator.StringToHash(_deathBehaviorString);

        m_collider = GetComponent<Collider>();
        m_audioSource = GetComponent<AudioSource>();

        m_currentState = EnemyState.Patrolling;

        m_patrolPointsCount = _patrolTransforms.Length;

        if (m_patrolPointsCount == 0)
        {
            m_patrolPoints.Add(transform.position);
            return;
        }

        for (int i = 0; i < m_patrolPointsCount; i++)
            m_patrolPoints.Add(_patrolTransforms[i].position);
    }

    public virtual void Update()
    {
        if (m_died) return;

        switch (m_currentState)
        {
            case EnemyState.Freeze:
                {
                    _agent.isStopped = true;
                    _agent.updateRotation = false;
                }
                break;
            case EnemyState.Patrolling:
                {
                    if (_viewField.playerTarget)
                    {
                        m_hasDestination = false;
                        m_currentState = EnemyState.Chasing;
                        GotTarget();
                    }
                    else if (!m_hasDestination && !_staticEnemy)
                    {
                        HeadToPoint(m_patrolPoints[m_currentPatrolPointIndex], m_currentPatrolPointIndex);
                        m_hasDestination = true;
                    }
                }
                break;
        }

        _anim.SetFloat(m_animatorHashedBlendTreeVariable, _agent.velocity.magnitude / _agent.speed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            m_collider.isTrigger = true;

            MyDayHasCame();
        }
        else if(!m_died && collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().HandleDeath();
            TriggerPlayerLose();
        }
    }

    private void OnEnable()
    {
        GameManager.GAME_STATE_CHANGED += CastGameState;
    }

    private void OnDisable()
    {
        GameManager.GAME_STATE_CHANGED -= CastGameState;
    }
    #endregion

    #region Public Methods
    public void MyDayHasCame()
    {
        m_currentState = EnemyState.Freeze;
        _agent.speed = 0;

        _anim.SetTrigger(_deathBehaviorString);

        _viewField.ThisIsTheEnd();
        m_collider.enabled = false;
        Destroy(this, 0.02f);
        
        m_died = true;
    }

    public virtual void GotTarget()
    {

    }
    #endregion

    #region Private Methods
    private void HeadToPoint(Vector3 position, int index)
    {
        _agent.isStopped = false;
        _agent.speed = m_patrolSpeed;

        _agent.SetDestination(position);
        
        if(index >= 0) StartCoroutine(CheckIfReachedPoint(index));
    }

    private void GetNextPoint(int currentPoint)
    {
        if (currentPoint < m_patrolPointsCount - 1)
            m_currentPatrolPointIndex = currentPoint + 1;
        
        else
        {
            m_currentPatrolPointIndex = 0;
        }

        HeadToPoint(m_patrolPoints[m_currentPatrolPointIndex], m_currentPatrolPointIndex);
    }

    private void HideIdleParticles()
    {
        _idleParticles.Stop();
    }

    private void CastGameState()
    {
        if (GameManager.GAME_STATE == GameManager.GameState.SettingsMenu ||
            GameManager.GAME_STATE == GameManager.GameState.EndLevel)
            m_currentState = EnemyState.Freeze;
        else
            m_currentState = EnemyState.Patrolling;
    }
    #endregion

    #region Coroutines
    IEnumerator CheckIfReachedPoint(int currentPointNumber)
    {
        while (!_agent.isStopped)
        {
            if (_agent.pathPending)
                yield return null;

            if (_agent.remainingDistance < 0.1f)
            {
                _agent.isStopped = true;

                if(m_currentPatrolPointIndex % m_patrolPointsCount == 0)
                {
                    _idleParticles.Play();
                    Invoke("HideIdleParticles", 0.5f);
                }

                yield return new WaitForSeconds(_timeDelayBetweenPoints);
                _agent.isStopped = false;
                GetNextPoint(currentPointNumber);
            }

            //Polish agent rotation by making it manualy and shut down the agent auto rotation

            yield return null;
        }

    }
    #endregion

    #region Internal Methods
    internal void TriggerPlayerLose()
    {
        m_audioSource.Play();
        Vibration.Vibrate();
        PLAYER_LOSE();
    }
    #endregion

}
