using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreenHandler : MonoBehaviour
{
    #region Private Serialized Fields
    [Header("Behavior")]
    [SerializeField] private CanvasGroup _madeWithLove;
    [SerializeField] private float _showAfter = 2.5f;
    #endregion

    #region Unity Callbacks
    private void Start()
    {
        StartCoroutine(SplashBehavior());
    }
    #endregion

    #region Private Methods
    private void EndSplash()
    {
        SceneManager.LoadSceneAsync("Map 0");
    }
    #endregion

    #region Coroutines
    IEnumerator SplashBehavior()
    {
        yield return new WaitForSeconds(_showAfter);

        while(_madeWithLove.alpha < 0.95f)
        {
            _madeWithLove.alpha += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }

        EndSplash();
    }
    #endregion

}
