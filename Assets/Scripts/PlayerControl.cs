using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

/// <summary>
/// 对玩家角色的管理
/// </summary>
public class PlayerControl : MonoBehaviour
{
    
    public float speed;//玩家移动速度
    //public float jumpSpeed;//跳跃速度
    public int MAX_HEALTH = 10;
    public int MIN_HEALTH = 3;

    //public GameObject heartPrefab;//引用生命值
    public TextMeshProUGUI scoreText;//分数显示
    public GameObject losePanel;//松散屏幕，用于游戏结束

    public AudioClip healthIncrease;//生命值增加音效
    public AudioClip healthDecrease;//生命值减少音效
    public AudioClip onlyScoreIncrease;//仅仅分数增加生命值不变音效
    public AudioClip dashAudio;//角色冲刺音效

    public delegate void HealthChangedEventHanlder();//生命值变化
    public event HealthChangedEventHanlder OnhealthChanged;//生命值变化时触发

    public float extraSpeed;//冲刺时角色的奔跑速度
    public float startDashTime;//起始冲刺持续时间
    public float jumpForce;//跳跃力度
    public LayerMask ground;//地面  (地面对象的图层设置为了"Ground")

    private float dashTime;//剩余冲刺持续时间
    private bool isDashing;//是否正在冲刺
    private bool isGrounded;//是否在地面上，只有在地面上按下空格键才能跳跃
    private bool isJumping =false;//是否在跳跃  触发跳跃动画
    private int jumpCount = 0;//跳跃次数
    private bool jumpPressed = false;//判断按键是否已经按下


    private float horizontala;
    private int health;//当前生命值
    private int score;//当前获得的分数
    

    private Animator animator;
    private new Rigidbody2D rigidbody;
    private AudioSource audio;//受伤音效

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        audio = GetComponent<AudioSource>();
        
