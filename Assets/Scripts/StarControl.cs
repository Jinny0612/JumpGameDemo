using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarControl : MonoBehaviour
{
    public float minSpeed;//最小降落速度
    public float maxSpeed;//最大降落速度
    public int scoreAmount;//单个分数变化

    float speed;//范围在minSpeed--maxSpeed

    private bool shouldFalling;

    public PlayerControl player;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(minSpeed, maxSpeed);
        animator = GetComponent<Animator>();
        shouldFalling = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldFalling)
        {
            //未碰撞，一直下落
            transform.Translate(Vector2.down * speed * Time.deltaTime);

        }
    }
   

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject.CompareTag("Player"))
        {
            //玩家与星星碰撞  积分增加，生命值不变
            if (player != null)
            {
                //碰撞，禁止下落
                shouldFalling |= false;
                //动画状态切换
                animator.SetTrigger("isHit");
                //player.healthChange(1, 1);
                player.scoreChange(1, scoreAmount);
            }
        }
    }

    public void destroyStar()
    {
        Destroy(gameObject);
    }

}
