using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 이벤트데이터 : MonoBehaviour
{
    string 이름;
    string 설명;
    string 대사;

    int 포만감;
    int 피로도;
    int 매력;
    int 아드레날린;
    int 인기;
    int 주머니사정;
    int 지식;
    int 지지도;
    int 시간;
    int 합산;

    public 이벤트데이터(string 이름, string 설명, string 대사, int 포만감, int 피로도, int 매력, int 아드레날린, int 인기, int 주머니사정, int 지식, int 지지도, int 시간, int 합산)
    {
        this.이름 = 이름;
        this.설명 = 설명;
        this.대사 = 대사;
        this.포만감 = 포만감;
        this.피로도 = 피로도;
        this.매력 = 매력;
        this.아드레날린 = 아드레날린;
        this.인기 = 인기;
        this.주머니사정 = 주머니사정;
        this.지식 = 지식;
        this.지지도 = 지지도;
        this.시간 = 시간;
        this.합산 = 합산;
    }
    
    public string Get이름()
    {
        return 이름;
    }
    public string Get설명()
    {
        return 설명;
    }
    public string Get대사()
    {
        return 대사;
    }

    public int Get포만감()
    {
        return 포만감;
    }
    public int Get피로도()
    {
        return 피로도;
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
