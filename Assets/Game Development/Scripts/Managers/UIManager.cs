using UnityEngine;
using TMPro;
using DG.Tweening;
public class UIManager : MonoBehaviour
{
    #region Private Serialized Fields
    [Header("Panels")]
    [SerializeField] private GameObject _mainMenuPanel;
    [SerializeField] private GameObject _gameplayPanel;
    [SerializeField] private GameObject _winPanel;
    [SerializeField] private GameObject _losePanel;
    [SerializeField] private GameObject _settingsPanel;
    [Space]
    [Header("Main Menu")]
    [SerializeField] private GameObject _animatedHand;
    [SerializeField] private float _xValue, _duration;
    [Space]
    [Header("Level")]
    [SerializeField] private TextMeshProUGUI _levelNumberText;
    [SerializeField] private int _levelNumber;
    [Space]
    [Header("Settings")]
    [SerializeField] private GameObject _vibrationOn;
    [SerializeField] private GameObject _vibrationOff;
    [SerializeField] private GameObject _soundOn;
    [SerializeField] private GameObject _soundOff;
    [Space]
    [Header("Ads Reward")]
    [SerializeField] private UnityEngine.UI.Image _loseStarImage;
    [SerializeField] private UnityEngine.UI.Image _winStarImage;
    #endregion

    #region Public Events
    public delegate void OnNullEventTrigger();
    public static event OnNullEventTrigger FIRST_INPUT = delegate { };
    #endregion

    #region Unity Callbacks
    private void OnEnable()
    {
        WinTrigger.PLAYER_WON += EndGameWin;
        EnemyController.PLAYER_LOSE += EndGameLose;
        RewardedAdsButton.REWARD_THE_PLAYER += PlayerSawAd;

        Init();
    }

    private void OnDisable()
    {
        WinTrigger.PLAYER_WON -= EndGameWin;
        EnemyController.PLAYER_LOSE -= EndGameLose;
        RewardedAdsButton.REWARD_THE_PLAYER -= PlayerSawAd;
    }
    #endregion

    #region Private Methods
    private void PlayerSawAd()
    {
        _winStarImage.color = _loseStarImage.color = Color.white;
    }

    private void Init()
    {
        _mainMenuPanel.SetActive(true);
        _settingsPanel.SetActive(true);

        _levelNumberText.text = _levelNumber.ToString();

        AnimateHandRight();
    }

    private void AnimateHandLeft()
    {
        if(_animatedHand)
            _animatedHand.transform.DOLocalMoveX(_xValue, _duration).onComplete = AnimateHandRight;
    }

    private void AnimateHandRight()
    {
        if(_animatedHand)
            _animatedHand.transform.DOLocalMoveX(-_xValue, _duration).onComplete = AnimateHandLeft;
    }

    private void EndGameWin()
    {
        _gameplayPanel.SetActive(false);
        _winPanel.SetActive(true);
    }

    private void EndGameLose()
    {
        _gameplayPanel.SetActive(false);
        _losePanel.SetActive(true);
    }
    #endregion

    #region Public Methods
    public void StartGame()
    {
        _mainMenuPanel.SetActive(false);
        _gameplayPanel.SetActive(true);

        FIRST_INPUT();
    }

    public void ToggleVibration()
    {
        bool ret = SaveManager.INSTANCE.ToggleVibration();

        _vibrationOn.SetActive(ret);
        _vibrationOff.SetActive(!ret);
        
    }

    public void ToggleSound()
    {
        bool  ret = SaveManager.INSTANCE.ToggleSound();

        _soundOn.SetActive(ret);
        _soundOff.SetActive(!ret);
    }
    #endregion
}
