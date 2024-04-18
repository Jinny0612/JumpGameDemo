using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    
    public float speed;//����ƶ��ٶ�
    public int MAX_HEALTH = 10;
    public int MIN_HEALTH = 3;

    private float horizontala;
    private int health;//��ǰ����ֵ
    private int score;//��ǰ��õķ���
    

    private Animator animator;
    private Rigidbody2D rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();

        //��ʼʱ�ӿ��е��䣬������ض���
        //todo Ŀǰ���⣺���������ڰ���оͳ���վ��������
        //todo Ԥ��Ч���������ǰ��ִ����ض���
        animator = GetComponent<Animator>();
        animator.SetTrigger("fall");
        //��ʼ����Ĭ��Ϊ3������unity���Զ��壬��Ϊ0����Ϸ����
        health = MIN_HEALTH >= 3 && MAX_HEALTH >= 3 ? Mathf.Min(MAX_HEALTH, MAX_HEALTH) : 3;
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
            rigidbody.velocity = new Vector2(horizontala * speed, rigidbody.velocity.y);
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

    /// <summary>
    /// ����ֵ�仯
    /// </summary>
    /// <param name="type">0-���� 1-����</param>
    /// <param name="amount">�仯��</param>
    public void healthChange(int type,int amount)
    {
        switch(type)
        {
            case 0:
                health -= amount; break;
            case 1:
                health += amount; break;
        }
        if (health <= 0)
        {
            //����ֵ�ľ�����Ϸ����

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
    /// <param name="type">0-���� 1-����</param>
    /// <param name="amount">�仯��</param>
    public void scoreChange(int type,int amount)
    {
        switch(type)
        {
            case 0: 
                score -= amount; break;
            case 1:
                score += amount; break;

        }
    }



    
}
