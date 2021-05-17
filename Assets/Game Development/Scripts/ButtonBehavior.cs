using UnityEngine;

public class ButtonBehavior : MonoBehaviour
{
    #region Private Serialized Fields
    [Header("Data")]
    [SerializeField] private Material _activateMat, _deactivateMat;
    [SerializeField] private MeshRenderer _mesh;
    [SerializeField] private GameObject _tutorialCanvas;
    #endregion

    #region Module Fields
    private bool m_isActivated;
    #endregion

    #region Public Events
    public delegate void OnButtonStateChange(bool newState);
    public static event OnButtonStateChange BUTTON_STATE_CHANGED = delegate { };
    #endregion

    #region Unity Callbacks
    private void Start()
    {
        if(_tutorialCanvas)
            Destroy(_tutorialCanvas, 5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        Vibration.Vibrate();

        m_isActivated = !m_isActivated;

        _mesh.material = m_isActivated ? _activateMat : _deactivateMat;

        BUTTON_STATE_CHANGED.Invoke(m_isActivated);
    }
    #endregion
}
