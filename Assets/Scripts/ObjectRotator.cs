using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    [SerializeField]
    int speed = 50;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var rotationSpeed = speed * Time.deltaTime;
        gameObject.transform.Rotate(new Vector3(rotationSpeed, -rotationSpeed, rotationSpeed * 0.5f));
    }
}
