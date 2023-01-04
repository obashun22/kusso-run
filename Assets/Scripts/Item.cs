using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int score;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, (Time.time * 150) % 360f, 0));
    }

    public void DestroyWithAction()
    {
        Destroy(this.gameObject);
    }
}
