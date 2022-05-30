using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floating : MonoBehaviour
{

    [SerializeField] private float amplitude;
    [SerializeField] private float speed;
    private float positionY;

    // Start is called before the first frame update
    void Start()
    {
        positionY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {

        var pos = new Vector3(0f, positionY + Mathf.Sin(Time.time * speed) * amplitude, 0f);
        transform.position = pos;
    }
}
