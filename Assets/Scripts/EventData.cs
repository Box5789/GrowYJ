using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventData 
{ 
    string 이름;
    string 설명;
    string 대사;

    int 포만감;
    int 피로도;

    enum 스탯num { 지식, 지지도, 아드레날린, 인기, 매력, 주머니사정 };
    int[] 스탯 = new int[6];

    int 시간;

    public EventData(string 이름, string 설명, string 대사, int 포만감, int 피로도, int[] 스탯 , int 시간)
    {
        this.이름 = 이름;
        this.설명 = 설명;
        this.대사 = 대사;

        this.포만감 = 포만감;
        this.피로도 = 피로도;

        for (int i = 0; i < System.Enum.GetValues(typeof(스탯num)).Length; i++)
        {
            this.스탯[i] = 스탯[i];
        }

        this.시간 = 시간;
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
    public int[] Get스탯()
    {
        return 스탯;
    }
   
    public int Get시간()
    {
        return 시간;
    }
}
