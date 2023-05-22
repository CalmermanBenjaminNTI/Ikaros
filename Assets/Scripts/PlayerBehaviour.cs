using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    // Variables
    // The player's velocity
    [SerializeField] private Vector3 _velocity;
    // Spaceship behaviours
    [SerializeField] private Dictionary<string, SpaceShip> _spaceShips = new Dictionary<string, SpaceShip>();
    // Spaceship prefabs
    [SerializeField] private List<GameObject> _spaceShipPrefabs = new List<GameObject>();
    // The current spaceship
    [SerializeField] private string _currentSpaceShip;

    // Start is called before the first frame update
    void Start()
    {
        // Add spaceships to the dictionary
        _spaceShips.Add("crescent", new AdvancedSpaceShip(0, new Vector3(200, 200, 200), 5f, new Vector3(2, 2, 2)));
        _spaceShips.Add("herald", new SmartSpaceShip(1, new Vector3(200, 200, 200), 5f));
        _spaceShips.Add("stargazer", new SpaceShip(2, new Vector3(200, 200, 200), 5f));

        // Try set the current spaceship to the one the player chose else default to Crescent
        try
        {
            _spaceShipPrefabs[_spaceShips[_currentSpaceShip.ToLower()].PrefabIndex].SetActive(true);
        }
        catch
        {
            Debug.Log("Space ship not found, defaulting to Crescent");
            _currentSpaceShip = "Crescent";
            _spaceShipPrefabs[_spaceShips[_currentSpaceShip.ToLower()].PrefabIndex].SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Move the player according to the spaceship's behaviour
        _velocity = _spaceShips[_currentSpaceShip].Move(transform, _velocity);

        // If the player is using an advanced spaceship, hover
        if (_spaceShips[_currentSpaceShip] is AdvancedSpaceShip)
        {
            // Cast the spaceship to an advanced spaceship
            AdvancedSpaceShip spaceShip = (AdvancedSpaceShip)_spaceShips[_currentSpaceShip];
            _velocity = spaceShip.Hover(transform, _velocity);
        }

        // Move the player according to the velocity
        transform.position += _velocity;
        // Apply drag to the velocity
        _velocity = _velocity * (1f - 1f * Time.deltaTime);

        // If the velocity is very small, set it to 0
        if (Mathf.Abs(_velocity.x) + Mathf.Abs(_velocity.y) + Mathf.Abs(_velocity.z) < 0.0001f) _velocity = new Vector3(0, 0, 0);
    }
}
