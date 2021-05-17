using UnityEngine;
using UnityEngine.AI;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private NavMeshSurface _unbreakableNavSurface, _breakableNavSurface;

    private void OnEnable()
    {
        Collectables.TOOK_SUPERPOWER_GEM += LoadBrokeWallData;
    }

    private void OnDisable()
    {
        Collectables.TOOK_SUPERPOWER_GEM -= LoadBrokeWallData;
    }

    public void LoadBrokeWallData()
    {
        if (!_unbreakableNavSurface || _breakableNavSurface) return;

        _unbreakableNavSurface.enabled = false;
        _breakableNavSurface.enabled = true;
    }
}
