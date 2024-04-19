using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerControl : MonoBehaviour
{
    public Transform[] spawnsPoints; //���ɶ����λ��
    public GameObject[] gameObjects;//��Ԥ����Ķ���󶨣���Ҫ���ɶ��������鳤�ȾͶ��

    private float timeBtwSpawns;//Ŀǰ���ж�������µĶ��� ��λ��
    public float startTimeBtwSpawns;//��ʼ�趨�������¶���ʱ���� ��λ��


    // Update is called once per frame
    void Update()
    {
        if(timeBtwSpawns<=0)
        {
            //��ʱӦ������һ��gameobject
            //���λ��
            Transform randomTransform = spawnsPoints[Random.Range(0, spawnsPoints.Length)];
            //�������
            GameObject randomPerfab = gameObjects[Random.Range(0, gameObjects.Length)];
            //��λ�ô�������
            Instantiate(randomPerfab, randomTransform.position,Quaternion.identity);
            //���ü��ʱ��
            timeBtwSpawns = startTimeBtwSpawns;
        } 
        else
        {
            //����ʱ��ֱ��0
            timeBtwSpawns -= Time.deltaTime;
        }
    }
}
