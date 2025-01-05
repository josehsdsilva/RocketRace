using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MeshRenderer spaceshipRenderer;

    [Header("Settings")]
    [SerializeField] private List<SpaceshipData> spaceshipDatas;
    [SerializeField] private Vector3 onQuestionPosition;

    private Vector3 initialPosition;

    private void Awake()
    {
        initialPosition = transform.position;
    }

    internal void SetSpaceshipColor(SpaceshipColor spaceshipColor)
    {
        spaceshipRenderer.material = spaceshipDatas.Find(x => x.spaceshipColor == spaceshipColor).material;
    }

    internal void SetOnQuestion()
    {
        transform.position = onQuestionPosition;
    }

    internal void ResetPosition()
    {
        transform.position = initialPosition;
    }
}

[Serializable]
public class SpaceshipData
{
    public SpaceshipColor spaceshipColor;
    public Material material;

}
