using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GoogleRequests : MonoBehaviour
{
    /// <summary>
    /// Fetches the stored highscore
    /// </summary>
    public void GetHighScore()
    {
        print("Tried something at least");
        StartCoroutine(GetHighscore("Gaymer"));
    }

    /// <summary>
    /// Updates the stored highscores
    /// </summary>
    public void SetData(string score)
    {
        StartCoroutine(UpdateHighscore("Gaymer", score));
    }

    /// <summary>
    /// Fetches the stored highscore
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    IEnumerator GetHighscore(string player)
    {
        print("Trying to create connection.");
        // Create a request for the URL
        UnityWebRequest www = UnityWebRequest.Get("https://sheetdb.io/api/v1/m26nws59rqzh5");
        // Send the request and wait for a response
        yield return www.SendWebRequest();
        // Check for errors
        if (www.isNetworkError || www.isHttpError || www.timeout > 2)
        {
            print("error" + www.error);
            print("Offline");
        }
        else
        {
            // Get json
            string jsonString = www.downloadHandler.text;
            // Reformat the json to avoid deserialization errors
            jsonString = "{\"data\": " + jsonString + "}";

            // Deserialize the json into a Highscores object
            var highscores = JsonUtility.FromJson<Highscores>(jsonString);
            
            // Set the highscore to the best time variable
            gameObject.GetComponent<Checkpoints>().BestTime = Highscore.GetHighscoreByPlayer(highscores.data, player);
        }
    }

    /// <summary>
    /// Updates the stored highscores
    /// </summary>
    /// <param name="player"></param>
    /// <param name="highscore"></param>
    /// <returns></returns>
    IEnumerator UpdateHighscore(string player, string highscore)
    {
        // Create a request for the URL
        UnityWebRequest request = new UnityWebRequest("https://sheetdb.io/api/v1/m26nws59rqzh5/player/" + player, "PATCH");
        // Manually serialize a json due to errors with Unity's JsonUtility
        string dataFormatted = "{\"data\":[{\"player\":\"" + player + "\",\"highscore\":\"" + highscore + "\"}]}";
        // Debug
        print(dataFormatted);
        // Add the json to the request (Got help from a friend with this block)
        byte[] data = new System.Text.UTF8Encoding().GetBytes(dataFormatted);
        request.uploadHandler = (UploadHandler) new UploadHandlerRaw(data);
        request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // Send the request and wait for a response
        yield return request.SendWebRequest();

        // Check if the destination is empty
        if (request.responseCode == 404)
        {
            StartCoroutine(CreateHighscore(player, highscore));
        }
        else if (request.isNetworkError || request.isHttpError || request.timeout > 2)
        {
            // ERRORZ
            print(request.responseCode);

            print("error" + request.error);
            print("Offline");
        }
        else
        {
            // Debug
            print($"Highscore of {highscore} has been updated for {player}.");
        }
    }

    /// <summary>
    /// Creates a new highscore entry if the player doesn't exist
    /// </summary>
    /// <param name="player"></param>
    /// <param name="highscore"></param>
    /// <returns></returns>
    IEnumerator CreateHighscore(string player, string highscore)
    {
        // Create a request for the URL
        UnityWebRequest request = new UnityWebRequest("https://sheetdb.io/api/v1/m26nws59rqzh5", "POST");
        // Manually serialize a json due to errors with Unity's JsonUtility
        string dataFormatted = "{\"data\":[{\"player\":\"" + player + "\",\"highscore\":\"" + highscore + "\"}]}";
        
        // Add the json to the request (Got help from a friend with this block)
        byte[] data = new System.Text.UTF8Encoding().GetBytes(dataFormatted);
        request.uploadHandler = (UploadHandler) new UploadHandlerRaw(data);
        request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // Send the request and wait for a response
        yield return request.SendWebRequest();

        // Check for errors
        if (request.isNetworkError || request.isHttpError || request.timeout > 2)
        {
            print("error" + request.error);
            print("Offline");
        }
        else
        {
            // Debug
            print($"Highscore of {highscore} has been created for {player}.");
        }
    }
}

// Highscores class
[System.Serializable]
public class Highscores
{
    public List<Highscore> data;
}

// Highscore class
[System.Serializable]
public class Highscore
{
    public string player;
    public string highscore;

    /// <summary>
    /// Returns the highscore of a player
    /// </summary>
    /// <param name="highscores"></param>
    /// <param name="player"></param>
    /// <returns></returns>
    public static string GetHighscoreByPlayer(List<Highscore> highscores, string player)
    {
        foreach (Highscore highscore in highscores)
        {
            if (highscore.player.Equals(player))
            {
                return highscore.highscore;
            }
        }

        return "";
    }
}