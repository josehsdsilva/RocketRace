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
    [SerializeField] private SpaceshipController spaceshipController;
    [SerializeField] private QuestionController questionController;

    private int currentRound = 0;
    private int currentTeamIndex = 0;
    private QuestionTheme questionTheme;
    private GameSettingsSO.TeamData currentTeam;
    private List<QuestionData> questionData;
    private List<QuestionThemeData> questionDatas;

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
    }

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
        currentTeam = gameSettings.currentTeams[currentTeamIndex];

        spaceshipController.SetOnQuestion(currentTeam.spaceshipType, currentTeam.spaceshipColor);
        questionController.SetQuestionData(questionData[currentTeamIndex + currentRound *  gameSettings.currentTeams.Count], currentRound, gameSettings.numberOfRounds, currentTeam.teamName);

        // Increment the team index
        currentTeamIndex++;
        if (currentTeamIndex >= gameSettings.currentTeams.Count)
        {
            currentTeamIndex = 0;
            currentRound++;
        }
    }
}