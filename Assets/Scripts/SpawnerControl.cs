using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerControl : MonoBehaviour
{
    public Transform[] spawnsPoints; //生成对象的位置
    public GameObject[] gameObjects;//与预制体的对象绑定，需要生成多少种数组长度就多大

    private float timeBtwSpawns;//目前还有多久生成新的对象 单位秒
    public float startTimeBtwSpawns;//初始设定的生成新对象时间间隔 单位秒


    // Update is called once per frame
    void Update()
    {
        if(timeBtwSpawns<=0)
        {
            //此时应该生成一个gameobject
            //随机位置
            Transform randomTransform = spawnsPoints[Random.Range(0, spawnsPoints.Length)];
            //随机对象
            GameObject randomPerfab = gameObjects[Random.Range(0, gameObjects.Length)];
            //将位置传给对象
            Instantiate(randomPerfab, randomTransform.position,Quaternion.identity);
            //重置间隔时间
            timeBtwSpawns = startTimeBtwSpawns;
        } 
        else
        {
            //减少时间直至0
            timeBtwSpawns -= Time.deltaTime;
        }
    }
}
