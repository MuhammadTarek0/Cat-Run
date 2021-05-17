using UnityEngine;

[CreateAssetMenu(menuName = "SO Variables/Vector3", fileName ="Vector3.asset")]
public class Vector3_Variable : ScriptableObject
{
    [SerializeField] private Vector3 _value;

    public Vector3 Value
    {
        get => _value;

        set => _value = value;
    }

    public enum Axis { X, Y, Z}
    
    public void SetAxis(Axis axis, float value)
    {
        switch(axis)
        {
            case Axis.X: { _value.x = value; } break;
            case Axis.Y: { _value.y = value; } break;
            case Axis.Z: { _value.z = value; } break;
        }
    }

}
