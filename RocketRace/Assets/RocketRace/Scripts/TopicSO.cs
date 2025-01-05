using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Topic", menuName = "RocketRace/Topic")]
internal class TopicSO : ScriptableObject
{
    [Header("Settings")]
    [SerializeField] internal string topicName;
    [SerializeField] internal Sprite topicImage;
    [SerializeField] internal List<QuestionData> questionDatas;
}
