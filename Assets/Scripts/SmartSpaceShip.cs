using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartSpaceShip : SpaceShip
{

    /// <summary>
    /// Creates a new advanced spaceship
    /// </summary>
    /// <param name="prefabIndex"></param>
    /// <param name="turningSpeed"></param>
    /// <param name="thrust"></param>
    /// <returns></returns>
    public SmartSpaceShip(int prefabIndex, Vector3 turningSpeed, float thrust) : base(prefabIndex, turningSpeed, thrust)
    {
    }

    /// <summary>
    /// Moves the player using standard plane controls bound through the Input Manager
    /// </summary>
    /// <param name="t"></param>
    /// <param name="v"></param>
    override public Vector3 Move(Transform t, Vector3 v)
    {
        //Rotate the spaceship based on rotational axes
        t.Rotate(Time.deltaTime * Input.GetAxis("Pitch") * _turningSpeed.x, 0, 0);
        t.Rotate(0, Time.deltaTime * Input.GetAxis("Yaw") * _turningSpeed.y, 0);
        t.Rotate(0, 0, Time.deltaTime * Input.GetAxis("Roll") * _turningSpeed.z);

        //Stabelize the spaceship
        if (Input.GetAxis("Stabilize") > 0)
        {
            // Lerp to the rotation where global up is the local up
            t.rotation = Quaternion.Slerp(t.rotation, Quaternion.Euler(0, t.rotation.eulerAngles.y, 0), Time.deltaTime * 2);
        }

        //Move the spaceship forward based on the throttle
        v += t.forward * Time.deltaTime * (Input.GetAxis("Throttle") + 1) * _thrust;

        //Return the new velocity
        return v;
    }
}
