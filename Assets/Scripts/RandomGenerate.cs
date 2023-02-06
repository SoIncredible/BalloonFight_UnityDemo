using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomGenerate : MonoBehaviour
{
    //随机生成气球
    // Start is called before the first frame update
    public Transform []point_array= new Transform[4];
    public GameObject prefab;
    public float StartTime;
    public int count = 10;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - StartTime >= 2f && count > 1)
        {
            Generate();
        }
    }
    void Generate()
    {

        int random = Random.Range(0, 4);
        Instantiate(prefab);
        prefab.transform.position = new Vector2(point_array[random].position.x,point_array[random].position.y);
        
        StartTime = Time.time;
        
        count--;
    }
}
