using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnswerOption : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Button answerButton;
    [SerializeField] private Image answerImage;
    [SerializeField] private TMP_Text answerText;

    internal void SetAnswerOption(AnswerData answerData, Action<int> onAnswer)
    {
        answerImage.gameObject.SetActive(answerData.sprite);
        answerImage.sprite = answerData.sprite;
        answerText.text = answerData.answerText;
        answerButton.onClick.AddListener(() => onAnswer?.Invoke(transform.GetSiblingIndex()));
    }
}