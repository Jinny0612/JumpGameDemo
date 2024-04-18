using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarControl : MonoBehaviour
{
    public float minSpeed;//��С�����ٶ�
    public float maxSpeed;//������ٶ�
    public int scoreAmount;//���������仯

    float speed;//��Χ��minSpeed--maxSpeed

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
            //δ��ײ��һֱ����
            transform.Translate(Vector2.down * speed * Time.deltaTime);

        }
    }
   

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject.CompareTag("Player"))
        {
            //�����������ײ  �������ӣ�����ֵ����
            if (player != null)
            {
                //��ײ����ֹ����
                shouldFalling |= false;
                //����״̬�л�
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
