using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeControls : MonoBehaviour
{
    [SerializeField] private int movingSpeed = 2;
    [SerializeField] private Transform cubeTransform;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveUp()
    {
        var position = cubeTransform.position;
        float y = position.y;
        y += movingSpeed/50f;
        var newPosition = new Vector3(position.x, y, position.z);
        cubeTransform.position = newPosition;
    }

    public void MoveDown()
    {
        var position = cubeTransform.position;
        float y = position.y;
        y -= movingSpeed/50f;
        var newPosition = new Vector3(position.x, y, position.z);
        cubeTransform.position = newPosition;
    }

    public void MoveLeft()
    {
        var position = cubeTransform.position;
        float x = position.x;
        x -= movingSpeed/50f;
        var newPosition = new Vector3(x, position.y, position.z);
        cubeTransform.position = newPosition;
    }

    public void MoveRight()
    {
        var position = cubeTransform.position;
        float x = position.x;
        x += movingSpeed/50f;
        var newPosition = new Vector3(x, position.y, position.z);
        cubeTransform.position = newPosition;
    }
}
