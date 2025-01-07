using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Linq;

public class TeamEntry : MonoBehaviour
{
    [SerializeField] private TMP_InputField teamNameInput;
    [SerializeField] private TMP_Dropdown spaceshipTypeDropdown;
    [SerializeField] private TMP_Dropdown spaceshipColorDropdown;

    private void Start()
    {
        InitializeDropdowns();
    }

    private void InitializeDropdowns()
    {
        // Setup rocket options
        spaceshipTypeDropdown.ClearOptions();
        spaceshipTypeDropdown.AddOptions(Enum.GetNames(typeof(SpaceshipType)).ToList());

        spaceshipColorDropdown.ClearOptions();
        spaceshipColorDropdown.AddOptions(Enum.GetNames(typeof(SpaceshipColor)).ToList());
    }

    public GameSettingsSO.TeamData GetTeamData()
    {
        return new GameSettingsSO.TeamData
        {
            teamName = string.IsNullOrEmpty(teamNameInput.text) ? "Team" : teamNameInput.text,
            spaceshipColor = (SpaceshipColor)spaceshipTypeDropdown.value,
            spaceshipType = (SpaceshipType)spaceshipColorDropdown.value
        };
    }

    internal SpaceshipColor GetSpaceshipColor()
    {
        return (SpaceshipColor)spaceshipColorDropdown.value;
    }

    internal SpaceshipType GetSpaceshipType()
    {
        return (SpaceshipType)spaceshipTypeDropdown.value;
    }
}

// lower case cause of font limitation
public enum SpaceshipColor
{
    black,
    cyan,
    orange,
    yellow,
    blue,
    grey,
    green,
    purple,
    red,
    white
}

// lower case cause of font limitation
public enum SpaceshipType
{
    quad,
    rocket,
    shuttle,
    starship
}