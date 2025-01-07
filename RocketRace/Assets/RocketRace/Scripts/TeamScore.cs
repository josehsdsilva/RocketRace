using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TeamScore : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Text teamNameText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private RawImage spaceshipRender;
    [SerializeField] private List<RenderTexture> spaceshipDisplayers;

    private int score;

    internal void AddScore(bool isCorrect)
    {
        score += isCorrect ? 30 : 10;
        scoreText.text = score.ToString();
    }

    internal void SetTeamScore(string teamName, int score, int teamID)
    {
        teamNameText.text = teamName;
        scoreText.text = score.ToString();
        spaceshipRender.texture = spaceshipDisplayers[teamID];
    }
}
