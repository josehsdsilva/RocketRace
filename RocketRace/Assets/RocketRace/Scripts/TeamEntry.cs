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
        
        spaceshipDropdown.AddOptions(Enum.GetNames(typeof(SpaceshipColor)).ToList());
    }

    public GameSettingsSO.TeamData GetTeamData()
    {
        return new GameSettingsSO.TeamData
        {
            teamName = string.IsNullOrEmpty(teamNameInput.text) ? "Team" : teamNameInput.text,
            spaceshipColor = (SpaceshipColor)spaceshipDropdown.value
        };
    }

    internal SpaceshipColor GetSpaceshipColor()
    {
        return (SpaceshipColor)spaceshipDropdown.value;
    }
}

public enum SpaceshipColor
{
    Black,
    Cyan,
    Orange,
    Yellow,
    Blue,
    Grey,
    Green,
    Purple,
    Red,
    White
}