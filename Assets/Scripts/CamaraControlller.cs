using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraControlller : MonoBehaviour
{
    public GameObject personaje;
    new Transform transform;
    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>();
        var t = personaje.GetComponent<Transform>();
        Debug.Log(t.position.x);
    }

    // Update is called once per frame
    void Update()
    {
        var t = personaje.GetComponent<Transform>();
        var x = t.position.x;
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }
}
