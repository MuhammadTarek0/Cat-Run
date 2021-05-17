using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Private Serialized Fields
    [Header("Data")]
    [SerializeField] private Vector3_Variable _playerInput;

    [Space]
    [Header("Movement")]
    [Range(0.0f, 10.0f)]
    [SerializeField] private int _horizontalSpeed = 5;
    [Range(0.0f, 10.0f)]
    [SerializeField] private int _rotationalSpeed = 7;

    [Space]
    [Header("Animator")]
    [SerializeField] private Animator _anim;
    [SerializeField] private string _blendtreeVariable;

    [Space]
    [Header("Sounds")]
    [SerializeField] private AudioClip _tap;
    #endregion

    #region Private Module Fields
    private Vector3 m_inputValue = Vector3.zero;

    private float m_angleToRotate;
    private int m_animatorHashedBlendTreeVariable, m_rotationSpeed, m_horizontalSpeed;

    private Rigidbody m_rigidbody;

    private AudioSource m_audioSource;
    #endregion

    #region Unity Callbacks
    private void Start()
    {
        m_rotationSpeed = _rotationalSpeed * 100;
        m_horizontalSpeed = _horizontalSpeed * 100;

        m_rigidbody = GetComponent<Rigidbody>();
        m_audioSource = GetComponentInChildren<AudioSource>();

        m_animatorHashedBlendTreeVariable = Animator.StringToHash(_blendtreeVariable);
    }

    private void Update()
    {
        if (GameManager.GAME_STATE != GameManager.GameState.Game) return;

        CastPlayerInput();
        LerpRotation();
        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        if (GameManager.GAME_STATE != GameManager.GameState.Game) return;

        PhysicsStep();
    }
    #endregion

    #region Private Methods
    private void PhysicsStep()
    {
        m_rigidbody.velocity = m_inputValue * m_horizontalSpeed * Time.fixedDeltaTime;
    }

    private void CastPlayerInput()
    {
        m_inputValue.x = _playerInput.Value.z;
        m_inputValue.z = -_playerInput.Value.x;
        m_inputValue.y = 0;

        m_angleToRotate = Vector3.Dot(transform.forward, _playerInput.Value) * Mathf.Rad2Deg;
    }

    private void LerpRotation()
    {
        transform.eulerAngles = transform.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y,
                                transform.eulerAngles.y + m_angleToRotate,
                                m_rotationSpeed * Time.deltaTime);
    }

    private void UpdateAnimation()
    {
        float threshold = _playerInput.Value.magnitude;
        _anim.SetFloat(m_animatorHashedBlendTreeVariable, threshold);

        if (m_audioSource.isPlaying && threshold < 0.1f)
            m_audioSource.Stop();
        else if (!m_audioSource.isPlaying && threshold > 0.1f)
            m_audioSource.Play();
        
    }
    #endregion

    #region Public Methods
    public void HandleDeath()
    {
        m_audioSource.loop = false;
        m_audioSource.enabled = false;
        this.gameObject.layer = LayerMask.NameToLayer("Dead");
        this.gameObject.tag = "Untagged";
        m_rigidbody.velocity = Vector3.zero;
    }

    public void TapSound()
    {
        m_audioSource.enabled = true;
        m_audioSource.PlayOneShot(_tap, 1);
    }
    #endregion

}
