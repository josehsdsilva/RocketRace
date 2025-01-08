using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestionController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Text questionText;
    [SerializeField] private TMP_Text currentTeamText;
    [SerializeField] private TMP_Text currentRoundText;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private RawImage spaceshipRender;
    [SerializeField] private AnswerOption answerOptionPrefab;
    [SerializeField] private Transform answerOptionsParent;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private List<RenderTexture> renderTextures;

    [Header("Settings")]
    [SerializeField] private AudioClip correctSound;
    [SerializeField] private AudioClip wrongSound;
    
    [Header("Data")]
    [SerializeField] private GameSettingsSO gameSettings;

    private QuestionData currentQuestionData;
    private Action<bool> onAnswered;
    private List<AnswerOption> answerOptions = new List<AnswerOption>();

    private float timer;
    private bool answering;

    private void Update()
    {
        if (answering)
        {
            timer -= Time.deltaTime;
            timerText.text = Mathf.CeilToInt(timer).ToString();
            if (timer <= 0)
            {
                OnAnswerClick(-1);
            }
        }
    }

    internal void SetQuestionData(QuestionData questionData, int currentRound, int currentTeamIndex, string currentTeamName, Action<bool> onAnswered)
    {
        this.onAnswered = onAnswered;
        currentQuestionData = questionData;

        currentRoundText.text = $"Round: {currentRound + 1} / {gameSettings.numberOfRounds}";
        currentTeamText.text = $"Team: {currentTeamName}";

        questionText.text = questionData.questionText;

        answering = true;
        timer = gameSettings.timerDuration;

        spaceshipRender.texture = renderTextures[currentTeamIndex];

        for (int i = 0; i < questionData.answerOptions.Count; i++)
        {
            AnswerOption answerOption = answerOptions.Count > i ? answerOptions[i] : Instantiate(answerOptionPrefab, answerOptionsParent);
            answerOption.gameObject.SetActive(true);
            answerOption.SetAnswerOption(questionData.answerOptions[i], OnAnswerClick);
            if(i >= answerOptions.Count) answerOptions.Add(answerOption);
        }
        for (int i = questionData.answerOptions.Count; i < answerOptions.Count; i++)
        {
            answerOptions[i].gameObject.SetActive(false);
        }

        audioSource.clip = questionData.questionAudio;
        audioSource.Play();
    }

    private void OnAnswerClick(int answerIndex)
    {
        if (!answering) return;

        answering = false;
        onAnswered?.Invoke(currentQuestionData.correctAnswerIndex == answerIndex);
        PlayAnswerSound(currentQuestionData.correctAnswerIndex == answerIndex);
    }

    private void PlayAnswerSound(bool correct)
    {
        audioSource.Stop();
        audioSource.clip = correct ? correctSound : wrongSound;
        audioSource.Play();
    }
}