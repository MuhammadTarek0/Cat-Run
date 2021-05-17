using UnityEngine;
using UnityEngine.SceneManagement;


public class SaveManager : MonoBehaviour
{
    #region Private Serialized Fields
    [Header("Save Strings")]
    [SerializeField] private int _levelCounts;
    [SerializeField] private string _levelNumber;
    [SerializeField] private string _soundToggle;
    [SerializeField] private string _vibrationToggle;
    #endregion

    #region Module Fields
    private int m_initialLevel, m_sounds = 1, m_vibration = 1;
    private int m_currentLevel, m_currentSound, m_currentVibration;
    #endregion

    #region Singleton
    public static SaveManager INSTANCE;

    private void Awake()
    {
        if (INSTANCE == null)
            INSTANCE = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
    #endregion

    #region Unity Callbacks
    private void OnEnable()
    {
        LoadData();
        LoadSavedScene();
    }
    #endregion

    #region Public Methods
    public void AdvanceLevel()
    {
        m_currentLevel++;

        if (m_currentLevel >= _levelCounts)
            m_currentLevel = 0;

        PlayerPrefs.SetInt(_levelNumber, m_currentLevel);
        PlayerPrefs.Save();
    }

    public void LoadSavedScene()
    {
        string level = "Map " + PlayerPrefs.GetInt(_levelNumber, m_initialLevel).ToString();
        SceneManager.LoadSceneAsync(level);
    }

    public bool ToggleVibration()
    {
        int isOn = GetVibrationsOn() == 1 ? 0 : 1;

        PlayerPrefs.SetInt(_vibrationToggle, isOn);
        PlayerPrefs.Save();

        return isOn == 1;
    }

    public bool ToggleSound()
    {
        int isOn = GetSoundsOn() == 1 ? 0 : 1;

        PlayerPrefs.SetInt(_soundToggle, isOn);
        PlayerPrefs.Save();

        return isOn == 1;
    }
    #endregion

    #region Private Methods
    private int GetLevelIndex()
    {
        if (PlayerPrefs.HasKey(_levelNumber))
            return PlayerPrefs.GetInt(_levelNumber);
        else
            return m_initialLevel;
    }

    private int GetSoundsOn()
    {
        if (PlayerPrefs.HasKey(_soundToggle))
            return PlayerPrefs.GetInt(_soundToggle);
        else
            return m_sounds;
    }

    private int GetVibrationsOn()
    {
        if (PlayerPrefs.HasKey(_vibrationToggle))
            return PlayerPrefs.GetInt(_vibrationToggle);
        else
            return m_vibration;
    }

    private void LoadData()
    {
        m_currentLevel = GetLevelIndex();
        m_currentSound = GetSoundsOn();
        m_currentVibration = GetVibrationsOn();
    }
    #endregion

}
