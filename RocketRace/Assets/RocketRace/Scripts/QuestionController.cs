using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestionController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Text questionText;
    [SerializeField] private TMP_Text currentTeamText;
    [SerializeField] private TMP_Text currentRoundText;
    [SerializeField] private AnswerOption answerOptionPrefab;
    [SerializeField] private Transform answerOptionsParent;

    private QuestionData currentQuestionData;

    internal void SetQuestionData(QuestionData questionData, int currentRound, int maxRounds, string currentTeamName)
    {
        currentRoundText.text = $"Round: {currentRound + 1} / {maxRounds}";
        currentTeamText.text = $"Team: {currentTeamName}";

        currentQuestionData = questionData;
        ClearQuestion();

        questionText.text = questionData.questionText;

        foreach (AnswerData answerData in questionData.answerOptions)
        {
            AnswerOption answerOption = Instantiate(answerOptionPrefab, answerOptionsParent);
            answerOption.SetAnswerOption(answerData, OnAnswer);
        }
    }

    private void OnAnswer(int answerIndex)
    {
        if(currentQuestionData.correctAnswerIndex == answerIndex)
        {
            Debug.Log("Correct Answer!");
        }
        else
        {
            Debug.Log("Wrong Answer!");
        }
    }

    private void ClearQuestion()
    {
        foreach (Transform child in answerOptionsParent)
        {
            Destroy(child.gameObject);
        }
    }

}

[Serializable]
internal class QuestionData
{
    public AudioClip questionAudio;
    public string questionText;
    public List<AnswerData> answerOptions;
    public int correctAnswerIndex;
}