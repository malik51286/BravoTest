using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

[System.Serializable]
class ScoreEntry
{
    public string _name;
    public int _score;
}
class ScoreList
{
    public List<ScoreEntry> _scoreEntryList;
}
public class LeaderBoardManager : MonoBehaviour
{
    private ScoreList _listScores;
    public Transform _scoreTransform;
    void Start()
    {
        GetScoreList();
        if (PlayerPrefs.GetString("PreviousScene") == SceneList.GameScene)
        {
            AddScoreEntry(GetCurrentPlayerName(), GetCurrentScore());
            SortScoreList();
            SaveScoreList();
        }
        SetScoreList();
    }

    private void GetScoreList()
    {
        if (PlayerPrefs.HasKey("BravoScoreList"))
        {
            string jsonString = PlayerPrefs.GetString("BravoScoreList");
            _listScores = JsonUtility.FromJson<ScoreList>(jsonString);
        }
        else
        {
            _listScores = new ScoreList();
            _listScores._scoreEntryList = new List<ScoreEntry>();
        }
    }

    private void SortScoreList()
    {
        for (int i = 0; i < _listScores._scoreEntryList.Count; i++)
        {
            for (int j = i + 1; j < _listScores._scoreEntryList.Count; j++)
            {
                if (_listScores._scoreEntryList[j]._score > _listScores._scoreEntryList[i]._score)
                {
                    ScoreEntry tmp = _listScores._scoreEntryList[i];
                    _listScores._scoreEntryList[i] = _listScores._scoreEntryList[j];
                    _listScores._scoreEntryList[j] = tmp;
                }
            }
        }
    }
    private void AddScoreEntry(string name,int score)
    {
        ScoreEntry scoreEntry= new ScoreEntry { _name = name , _score = score };
        _listScores._scoreEntryList.Add(scoreEntry);
    }
    private void SetScoreList()
    {
       for(int i=0;i< _scoreTransform.childCount;i++)
        {
            if (i < _listScores._scoreEntryList.Count)
            {
                _scoreTransform.GetChild(i).GetComponent<UIScoreEntry>().SetID((i + 1).ToString());
                _scoreTransform.GetChild(i).GetComponent<UIScoreEntry>().SetName(_listScores._scoreEntryList[i]._name);
                _scoreTransform.GetChild(i).GetComponent<UIScoreEntry>().SetScore(_listScores._scoreEntryList[i]._score.ToString());
            }
            else
            {
                _scoreTransform.GetChild(i).GetComponent<UIScoreEntry>().SetBlank();
            }
        }
    }

    private void SaveScoreList()
    {
        string json = JsonUtility.ToJson(_listScores);
        PlayerPrefs.SetString("BravoScoreList", json);
        PlayerPrefs.Save();
    }

    private string GetCurrentPlayerName()
    {
        return "TempPlayerName";
    }

    private int GetCurrentScore()
    {
       return PlayerPrefs.GetInt("BravoScore");
    }
    public void ShowMainMenu()
    {
        SceneManager.LoadScene(SceneList.MenuScene);
    }
}