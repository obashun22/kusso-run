using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator _animator;
    [SerializeField] GameObject _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        _animator = this.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (_gameManager.GetComponent<GameManager>().gameState)
        {
            case GameState.Ready:
                if (Input.GetKey(KeyCode.Space)) {
                    _animator.SetTrigger("Start");
                }
                break;
            case GameState.Play:
                if (Input.GetKey(KeyCode.A)) {
                    if (!_animator.IsInTransition(0)) {
                        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("RunLeft")) {
                            _animator.SetTrigger("MoveLeft");
                        }
                    }
                }
                if (Input.GetKey(KeyCode.D)) {
                    if (!_animator.IsInTransition(0)) {
                        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("RunRight")) {
                            _animator.SetTrigger("MoveRight");
                        }
                    }
                }
                if (Input.GetKey(KeyCode.Space)) {
                    if (!_animator.IsInTransition(0)) {
                        if (!(_animator.GetCurrentAnimatorStateInfo(0).IsName("JumpLeft") || _animator.GetCurrentAnimatorStateInfo(0).IsName("JumpMiddle") || _animator.GetCurrentAnimatorStateInfo(0).IsName("JumpRight")))
                        _animator.SetTrigger("Jump");
                    }
                }
                break;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Obstacle") {
            Debug.Log("Hit!!!");
            _gameManager.GetComponent<GameManager>().SetGameState(GameState.GameOver);
            _animator.SetTrigger("Death");
        } else if (other.gameObject.tag == "Coin") {
            Debug.Log("Coin!!!");
            other.gameObject.GetComponent<Item>().DestroyWithAction();
        }
    }
}
