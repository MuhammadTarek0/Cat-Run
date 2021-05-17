using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

public class BannerAdScript : MonoBehaviour
{
    #region Private Serialized Fields
    [Header("Data")]
    [SerializeField] private string gameId = "1234567";
    [SerializeField] private string surfacingId = "bannerPlacement";
    [SerializeField] private bool testMode = true;
    #endregion

    #region Unity Callbacks
    private void OnEnable()
    {
        WinTrigger.PLAYER_WON += HideBanner;
        EnemyController.PLAYER_LOSE += HideBanner;

        StartCoroutine(ShowBannerWhenReady());
    }

    private void OnDisable()
    {
        WinTrigger.PLAYER_WON -= HideBanner;
        EnemyController.PLAYER_LOSE -= HideBanner;
    }
    #endregion

    #region Private Methods
    private void HideBanner()
    {
        Advertisement.Banner.Hide();
    }
    #endregion

    #region Coroutines
    IEnumerator ShowBannerWhenReady()
    {
        while (!Advertisement.IsReady(surfacingId))
        {
            yield return new WaitForSeconds(0.5f);
        }

        Advertisement.Banner.Show(surfacingId);
        Advertisement.Banner.SetPosition(BannerPosition.TOP_CENTER);
    }
    #endregion
}