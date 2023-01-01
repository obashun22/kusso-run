using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    [SerializeField] GameObject _gameManager;
    [SerializeField] float _endLineZ;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (_gameManager.GetComponent<GameManager>().gameState)
        {
            case GameState.Play:
                for (int index = 0; index < this.gameObject.transform.childCount; index++)
                {
                    GameObject child = this.gameObject.transform.GetChild(index).gameObject;
                    if (child.transform.position.z < _endLineZ) {
                        Destroy(child);
                        continue;
                    }
                    float speed = _gameManager.GetComponent<GameManager>().speed;
                    child.transform.position = new Vector3(child.transform.position.x, child.transform.position.y, child.transform.position.z - speed * Time.deltaTime);
                }
                break;
        }
    }
}
