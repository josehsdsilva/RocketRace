using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameSettingsSO gameSettings;

    void Start()
    {
        // Access settings directly from the SO
        Debug.Log($"Number of teams: {gameSettings.currentTeams.Count}");
        Debug.Log($"Sound set: {gameSettings.soundSet}");
        Debug.Log($"Timer duration: {gameSettings.timerDuration}");
        
        // Use the settings
        foreach (var team in gameSettings.currentTeams)
        {
            Debug.Log($"Team: {team.teamName}, Rocket: {team.rocket}");
            // Create your rockets here
        }
    }
}
