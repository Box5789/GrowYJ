using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EventScene : MonoBehaviour
{
    [HideInInspector] public GameObject ������;
    [HideInInspector] public TMP_Text ��¥, ���;
    [HideInInspector] public GameObject ���ư���;
    [HideInInspector] public GameObject ��������;

    private void Awake()
    {
        ������ = GameObject.FindGameObjectWithTag("������");
        ��¥ = GameObject.Find("��¥").GetComponent<TMP_Text>();
        ��� = GameObject.Find("����").GetComponent<TMP_Text>();
        ���ư��� = GameObject.Find("Canvas").transform.Find("���ư���").gameObject;
        �������� = GameObject.Find("Canvas").transform.Find("��������").gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        ���ư���.GetComponent<Button>().onClick.AddListener(delegate { SceneManager.LoadScene("Play Scene"); });
        ��������.GetComponent<Button>().onClick.AddListener(delegate { SceneManager.LoadScene("Main Scene"); });

        if(GameManager.Instance.deathKey!= null)
        {
            ��¥.gameObject.SetActive(false);

            if(GameManager.Instance.���������.TryGetValue(GameManager.Instance.deathKey, out GameManager.������ value))
            {
                string text = value.���;
                //������ sprite �߰�
                StartCoroutine(Ÿ�����ڷ�ƾ(���, text, 0.1f));

                GameManager.Instance.����������߰�();
            }
            
        }
        else if(GameManager.Instance.endingKey != null)
        {
            ��¥.gameObject.SetActive(false);

            if (GameManager.Instance.����������.TryGetValue(GameManager.Instance.endingKey, out GameManager.������ value))
            {
                string text = value.���;
                //������ sprite �߰�
                StartCoroutine(Ÿ�����ڷ�ƾ(���, text, 0.1f));

                GameManager.Instance.�����������߰�();

            }
        }
        else
        {
            ��¥.text = GameManager.Instance.day.ToString() + " ����";
            //������.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("������/Sprite/" + GameManager.Instance.result_state);
            StartCoroutine(Ÿ�����ڷ�ƾ(���, GameManager.Instance.result_text, 0.1f));
        }
        
    }

    IEnumerator Ÿ�����ڷ�ƾ(TMP_Text text, string message, float speed)
    {
        for (int i = 0; i < message.Length; i++)
        {
            text.text = message.Substring(0, i + 1);
            if (i == message.Length - 1)
            {
                if (GameManager.Instance.endingKey != null || GameManager.Instance.deathKey != null)
                {
                    ��������.SetActive(true);
                }
                else
                {
                    ���ư���.SetActive(true);
                }
                yield return null;
            }
            yield return new WaitForSeconds(speed);
        }

    }
}
