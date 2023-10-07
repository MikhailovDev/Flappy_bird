using UnityEngine;
using System.IO;
using System.Collections.Generic;
using Unity.VisualScripting;

public class ScoreData : IDataManager
{
    public int CurrentScore { get; set; }
    public List<int> ScoreNumbersInTabelList => _scoreNumbersInTabelList;

    private SerialiazableScoreData _scoreData;
    private List<int> _scoreNumbersInTabelList;

    private string _path;
    private string _directoryPath;
    private string _directoryName;
    private string _fileName;

    public ScoreData()
    {
        _scoreNumbersInTabelList = new List<int>();

        _scoreData = new SerialiazableScoreData(_scoreNumbersInTabelList);

        _directoryName = "Data";
        _fileName = "ScoreData";

        _path = Path.Combine(Application.persistentDataPath, _directoryName, _fileName);
        _directoryPath = Path.Combine(Application.persistentDataPath, _directoryName);

        if (!Directory.Exists(_directoryPath))
        {
            Directory.CreateDirectory(_directoryPath);
        }
    }

    public void LoadData()
    {
        if (File.Exists(_path))
        {
            string loadedJsonDataString = File.ReadAllText(_path);

            _scoreData = JsonUtility.FromJson<SerialiazableScoreData>(loadedJsonDataString) ?? new SerialiazableScoreData(_scoreNumbersInTabelList);

            _scoreNumbersInTabelList = _scoreData.scoreNumbersInTabelList;
        }
        else
        {
            FileCreate();
        }
    }

    public void SaveData()
    {
        ScoreSorting();

        _scoreData.scoreNumbersInTabelList = _scoreNumbersInTabelList;

        string jsonDataString = JsonUtility.ToJson(_scoreData, true);

        File.WriteAllText(_path, jsonDataString);
    }

    public void ResetData()
    {
        _scoreData.scoreNumbersInTabelList = null;

        string jsonDataString = JsonUtility.ToJson(_scoreData, true);

        File.WriteAllText(_path, jsonDataString);
    }

    private void OnApplicationQuit()
    {
        if (_scoreData != null)
        {
            SaveData();
        }
    }

    private void FileCreate()
    {
        FileStream file = File.Create(_path);
        file.Close();
    }

    private void ScoreSorting()
    {
        int countOfScores = _scoreNumbersInTabelList.Count;

        if (countOfScores < 5 && CurrentScore > 0)
        {
            if (countOfScores == 0)
            {
                _scoreNumbersInTabelList.Add(CurrentScore);
            }
            else
            {
                for (int i = 0; i < countOfScores; i++)
                {
                    if (CurrentScore > _scoreNumbersInTabelList[i])
                    {
                        _scoreNumbersInTabelList.Insert(i, CurrentScore);
                        break;
                    }
                    else if (_scoreNumbersInTabelList[i] == CurrentScore)
                    {
                        break;
                    }
                    else if (CurrentScore < _scoreNumbersInTabelList[i] && i == countOfScores - 1)
                    {
                        _scoreNumbersInTabelList.Insert(i + 1, CurrentScore);
                        break;
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < countOfScores; i++)
            {
                if (_scoreNumbersInTabelList[i] < CurrentScore)
                {
                    _scoreNumbersInTabelList.Insert(i, CurrentScore);
                    _scoreNumbersInTabelList.RemoveAt(countOfScores);
                    break;
                }
                else if (_scoreNumbersInTabelList[i] == CurrentScore)
                {
                    break;
                }
            }
        }
    }
}