        animator = GetComponent<Animator>();
        //animator.SetTrigger("fall");
        //初始生命默认为3或者在unity中自定义，降为0则游戏结束
        health = MIN_HEALTH >= 3 && MAX_HEALTH >= 3 ? Mathf.Min(MIN_HEALTH, MAX_HEALTH) : 3;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //判断player和Ground的碰撞
        if (collision.gameObject.CompareTag("Ground"))
        {
            //player站在地面上
            isGrounded = true;
        }
        //Debug.Log("是否在地面上 = " + isGrounded);
    }

    // 处理物理逻辑(RigidBody等组件)相关必须要在FixedUpdaate中
    void FixedUpdate()
    {
        //检测是否在地面上
        //isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, ground);
        
        PlayerMovement();//水平移动
        Jump();//跳跃逻辑
        switchAnimation();
        
    }

    private void Update()
    {

        //jumpCount控制跳跃次数，1-一次跳跃，2-执行二段跳
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount > 0)
        {
            jumpPressed = true;//按下了空格
            //在地面上，按下空格键，跳跃
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpForce);
            
        }

        if (horizontala != 0)
        {
            //键盘有方向输入
            //移动时启用跑步动画
            //question ：后面动画无法切换到站立
            // 已解决  在Animator中，idle和run的切换都需要设置condition，缺少了就无法切换动作
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing && !isJumping)
        {
            //左shift加速
            speed += extraSpeed;
            isDashing = true;
            dashTime = startDashTime;
            //触发冲刺音效
            audio.clip = dashAudio;
            audio.Play();
        }
        if (dashTime <= 0 && isDashing)
        {
            //冲刺持续时间结束，减速
            isDashing = false;
            speed -= extraSpeed;
        }
        else
        {
            //松开左shift，冲刺时间开始逐渐减少
            dashTime -= Time.deltaTime;
        }
    }

    /// <summary>
    /// 水平移动
    /// </summary>
    private void PlayerMovement()
    {
        //获取水平轴
        //horizontala = Input.GetAxisRaw("Horizontal");
        horizontala = Input.GetAxis("Horizontal");

        //Debug.Log(speed);
        //设置刚体线性速度
        //question: 这种写法不知道为什么无法移动
        //          发现的问题应该是因为speed的值有问题，每次debug输出的值都不一样，设置的值和0间隔输出不知道为什么
        // 已解决  Unity缓存导致的，重启就好了 （神经病啊！！！！！气死我了搞了我一个多小时两个小时才解决
        if (horizontala != 0)
        {

            // question：不再输入左右方向时，角色依旧在移动
            //期望效果: 松开方向键角色立刻不移动
            //已解决：上面的Input.GetAxisRaw改为Input.GetAxis即可，raw的值是会逐渐变为0 (如果需要加减速的适合使用raw这个方法
            //          GetAxis在松开方向键后会立即变为0，因此会立刻停止移动
            rigidbody.velocity = new Vector2(horizontala * speed, rigidbody.velocity.y);
            //使用这个方法移动能立刻在松开时停止移动
            //但是使用translate方法，会导致下面改动移动朝向，左移的时候向右移动
            //transform.Translate(new Vector2(horizontala * Time.deltaTime * speed, rigidbody.velocity.y));
            //移动时面朝移动方向
            if (horizontala > 0)
            {
                //右边
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
            else if (horizontala < 0)
            {
                //左边
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
        }
    }

    /// <summary>
    /// 角色跳跃
    /// </summary>
    private void Jump()
    {
        if (isGrounded)
        {
            //落地，可以进行两段跳跃，跳跃状态置为false
            jumpCount = 2;
            isJumping = false;
        }
        if(jumpPressed)
        {
            //这里需要重置isGround状态，否则可以在空中n段跳跃而不是二段跳
            if (isGrounded)
            {
                //animator.SetTrigger("jump");//触发跳跃
                //animator.SetTrigger("fall");
                //按下跳跃键，且在地面上，跳跃
                isJumping = true;
                rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpForce);
                jumpCount--;
                jumpPressed = false;
                isGrounded = false;

            }
            else if (jumpCount > 0 && isJumping)
            {
                //animator.SetTrigger("jump");//触发跳跃
                //animator.SetTrigger("fall");
                //在空中，按下跳跃键，且可跳跃次数大于零，可再次跳跃
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


    //动画切换
    void switchAnimation()
    {
        //animator.SetFloat("isRunning",MathF.Abs(rigidbody.velocity.x));
        if (isGrounded)
        {
            //在地面上停止下落
            animator.SetBool("isFalling", false);
        }
        else
        {
            //在空中
            if (rigidbody.velocity.y > 0)
            {
                //在空中且有y方向向上的速度,正在跳跃
                animator.SetBool("isJumping", true);
            }
            else if (rigidbody.velocity.y < 0)
            {
                //有一个y方向向下的速度，正在下落
                animator.SetBool("isFalling", true);
                animator.SetBool("isJumping", false);
            }
        }
        
    }


    /// <summary>
    /// 生命值变化
    /// </summary>
    /// <param name="type"> -1-减少 0-不变 1-增加</param>
    /// <param name="amount">变化量</param>
    public void healthChange(int type,int amount)
    {
            if (type != 0)
            {
                switch (type)
                {
                    case 1:
                        if (health < MAX_HEALTH)
                        {
                            //只有在生命值不足最大生命值时生命值会增加
                            health = Mathf.Min(health + amount, MAX_HEALTH);
                            //触发生命值增加音效
                            audio.clip = healthIncrease;
                        }
                        else
                        {
                            //触发分数增加音效
                            audio.clip = onlyScoreIncrease;
                        }
                        break;
                    case -1:
                        health -= amount;
                        //生命值降低，触发受伤音效
                        audio.clip = healthDecrease;
                        //animator.SetTrigger("getHurt");
                        break;
                }
                OnhealthChanged();//触发事件
                if (health <= 0)
                {
                    //生命值降为0
                    losePanel.SetActive(true);
                    enabled = false;//禁用playerControl脚本，角色不会再移动
                    GameObject swpawner = GameObject.FindGameObjectWithTag("Respawn");
                    if (swpawner != null)
                    {
                        swpawner.SetActive(false);//禁用spawnerControl脚本，不再有物体坠落
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
    /// 得分变化
    /// </summary>
    /// <param name="type"> -1-减少 0-不变 1-增加</param>
    /// <param name="amount">变化量</param>
    public void scoreChange(int type,int amount)
    {
        //未触碰过，分数更新s
        
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
