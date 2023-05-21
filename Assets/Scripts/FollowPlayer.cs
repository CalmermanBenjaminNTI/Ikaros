using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    
    // Update is called once per frame
    void LateUpdate()
    {
        //Follow the player
        transform.position = _player.transform.position + new Vector3(0,7,-3);
    }
}
