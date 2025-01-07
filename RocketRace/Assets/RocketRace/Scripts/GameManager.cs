using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum QuestionTheme
{
    instruments,
    chords
}

[Serializable]
internal class QuestionThemeData
{
    public QuestionTheme theme;
    public List<QuestionData> questions;
}

[Serializable]
internal class QuestionData
{
    public AudioClip questionAudio;
    public string questionText;
    public List<AnswerData> answerOptions;
    public int correctAnswerIndex;
}

[Serializable]
internal class AnswerData
{
    public string answerText;
    public Sprite sprite;
}

public class GameManager : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private GameSettingsSO gameSettings;
    [SerializeField] private QuestionDataLoader questionDataLoader;

    [Header("References")]
    [SerializeField] private QuestionController questionController;
    [SerializeField] private TeamScore teamScorePrefab;
    [SerializeField] private Transform teamScoresParent;
    [SerializeField] private NotificationController notificationController;

    private int currentRound = 0;
    private int currentTeamIndex = 0;
    private QuestionTheme questionTheme;
    private GameSettingsSO.TeamData currentTeam;
    private List<QuestionData> questionData;
    private List<QuestionThemeData> questionDatas;
    private QuestionData currentQuestionData;
    private List<TeamScore> teamScores = new List<TeamScore>();

    private void Start()
    {
        questionDataLoader.LoadQuestionData(OnQuestionDataLoaded);
    }

    private void OnQuestionDataLoaded(List<QuestionThemeData> loadedData)
    {
        questionDatas = loadedData;
        Debug.Log("Question data loaded and ready to use");
        questionTheme = gameSettings.questionTheme;
        questionData = questionDatas.Find(x => x.theme == questionTheme).questions;
        questionData = questionData.OrderBy(x => UnityEngine.Random.value).ToList();
        InitializeScores();
        StartNextTurn();
    }
    
    private void InitializeScores()
    {
        for (int i = 0; i < gameSettings.currentTeams.Count; i++)
        {
            TeamScore teamScore = Instantiate(teamScorePrefab, teamScoresParent);
            teamScore.SetTeamScore(gameSettings.currentTeams[i].teamName, 0, i);
            teamScores.Add(teamScore);
        }
    }

    private void StartNextTurn()
    {
        // Get the current team
        currentTeam = gameSettings.currentTeams[currentTeamIndex];
        currentQuestionData = questionData[currentTeamIndex + currentRound *  gameSettings.currentTeams.Count];
        questionController.SetQuestionData(currentQuestionData, currentRound, currentTeamIndex, currentTeam.teamName, OnPlayAnswered);

        // Increment the team index
        currentTeamIndex++;
        if (currentTeamIndex >= gameSettings.currentTeams.Count)
        {
            currentTeamIndex = 0;
            currentRound++;
        }
    }

    private void OnPlayAnswered(bool isCorrect)
    {
        if (isCorrect)
        {
            notificationController.ShowNotification("Correct Answer!", "Well done!", OnNotificationClosed);
        }
        else
        {
            notificationController.ShowNotification("Incorrect Answer!", "Better luck next time!", OnNotificationClosed);
        }

        teamScores[currentTeamIndex].AddScore(isCorrect);
    }

    private void OnNotificationClosed()
    {
        if (currentRound >= gameSettings.numberOfRounds)
        {
            notificationController.ShowNotification("Game ended", "Let's see who won", ShowWinners);
            return;
        }

        StartNextTurn();
    }

    private void ShowWinners()
    {
        // ToDo: ShowWinners
    }
}