using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float speed;
    [SerializeField] float _unitInterval;
    [SerializeField] int _maxUnitCount;
    [SerializeField] int _minUnitCount;
    [SerializeField] float _speedUpRate;
    public GameState gameState;
    [SerializeField] GameObject[] _obstacles;
    [SerializeField] GameObject[] _obstaclesM;
    [SerializeField] GameObject[] _envObjects;
    [SerializeField] GameObject[] _sponers;
    [SerializeField] GameObject _envSponerL;
    [SerializeField] GameObject _envSponerR;
    [SerializeField] GameObject _groundSponer;
    [SerializeField] GameObject _field;
    [SerializeField] GameObject _ground;
    [SerializeField] float _groundDiff;
    [SerializeField] GameObject _coin;

    private float _envSponIntervalL;
    private float _envSponIntervalR;
    private float time;
    private float envTimeL;
    private float envTimeR;
    private float diff;
    private int _unitCount;
    private int _sponUnitCount;
    private int _coinLine;

    // Start is called before the first frame update
    void Start()
    {
        _envSponIntervalL = 0f;
        _envSponIntervalR = 0f;
        _sponUnitCount = 0;
        _coinLine = UnityEngine.Random.Range(0, 3);
    }

    // Update is called once per frame
    void Update()
    {
        switch (gameState) {
            case GameState.Ready:
                if (Input.GetKey(KeyCode.Space)) {
                    SetGameState(GameState.Play);
                }
                break;
            case GameState.Play:
                time += Time.deltaTime;
                envTimeL += Time.deltaTime;
                envTimeR += Time.deltaTime;
                diff += speed * Time.deltaTime;

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
                        GameObject coin = Instantiate(_coin, _sponers[_coinLine].transform.position, Quaternion.identity);
                        coin.transform.parent = _field.transform;
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
            case GameState.GameOver:
                if (Input.GetKey(KeyCode.Space)) {
                    Scene loadScene = SceneManager.GetActiveScene();
                    SceneManager.LoadScene(loadScene.name);
                }
                break;
        }
    }

    public void SetGameState(GameState state)
    {
        gameState = state;
        Debug.Log("SetState: " + state);
    }
}
