using UnityEngine;
using CartoonFX;

public class ElectricShock : MonoBehaviour
{
    [SerializeField] private EnemyController[] _enemiesNearBy;
    [SerializeField] private ParticleSystem _groundIndicator, _lightingEffect;
    [SerializeField] private CFXR_Effect _lightImpact;

    private bool m_shoot;

    private void OnEnable()
    {
        ButtonBehavior.BUTTON_STATE_CHANGED += Activate;
        _lightImpact.cameraShake.StopShake();
    }

    private void OnDisable()
    {
        ButtonBehavior.BUTTON_STATE_CHANGED -= Activate;
    }

    public void Activate(bool activate)
    {
        if (m_shoot) return;

        m_shoot = true;

        foreach(EnemyController enemy in _enemiesNearBy)
            enemy.MyDayHasCame();

        _groundIndicator.Stop();
        _lightingEffect.Play();
        _lightImpact.cameraShake.StartShake();
    }
}
