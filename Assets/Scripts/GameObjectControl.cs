using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectControl : MonoBehaviour
{
    public float minSpeed;//最小移动速度
    public float maxSpeed;//最大移动速度
    public int scoreAmount;//分数变化量
    public int healthAmount;//生命值变化量
    public int scoreType;//分数变化类型
    public int healthType;//生命值变化类型
    public bool isAnimatorEnabledStart;//初始是否执行动画
    public bool isAnimatorEnableOnTrigger;//触发后是否执行动画

    public GameObject explosion;//爆炸效果预制体



    protected PlayerControl player;//绑定玩家角色
    protected Animator animator;//动画管理

    protected float speed;//范围在minSpeed--maxSpeed
    protected bool shouldMoving;//物体是否应该继续移动



    // Start is called before the first frame update
    void Start()
    {
        //这种方法也可以获取到PlayerControl而无需在inspection中绑定player
        //player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        //speed = Random.Range(minSpeed, maxSpeed);
        //animator = GetComponent<Animator>();
        //animator.enabled = false;//开始不触发动画
        //shouldMoving = true;
        //initPlayerControl();//player初始化
        //initAnimator(false);//动画控制器初始化，初始不播放动画
        //initSpeed();//物体移动速度初始化
        //shouldMoving = true;//允许移动
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
            //未碰撞，一直下落
            transform.Translate(Vector2.down * speed * Time.deltaTime);
        }
    }
   

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                //玩家与星星碰撞  积分增加，生命值不变
                if (player != null)
                {
                    //碰撞，禁止下落
                    shouldMoving = false;
                    //播放star动画
                    animator.enabled = isAnimatorEnableOnTrigger;
                    //Debug.Log(gameObject.name + " : " +animator.enabled);
                    //player.healthChange(1, 1);
                    //player.scoreChange(1, scoreAmount);
                    scoreAndHealthChange(scoreType, scoreAmount, healthType, healthAmount);

                    //Debug.Log("是否下落" + shouldFalling);
                    //Debug.Log("============= onTrigger =============");
                    //Debug.Log("health = " + player.getHealth());
                    //Debug.Log("score = " + player.getScore());
                }
            } 
            else if (collision.gameObject.CompareTag("Ground"))
            {
                //掉落物体与地面碰撞，自动消失
                //碰撞的必要条件，两个碰撞的物体必须有box collider组件，其中一个要有Rigidbody组件

                //在物体当前位置爆炸
                //爆炸预制体（粒子系统）的销毁需要在inspector中设置，使用destroy方法会报错
                Instantiate(explosion,transform.position, Quaternion.identity);
                
                Destroy(gameObject);
            }
            
        }
    }

    /// <summary>
    /// 绑定Player
    /// </summary>
    protected void initPlayerControl()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
    }

    /// <summary>
    /// 初始化动画控制器
    /// </summary>
    /// <param name="enabled">是否触发动画</param>
    protected void initAnimator(bool enabled)
    {
        animator = GetComponent<Animator>();
        animator.enabled = enabled;
    }

    /// <summary>
    /// 物体移动速度初始化，范围在minSpeed--maxSpeed
    /// </summary>
    protected void initSpeed()
    {
        speed = Random.Range(minSpeed, maxSpeed);
    }

    /// <summary>
    /// 游戏物体初始化
    /// </summary>
    /// <param name="animatorEnabled">是否启用动画</param>
    /// <param name="shouldMoving">是否允许移动</param>
    protected void initGameObject(bool animatorEnabled, bool shouldMoving)
    {
        initPlayerControl();
        initAnimator(animatorEnabled);
        initSpeed();
        this.shouldMoving = shouldMoving;

    }

    /// <summary>
    /// 得分和生命值变化
    /// </summary>
    /// <param name="scoreType">-1-减少 0-不变 1-增加</param>
    /// <param name="scoreAmount">分数变化量</param>
    /// <param name="healthType">同scoreType的类型</param>
    /// <param name="healthAmount">生命值变化量</param>
    protected void scoreAndHealthChange(int scoreType, int scoreAmount, int healthType, int healthAmount)
    {
        if (player != null)
        {
            player.scoreChange(scoreType, scoreAmount);
            player.healthChange(healthType, healthAmount);
        }
    }

    /// <summary>
    /// 销毁物体
    /// </summary>
    public void destroyStar()
    {
        Destroy(gameObject);
    }


}
