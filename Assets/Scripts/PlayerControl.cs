using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    
    public float speed;//玩家移动速度
    public int MAX_HEALTH = 10;
    public int MIN_HEALTH = 3;

    private float horizontala;
    private int health;//当前生命值
    private int score;//当前获得的分数
    

    private Animator animator;
    private Rigidbody2D rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();

        //初始时从空中掉落，触发落地动画
        //todo 目前问题：动画结束在半空中就持续站立动画了
        //todo 预期效果：在落地前都执行落地动画
        animator = GetComponent<Animator>();
        animator.SetTrigger("fall");
        //初始生命默认为3或者在unity中自定义，降为0则游戏结束
        health = MIN_HEALTH >= 3 && MAX_HEALTH >= 3 ? Mathf.Min(MAX_HEALTH, MAX_HEALTH) : 3;
    }

    // 处理物理逻辑(RigidBody等组件)相关必须要在FixedUpdaate中
    void FixedUpdate()
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

    private void Update()
    {
        if(horizontala != 0)
        {
            //键盘有方向输入
            //移动时启用跑步动画
            //question ：后面动画无法切换到站立
            // 已解决  在Animator中，idle和run的切换都需要设置condition，缺少了就无法切换动作
            animator.SetBool("run", true);
        }
        else
        {
            animator.SetBool("run", false);
        }
    }

    /// <summary>
    /// 生命值变化
    /// </summary>
    /// <param name="type">0-减少 1-增加</param>
    /// <param name="amount">变化量</param>
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
            //生命值耗尽，游戏结束

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
    /// 得分变化
    /// </summary>
    /// <param name="type">0-减少 1-增加</param>
    /// <param name="amount">变化量</param>
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
