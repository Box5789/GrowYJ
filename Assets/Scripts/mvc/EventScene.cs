using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EventScene : MonoBehaviour
{
    [HideInInspector] public GameObject 영준이;
    [HideInInspector] public TMP_Text 날짜, 결과;
    [HideInInspector] public GameObject 돌아가기;
    [HideInInspector] public GameObject 메인으로;

    private void Awake()
    {
        영준이 = GameObject.FindGameObjectWithTag("영준이");
        날짜 = GameObject.Find("날짜").GetComponent<TMP_Text>();
        결과 = GameObject.Find("문구").GetComponent<TMP_Text>();
        돌아가기 = GameObject.Find("Canvas").transform.Find("돌아가기").gameObject;
        메인으로 = GameObject.Find("Canvas").transform.Find("메인으로").gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        돌아가기.GetComponent<Button>().onClick.AddListener(delegate { SceneManager.LoadScene("Play Scene"); });
        메인으로.GetComponent<Button>().onClick.AddListener(delegate { SceneManager.LoadScene("Main Scene"); });

        if(GameManager.Instance.deathKey!= null)
        {
            날짜.gameObject.SetActive(false);

            if(GameManager.Instance.사망데이터.TryGetValue(GameManager.Instance.deathKey, out GameManager.데이터 value))
            {
                string text = value.대사;
                //영준이 sprite 추가
                StartCoroutine(타이핑코루틴(결과, text, 0.1f));

                GameManager.Instance.사망모음집추가();
            }
            
        }
        else if(GameManager.Instance.endingKey != null)
        {
            날짜.gameObject.SetActive(false);

            if (GameManager.Instance.엔딩데이터.TryGetValue(GameManager.Instance.endingKey, out GameManager.데이터 value))
            {
                string text = value.대사;
                //영준이 sprite 추가
                StartCoroutine(타이핑코루틴(결과, text, 0.1f));

                GameManager.Instance.엔딩모음집추가();

            }
        }
        else
        {
            날짜.text = GameManager.Instance.day.ToString() + " 일차";
            //영준이.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("영준이/Sprite/" + GameManager.Instance.result_state);
            StartCoroutine(타이핑코루틴(결과, GameManager.Instance.result_text, 0.1f));
        }
        
    }

    IEnumerator 타이핑코루틴(TMP_Text text, string message, float speed)
    {
        for (int i = 0; i < message.Length; i++)
        {
            text.text = message.Substring(0, i + 1);
            if (i == message.Length - 1)
            {
                if (GameManager.Instance.endingKey != null || GameManager.Instance.deathKey != null)
                {
                    메인으로.SetActive(true);
                }
                else
                {
                    돌아가기.SetActive(true);
                }
                yield return null;
            }
            yield return new WaitForSeconds(speed);
        }

    }
}
