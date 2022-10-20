using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class 플레이어 : MonoBehaviour
{
    enum 스탯num { 지식, 지지도, 아드레날린, 인기, 매력, 주머니사정 };
    
    GameObject 밥, 잠;
    GameObject[] 배터리오브젝트;
    Sprite[][] 영준이이미지;

    public int 밥카운트 = 0;
    public int 잠카운트 = 0;

    [SerializeField] const int EnergyMax = 10;
    [SerializeField] const int BatteryMax = 5;
    [SerializeField] const int StatMax = 100;
    [SerializeField] const int High = 70;
    [SerializeField] const int Low = 30;

    [Header("- 에너지")]
    [SerializeField] private int 포만감;
    [SerializeField] private int 피로도;
    [SerializeField] private int 배터리;

    [Header("- 스탯(지식 지지도 아드레날린 인기 매력 주머니사정)")]
    [SerializeField] private int[] 스탯 = new int[6];

    private void Awake()
    {
        //밥버튼, 잠버튼 셋팅
        밥 = GameObject.FindGameObjectsWithTag("이벤트버튼")[0];
        잠 = GameObject.FindGameObjectsWithTag("이벤트버튼")[1];
        밥.GetComponent<Button>().onClick.AddListener(delegate { 밥먹기(); });
        잠.GetComponent<Button>().onClick.AddListener(delegate { 잠자기(); });
        배터리오브젝트 = GameObject.FindGameObjectsWithTag("배터리");
        
        //영준이 스프라이트 Load

    }

    public void 밥먹기()
    {
        if(밥카운트 < 3)
        {
            밥카운트++;

            //포만감 조정
            배터리오브젝트[배터리++].SetActive(true);

            //화면효과
        }
        else
        {
            //밥 3번 이상 먹으면?
        }

    }

    public void 잠자기()
    {
        if(잠카운트 < 1)
        {
            //피로도 조정
            for (int i = 0; i < 3; i++)
                배터리오브젝트[배터리 + i].SetActive(true);
            배터리 += 3;

            //화면 효과
        }
        else
        {
            //잠 한번 더 자면?
        }
    }

    public void 이벤트발생(EventData 발생이벤트)
    {
        //스탯 조정
        배터리오브젝트[배터리 - 1].SetActive(false);
        배터리--;
        포만감 += 발생이벤트.Get포만감();
        피로도 += 발생이벤트.Get피로도();
        for (int i = 0; i < 스탯.Length; i++)
            스탯[i] += 발생이벤트.Get스탯()[i];


        //스탯 높낮 확인
        int min = 0, max = 0;
        for (int i = 0; i < 스탯.Length; i++)
        {
            if (스탯[i] >= High && 스탯[i] >= 스탯[max])
                max = i;
            if (스탯[i] <= Low && 스탯[i] <= 스탯[min])
                min = i;
        }

        /*높낮 스탯 있다면 스프라이트 변경 - 코드 수정
        int index = min;
        if (!(min == 0 && max == 0)) {
            if ((100 - 스탯[max]) > 스탯[min]) index = max;
            GetComponent<SpriteRenderer>().sprite = 영준이이미지[min][index];
        }
        */
    }

    
    public int Get포만감() { return 포만감; }
    public int Get피로도() { return 피로도; }
    public int Get배터리() { return 배터리; }
    public int[] Get스탯() { return 스탯; }

    public void Set포만감(int i) { 포만감 = i; }
    public void Set피로도(int i) { 피로도 = i; }
    public void Set배터리(int i) { 배터리 = i; }
    public void Set스탯(int[] i) { 스탯 = i; }
}
