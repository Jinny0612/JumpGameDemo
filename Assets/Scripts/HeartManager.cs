using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI中对生命值显示的管理
/// </summary>
public class HeartManager : MonoBehaviour
{

    public GameObject heartPerfab;//生命值爱心预制体
    public float firstHeartX;//初始相对Canvas的X坐标
    public float firstHeartY;//初始相对Canvas的Y坐标


    private PlayerControl playerControl;//player

    private float heartSpacing;//爱心间隔
    private List<GameObject> curHeartList = new List<GameObject>();//当前爱心集合
    

    // Start is called before the first frame update
    void Start()
    {
        playerControl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        if (playerControl != null )
        {
            //订阅player生命值变化事件
            playerControl.OnhealthChanged += UpdateHearts;
            //爱心间隔
            heartSpacing = getHeartSpacing();

            //初始化爱心数量
            //这里需要用到player已经初始化了读取初始生命值才能正常显示，因此需要设定好先执行PlayerControl再执行HeartManager
            //在 编辑-项目设置-脚本执行顺序中添加脚本，值越小优先级越高越早执行
            UpdateHearts();

        }
        
        
    }

    /// <summary>
    /// 获取生命值游戏物体的间隔
    /// </summary>
    /// <returns></returns>
    private float getHeartSpacing()
    {
        //获取爱心的间隔  （预制体锚点设置为左上角，保证第一个坐标为(0,0)  间隔为图片长度*缩放）
        Image heartImg = heartPerfab.GetComponent<Image>();
        if (heartImg != null)
        {
            RectTransform rectTransform = heartImg.rectTransform;
            float width = rectTransform.rect.width;
            float scaleX = rectTransform.localScale.x;
            return width * scaleX;
        }
        return 0;
    }

    //更新爱心数量
    private void UpdateHearts()
    {
        int health = playerControl.getHealth();
        if (health > curHeartList.Count)
        {

            //爱心需要增加
            for (int i = curHeartList.Count; i < Mathf.Min(health, playerControl.MAX_HEALTH); i++)
            {
                Transform parentTransform = gameObject.transform;

                //往后增加爱心数量    父对象要设置为Canvas
                //相对于Canvas来生成爱心，相对坐标
                Vector2 heartPosition = new Vector2(firstHeartX + i * heartSpacing, firstHeartY);
                GameObject heartObj = Instantiate(heartPerfab, Vector3.zero, Quaternion.identity, parentTransform);
                RectTransform heartRectTransform = heartObj.GetComponent<RectTransform>();
                heartRectTransform.anchoredPosition = heartPosition;

                curHeartList.Add(heartObj);
            }
        } 
        else 
        {
            //爱心需要减少
            if(curHeartList.Count > 0)
            {
                for (int i = curHeartList.Count - 1; i >= health; i--)
                {
                    GameObject curHeart = curHeartList[i];
                    Destroy(curHeart);//删除当前爱心对象
                    curHeartList.RemoveAt(i);//从集合中删除当前爱心对象

                }

            }
            
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
