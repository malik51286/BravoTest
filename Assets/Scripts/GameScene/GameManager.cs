using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public Transform _grid;
    public UIManager _uiManager;
    private GameObject _player;
    private GameObject _trophy;

    private Vector3 _playerSpawnPosition;
    private Vector3[] _enemySpawnPosition;
    private Vector3 _trophySpawnPosition;

    private int _currentLevel=0;
    private int _currentScore = 0;
    private int _highScore=0;

    private int _totalPlayerLives=3;
    private int _currentPlayerLives = 0;

    private int _totalEnemyNo = 10;
    private int _currentEnemyNo = 0;
    private int _enemiesKiled = 0;
    private int _enemyKillScore = 10;

    private float _minEnemySpawnDelay;
    private float _maxEnemySpawnDelay;
    private bool _isGameOn;

    void Start()
    {
        PlayerPrefs.SetString("PreviousScene", SceneList.GameScene);
        PlayerPrefs.Save();

        _playerSpawnPosition = new Vector3(-3,-7,0);
        _enemySpawnPosition = new Vector3[] { new Vector3(12, 8, 0),new Vector3(-12, 8, 0)};
        _trophySpawnPosition = new Vector3(0, -9, 0);

        GetHighScore();
        SpawnPlayer();
        SpawnTrophy();

        StartNewLevel();

    }

    private void SetRandomGrid()
    {
        if(_grid.childCount>0)
        {
            Destroy(_grid.GetChild(0).gameObject);
        }
        int index = (_currentLevel-1) % 3;
        GameObject tilemap = Instantiate(Resources.Load(ResourceList.CommonPath + ResourceList.TileMap + index), _grid.position, Quaternion.identity) as GameObject;
        tilemap.transform.parent = _grid;
    }

    private void StartNewLevel()
    {
        _currentLevel++;
        _enemyKillScore = _currentLevel * _totalEnemyNo;
        _currentEnemyNo = 0;
        _enemiesKiled = 0;

        _uiManager.SetLevel(_currentLevel.ToString());
        _uiManager.SetScore(_currentScore.ToString());
        _uiManager.SetEnemiesCount((_totalEnemyNo - _enemiesKiled).ToString());

        _player.transform.position = _playerSpawnPosition;

        SetRandomGrid();

        _minEnemySpawnDelay = 5f - _currentLevel * 0.1f;
        if (_minEnemySpawnDelay < 2f) _minEnemySpawnDelay = 2f;

        _maxEnemySpawnDelay = 7f - _currentLevel * 0.1f;
        if (_maxEnemySpawnDelay < 3f) _maxEnemySpawnDelay = 3f;

        this.PerformActionWithDelay(Random.Range(_minEnemySpawnDelay, _maxEnemySpawnDelay), SpawnRandomEnemy);

    }

    private void SpawnTrophy()
    {
        _trophy = Instantiate(Resources.Load(ResourceList.CommonPath + ResourceList.Trophy), _trophySpawnPosition, Quaternion.identity) as GameObject;
        _trophy.GetComponent<IDestroyable>().SetDestroyAction(OnTrophyDestroyed);
    }
    private void SpawnPlayer()
    {
        _isGameOn = true;
        _player = Instantiate(Resources.Load(ResourceList.CommonPath + ResourceList.PlayerTank), _playerSpawnPosition, Quaternion.identity) as GameObject;
        _player.GetComponent<IMovable>().type = MovableType.Player;
        _player.GetComponent<IDestroyable>().SetDestroyAction( OnPlayerDestroyed);
    }

    private void SpawnEnemy()
    {
        GameObject enemy = Instantiate(Resources.Load(ResourceList.CommonPath + ResourceList.EnemyTank), _enemySpawnPosition[Random.Range(0, _enemySpawnPosition.Length)], Quaternion.identity) as GameObject;
        enemy.GetComponent<IMovable>().type = MovableType.Enemy;
        enemy.GetComponent<IDestroyable>().SetDestroyAction(OnEnemyDestroyed);
        enemy.GetComponent<EnemyMovement>().SetLevel(_currentLevel);
    }
    private void SpawnRandomEnemy()
    {
        if (_currentEnemyNo < _totalEnemyNo)
        {
            _currentEnemyNo++;
            SpawnEnemy();
            this.PerformActionWithDelay(Random.Range(_minEnemySpawnDelay, _maxEnemySpawnDelay), SpawnRandomEnemy);
        }
    }

    private void OnPlayerDestroyed()
    {
        ReduceLife();
    }

    private void ReduceLife()
    {
        if (_isGameOn)
        {
            _currentPlayerLives++;
            _uiManager.ReduceLife();
            _isGameOn = false;

            if (_currentPlayerLives == _totalPlayerLives)
            {
                EndGame();
            }
            else
            {
                _player.GetComponent<IDestroyable>().InflictDamage(_player.GetComponent<IDestroyable>().health);
                _trophy.GetComponent<IDestroyable>().InflictDamage(_trophy.GetComponent<IDestroyable>().health);

                this.PerformActionWithDelay(1f, () =>
                {
                    SpawnPlayer();
                    SpawnTrophy();
                });
            }
        }
    }
    private void OnEnemyDestroyed()
    {
        IncrementScore();
        DecreaseEnemy();
    }

    private void DecreaseEnemy()
    {
        _enemiesKiled++;
        _uiManager.SetEnemiesCount((_totalEnemyNo - _enemiesKiled).ToString());

        if (_enemiesKiled == _totalEnemyNo)
        {
            this.PerformActionWithDelay(1f, () =>
            {
                StartNewLevel();
            });
        }
    }

    private void IncrementScore()
    {
        _currentScore += _enemyKillScore;
        _uiManager.SetScore(_currentScore.ToString());

        CheckHighScore();
    }

    private void OnTrophyDestroyed()
    {
        ReduceLife();
    }
    private void CheckHighScore()
    {
        if (_currentScore > _highScore)
        {
            _highScore = _currentScore;
            _uiManager.SetHighScore(_highScore.ToString());
        }
    }

    private void GetHighScore()
    {
        if (PlayerPrefs.HasKey("BravoHighScore"))
        {
            _highScore = PlayerPrefs.GetInt("BravoHighScore");
        }
        else
        {
            _highScore = 0;
        }
        _uiManager.SetHighScore(_highScore.ToString());
    }
    private void SaveScore()
    {
        PlayerPrefs.SetInt("BravoHighScore", _highScore);
        PlayerPrefs.SetInt("BravoScore", _currentScore);
        PlayerPrefs.Save();
    }
    private void ShowScoreBoard()
    {
        SceneManager.LoadScene(SceneList.LeaderBoardScene);
    }
    private void EndGame()
    {
        SaveScore();
        this.PerformActionWithDelay(1f, ShowScoreBoard);
    }

}
