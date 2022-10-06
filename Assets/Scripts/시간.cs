using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class 시간 : MonoBehaviour
{
    public GameObject sun_moon;
    public GameObject background;
    Vector2 bg_max = new Vector2(0, 9.18f);
    Vector2 bg_min = new Vector2(0, -9.18f);
    

    [Header("- 시간")]
    [SerializeField] private int 시;
    [SerializeField] private int 분;
    [SerializeField] private int 일;

    enum TL { 낮, 밤};
    [SerializeField]TL 시간대 = TL.낮;

    public void 이벤트발생(int m)
    {
        시 += (분 + m) / 60;
        if(시 >= 24)
        {
            일 += (시 + (분 + m) / 60) / 24;
            시 %= 24;
        }
        분 = (분 + m) % 60;

        if(시간대 == TL.낮 && 시 == 18)
        {
            시간대 = TL.밤;
            //밤으로 바뀜
        }
        else if(시간대 == TL.밤 && 시 == 6 )
        {
            시간대 = TL.낮;
            //낮으로 바뀜
        }

        시간검사();
    }

    private void Update()
    {
        //시간 흐름에 따라 UI 바꾸기
        if((시간대 == TL.밤) && (background.transform.position.y > bg_min.y))
        {
            background.transform.position = Vector3.MoveTowards(background.transform.position, bg_min, 0.05f);

            RectTransform rt = sun_moon.GetComponent<RectTransform>();
            if (rt.anchoredPosition.y < (rt.rect.height / 4))
                rt.anchoredPosition = Vector2.MoveTowards(rt.anchoredPosition, new Vector2(0, (rt.rect.height / 4)), 2);
        }
        else if ((시간대 == TL.낮) && (background.transform.position.y < bg_max.y))
        {
            background.transform.position = Vector3.MoveTowards(background.transform.position, bg_max, 0.05f);

            RectTransform rt = sun_moon.GetComponent<RectTransform>();
            if (rt.anchoredPosition.y > (rt.rect.height / -4))
                rt.anchoredPosition = Vector2.MoveTowards(rt.anchoredPosition, new Vector2(0, (rt.rect.height / -4)), 2);
        }
    }

    void 시간검사()
    {
        //일주일 지났는지
        //엔딩봐야 하는지
    }
}