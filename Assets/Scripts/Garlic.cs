using System;
using UnityEngine;

public class Garlic : MonoBehaviour
{
    [Range(1,20)]
    public float range;

    public int damage;

    public int durationInTurns;
    public bool DecrementLifeTime()
    {
        durationInTurns--;
        if (durationInTurns <= 0)
        {
            Destroy(gameObject);
            return true;
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    public bool IsInRange(Vector2 pos)
    {
        return Vector2.Distance(pos, transform.position) < range;
    }
}