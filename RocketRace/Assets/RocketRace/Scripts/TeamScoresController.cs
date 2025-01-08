using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamScoresController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TeamScore teamScorePrefab;
    [SerializeField] private Transform teamScoresParent;

    [Header("Data")]
    [SerializeField] private GameSettingsSO gameSettings;

    private List<TeamScore> teamScores = new List<TeamScore>();
    private int previousTeamIndex;

    internal void InitializeScores()
    {
        for (int i = 0; i < gameSettings.currentTeams.Count; i++)
        {
            TeamScore teamScore = Instantiate(teamScorePrefab, teamScoresParent);
            teamScore.SetTeamScore(gameSettings.currentTeams[i].teamName, 0, i);
            teamScores.Add(teamScore);
        }
    }

    internal void UpdateTeamScore(int currentTeamIndex, bool isCorrect)
    {
        teamScores[currentTeamIndex].AddScore(isCorrect);
    }

    internal void HighlightCurrentTeam(int currentTeamIndex)
    {
        teamScores[previousTeamIndex].ToggleHighlight(false);
        previousTeamIndex = currentTeamIndex;
        teamScores[currentTeamIndex].ToggleHighlight(true);
    }

    internal List<TeamScore> GetWinners()
    {
        List<TeamScore> winners = new List<TeamScore>();
        
        // If no teams, return empty list
        if (teamScores.Count == 0)
            return winners;
            
        // Find the highest score
        int highestScore = teamScores[0].Score;
        foreach (TeamScore team in teamScores)
        {
            if (team.Score > highestScore)
            {
                highestScore = team.Score;
            }
        }
        
        // Add all teams that have the highest score (handles ties)
        foreach (TeamScore team in teamScores)
        {
            if (team.Score == highestScore)
            {
                winners.Add(team);
            }
        }
        
        return winners;
    }
}
