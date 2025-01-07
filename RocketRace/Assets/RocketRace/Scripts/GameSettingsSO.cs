using UnityEngine;
using System.Collections.Generic;
using System;

[CreateAssetMenu(fileName = "GameSettings", menuName = "RocketRace/GameSettings")]
public class GameSettingsSO : ScriptableObject
{
    [Header("Game Settings")]
    public string soundSet = "instruments";
    public int timerDuration = 120;
    public int numberOfRounds = 5;
    public readonly int maxTeams = 4;
    public QuestionTheme questionTheme;

    [Header("Runtime Data")]
    public List<TeamData> currentTeams = new List<TeamData>();
    
    [Serializable]
    public class TeamData
    {
        public string teamName;
        public SpaceshipColor spaceshipColor;
        public SpaceshipType spaceshipType;
    }

    public void UpdateTeams(List<TeamData> teams)
    {
        currentTeams = teams;
    }
}