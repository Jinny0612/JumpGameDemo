using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectControl : MonoBehaviour
{
    public float minSpeed;//��С�ƶ��ٶ�
    public float maxSpeed;//����ƶ��ٶ�
    public int scoreAmount;//�����仯��
    public int healthAmount;//����ֵ�仯��
    public int scoreType;//�����仯����
    public int healthType;//����ֵ�仯����
    public bool isAnimatorEnabledStart;//��ʼ�Ƿ�ִ�ж���
    public bool isAnimatorEnableOnTrigger;//�������Ƿ�ִ�ж���

    public GameObject explosion;//��ըЧ��Ԥ����



    protected PlayerControl player;//����ҽ�ɫ
    protected Animator animator;//��������

    protected float speed;//��Χ��minSpeed--maxSpeed
    protected bool shouldMoving;//�����Ƿ�Ӧ�ü����ƶ�



    // Start is called before the first frame update
    void Start()
    {
        //���ַ���Ҳ���Ի�ȡ��PlayerControl��������inspection�а�player
        //player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        //speed = Random.Range(minSpeed, maxSpeed);
        //animator = GetComponent<Animator>();
        //animator.enabled = false;//��ʼ����������
        //shouldMoving = true;
        //initPlayerControl();//player��ʼ��
        //initAnimator(false);//������������ʼ������ʼ�����Ŷ���
        //initSpeed();//�����ƶ��ٶȳ�ʼ��
        //shouldMoving = true;//�����ƶ�
        initGameObject(isAnimatorEnabledStart, true);
        //Debug.Log(gameObject.name + " : " + animator.enabled);
        //Debug.Log("============= Start =============");
        //Debug.Log("health = " + player.getHealth());
        //Debug.Log("score = " + player.getScore());
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(shouldFalling);
        if (shouldMoving)
        {
            //δ��ײ��һֱ����
            transform.Translate(Vector2.down * speed * Time.deltaTime);
        }
    }
   

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                //�����������ײ  �������ӣ�����ֵ����
                if (player != null)
                {
                    //��ײ����ֹ����
                    shouldMoving = false;
                    //����star����
                    animator.enabled = isAnimatorEnableOnTrigger;
                    //Debug.Log(gameObject.name + " : " +animator.enabled);
                    //player.healthChange(1, 1);
                    //player.scoreChange(1, scoreAmount);
                    scoreAndHealthChange(scoreType, scoreAmount, healthType, healthAmount);

                    //Debug.Log("�Ƿ�����" + shouldFalling);
                    //Debug.Log("============= onTrigger =============");
                    //Debug.Log("health = " + player.getHealth());
                    //Debug.Log("score = " + player.getScore());
                }
            } 
            else if (collision.gameObject.CompareTag("Ground"))
            {
                //���������������ײ���Զ���ʧ
                //��ײ�ı�Ҫ������������ײ�����������box collider���������һ��Ҫ��Rigidbody���

                //�����嵱ǰλ�ñ�ը
                //��ըԤ���壨����ϵͳ����������Ҫ��inspector�����ã�ʹ��destroy�����ᱨ��
                Instantiate(explosion,transform.position, Quaternion.identity);
                
                Destroy(gameObject);
            }
            
        }
    }

    /// <summary>
    /// ��Player
    /// </summary>
    protected void initPlayerControl()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
    }

    /// <summary>
    /// ��ʼ������������
    /// </summary>
    /// <param name="enabled">�Ƿ񴥷�����</param>
    protected void initAnimator(bool enabled)
    {
        animator = GetComponent<Animator>();
        animator.enabled = enabled;
    }

    /// <summary>
    /// �����ƶ��ٶȳ�ʼ������Χ��minSpeed--maxSpeed
    /// </summary>
    protected void initSpeed()
    {
        speed = Random.Range(minSpeed, maxSpeed);
    }

    /// <summary>
    /// ��Ϸ�����ʼ��
    /// </summary>
    /// <param name="animatorEnabled">�Ƿ����ö���</param>
    /// <param name="shouldMoving">�Ƿ������ƶ�</param>
    protected void initGameObject(bool animatorEnabled, bool shouldMoving)
    {
        initPlayerControl();
        initAnimator(animatorEnabled);
        initSpeed();
        this.shouldMoving = shouldMoving;

    }

    /// <summary>
    /// �÷ֺ�����ֵ�仯
    /// </summary>
    /// <param name="scoreType">-1-���� 0-���� 1-����</param>
    /// <param name="scoreAmount">�����仯��</param>
    /// <param name="healthType">ͬscoreType������</param>
    /// <param name="healthAmount">����ֵ�仯��</param>
    protected void scoreAndHealthChange(int scoreType, int scoreAmount, int healthType, int healthAmount)
    {
        if (player != null)
        {
            player.scoreChange(scoreType, scoreAmount);
            player.healthChange(healthType, healthAmount);
        }
    }

    /// <summary>
    /// ��������
    /// </summary>
    public void destroyStar()
    {
        Destroy(gameObject);
    }


}
