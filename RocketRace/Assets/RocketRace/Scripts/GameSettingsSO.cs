using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "GameSettings", menuName = "RocketRace/GameSettings")]
public class GameSettingsSO : ScriptableObject
{
    [Header("Game Settings")]
    public string soundSet = "instruments";
    public int timerDuration = 120;
    public int numberOfRounds = 5;
    public int maxTeams = 20;

    [Header("Runtime Data")]
    public List<TeamData> currentTeams = new List<TeamData>();
    
    [System.Serializable]
    public class TeamData
    {
        public string teamName;
        public string rocket;
    }

    public void UpdateTeams(List<TeamData> teams)
    {
        currentTeams = teams;
    }
}