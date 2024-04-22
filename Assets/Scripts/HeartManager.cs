using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI�ж�����ֵ��ʾ�Ĺ���
/// </summary>
public class HeartManager : MonoBehaviour
{

    public GameObject heartPerfab;//����ֵ����Ԥ����
    public float firstHeartX;//��ʼ���Canvas��X����
    public float firstHeartY;//��ʼ���Canvas��Y����


    private PlayerControl playerControl;//player

    private float heartSpacing;//���ļ��
    private List<GameObject> curHeartList = new List<GameObject>();//��ǰ���ļ���
    

    // Start is called before the first frame update
    void Start()
    {
        playerControl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        if (playerControl != null )
        {
            //����player����ֵ�仯�¼�
            playerControl.OnhealthChanged += UpdateHearts;
            //���ļ��
            heartSpacing = getHeartSpacing();

            //��ʼ����������
            //������Ҫ�õ�player�Ѿ���ʼ���˶�ȡ��ʼ����ֵ����������ʾ�������Ҫ�趨����ִ��PlayerControl��ִ��HeartManager
            //�� �༭-��Ŀ����-�ű�ִ��˳������ӽű���ֵԽС���ȼ�Խ��Խ��ִ��
            UpdateHearts();

        }
        
        
    }

    /// <summary>
    /// ��ȡ����ֵ��Ϸ����ļ��
    /// </summary>
    /// <returns></returns>
    private float getHeartSpacing()
    {
        //��ȡ���ĵļ��  ��Ԥ����ê������Ϊ���Ͻǣ���֤��һ������Ϊ(0,0)  ���ΪͼƬ����*���ţ�
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

    //���°�������
    private void UpdateHearts()
    {
        int health = playerControl.getHealth();
        if (health > curHeartList.Count)
        {

            //������Ҫ����
            for (int i = curHeartList.Count; i < Mathf.Min(health, playerControl.MAX_HEALTH); i++)
            {
                Transform parentTransform = gameObject.transform;

                //�������Ӱ�������    ������Ҫ����ΪCanvas
                //�����Canvas�����ɰ��ģ��������
                Vector2 heartPosition = new Vector2(firstHeartX + i * heartSpacing, firstHeartY);
                GameObject heartObj = Instantiate(heartPerfab, Vector3.zero, Quaternion.identity, parentTransform);
                RectTransform heartRectTransform = heartObj.GetComponent<RectTransform>();
                heartRectTransform.anchoredPosition = heartPosition;

                curHeartList.Add(heartObj);
            }
        } 
        else 
        {
            //������Ҫ����
            if(curHeartList.Count > 0)
            {
                for (int i = curHeartList.Count - 1; i >= health; i--)
                {
                    GameObject curHeart = curHeartList[i];
                    Destroy(curHeart);//ɾ����ǰ���Ķ���
                    curHeartList.RemoveAt(i);//�Ӽ�����ɾ����ǰ���Ķ���

                }

            }
            
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
