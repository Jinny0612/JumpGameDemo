using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    
    public float speed;//����ƶ��ٶ�
    //public float jumpSpeed;//��Ծ�ٶ�
    public int MAX_HEALTH = 10;
    public int MIN_HEALTH = 3;

    //public GameObject heartPrefab;//��������ֵ
    public TextMeshProUGUI scoreText;//������ʾ

    private float horizontala;
    private int health;//��ǰ����ֵ
    private int score;//��ǰ��õķ���

    private float distanceBtwHearts = 100;//���Ͻ�����ֵ���ĵļ��  800

    GameObject[] heartOrigin;

    private Animator animator;
    private new Rigidbody2D rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();

        //��ʼʱ�ӿ��е��䣬������ض���
        //todo Ŀǰ���⣺���������ڰ���оͳ���վ��������
        //todo Ԥ��Ч���������ǰ��ִ����ض���
        animator = GetComponent<Animator>();
        //animator.SetTrigger("fall");
        //��ʼ����Ĭ��Ϊ3������unity���Զ��壬��Ϊ0����Ϸ����
        health = MIN_HEALTH >= 3 && MAX_HEALTH >= 3 ? Mathf.Min(MIN_HEALTH, MAX_HEALTH) : 3;
        heartOrigin = GameObject.FindGameObjectsWithTag("Heart");
        Array.Reverse(heartOrigin);//��ת����ȡ���������Ƿ���

        showHeart();

        //heartPrefab.transform.SetParent(heartOrigin.transform.parent,false);

        //���ɳ�ʼ����ֵ
        //for (int i = 0; i < health - 1; i++)
        //{
        //    Instantiate(heartPrefab, new Vector3(heartOrigin.transform.position.x + distanceBtwHearts,heartOrigin.transform.position.y,heartOrigin.transform.position.z), Quaternion.identity);
        //}
        //Debug.Log("============= Start =============");
        //Debug.Log("Player Health = " +  health);
        //Debug.Log("Player Score = " + score);
    }

    // ���������߼�(RigidBody�����)��ر���Ҫ��FixedUpdaate��
    void FixedUpdate()
    {
        //��ȡˮƽ��
        //horizontala = Input.GetAxisRaw("Horizontal");
        horizontala = Input.GetAxis("Horizontal");

        //Debug.Log(speed);
        //���ø��������ٶ�
        //question: ����д����֪��Ϊʲô�޷��ƶ�
        //          ���ֵ�����Ӧ������Ϊspeed��ֵ�����⣬ÿ��debug�����ֵ����һ�������õ�ֵ��0��������֪��Ϊʲô
        // �ѽ��  Unity���浼�µģ������ͺ��� ���񾭲��������������������˸�����һ����Сʱ����Сʱ�Ž��
        if (horizontala != 0)
        {

            // question�������������ҷ���ʱ����ɫ�������ƶ�
            //����Ч��: �ɿ��������ɫ���̲��ƶ�
            //�ѽ���������Input.GetAxisRaw��ΪInput.GetAxis���ɣ�raw��ֵ�ǻ��𽥱�Ϊ0 (�����Ҫ�Ӽ��ٵ��ʺ�ʹ��raw�������
            //          GetAxis���ɿ���������������Ϊ0����˻�����ֹͣ�ƶ�
            rigidbody.velocity = new Vector2(horizontala * speed, rigidbody.velocity.y );
            //ʹ����������ƶ����������ɿ�ʱֹͣ�ƶ�
            //����ʹ��translate�������ᵼ������Ķ��ƶ��������Ƶ�ʱ�������ƶ�
            //transform.Translate(new Vector2(horizontala * Time.deltaTime * speed, rigidbody.velocity.y));
            //�ƶ�ʱ�泯�ƶ�����
            if (horizontala > 0)
            {
                //�ұ�
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
            else if (horizontala < 0)
            {
                //���
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
        }
        
    }

    private void Update()
    {
        if(horizontala != 0)
        {
            //�����з�������
            //�ƶ�ʱ�����ܲ�����
            //question �����涯���޷��л���վ��
            // �ѽ��  ��Animator�У�idle��run���л�����Ҫ����condition��ȱ���˾��޷��л�����
            animator.SetBool("run", true);
        }
        else
        {
            animator.SetBool("run", false);
        }
    }

    public void switchPlayerHurtWhenJump()
    {
        
    }

    /// <summary>
    /// ����ֵ�仯
    /// </summary>
    /// <param name="type"> -1-���� 0-���� 1-����</param>
    /// <param name="amount">�仯��</param>
    public void healthChange(int type,int amount)
    {
        switch(type)
        {
            case 0:
                break;
            case 1:
                health += amount; break;
            case -1:
                health -= amount; break;
        }

        showHeart();
        
    }

    /// <summary>
    /// ����ֵ��ʾ��todo ��Ҫ�Ż���������˵��̬�������
    /// </summary>
    public void showHeart()
    {
        for (int i = 0; i < heartOrigin.Length; i++)
        {
            if (health > i)
            {
                if (!heartOrigin[i].active)
                {
                    heartOrigin[i].SetActive(true);
                }
            }
            else
            {
                if (heartOrigin[i].active)
                {
                    heartOrigin[i].SetActive(false);
                }
            }
        }
    }

    public int getHealth()
    {
        return health;
    }

    public int getScore()
    {
        return score;
    }

    /// <summary>
    /// �÷ֱ仯
    /// </summary>
    /// <param name="type"> -1-���� 0-���� 1-����</param>
    /// <param name="amount">�仯��</param>
    public void scoreChange(int type,int amount)
    {
        switch(type)
        {
            case 0: 
                 break;
            case 1:
                score += amount; break;
            case -1:
                score -= amount; break;

        }
        scoreText.text = "SCORE\n" + score;
    }

    
}
