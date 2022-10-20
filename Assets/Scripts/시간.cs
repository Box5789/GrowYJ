using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class 시간 : MonoBehaviour
{
    enum TL { 낮, 밤 };

    플레이어 영준이;
    GameObject 밥, 잠;
    RectTransform 배경rt;
    bool 낮으로 = false, 밤으로 = false, 배경 = false, 해_달 = false;

    
    [Header("- 상수")]
    [SerializeField] private int 밥시간 = 60;
    [SerializeField] private int 잠시간 = 480;

    [Header("- 오브젝트")]
    [SerializeField] private GameObject 시간대이미지;
    [SerializeField] private GameObject 배경이미지;
    [SerializeField] private float 해_달이동속도;
    [SerializeField] private float 배경이동속도;

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

        밥.GetComponent<Button>().onClick.AddListener(delegate { 이벤트발생(밥시간); });
        잠.GetComponent<Button>().onClick.AddListener(delegate { 이벤트발생(잠시간); });

        배경rt = 배경이미지.GetComponent<RectTransform>();
    }


    public void 이벤트발생(int m)
    {
        시간지남(m);

        시간대확인();

        GetComponent<DataManager>().gameData.SaveTime(시, 분, 일);

        시간검사();

    }

    void 시간지남(int m)//분단위
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
        if (시간대 == TL.낮 && 시 >= 18)
        {
            시간대 = TL.밤;
            밤으로 = true;
            //밤으로 코루틴
        }
        else if (시간대 == TL.밤 && (시 - 6) % 24 >= 12)
        {
            시간대 = TL.낮;
            낮으로 = true;
            //낮으로 코루친
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
        if(낮으로)//밤 >> 낮
        {
            //밥/잠 카운트 초기화 >> 언제?
            영준이.밥카운트 = 0;
            영준이.잠카운트 = 0;

            //배경이미지 이동
            if (배경rt.anchoredPosition.y < (배경rt.rect.height / 3))
                배경rt.anchoredPosition = Vector2.MoveTowards(배경rt.anchoredPosition, new Vector2(0, (배경rt.rect.height / 3)), Time.deltaTime * 배경이동속도);
            else 배경 = true;

            //해&달 이미지 이동
            RectTransform 해rt = 시간대이미지.GetComponent<RectTransform>();
            if (해rt.anchoredPosition.y > (해rt.rect.height / -4))
                해rt.anchoredPosition = Vector2.MoveTowards(해rt.anchoredPosition, new Vector2(0, (해rt.rect.height / -4)), Time.deltaTime * 해_달이동속도);
            else 해_달 = true;

            if (배경 && 해_달) 
            {
                낮으로 = false;
                배경 = false;
                해_달 = false;
            }
        }
        else if (밤으로)//낮 >> 밤
        {
            //배경이미지 이동
            if (배경rt.anchoredPosition.y > (배경rt.rect.height / -3))
                배경rt.anchoredPosition = Vector2.MoveTowards(배경rt.anchoredPosition, new Vector2(0, (배경rt.rect.height / -3)), Time.deltaTime * 배경이동속도);
            else 배경 = true;

            //해&달 이미지 이동
            RectTransform 달rt = 시간대이미지.GetComponent<RectTransform>();
            if (달rt.anchoredPosition.y < (달rt.rect.height / 4))
                달rt.anchoredPosition = Vector2.MoveTowards(달rt.anchoredPosition, new Vector2(0, (달rt.rect.height / 4)), Time.deltaTime * 해_달이동속도);
            else 해_달 = true;

            if (배경 && 해_달)
            {
                밤으로 = false;
                배경 = false;
                해_달 = false;
            }
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