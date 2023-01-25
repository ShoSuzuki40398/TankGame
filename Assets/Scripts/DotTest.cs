using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotTest : MonoBehaviour
{
    public Transform Enemy;
    public Transform Player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var vec = Player.transform.position - Enemy.transform.position;

        var dot = Vector3.Dot(vec.normalized,Enemy.transform.forward);

        Debug.Log(dot);
    }
}
