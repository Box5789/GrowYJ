using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class 스탯 : MonoBehaviour
{
    const int EnergyMax = 10;
    const int BatteryMax = 5;
    const int StatMax = 100;

    [Header("- 버튼")]
    [SerializeField] private GameObject 공부버튼;
    [SerializeField] private GameObject 여가버튼;
    [SerializeField] private GameObject 사랑버튼;
    [SerializeField] private GameObject 돈버튼;

    [Header("- 에너지")]
    [SerializeField] private int 포만감;
    [SerializeField] private int 피로도;
    [SerializeField] private int 배터리;

    [Header("- 스탯")]
    [SerializeField] private int 지식;
    [SerializeField] private int 지지도;
    [SerializeField] private int 아드레날린;
    [SerializeField] private int 인기;
    [SerializeField] private int 매력;
    [SerializeField] private int 주머니사정;



    // Start is called before the first frame update
    void Start()
    {
        공부버튼.GetComponent<Button>().onClick.AddListener(delegate { 공부(); });
        여가버튼.GetComponent<Button>().onClick.AddListener(delegate { 여가(); });
        사랑버튼.GetComponent<Button>().onClick.AddListener(delegate { 사랑(); });
        돈버튼.GetComponent<Button>().onClick.AddListener(delegate { 돈(); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void 공부()
    {
        지식 += 10;
        지지도 += 10;
    }

    public void 여가()
    {
        아드레날린 += 10;
        인기 += 10;
    }

    public void 사랑()
    {
        인기 += 10;
        매력 += 10;
    }

    public void 돈()
    {
        주머니사정 += 10;
        지지도 += 10;
    }
}
