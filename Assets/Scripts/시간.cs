using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class 시간 : MonoBehaviour
{
    enum TL { 낮, 밤 };


    플레이어 영준이;
    GameObject 밥, 잠;


    public GameObject sun_moon;
    public GameObject background;
    Vector2 bg_max = new Vector2(0, 9.18f);
    Vector2 bg_min = new Vector2(0, -9.18f);


    const int 밥시간 = 60;
    const int 잠시간 = 480;


    [Header("- 시간")]
    [SerializeField] private int 시;
    [SerializeField] private int 분;
    [SerializeField] private int 일;
    [SerializeField]TL 시간대 = TL.낮;


    private void Awake()
    {
        영준이 = GameObject.FindGameObjectWithTag("영준이").GetComponent<플레이어>();
        밥 = GameObject.FindGameObjectsWithTag("이벤트버튼")[0];
        잠 = GameObject.FindGameObjectsWithTag("이벤트버튼")[1];
        밥.GetComponent<Button>().onClick.AddListener(delegate { 시간지남(밥시간); });
        잠.GetComponent<Button>().onClick.AddListener(delegate { 시간지남(잠시간); });
    }


    public void 이벤트발생(int m)
    {
        시간지남(m);

        시간대확인();

        시간검사();
    }

    void 시간지남(int m)
    {
        시 += (분 + m) / 60;
        if (시 >= 24)
        {
            일 += (시 + (분 + m) / 60) / 24;
            시 %= 24;
        }
        분 = (분 + m) % 60;
    }

    void 시간대확인()
    {
        if (시간대 == TL.낮 && 시 == 18)
        {
            시간대 = TL.밤;
            //밤으로 바뀜
        }
        else if (시간대 == TL.밤 && 시 == 6)
        {
            시간대 = TL.낮;
            //낮으로 바뀜
        }
    }

    void 시간검사()
    {
        //일주일 지났는지
        //엔딩봐야 하는지
    }

    private void Update()
    {
        //시간 흐름
        if((시간대 == TL.밤) && (background.transform.position.y > bg_min.y))//밤 >> 낮
        {
            //UI변경
            background.transform.position = Vector3.MoveTowards(background.transform.position, bg_min, 0.05f);
            RectTransform rt = sun_moon.GetComponent<RectTransform>();
            if (rt.anchoredPosition.y < (rt.rect.height / 4))
                rt.anchoredPosition = Vector2.MoveTowards(rt.anchoredPosition, new Vector2(0, (rt.rect.height / 4)), 2);

            //밥/잠 카운트 초기화
            영준이.밥카운트 = 0;
            영준이.잠카운트 = 0;
        }
        else if ((시간대 == TL.낮) && (background.transform.position.y < bg_max.y))//낮 >> 밤
        {
            //UI변경
            background.transform.position = Vector3.MoveTowards(background.transform.position, bg_max, 0.05f);
            RectTransform rt = sun_moon.GetComponent<RectTransform>();
            if (rt.anchoredPosition.y > (rt.rect.height / -4))
                rt.anchoredPosition = Vector2.MoveTowards(rt.anchoredPosition, new Vector2(0, (rt.rect.height / -4)), 2);
        }
    }

    public int Get시() { return 시; }
    public int Get분() { return 분; }
    public int Get일() { return 일; }

    public void SetTime(int h, int m, int d)
    {
        시 = h;
        분 = m;
        일 = d;

        시간대확인();
    }
}