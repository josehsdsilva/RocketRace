using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class ThemesEditorWindow : EditorWindow
{
    private List<JSONThemeData> themes = new List<JSONThemeData>();
    private string jsonPath;
    private Vector2 scrollPosition;
    private Dictionary<string, bool> themeFoldouts = new Dictionary<string, bool>();
    private Dictionary<string, bool> questionFoldouts = new Dictionary<string, bool>();

    [MenuItem("Window/Game/Themes Editor")]
    public static void ShowWindow()
    {
        var window = GetWindow<ThemesEditorWindow>("Themes Editor");
        window.jsonPath = Path.Combine(Application.streamingAssetsPath, "themes.json");
        window.LoadFromJson();
        window.Show();
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();

        // Toolbar
        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
        if (GUILayout.Button("Save", EditorStyles.toolbarButton))
        {
            SaveToJson();
        }
        if (GUILayout.Button("Load", EditorStyles.toolbarButton))
        {
            LoadFromJson();
        }
        if (GUILayout.Button("Add Theme", EditorStyles.toolbarButton))
        {
            var newTheme = new JSONThemeData { 
                theme = "New Theme", 
                questions = new List<JSONQuestionData>() 
            };
            themes.Add(newTheme);
            themeFoldouts[newTheme.theme] = true;
        }
        EditorGUILayout.EndHorizontal();

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        // Draw Themes
        for (int i = 0; i < themes.Count; i++)
        {
            var theme = themes[i];
            
            if (!themeFoldouts.ContainsKey(theme.theme))
                themeFoldouts[theme.theme] = false;

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            
            EditorGUILayout.BeginHorizontal();
            themeFoldouts[theme.theme] = EditorGUILayout.Foldout(themeFoldouts[theme.theme], "", true);
            theme.theme = EditorGUILayout.TextField(theme.theme);
            
            if (GUILayout.Button("×", GUILayout.Width(20)))
            {
                if (EditorUtility.DisplayDialog("Remove Theme", 
                    $"Are you sure you want to remove the theme '{theme.theme}'?", 
                    "Remove", "Cancel"))
                {
                    themes.RemoveAt(i);
                    themeFoldouts.Remove(theme.theme);
                    break;
                }
            }
            EditorGUILayout.EndHorizontal();

            if (themeFoldouts[theme.theme])
            {
                EditorGUI.indentLevel++;

                if (theme.questions == null)
                    theme.questions = new List<JSONQuestionData>();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Questions", EditorStyles.boldLabel);
                if (GUILayout.Button("Add Question", GUILayout.Width(100)))
                {
                    var newQuestion = new JSONQuestionData
                    {
                        questionText = "New Question",
                        questionAudio = "",
                        answerOptions = new JSONAnswerOption[] 
                        { 
                            new JSONAnswerOption { answerText = "Option 1", spritePath = "" },
                            new JSONAnswerOption { answerText = "Option 2", spritePath = "" }
                        },
                        correctAnswerIndex = 0
                    };
                    theme.questions.Add(newQuestion);
                    questionFoldouts[$"{theme.theme}_{theme.questions.Count-1}"] = true;
                }
                EditorGUILayout.EndHorizontal();

                for (int j = 0; j < theme.questions.Count; j++)
                {
                    var question = theme.questions[j];
                    string questionKey = $"{theme.theme}_{j}";
                    
                    if (!questionFoldouts.ContainsKey(questionKey))
                        questionFoldouts[questionKey] = false;

                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    
                    EditorGUILayout.BeginHorizontal();
                    questionFoldouts[questionKey] = EditorGUILayout.Foldout(questionFoldouts[questionKey], $"Question {j + 1}", true);
                    
                    if (GUILayout.Button("×", GUILayout.Width(20)))
                    {
                        if (EditorUtility.DisplayDialog("Remove Question", 
                            "Are you sure you want to remove this question?", 
                            "Remove", "Cancel"))
                        {
                            theme.questions.RemoveAt(j);
                            questionFoldouts.Remove(questionKey);
                            break;
                        }
                    }
                    EditorGUILayout.EndHorizontal();

                    if (questionFoldouts[questionKey])
                    {
                        EditorGUI.indentLevel++;
                        
                        question.questionText = EditorGUILayout.TextField("Question Text", question.questionText);
                        
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.PrefixLabel("Question Audio");
                        question.questionAudio = EditorGUILayout.TextField(question.questionAudio);
                        if (GUILayout.Button("Browse", GUILayout.Width(60)))
                        {
                            string selectedPath = EditorUtility.OpenFilePanel("Select Audio", "Assets/Resources", "wav,mp3,ogg");
                            if (!string.IsNullOrEmpty(selectedPath))
                            {
                                question.questionAudio = GetResourcesPath(selectedPath);
                            }
                        }
                        EditorGUILayout.EndHorizontal();

                        // Answer Options
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Answer Options");
                        if (GUILayout.Button("+", GUILayout.Width(20)))
                        {
                            var newOptions = new JSONAnswerOption[question.answerOptions?.Length + 1 ?? 1];
                            if (question.answerOptions != null)
                                question.answerOptions.CopyTo(newOptions, 0);
                            newOptions[newOptions.Length - 1] = new JSONAnswerOption { answerText = "New Option", spritePath = "" };
                            question.answerOptions = newOptions;
                        }
                        EditorGUILayout.EndHorizontal();

                        if (question.answerOptions != null)
                        {
                            EditorGUI.indentLevel++;
                            for (int k = 0; k < question.answerOptions.Length; k++)
                            {
                                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                                EditorGUILayout.BeginHorizontal();
                                EditorGUILayout.LabelField($"Option {k + 1}");
                                
                                if (GUILayout.Button("×", GUILayout.Width(20)))
                                {
                                    var newOptions = new JSONAnswerOption[question.answerOptions.Length - 1];
                                    System.Array.Copy(question.answerOptions, 0, newOptions, 0, k);
                                    System.Array.Copy(question.answerOptions, k + 1, newOptions, k, question.answerOptions.Length - k - 1);
                                    question.answerOptions = newOptions;
                                    
                                    if (question.correctAnswerIndex >= newOptions.Length)
                                    {
                                        question.correctAnswerIndex = newOptions.Length - 1;
                                    }
                                    break;
                                }
                                EditorGUILayout.EndHorizontal();

                                question.answerOptions[k].answerText = EditorGUILayout.TextField("Text", question.answerOptions[k].answerText);
                                
                                EditorGUILayout.BeginHorizontal();
                                EditorGUILayout.PrefixLabel("Sprite");
                                question.answerOptions[k].spritePath = EditorGUILayout.TextField(question.answerOptions[k].spritePath);
                                if (GUILayout.Button("Browse", GUILayout.Width(60)))
                                {
                                    string selectedPath = EditorUtility.OpenFilePanel("Select Sprite", "Assets/Resources", "png,jpg,jpeg");
                                    if (!string.IsNullOrEmpty(selectedPath))
                                    {
                                        question.answerOptions[k].spritePath = GetResourcesPath(selectedPath);
                                    }
                                }
                                EditorGUILayout.EndHorizontal();
                                EditorGUILayout.EndVertical();
                            }
                            EditorGUI.indentLevel--;
                        }

                        if (question.answerOptions != null && question.answerOptions.Length > 0)
                        {
                            string[] options = new string[question.answerOptions.Length];
                            for (int k = 0; k < question.answerOptions.Length; k++)
                            {
                                options[k] = $"Option {k + 1}: {question.answerOptions[k].answerText}";
                            }
                            question.correctAnswerIndex = EditorGUILayout.Popup("Correct Answer", 
                                question.correctAnswerIndex, 
                                options);
                        }
                        
                        EditorGUI.indentLevel--;
                    }
                    
                    EditorGUILayout.EndVertical();
                }
                EditorGUI.indentLevel--;
            }
            
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
        }

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }

    private string GetResourcesPath(string fullPath)
    {
        int resourcesIndex = fullPath.IndexOf("/Resources/");
        if (resourcesIndex >= 0)
        {
            string relativePath = fullPath.Substring(resourcesIndex + 11); // +11 to skip "/Resources/"
            return Path.ChangeExtension(relativePath, null); // Remove the extension
        }
        Debug.LogWarning("Selected file must be in a Resources folder!");
        return "";
    }

    private void SaveToJson()
    {
        if (!Directory.Exists(Application.streamingAssetsPath))
        {
            Directory.CreateDirectory(Application.streamingAssetsPath);
        }

        var gameData = new GameData { themes = this.themes };
        string json = JsonUtility.ToJson(gameData, true);
        File.WriteAllText(jsonPath, json);
        Debug.Log($"Themes saved to {jsonPath}");
        AssetDatabase.Refresh();
    }

    private void LoadFromJson()
    {
        if (File.Exists(jsonPath))
        {
            string jsonContent = File.ReadAllText(jsonPath);
            var gameData = JsonUtility.FromJson<GameData>(jsonContent);
            themes = gameData.themes ?? new List<JSONThemeData>();
            
            themeFoldouts.Clear();
            questionFoldouts.Clear();
            
            Debug.Log($"Themes loaded from {jsonPath}");
        }
        else
        {
            themes = new List<JSONThemeData>();
            Debug.Log($"No file found at {jsonPath}, created new data");
        }
    }
}