using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestData : MonoBehaviour
{
    [Header("- 스탯")]
    public int 지식;
    public int 지지도;
    public int 아드레날린;
    public int 인기;
    public int 매력;
    public int 주머니사정;

    [Header("- 카운트")]
    public int 밥카운트;
    public int 잠카운트;

    [Header("부가스탯")]
    public int 배터리;
    public int 포만감;
    public int 피로도;

    [Header("시간")]
    public int 시;
    public int 분;
    public int 일;
    enum 시간 { 낮, 밤 };
    [SerializeField]시간 전시간대, 현시간대;

    [Header("상태 & 대사")]
    public string 이전상태;
    public string 현재상태;
    public string 대사;
    
    public void DataSet(player.GameData 데이터)
    {
        지식 = 데이터.스탯[0];
        지지도 = 데이터.스탯[1];
        아드레날린 = 데이터.스탯[2];
        인기 = 데이터.스탯[3];
        매력 = 데이터.스탯[4];
        주머니사정 = 데이터.스탯[5];

        밥카운트 = 데이터.밥카운트;
        잠카운트 = 데이터.잠카운트;

        배터리 = 데이터.배터리;
        포만감 = 데이터.포만감;
        피로도 = 데이터.피로도;

        시 = 데이터.시;
        분 = 데이터.분;
        일 = 데이터.일;

        전시간대 = (시간)데이터.전시간대;
        현시간대 = (시간)데이터.현시간대;

        이전상태 = 데이터.이전상태;
        현재상태 = 데이터.현재상태;
        대사 = 데이터.대사;
    }
}
