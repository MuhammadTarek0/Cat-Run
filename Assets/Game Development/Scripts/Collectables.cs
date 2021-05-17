using UnityEngine;

public class Collectables : MonoBehaviour
{
    #region Private Serialized Field
    [Header("Data")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private ParticleSystem _poofParticle;
    [SerializeField] private MeshRenderer _renderer;
    #endregion

    #region Enums
    internal enum Type
    {
        Gem,
        SuperAbility,
        RunItBack
    }
    [SerializeField] private Type _type;
    #endregion

    #region Public Events
    public delegate void OnNullEventTrigger();
    public static event OnNullEventTrigger TOOK_SUPERPOWER_GEM = delegate { };
    public static event OnNullEventTrigger RUN_IT_BACK = delegate { };
    #endregion

    #region Module Fields
    private bool m_isDirty;
    #endregion

    #region Unity Callbacks
    private void OnTriggerEnter(Collider other)
    {
        if (m_isDirty) return;

        m_isDirty = true;
        _renderer.enabled = false;

        _audioSource.Play();
        _poofParticle.Play();

        Vibration.Vibrate();

        if (_type == Type.SuperAbility)
            TOOK_SUPERPOWER_GEM();
        else if (_type == Type.RunItBack)
            RUN_IT_BACK();

        Destroy(this.gameObject, 1f);
    }
    #endregion
}
