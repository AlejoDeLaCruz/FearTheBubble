using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour

{
    public Transform target;

    public float Yoffset = 0.5f;
    public float Xoffset = 0;
    public float Zoffset = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(target.position.x - Xoffset, target.position.y - Yoffset, target.position.z - Zoffset);
    }
}
