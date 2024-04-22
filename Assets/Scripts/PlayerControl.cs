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
    public float jumpForce;//��Ծ����
    public LayerMask ground;//����  (��������ͼ������Ϊ��"Ground")

    private float dashTime;//ʣ���̳���ʱ��
    private bool isDashing;//�Ƿ����ڳ��
    private bool isGrounded;//�Ƿ��ڵ����ϣ�ֻ���ڵ����ϰ��¿ո��������Ծ
    private bool isJumping =false;//�Ƿ�����Ծ  ������Ծ����
    private int jumpCount = 0;//��Ծ����
    private bool jumpPressed = false;//�жϰ����Ƿ��Ѿ�����


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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //�ж�player��Ground����ײ
        if (collision.gameObject.CompareTag("Ground"))
        {
            //playerվ�ڵ�����
            isGrounded = true;
        }
        //Debug.Log("�Ƿ��ڵ����� = " + isGrounded);
    }

    // ���������߼�(RigidBody�����)��ر���Ҫ��FixedUpdaate��
    void FixedUpdate()
    {
        //����Ƿ��ڵ�����
        //isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, ground);
        
        PlayerMovement();//ˮƽ�ƶ�
        Jump();//��Ծ�߼�
        switchAnimation();
        
    }

    private void Update()
    {

        //jumpCount������Ծ������1-һ����Ծ��2-ִ�ж�����
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount > 0)
        {
            jumpPressed = true;//�����˿ո�
            //�ڵ����ϣ����¿ո������Ծ
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpForce);
            
        }

        if (horizontala != 0)
        {
            //�����з�������
            //�ƶ�ʱ�����ܲ�����
            //question �����涯���޷��л���վ��
            // �ѽ��  ��Animator�У�idle��run���л�����Ҫ����condition��ȱ���˾��޷��л�����
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing && !isJumping)
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
    /// ˮƽ�ƶ�
    /// </summary>
    private void PlayerMovement()
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

    /// <summary>
    /// ��ɫ��Ծ
    /// </summary>
    private void Jump()
    {
        if (isGrounded)
        {
            //��أ����Խ���������Ծ����Ծ״̬��Ϊfalse
            jumpCount = 2;
            isJumping = false;
        }
        if(jumpPressed)
        {
            //������Ҫ����isGround״̬����������ڿ���n����Ծ�����Ƕ�����
            if (isGrounded)
            {
                //animator.SetTrigger("jump");//������Ծ
                //animator.SetTrigger("fall");
                //������Ծ�������ڵ����ϣ���Ծ
                isJumping = true;
                rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpForce);
                jumpCount--;
                jumpPressed = false;
                isGrounded = false;

            }
            else if (jumpCount > 0 && isJumping)
            {
                //animator.SetTrigger("jump");//������Ծ
                //animator.SetTrigger("fall");
                //�ڿ��У�������Ծ�����ҿ���Ծ���������㣬���ٴ���Ծ
                rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpForce);
                jumpCount--;
                jumpPressed = false;
                isGrounded = false;

            } 
            else if(jumpCount == 0)
            {
                jumpPressed = false;
                isGrounded = false;
            }
        }
        
    }


    //�����л�
    void switchAnimation()
    {
        //animator.SetFloat("isRunning",MathF.Abs(rigidbody.velocity.x));
        if (isGrounded)
        {
            //�ڵ�����ֹͣ����
            animator.SetBool("isFalling", false);
        }
        else
        {
            //�ڿ���
            if (rigidbody.velocity.y > 0)
            {
                //�ڿ�������y�������ϵ��ٶ�,������Ծ
                animator.SetBool("isJumping", true);
            }
            else if (rigidbody.velocity.y < 0)
            {
                //��һ��y�������µ��ٶȣ���������
                animator.SetBool("isFalling", true);
                animator.SetBool("isJumping", false);
            }
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
                        //animator.SetTrigger("getHurt");
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
                    score += amount;
                    //animator.SetTrigger("hit");
                break;
                case -1:
                    score -= amount; break;

            }
            scoreText.text = "SCORE\n" + score;
            
        
        
    }

    
}
