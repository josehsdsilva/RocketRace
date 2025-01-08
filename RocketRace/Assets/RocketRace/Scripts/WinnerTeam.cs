using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinnerTeam : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Text teamNameText;
    [SerializeField] private RawImage spaceshipRender;
    [SerializeField] private List<RenderTexture> spaceshipDisplayers;

    private int score;

    internal void SetTeam(string teamName, int teamID)
    {
        teamNameText.text = teamName;
        spaceshipRender.texture = spaceshipDisplayers[teamID];
    }
}
