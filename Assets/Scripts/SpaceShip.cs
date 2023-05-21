using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShip
{
    // The index of the prefab to use for this spaceship
    public int PrefabIndex { get; private set; }
    // The turning speed of the spaceship along all axes separately
    protected Vector3 _turningSpeed = new Vector3(200, 200, 200);
    // The acceleration of the spaceship
    protected float _thrust = 5f;

    /// <summary>
    /// Creates a new spaceship
    /// </summary>
    /// <param name="prefabIndex"></param>
    /// <param name="turningSpeed"></param>
    /// <param name="thrust"></param>
    public SpaceShip(int prefabIndex, Vector3 turningSpeed, float thrust)
    {
        this.PrefabIndex = prefabIndex;
        this._turningSpeed = turningSpeed;
        this._thrust = thrust;
    }

    /// <summary>
    /// Moves the player using standard plane controls bound through the Input Manager
    /// </summary>
    /// <param name="t"></param>
    /// <param name="v"></param>
    public Vector3 Move(Transform t, Vector3 v)
    {
        //Rotate the spaceship based on rotational axes
        t.Rotate(Time.deltaTime * Input.GetAxis("Pitch") * _turningSpeed.x, 0, 0);
        t.Rotate(0, Time.deltaTime * Input.GetAxis("Yaw") * _turningSpeed.y, 0);
        t.Rotate(0, 0, Time.deltaTime * Input.GetAxis("Roll") * _turningSpeed.z);

        //Move the spaceship forward based on the throttle
        v += t.forward * Time.deltaTime * (Input.GetAxis("Throttle") + 1) * _thrust;

        //Return the new velocity
        return v;
    }
}
