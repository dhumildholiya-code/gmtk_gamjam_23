using UnityEngine;

namespace gmtk_gamejam
{
    public interface ITarget
    {
        ITakeDamage Damagable { get; }
        Transform GetTransform();
        Vector2 GetPos(float timePased);
    }
}