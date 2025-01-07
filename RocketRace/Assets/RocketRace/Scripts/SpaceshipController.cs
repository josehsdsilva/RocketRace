using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private List<SpaceshipColorData> spaceshipColorDatas;
    [SerializeField] private List<SpaceshipTypeData> spaceshipTypeDatas;
    [SerializeField] private Vector3 onQuestionPosition;

    private Vector3 initialPosition;
    private SpaceshipTypeData currentSpaceshipType;

    private void Awake()
    {
        initialPosition = transform.position;
    }

    internal void SetSpaceshipAndColor(SpaceshipType spaceshipType, SpaceshipColor spaceshipColor)
    {
        ToggleAllSpaceships(false);
        currentSpaceshipType = spaceshipTypeDatas.Find(x => x.spaceshipType == spaceshipType);
        currentSpaceshipType.spaceshipObject.SetActive(true);
        currentSpaceshipType.spaceshipRenderer.material = spaceshipColorDatas.Find(x => x.spaceshipColor == spaceshipColor).material;
    }

    private void ToggleAllSpaceships(bool active)
    {
        foreach (var spaceshipTypeData in spaceshipTypeDatas)
        {
            spaceshipTypeData.spaceshipObject.SetActive(active);
        }
    }

    internal void SetOnQuestion(SpaceshipType spaceshipType, SpaceshipColor spaceshipColor)
    {
        SetSpaceshipAndColor(spaceshipType, spaceshipColor);
        transform.position = onQuestionPosition;
    }

    internal void ResetPosition()
    {
        transform.position = initialPosition;
    }
}

[Serializable]
public class SpaceshipColorData
{
    public SpaceshipColor spaceshipColor;
    public Material material;

}

[Serializable]
public class SpaceshipTypeData
{
    public SpaceshipType spaceshipType;
    public GameObject spaceshipObject;
    public MeshRenderer spaceshipRenderer;
}
