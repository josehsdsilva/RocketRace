using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class TeamScore : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Text teamNameText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private RawImage spaceshipRender;
    [SerializeField] private List<RenderTexture> spaceshipDisplayers;

    private int score;
    internal int Score => score;
    private string teamName;
    internal string TeamName => teamName;
    private int teamID;
    internal int TeamID => teamID;

    internal void AddScore(bool isCorrect)
    {
        score += isCorrect ? 30 : 10;
        scoreText.text = score.ToString();
    }

    internal void SetTeamScore(string teamName, int score, int teamID)
    {
        this.teamID = teamID;
        this.teamName = teamName;
        teamNameText.text = teamName;
        scoreText.text = score.ToString();
        spaceshipRender.texture = spaceshipDisplayers[teamID];
    }
}
