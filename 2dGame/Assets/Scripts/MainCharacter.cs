using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 newPosition = new Vector3(-100, 0, 0);
    void Start()
    {
        gameObject.transform.position = newPosition;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
