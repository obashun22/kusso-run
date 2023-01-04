using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public float speed;
    [SerializeField] float _unitInterval;
    [SerializeField] int _maxUnitCount;
    [SerializeField] int _minUnitCount;
    [SerializeField] float _speedUpRate;
    static public GameState? initialGameState;
    public GameState gameState;
    [SerializeField] GameObject _player;
    [SerializeField] GameObject[] _obstacles;
    [SerializeField] GameObject[] _obstaclesM;
    [SerializeField] GameObject[] _raresS;
    [SerializeField] GameObject[] _raresA;
    [SerializeField] GameObject[] _raresB;
    [SerializeField] GameObject[] _envObjects;
    [SerializeField] GameObject[] _sponers;
    [SerializeField] GameObject _envSponerL;
    [SerializeField] GameObject _envSponerR;
    [SerializeField] GameObject _groundSponer;
    [SerializeField] GameObject _field;
    [SerializeField] GameObject _ground;
    [SerializeField] float _groundDiff;
    [SerializeField] GameObject _coin;
    [SerializeField] TextMeshProUGUI _scoreText;
    [SerializeField] GameObject _homeUI;
    [SerializeField] GameObject _playUI;
    [SerializeField] GameObject _stopUI;
    [SerializeField] GameObject _gameOverUI;
    [SerializeField] GameObject _newItemUI;

    private float _envSponIntervalL;
    private float _envSponIntervalR;
    private float time;
    private float envTimeL;
    private float envTimeR;
    private float diff;
    private int _unitCount;
    private int _sponUnitCount;
    private int _coinLine;
    private int _score;

    // Start is called before the first frame update
    void Start()
    {
        _envSponIntervalL = 0f;
        _envSponIntervalR = 0f;
        _sponUnitCount = 0;
        _coinLine = UnityEngine.Random.Range(0, 3);
        _score = 0;
        if (GameManager.initialGameState == null) {
            SetGameState(GameState.Ready);
        } else if (GameManager.initialGameState == GameState.Play) {
            SetGameState(GameState.Play);
            GameManager.initialGameState = null;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (gameState) {
            case GameState.Ready:
                if (Input.GetKey(KeyCode.Space)) {
                    StartGame();
                }
                break;
            case GameState.Play:
                time += Time.deltaTime;
                envTimeL += Time.deltaTime;
                envTimeR += Time.deltaTime;
                diff += speed * Time.deltaTime;
                _scoreText.text = _score.ToString("D8");

                // 生成処理
                if (time > _unitInterval) {
                    time = 0f;
                    _unitCount++;
                    speed += _speedUpRate;
                    if (_unitCount >= _sponUnitCount) {
                        float rand = UnityEngine.Random.Range(0f, 2f);
                        if (rand > 1f) {
                            GameObject obj = Instantiate(_obstaclesM[UnityEngine.Random.Range(0, _obstaclesM.Length)], _sponers[1].transform.position, Quaternion.Euler(0, 0, 0));
                            obj.transform.parent = _field.transform;
                        } else {
                            GameObject obj = Instantiate(_obstacles[UnityEngine.Random.Range(0, _obstacles.Length)], _sponers[UnityEngine.Random.Range(0, _sponers.Length)].transform.position, Quaternion.Euler(0, 0, 0));
                            obj.transform.parent = _field.transform;
                        }
                        _unitCount = 0;
                        _sponUnitCount = UnityEngine.Random.Range(_minUnitCount, _maxUnitCount);

                        _coinLine = UnityEngine.Random.Range(0, 3);
                    } else {
                        float rand = UnityEngine.Random.Range(0f, 1f);
                        if (rand < 0.00001) {
                            // 超レアアイテム
                            GameObject obj = Instantiate(_raresS[UnityEngine.Random.Range(0, _raresS.Length)], _sponers[_coinLine].transform.position, Quaternion.identity);
                            obj.transform.parent = _field.transform;
                        } else if (rand < 0.0001) {
                            // 結構レアアイテム
                            GameObject obj = Instantiate(_raresA[UnityEngine.Random.Range(0, _raresA.Length)], _sponers[_coinLine].transform.position, Quaternion.identity);
                            obj.transform.parent = _field.transform;
                        } else if (rand < 0.001) {
                            // 普通のレアアイテム
                            GameObject obj = Instantiate(_raresB[UnityEngine.Random.Range(0, _raresB.Length)], _sponers[_coinLine].transform.position, Quaternion.identity);
                            obj.transform.parent = _field.transform;
                        } else {
                            // 普通のコイン
                            GameObject coin = Instantiate(_coin, _sponers[_coinLine].transform.position, Quaternion.identity);
                            coin.transform.parent = _field.transform;
                        }
                    }
                }
                if (envTimeL > _envSponIntervalL) {
                    envTimeL = 0f;
                    GameObject obj = Instantiate(_envObjects[UnityEngine.Random.Range(0, _envObjects.Length)], _envSponerL.transform.position, Quaternion.Euler(0, UnityEngine.Random.Range(0f, 360f), 0));
                    obj.transform.parent = _field.transform;
                    _envSponIntervalL = UnityEngine.Random.Range(0.2f, 1.5f);
                    obj.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                }
                if (envTimeR > _envSponIntervalR) {
                    envTimeR = 0f;
                    GameObject obj = Instantiate(_envObjects[UnityEngine.Random.Range(0, _envObjects.Length)], _envSponerR.transform.position, Quaternion.Euler(0, UnityEngine.Random.Range(0f, 360f), 0));
                    obj.transform.parent = _field.transform;
                    obj.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    _envSponIntervalR = UnityEngine.Random.Range(0.2f, 1.5f);
                }

                // 地面の生成処理
                if (diff > _groundDiff) {
                    diff = 0f;
                    GameObject ground = Instantiate(_ground, _groundSponer.transform.position, Quaternion.identity);
                    ground.transform.parent = _field.transform;
                }
                break;
            case GameState.Stop:
                break;
            case GameState.GameOver:
                if (Input.GetKey(KeyCode.Space)) {
                    ReloadGame();
                }
                if (Input.GetKey(KeyCode.R)) {
                    RestartGame();
                }
                break;
        }
    }

    public void SetGameState(GameState state)
    {
        gameState = state;
        Debug.Log("SetState: " + state);
        GameStateDidChange(state);
    }

    private void GameStateDidChange(GameState state)
    {
        switch (state)
        {
            case GameState.Ready:
                _homeUI.SetActive(true);
                _playUI.SetActive(false);
                _stopUI.SetActive(false);
                _gameOverUI.SetActive(false);
                _newItemUI.SetActive(false);
                Time.timeScale = 1;
                break;
            case GameState.Play:
                _homeUI.SetActive(false);
                _playUI.SetActive(true);
                _stopUI.SetActive(false);
                _gameOverUI.SetActive(false);
                Time.timeScale = 1;
                _player.GetComponent<Player>().StartRunning();
                break;
            case GameState.Stop:
                _homeUI.SetActive(false);
                _playUI.SetActive(true);
                _stopUI.SetActive(true);
                _gameOverUI.SetActive(false);
                Time.timeScale = 0;
                break;
            case GameState.GameOver:
                _homeUI.SetActive(false);
                _playUI.SetActive(false);
                _stopUI.SetActive(false);
                _gameOverUI.SetActive(true);
                Time.timeScale = 1;
                break;
        }
    }

    public void ScoreUp(int score)
    {
        _score += score;
        Debug.Log("ScoreUp: " + _score);
    }

    public void StopGame()
    {
        Debug.Log("StopGame");
        SetGameState(GameState.Stop);
    }

    public void ContinueGame()
    {
        Debug.Log("ContinueGame");
        SetGameState(GameState.Play);
    }

    public void ReloadGame()
    {
        Debug.Log("ReloadGame");
        Scene loadScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(loadScene.name);
    }

    public void RestartGame()
    {
        GameManager.initialGameState = GameState.Play;
        Scene loadScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(loadScene.name);
    }

    public void StartGame()
    {
        Debug.Log("StartGame");
        SetGameState(GameState.Play);
    }

    public void GameOver()
    {
        Debug.Log("GameOver");
        SetGameState(GameState.GameOver);
    }
}
