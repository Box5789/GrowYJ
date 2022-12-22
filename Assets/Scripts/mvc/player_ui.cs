using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class player_ui : MonoBehaviour
{
    public GameObject[] 배터리;
    public GameObject 영준이;
    public GameObject 낮밤배경;
    public TMP_Text 대사;

    public Transform 배경t;
    public RectTransform 해달rt;

    player.GameData 결과데이터;

    bool 해달 = false, 배경 = false;
    Coroutine 코루틴 = null;
    float 해달이속 = 2, 배경이속 = 0.2f;


    public void 뷰셋팅()
    {
        배터리 = new GameObject[5];
        배터리 = GameObject.FindGameObjectsWithTag("배터리");
        영준이 = GameObject.FindGameObjectWithTag("영준이");
        배경t = GameObject.FindGameObjectsWithTag("낮밤")[0].GetComponent<Transform>();
        해달rt = GameObject.FindGameObjectsWithTag("낮밤")[1].GetComponent<RectTransform>();
        대사 = GameObject.Find("대사").GetComponent<TMP_Text>();

        Debug.Log("view : 뷰 셋팅");
    }

    public void 뷰조정(player.GameData 게임데이터)
    {
        Debug.Log("[   뷰 조정   ]");
        //게임데이터 셋
        결과데이터 = 게임데이터;

        배터리변경();
        영준이상태변경();
        시간변경();
        대사변경();
    }

    void 배터리변경()
    {
        if (배터리 != null)
        {
            for (int i = 0; i < 5; i++)
                if (i < 결과데이터.배터리)
                    배터리[i].SetActive(true);
                else
                    배터리[i].SetActive(false);

            if (결과데이터.배터리 > 3)
                for (int i = 0; i < 배터리.Length; i++)
                    배터리[i].GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/battery/green");
            else if (결과데이터.배터리 > 1)
                for (int i = 0; i < 결과데이터.배터리; i++)
                    배터리[i].GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/battery/yellow");
            else
                배터리[0].GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/battery/red");

            Debug.Log("- 배터리 변경 완료");
        }
    }

    void 영준이상태변경()
    {
        
        Animator anim = 영준이.GetComponent<Animator>();
        anim.SetBool(결과데이터.현재상태, true);

        Debug.Log("- 영준이 상태 변경 완료");
    }

    void 시간변경()
    {
        if (결과데이터.전시간대 != 결과데이터.현시간대)//시간 변경 코루틴 실행
        {
            //해달rt = 해와달.GetComponent<RectTransform>();

            if (결과데이터.현시간대 == player.시간대.밤)
            {
                if (!해달 && !배경)
                {
                    if (코루틴 != null)
                        StopCoroutine(코루틴);
                    코루틴 = StartCoroutine(밤으로());
                }
            }
            else if (결과데이터.현시간대 == player.시간대.낮)
            {
                if (!해달 && !배경)
                {
                    if (코루틴 != null)
                        StopCoroutine(코루틴);
                    StartCoroutine(낮으로());
                }
            }
        }
        else //시간대 이미지 위치 조정
        {
            if (결과데이터.현시간대 == player.시간대.밤)
            {
                float y = 해달rt.rect.height / 4;
                해달rt.anchoredPosition = new Vector3(0, y, 0);

                int Y = (int)(배경t.localScale.y * -10f);
                배경t.position = new Vector3(0, Y, -1);

                Debug.Log("- 시간 변경 완료");
            }
            else
                Debug.Log("- 시간 변경 완료");
        }


    }
    IEnumerator 낮으로()
    {
        while (true)
        {
            //배경 이동
            //소수 계산 오류 때문에 int로 변환해서 사용
            int 배경위치 = (int)(배경t.position.y * 10f);
            int 배경목표 = (int)(배경t.localScale.y * 100f);

            if (배경위치 < 배경목표)
                배경t.position = Vector2.MoveTowards(배경t.position, new Vector2(0, 배경목표/10), 배경이속);
            else 배경 = true;

            //해&달 변경
            if (해달rt.anchoredPosition.y > (해달rt.rect.height / -4))
                해달rt.anchoredPosition = Vector2.MoveTowards(해달rt.anchoredPosition, new Vector2(0, (해달rt.rect.height / -4)), 해달이속);
            else 해달 = true;

            if (해달 && 배경)
            {
                해달 = false;
                배경 = false;
                Debug.Log("- 시간 변경 완료");
                yield break;
            }
            else
                yield return new WaitForSecondsRealtime(0.01f);
        }
    }
    IEnumerator 밤으로()
    {
        while (true)
        {
            //배경 이동
            //소수 계산 오류 때문에 int로 변환해서 사용
            int 배경위치 = (int)(배경t.position.y * 10f);
            int 배경목표 = (int)(배경t.localScale.y * -100f);

            if (배경위치 > 배경목표)
                배경t.position = Vector2.MoveTowards(배경t.position, new Vector2(0, 배경목표/10), 배경이속);
            else
                배경 = true;

            //해&달 변경
            if (해달rt.anchoredPosition.y < (해달rt.rect.height / 4))
                해달rt.anchoredPosition = Vector2.MoveTowards(해달rt.anchoredPosition, new Vector2(0, (해달rt.rect.height / 4)), 해달이속);
            else 해달 = true;

            if (해달 && 배경)
            {
                해달 = false;
                배경 = false;
                Debug.Log("- 시간 변경 완료");
                yield break;
            }
            else
                yield return new WaitForSecondsRealtime(0.01f);
        }
    }

    void 대사변경()
    {
        
        대사.text = 결과데이터.대사;

        Debug.Log("- 대사 변경 완료");
    }
}
