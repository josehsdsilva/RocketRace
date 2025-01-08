using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinnersDisplayController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject winnersUI;
    [SerializeField] private WinnerTeam winnerTeamPrefab;
    [SerializeField] private Transform winnersHolder;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        winnersUI.SetActive(false);
        mainMenuButton.onClick.AddListener(GoToMainMenu);
    }

    private void GoToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    internal void ShowWinners(List<TeamScore> winners)
    {
        winnersUI.SetActive(true);
        for (int i = 0; i < winners.Count; i++)
        {
            WinnerTeam winnerTeam = Instantiate(winnerTeamPrefab, winnersHolder);
            winnerTeam.SetTeam(winners[i].TeamName, winners[i].TeamID);
        }
        audioSource.Play();
    }
}
