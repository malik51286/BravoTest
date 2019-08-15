using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text _levelNumber;
    public Text _score;
    public Text _highScore;
    public Text _enemiesCount;
    public RectTransform _lives;

    public void SetLevel(string value)
    {
        _levelNumber.text = value;
    }

    public void SetScore(string value)
    {
        _score.text = value;
    }

    public void SetHighScore(string value)
    {
        _highScore.text = value;
    }

    public void SetEnemiesCount(string value)
    {
        _enemiesCount.text = value;
    }

    public void ReduceLife()
    {
        Destroy(_lives.GetChild(0).gameObject);
    }

}
