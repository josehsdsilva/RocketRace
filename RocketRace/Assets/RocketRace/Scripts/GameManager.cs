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
    [SerializeField] private NotificationController notificationController;
    [SerializeField] private TeamScoresController teamScoresController;
    [SerializeField] private WinnersDisplayController winnersDisplayController;
    [SerializeField] private SpaceshipProjectorsController spaceshipProjectorsController;
    [SerializeField] private GameObject gameUI;

    private int currentRound = 0;
    private int currentTeamIndex = 0;
    private QuestionTheme questionTheme;
    private GameSettingsSO.TeamData currentTeam;
    private List<QuestionData> questionData;
    private List<QuestionThemeData> questionDatas;
    private QuestionData currentQuestionData;

    private void Start()
    {
        gameUI.SetActive(true);

        questionDataLoader.LoadQuestionData(OnQuestionDataLoaded);
    }

    private void OnQuestionDataLoaded(List<QuestionThemeData> loadedData)
    {
        questionDatas = loadedData;
        Debug.Log("Question data loaded and ready to use");
        questionTheme = gameSettings.questionTheme;
        questionData = questionDatas.Find(x => x.theme == questionTheme).questions;
        questionData = questionData.OrderBy(x => UnityEngine.Random.value).ToList();
        teamScoresController.InitializeScores();
        StartNextTurn();
    }
    
    

    private void StartNextTurn()
    {
        // Get the current team
        currentTeam = gameSettings.currentTeams[currentTeamIndex];
        currentQuestionData = questionData[currentTeamIndex + currentRound *  gameSettings.currentTeams.Count];
        questionController.SetQuestionData(currentQuestionData, currentRound, OnPlayAnswered);
        teamScoresController.HighlightCurrentTeam(currentTeamIndex);
        HideAllSpaceshipTrails();
    }

    private void HideAllSpaceshipTrails()
    {
        for (int i = 0; i < gameSettings.currentTeams.Count; i++)
        {
            spaceshipProjectorsController.ResetParticleEffects(i);
        }
    }

    private void OnPlayAnswered(bool isCorrect)
    {
        spaceshipProjectorsController.OnAnswer(currentTeamIndex, isCorrect);

        if (isCorrect)
        {
            notificationController.ShowNotification("Correct Answer!", "Well done!", OnNotificationClosed);
        }
        else
        {
            notificationController.ShowNotification("Incorrect Answer!", "Better luck next time!", OnNotificationClosed);
        }

        teamScoresController.UpdateTeamScore(currentTeamIndex, isCorrect);
    }

    private void OnNotificationClosed()
    {
        spaceshipProjectorsController.ResetParticleEffects(currentTeamIndex);


        // Increment the team index
        currentTeamIndex++;
        if (currentTeamIndex >= gameSettings.currentTeams.Count)
        {
            currentTeamIndex = 0;
            currentRound++;
        }

        if (currentRound >= gameSettings.numberOfRounds)
        {
            notificationController.ShowNotification("Game ended", "Let's see who won", ShowWinners);
            return;
        }

        StartNextTurn();
    }

    private void ShowWinners()
    {
        gameUI.SetActive(false);
        winnersDisplayController.ShowWinners(teamScoresController.GetWinners());
    }
}