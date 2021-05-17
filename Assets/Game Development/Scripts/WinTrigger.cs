using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    #region Public Events
    public delegate void OnNullEventTrigger();
    public static event OnNullEventTrigger PLAYER_WON = delegate { };
    #endregion

    #region Unity Callbacks
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        other.GetComponent<PlayerController>().HandleDeath();
        Vibration.Vibrate();
        PLAYER_WON();
    }
    #endregion

}
