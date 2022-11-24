using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class 플레이어 : MonoBehaviour
{
    enum 스탯num { 지식, 지지도, 아드레날린, 인기, 매력, 주머니사정 };
    
    GameObject 밥, 잠;
    GameObject[] 배터리오브젝트;

    [Header("- 스탯 기준 상수")]
    [SerializeField] int EnergyMax = 10;
    [SerializeField] int BatteryMax = 5;
    [SerializeField] int StatMax = 100;
    [SerializeField] int High = 70;
    [SerializeField] int Low = 30;

    [Header("- 에너지")]
    public int 밥카운트 = 0;
    public int 잠카운트 = 0;
    [SerializeField] private int 배터리;
    [SerializeField] private int 포만감;
    [SerializeField] private int 피로도;

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
        for (int i = 0; i < 배터리오브젝트.Length; i++)
            배터리오브젝트[i].SetActive(false);
    }

    private void Start()
    {
        //배터리 셋팅
        for (int i = 0; i < 배터리; i++)
            배터리오브젝트[i].SetActive(true);
        배터리색변경();
    }

    void 배터리색변경()
    {
        if (배터리 > 3)
            for (int i = 0; i < 배터리오브젝트.Length; i++)
                배터리오브젝트[i].GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/battery/green");
        else if (배터리 > 1)
            for (int i = 0; i < 배터리; i++)
                배터리오브젝트[i].GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/battery/yellow");
        else
            배터리오브젝트[0].GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/battery/red");
    }

    public void 밥먹기()
    {
        밥카운트++;
        배터리오브젝트[배터리++].SetActive(true);
        배터리색변경();
        
        //밥 횟수 제한 > 나중에
    }

    public void 잠자기()
    {
        잠카운트++;
        for (int i = 0; i < 3; i++)
            if (배터리 == 5)
                break;
            else
                배터리오브젝트[배터리++].SetActive(true);
        배터리색변경();

        //잠 횟수 제한 > 나중에
    }

    public void 이벤트발생(EventData 발생이벤트)
    {
        //스탯 조정
        배터리오브젝트[배터리 - 1].SetActive(false);
        배터리--;
        배터리색변경();
        포만감 += 발생이벤트.Get포만감();
        피로도 += 발생이벤트.Get피로도();

        for (int i = 0; i < 스탯.Length; i++)
            스탯[i] += 발생이벤트.Get스탯()[i];
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
