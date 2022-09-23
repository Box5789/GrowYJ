using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class 스탯관리 : MonoBehaviour
{
    const int EnergyMax = 10;
    const int BatteryMax = 5;
    const int StatMax = 100;

    [Header("- 시간관리")]
    [SerializeField] private int 시;
    [SerializeField] private int 분;
    [SerializeField] private int 날짜;

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

    public void 이벤트발생(int 포만감, int 피로도, int 지식, int 지지도, int 아드레날린, int 인기, int 매력, int 주머니사정)
    {
        this.포만감 += 포만감;
        this.피로도 += 피로도;
        배터리--;
        this.지식 += 지식;
        this.지지도 += 지지도;
        this.아드레날린 += 아드레날린;
        this.인기 += 인기;
        this.매력 += 매력;
        this.주머니사정 += 주머니사정;
    }
    
    public int Get포만감()
    {
        return 포만감;
    }
    public int Get피로도()
    {
        return 피로도;
    }
    public int Get배터리()
    {
        return 배터리;
    }
    public int Get지식()
    {
        return 지식;
    }
    public int Get지지도()
    {
        return 지지도;
    }
    public int Get아드레날린()
    {
        return 아드레날린;
    }
    public int Get인기()
    {
        return 인기;
    }
    public int Get매력()
    {
        return 매력;
    }
    public int Get주머니사정()
    {
        return 주머니사정;
    }


}
