using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

[Serializable]
public class JSONAnswerOption
{
    public string answerText;
    public string spritePath;
}

[Serializable]
public class JSONQuestionData
{
    public string questionText;
    public string questionAudio;
    public JSONAnswerOption[] answerOptions;
    public int correctAnswerIndex;
}

[Serializable]
public class JSONThemeData
{
    public string theme;
    public List<JSONQuestionData> questions;
}

[Serializable]
public class GameData
{
    public List<JSONThemeData> themes;
}

public class QuestionDataLoader : MonoBehaviour
{
    internal delegate void LoadComplete(List<QuestionThemeData> data);
    private event LoadComplete OnLoadComplete;

    private List<QuestionThemeData> cachedQuestionData;
    private bool isLoading = false;

    internal void LoadQuestionData(LoadComplete callback)
    {
        if (isLoading)
        {
            Debug.LogWarning("Already loading question data!");
            return;
        }

        if (cachedQuestionData != null)
        {
            callback?.Invoke(cachedQuestionData);
            return;
        }

        OnLoadComplete += callback;
        StartCoroutine(LoadJSON());
    }

    private IEnumerator LoadJSON()
    {
        isLoading = true;
        string path = Path.Combine(Application.streamingAssetsPath, "themes.json");

        #if UNITY_WEBGL && !UNITY_EDITOR
            path = Path.Combine(Application.streamingAssetsPath, "themes.json");
            path = System.Uri.EscapeUriString(path);
        #endif

        using UnityWebRequest www = UnityWebRequest.Get(path);
        yield return www.SendWebRequest();
    
        if (www.result == UnityWebRequest.Result.Success)
        {
            string jsonContent = www.downloadHandler.text;
            try 
            {
                var gameData = JsonUtility.FromJson<GameData>(jsonContent);
                if (gameData != null && gameData.themes != null)
                {
                    cachedQuestionData = ConvertToQuestionThemeData(gameData.themes);
                    Debug.Log($"Successfully loaded {cachedQuestionData.Count} themes");
                }
                else
                {
                    Debug.LogWarning("JSON file was empty or had no themes");
                    cachedQuestionData = new List<QuestionThemeData>();
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error parsing JSON: {e.Message}");
                cachedQuestionData = new List<QuestionThemeData>();
            }
        }
        else
        {
            Debug.LogError($"Failed to load JSON: {www.error}");
            cachedQuestionData = new List<QuestionThemeData>();
        }

        isLoading = false;
        if (OnLoadComplete != null)
        {
            OnLoadComplete.Invoke(cachedQuestionData);
            OnLoadComplete = null;
        }
    }

    private List<QuestionThemeData> ConvertToQuestionThemeData(List<JSONThemeData> jsonThemes)
    {
        var result = new List<QuestionThemeData>();
    
        foreach (var jsonTheme in jsonThemes)
        {
            var themeData = new QuestionThemeData
            {
                theme = ParseThemeEnum(jsonTheme.theme),
                questions = ConvertToQuestionData(jsonTheme.questions)
            };
        
            result.Add(themeData);
        }
    
        return result;
    }

    private List<QuestionData> ConvertToQuestionData(List<JSONQuestionData> jsonQuestions)
    {
        var result = new List<QuestionData>();
    
        foreach (var jsonQuestion in jsonQuestions)
        {
            var questionData = new QuestionData
            {
                questionText = jsonQuestion.questionText,
                questionAudio = LoadQuestionAudio(jsonQuestion.questionAudio),
                answerOptions = CreateAnswerOptions(jsonQuestion.answerOptions, jsonQuestion.correctAnswerIndex)
            };
        
            result.Add(questionData);
        }
    
        return result;
    }

    private List<AnswerData> CreateAnswerOptions(JSONAnswerOption[] options, int correctIndex)
    {
        var answers = new List<AnswerData>();
    
        for (int i = 0; i < options.Length; i++)
        {
            answers.Add(new AnswerData
            {
                answerText = options[i].answerText,
                sprite = LoadAnswerSprite(options[i].spritePath)
            });
        }
    
        return answers;
    }

    private Sprite LoadAnswerSprite(string spritePath)
    {
        if (string.IsNullOrEmpty(spritePath))
        {
            Debug.LogWarning("Sprite path is empty");
            return null;
        }

        var sprite = Resources.Load<Sprite>(spritePath);
        if (sprite == null)
        {
            Debug.LogError($"Failed to load sprite at path: {spritePath}");
        }
        return sprite;
    }

    private QuestionTheme ParseThemeEnum(string themeName)
    {
        try
        {
            return (QuestionTheme)System.Enum.Parse(typeof(QuestionTheme), themeName);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to parse theme name '{themeName}': {e.Message}");
            return QuestionTheme.Instruments;
        }
    }

    private AudioClip LoadQuestionAudio(string audioPath)
    {
        if (string.IsNullOrEmpty(audioPath))
        {
            Debug.LogWarning("Audio path is empty");
            return null;
        }

        var audioClip = Resources.Load<AudioClip>(audioPath);
        if (audioClip == null)
        {
            Debug.LogError($"Failed to load audio clip at path: {audioPath}");
        }
        return audioClip;
    }
}