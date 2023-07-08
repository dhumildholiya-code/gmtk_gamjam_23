using UnityEngine;

namespace gmtk_gamejam
{
    public interface ITarget
    {
        ITakeDamage Damagable { get; }
        Vector2 GetPos(float timePased);
    }
}