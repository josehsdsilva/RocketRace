using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private GameSettingsSO gameSettings;
    [SerializeField] private List<QuestionData> questionDatas;

    [Header("References")]
    [SerializeField] private SpaceshipController spaceshipController;
    [SerializeField] private QuestionController questionController;

    private int currentRound = 0;
    private int currentTeamIndex = 0;

    private void Update()
    {
        // Check for game over condition
        if (currentRound >= gameSettings.numberOfRounds)
        {
            spaceshipController.ResetPosition();
            Debug.Log("Game Ended!");
            return;
        }

        // Check for turn end condition
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartNextTurn();
        }
    }

    private void StartNextTurn()
    {
        // Get the current team
        GameSettingsSO.TeamData currentTeam = gameSettings.currentTeams[currentTeamIndex];
        Debug.Log($"Current team: {currentTeam.teamName}");

        spaceshipController.SetSpaceshipColor(currentTeam.spaceshipColor);
        spaceshipController.SetOnQuestion();
        questionController.SetQuestionData(questionDatas[currentTeamIndex + currentRound *  gameSettings.currentTeams.Count], currentRound, gameSettings.numberOfRounds, currentTeam.teamName);

        // Increment the team index
        currentTeamIndex++;
        if (currentTeamIndex >= gameSettings.currentTeams.Count)
        {
            currentTeamIndex = 0;
            currentRound++;
        }
    }

}