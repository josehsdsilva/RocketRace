using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;

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

public class AudioLoader : CustomYieldInstruction
{
    private UnityWebRequest www;
    public AudioClip Clip { get; private set; }
    public string Error { get; private set; }
    private bool isDone;

    public override bool keepWaiting => !isDone;

    public AudioLoader(string audioPath)
    {
        if (string.IsNullOrEmpty(audioPath))
        {
            isDone = true;
            return;
        }

        string fullPath = Path.Combine(Application.streamingAssetsPath, audioPath + ".wav");
        #if UNITY_WEBGL && !UNITY_EDITOR
            fullPath = System.Uri.EscapeUriString(fullPath);
        #endif
        
        www = UnityWebRequestMultimedia.GetAudioClip(fullPath, AudioType.WAV);
        www.SendWebRequest().completed += _ => OnRequestComplete(GetAudioFileName(audioPath));
    }

    private void OnRequestComplete(string audioName)
    {
        if (www.result == UnityWebRequest.Result.Success)
        {
            Clip = DownloadHandlerAudioClip.GetContent(www);
            Clip.name = audioName;
        }
        else
        {
            Error = www.error;
            Debug.LogError($"Failed to load audio clip {audioName}. Error: {Error}");
        }
        isDone = true;
        www?.Dispose();
    }

    string GetAudioFileName(string fullPath)
    {
        return fullPath.Split('/').Last();
    }
}

public class SpriteLoader : CustomYieldInstruction
{
    private UnityWebRequest www;
    public Sprite Sprite { get; private set; }
    public string Error { get; private set; }
    private bool isDone;

    public override bool keepWaiting => !isDone;

    public SpriteLoader(string spritePath)
    {
        if (string.IsNullOrEmpty(spritePath))
        {
            isDone = true;
            return;
        }

        if (!spritePath.EndsWith(".png"))
        {
            spritePath += ".png";
        }

        string fullPath = Path.Combine(Application.streamingAssetsPath, spritePath);
        #if UNITY_WEBGL && !UNITY_EDITOR
            fullPath = System.Uri.EscapeUriString(fullPath);
        #endif

        www = UnityWebRequestTexture.GetTexture(fullPath);
        www.SendWebRequest().completed += _ => OnRequestComplete(spritePath);
    }

    private void OnRequestComplete(string spritePath)
    {
        if (www.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(www);
            Sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
        else
        {
            Error = www.error;
            Debug.LogError($"Failed to load sprite {spritePath}. Error: {Error}");
        }
        isDone = true;
        www?.Dispose();
    }
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
            path = System.Uri.EscapeUriString(path);
        #endif

        using UnityWebRequest www = UnityWebRequest.Get(path);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"Failed to load JSON: {www.error}");
            cachedQuestionData = new List<QuestionThemeData>();
            FinishLoading();
            yield break;
        }

        string jsonContent = www.downloadHandler.text;
        GameData gameData = null;
        
        try 
        {
            gameData = JsonUtility.FromJson<GameData>(jsonContent);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error parsing JSON: {e.Message}");
            cachedQuestionData = new List<QuestionThemeData>();
            FinishLoading();
            yield break;
        }

        if (gameData != null && gameData.themes != null)
        {
            yield return StartCoroutine(ConvertToQuestionThemeDataCoroutine(gameData.themes));
            Debug.Log($"Successfully loaded {cachedQuestionData.Count} themes");
        }
        else
        {
            Debug.LogWarning("JSON file was empty or had no themes");
            cachedQuestionData = new List<QuestionThemeData>();
        }

        FinishLoading();
    }

    private void FinishLoading()
    {
        isLoading = false;
        if (OnLoadComplete != null)
        {
            OnLoadComplete.Invoke(cachedQuestionData);
            OnLoadComplete = null;
        }
    }

    private IEnumerator ConvertToQuestionThemeDataCoroutine(List<JSONThemeData> jsonThemes)
    {
        var result = new List<QuestionThemeData>();
    
        foreach (var jsonTheme in jsonThemes)
        {
            var questions = new List<QuestionData>();
            yield return StartCoroutine(ConvertToQuestionDataCoroutine(jsonTheme.questions, questions));
            
            var themeData = new QuestionThemeData
            {
                theme = ParseThemeEnum(jsonTheme.theme),
                questions = questions
            };
        
            result.Add(themeData);
        }
    
        cachedQuestionData = result;
    }

    private IEnumerator ConvertToQuestionDataCoroutine(List<JSONQuestionData> jsonQuestions, List<QuestionData> result)
    {
        foreach (var jsonQuestion in jsonQuestions)
        {
            var audioLoader = new AudioLoader(jsonQuestion.questionAudio);
            yield return audioLoader;

            var answerOptions = new List<AnswerData>();
            foreach (var option in jsonQuestion.answerOptions)
            {
                var spriteLoader = new SpriteLoader(option.spritePath);
                yield return spriteLoader;

                answerOptions.Add(new AnswerData
                {
                    answerText = option.answerText,
                    sprite = spriteLoader.Sprite
                });
            }

            var questionData = new QuestionData
            {
                questionText = jsonQuestion.questionText,
                questionAudio = audioLoader.Clip,
                answerOptions = answerOptions,
                correctAnswerIndex = jsonQuestion.correctAnswerIndex
            };
        
            result.Add(questionData);
        }
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
            return QuestionTheme.instruments;
        }
    }
}