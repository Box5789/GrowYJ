using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_model
{

    enum 스탯num { 지식, 지지도, 아드레날린, 인기, 매력, 주머니사정 };

    GameObject 밥, 잠;
    GameObject[] 배터리오브젝트;

    [Header("- 스탯(지식 지지도 아드레날린 인기 매력 주머니사정)")]
    [SerializeField] private int[] 스탯 = new int[6];

    public player_model()
    {
        //데이터 셋팅
        

    }

    public void 이벤트발생()
    {

    }
}
