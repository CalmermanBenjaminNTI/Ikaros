using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedSpaceShip : SpaceShip
{
    // The speed at which the plane can move along the cardinal translation axes
    protected Vector3 _hoverSpeed = new Vector3(1,1,1);

    /// <summary>
    /// Creates a new advanced spaceship
    /// </summary>
    /// <param name="prefabIndex"></param>
    /// <param name="turningSpeed"></param>
    /// <param name="thrust"></param>
    /// <param name="hoverSpeed"></param>
    /// <returns></returns>
    public AdvancedSpaceShip(int prefabIndex, Vector3 turningSpeed, float thrust, Vector3 hoverSpeed) : base(prefabIndex, turningSpeed, thrust)
    {
        this._hoverSpeed = hoverSpeed;
    }

    /// <summary>
    /// Moves the plane along cardinal translation axes
    /// </summary>
    /// <param name="t"></param>
    /// <param name="v"></param>
    public Vector3 Hover(Transform t, Vector3 v)
    {
        // Move the spaceship along the cardinal axes
        v += (Time.deltaTime * _hoverSpeed.x * Input.GetAxis("Right") * t.right);
        v += (Time.deltaTime * _hoverSpeed.y * Input.GetAxis("Up") * t.up);
        v += (Time.deltaTime * _hoverSpeed.z * Input.GetAxis("Forward") * t.forward);

        // Return the new velocity
        return v;
    }
}
