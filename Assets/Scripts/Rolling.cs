using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rolling : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.RotateAround(this.gameObject.transform.position, Vector3.right, -UnityEngine.Random.Range(5f, 15f));
        this.gameObject.transform.position += new Vector3(0, 0, -0.1f);
    }
}
