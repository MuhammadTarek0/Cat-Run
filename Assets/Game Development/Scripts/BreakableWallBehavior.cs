using UnityEngine;

public class BreakableWallBehavior : MonoBehaviour
{
    #region Private Serialized Fields
    [SerializeField] private Collider[] _blockingCollider;
    [SerializeField] private GameObject _tutorialStuff;
    #endregion

    #region Unity Callbacks
    private void OnEnable()
    {
        Collectables.TOOK_SUPERPOWER_GEM += MakeItBreakable;
    }

    private void OnDisable()
    {
        Collectables.TOOK_SUPERPOWER_GEM -= MakeItBreakable;
    }
    #endregion

    #region Private Methods
    private void MakeItBreakable()
    {
        foreach (Collider collider in _blockingCollider)
            collider.enabled = false;

        if (_tutorialStuff)
        {
            _tutorialStuff.SetActive(true);
            Destroy(_tutorialStuff.gameObject, 4f);
        }
    }
    #endregion
}
