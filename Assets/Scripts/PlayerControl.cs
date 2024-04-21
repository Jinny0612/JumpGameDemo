using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

/// <summary>
/// ����ҽ�ɫ�Ĺ���
/// </summary>
public class PlayerControl : MonoBehaviour
{
    
    public float speed;//����ƶ��ٶ�
    //public float jumpSpeed;//��Ծ�ٶ�
    public int MAX_HEALTH = 10;
    public int MIN_HEALTH = 3;

    //public GameObject heartPrefab;//��������ֵ
    public TextMeshProUGUI scoreText;//������ʾ
    public GameObject losePanel;//��ɢ��Ļ��������Ϸ����

    public AudioClip healthIncrease;//����ֵ������Ч
    public AudioClip healthDecrease;//����ֵ������Ч
    public AudioClip onlyScoreIncrease;//����������������ֵ������Ч
    public AudioClip dashAudio;//��ɫ�����Ч

    public delegate void HealthChangedEventHanlder();//����ֵ�仯
    public event HealthChangedEventHanlder OnhealthChanged;//����ֵ�仯ʱ����

    public float extraSpeed;//���ʱ��ɫ�ı����ٶ�
    public float startDashTime;//��ʼ��̳���ʱ��
    public float jumpSpeed;//��Ծ�ٶ�

    private float dashTime;//ʣ���̳���ʱ��
    private bool isDashing;//�Ƿ����ڳ��
    private bool isJumping;//�Ƿ�������Ծ

    private float horizontala;
    private int health;//��ǰ����ֵ
    private int score;//��ǰ��õķ���
    

    private Animator animator;
    private new Rigidbody2D rigidbody;
    private AudioSource audio;//������Ч

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        audio = GetComponent<AudioSource>();
        
        animator = GetComponent<Animator>();
        //animator.SetTrigger("fall");
        //��ʼ����Ĭ��Ϊ3������unity���Զ��壬��Ϊ0����Ϸ����
        health = MIN_HEALTH >= 3 && MAX_HEALTH >= 3 ? Mathf.Min(MIN_HEALTH, MAX_HEALTH) : 3;
    }

    // ���������߼�(RigidBody�����)��ر���Ҫ��FixedUpdaate��
    void FixedUpdate()
    {
        //��ȡˮƽ��
        //horizontala = Input.GetAxisRaw("Horizontal");
        horizontala = Input.GetAxis("Horizontal");

        float positiony = rigidbody.velocity.y;
        /*if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            //���¿ո������Ծ
             positiony += speed;
        }*/

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
            rigidbody.velocity = new Vector2(horizontala * speed, positiony );
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
        if(Input.GetKeyDown(KeyCode.LeftShift) && !isDashing)
        {
            //��shift����
            speed += extraSpeed;
            isDashing = true;
            dashTime = startDashTime;
            //���������Ч
            audio.clip = dashAudio;
            audio.Play();
        }
        if (dashTime <= 0 && isDashing)
        {
            //��̳���ʱ�����������
            isDashing = false;
            speed -= extraSpeed;
        }
        else
        {
            //�ɿ���shift�����ʱ�俪ʼ�𽥼���
            dashTime -= Time.deltaTime;
        }
    }


    /// <summary>
    /// ����ֵ�仯
    /// </summary>
    /// <param name="type"> -1-���� 0-���� 1-����</param>
    /// <param name="amount">�仯��</param>
    public void healthChange(int type,int amount)
    {
            if (type != 0)
            {
                switch (type)
                {
                    case 1:
                        if (health < MAX_HEALTH)
                        {
                            //ֻ��������ֵ�����������ֵʱ����ֵ������
                            health = Mathf.Min(health + amount, MAX_HEALTH);
                            //��������ֵ������Ч
                            audio.clip = healthIncrease;
                        }
                        else
                        {
                            //��������������Ч
                            audio.clip = onlyScoreIncrease;
                        }
                        break;
                    case -1:
                        health -= amount;
                        //����ֵ���ͣ�����������Ч
                        audio.clip = healthDecrease;

                        break;
                }
                OnhealthChanged();//�����¼�
                if (health <= 0)
                {
                    //����ֵ��Ϊ0
                    losePanel.SetActive(true);
                    enabled = false;//����playerControl�ű�����ɫ�������ƶ�
                    GameObject swpawner = GameObject.FindGameObjectWithTag("Respawn");
                    if (swpawner != null)
                    {
                        swpawner.SetActive(false);//����spawnerControl�ű�������������׹��
                    }
                }
            }
            else
            {
                audio.clip = onlyScoreIncrease;
            }
            audio.Play();
        
        
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
        //δ����������������s
        
            switch (type)
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
