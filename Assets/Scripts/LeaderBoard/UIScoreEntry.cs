using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScoreEntry : MonoBehaviour
{
    public Text _id;
    public Text _name;
    public Text _score;

    public void SetID(string value)
    {
        _id.text = value;
    }
    public void SetName(string value)
    {
        _name.text = value;
    }
    public void SetScore(string value)
    {
        _score.text = value;
    }
    public void SetBlank()
    {
        SetID("");
        SetName("");
        SetScore("");
    }
}
