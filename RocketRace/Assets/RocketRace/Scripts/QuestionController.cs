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
    [SerializeField] private AudioSource audioSource;
    
    [Header("Data")]
    [SerializeField] private GameSettingsSO gameSettings;

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
            answerOption.SetAnswerOption(answerData, OnAnswerClick);
        }

        audioSource.clip = questionData.questionAudio;
        audioSource.Play();
    }

    private void OnAnswerClick(int answerIndex)
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