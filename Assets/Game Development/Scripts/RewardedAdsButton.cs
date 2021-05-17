using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

[RequireComponent(typeof(Button))]
public class RewardedAdsButton : MonoBehaviour, IUnityAdsListener
{
    #region Private Serialized Fields
    [SerializeField] private string m_rewardedSurfacingId = "rewardedVideo";
    #endregion

    #region Public Events
    public delegate void OnNullEventTrigger();
    public static event OnNullEventTrigger REWARD_THE_PLAYER = delegate { };
    #endregion

    #region Module Fields
    private Button m_button;
    private bool m_sawAdAlready;
    #endregion

    #region Unity Callbacks
    private void Start()
    {
        m_button = GetComponent<Button>();

        m_button.interactable = Advertisement.IsReady(m_rewardedSurfacingId);

        if (m_button) m_button.onClick.AddListener(ShowRewardedVideo);

        Advertisement.AddListener(this);
    }

    private void ShowRewardedVideo()
    {
        Advertisement.Show(m_rewardedSurfacingId);
    }

    public void OnUnityAdsReady(string surfacingId)
    { 
        if (surfacingId == m_rewardedSurfacingId && !m_sawAdAlready)
            m_button.interactable = true;
        else
            m_button.interactable = false;
    }

    public void OnUnityAdsDidFinish(string surfacingId, ShowResult showResult)
    {
        if (showResult == ShowResult.Finished)
        {
            Debug.Log("You did it you deserve a kiss");
            m_sawAdAlready = true;
            REWARD_THE_PLAYER();
        }
        else if (showResult == ShowResult.Skipped)
        {
            Debug.Log("You dont deserve a kiss");
        }
        else if (showResult == ShowResult.Failed)
        {
            Debug.LogWarning("The ad did not finish due to an error.");
        }
    }

    public void OnUnityAdsDidError(string message)
    {
    }

    public void OnUnityAdsDidStart(string surfacingId)
    {
    }
    #endregion
}