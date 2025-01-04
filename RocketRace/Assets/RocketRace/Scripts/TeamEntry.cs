using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Linq;

public class TeamEntry : MonoBehaviour
{
    [SerializeField] private TMP_InputField teamNameInput;
    [SerializeField] private TMP_Dropdown spaceshipDropdown;

    private void Start()
    {
        InitializeDropdowns();
    }

    private void InitializeDropdowns()
    {
        // Setup rocket options
        spaceshipDropdown.ClearOptions();
        
        spaceshipDropdown.AddOptions(Enum.GetNames(typeof(Spaceship)).ToList());
    }

    public GameSettingsSO.TeamData GetTeamData()
    {
        return new GameSettingsSO.TeamData
        {
            teamName = string.IsNullOrEmpty(teamNameInput.text) ? "Team" : teamNameInput.text,
            rocket = spaceshipDropdown.options[spaceshipDropdown.value].text
        };
    }

    internal Spaceship GetSpaceship()
    {
        return (Spaceship)spaceshipDropdown.value;
    }
}

public enum Spaceship
{
    Yellow,
    Red,
    Black,
    Purple,
    Blue,
    Orange,
    Gray,
    Green
}