using UnityEngine;

public class InputManager : MonoBehaviour
{
    #region Private Serialized Fields
    [Header("Data")]
    [SerializeField] private Vector3_Variable _vector3_Variable;
    [SerializeField] private VariableJoystick _joystick;
    #endregion

    #region Unity Callbacks
    private void Update()
    {
        _vector3_Variable.SetAxis(Vector3_Variable.Axis.X, _joystick.Horizontal);
        _vector3_Variable.SetAxis(Vector3_Variable.Axis.Z, _joystick.Vertical);
    }
    #endregion
}
