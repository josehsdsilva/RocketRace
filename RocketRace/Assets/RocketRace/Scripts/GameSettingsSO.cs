using UnityEngine;
using System.Collections.Generic;
using System;

[CreateAssetMenu(fileName = "GameSettings", menuName = "RocketRace/GameSettings")]
public class GameSettingsSO : ScriptableObject
{
    [Header("Game Settings")]
    public int soundSet = 0;
    public int timerDuration = 120;
    public int numberOfRounds = 5;
    public readonly int maxTeams = 4;
    public QuestionTheme questionTheme => (QuestionTheme)soundSet;

    [Header("Runtime Data")]
    public List<TeamData> currentTeams = new List<TeamData>();
    
    [Serializable]
    public class TeamData
    {
        public string teamName;
        public SpaceshipType spaceshipType;
        public SpaceshipColor spaceshipColor;
    }

    public void UpdateTeams(List<TeamData> teams)
    {
        currentTeams = teams;
    }
}