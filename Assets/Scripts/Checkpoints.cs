using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.Networking;

public class Checkpoints : MonoBehaviour
{
    // Current checkpoint index
    private int _checkpointIndex = 0;
    // List of checkpoints
    [SerializeField] private List<GameObject> _checkpoints = new List<GameObject>();
    // Player object
    [SerializeField] private GameObject _player;
    // Text output
    [SerializeField] private TMP_Text _outpuText;
    // Start, end and best time
    private DateTime _startTime;
    private DateTime _endTime;
    public string BestTime { get; set;}
    
    // Start is called before the first frame update
    void Start()
    {
        //print(GameObject.FindGameObjectsWithTag("Checkpoint"));
        // Add all game objects with the tag "Checkpoint" to the list
        foreach (GameObject checkpoint in GameObject.FindGameObjectsWithTag("Checkpoint"))
        {
            _checkpoints.Add(checkpoint);
            checkpoint.SetActive(false);
        }   

        // After some file management these lines were added to sort the checkpoints in the correct order
        _checkpoints.Sort((x, y) => x.name.CompareTo(y.name));
        _checkpoints.Reverse();
        
        // Set the first checkpoint to active
        _checkpointIndex = _checkpoints.Count - 1;
        _checkpoints[_checkpointIndex].SetActive(true);
        
        print("Getting data");
        // Get the best time from the Google Sheets API
        gameObject.GetComponent<GoogleRequests>().GetHighScore();
        print("Got data");
        // Debug purposes
        //gameObject.GetComponent<GoogleRequests>().SetData("69:69:69");
        //print("Sent data");
    }

    // Update is called once per frame
    void Update()
    {
        // If the player has reached the last checkpoint
        if (_checkpointIndex == 0)
        {
            if (Vector3.Distance(_player.transform.position, _checkpoints[_checkpointIndex].transform.position) < 50)
            {
                // Set the end time and find out how long it took to complete the course
                _endTime = DateTime.Now;
                TimeSpan timeSpan = _endTime - _startTime;
                Debug.Log("Time: " + timeSpan);
                
                // If the time is better than the current best time, replace it
                string[] bestTimes = {timeSpan.ToString(), BestTime};
                if (BestTime != "" && BestTime != null) Array.Sort(bestTimes);

                // Display the time it took to complete the course and the best time
                _outpuText.text = "Completion time:\n" + timeSpan.ToString() + "\nBest time:\n" + bestTimes[0];
                _checkpointIndex--;
                
                // Send the best time to the Google Sheets API
                gameObject.GetComponent<GoogleRequests>().SetData(bestTimes[0]);
            }
            else
                _outpuText.text = "Distance to next checkpoint:\n" + Vector3.Distance(_player.transform.position, _checkpoints[_checkpointIndex].transform.position).ToString();
        }
        else
        {
            // If the player is still playing
            if (_checkpointIndex > 0)
            {
                // Display the distance to the next checkpoint
                _outpuText.text = "Distance to next checkpoint:\n" + Vector3.Distance(_player.transform.position, _checkpoints[_checkpointIndex].transform.position).ToString();
                // If the player is close enough to the next checkpoint, deactivate it and activate the next one
                if (Vector3.Distance(_player.transform.position, _checkpoints[_checkpointIndex].transform.position) < 50)
                {
                    // If it's the first checkpoint, set the start time
                    if (_checkpointIndex == _checkpoints.Count - 1) _startTime = DateTime.Now;
                    _checkpoints[_checkpointIndex].SetActive(false);
                    _checkpointIndex--;
                    _checkpoints[_checkpointIndex].SetActive(true);
                }
            }
        }
    }
}
