using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class player_ui : MonoBehaviour
{
    public GameObject[] ���͸�;
    public GameObject ������;
    public GameObject ������;
    public TMP_Text ���;

    public Transform ���t;
    public RectTransform �ش�rt;

    player.GameData ���������;

    bool �ش� = false, ��� = false;
    Coroutine �ڷ�ƾ = null;
    float �ش��̼� = 2, ����̼� = 0.2f;


    public void �����()
    {
        ���͸� = new GameObject[5];
        ���͸� = GameObject.FindGameObjectsWithTag("���͸�");
        ������ = GameObject.FindGameObjectWithTag("������");
        ���t = GameObject.FindGameObjectsWithTag("����")[0].GetComponent<Transform>();
        �ش�rt = GameObject.FindGameObjectsWithTag("����")[1].GetComponent<RectTransform>();
        ��� = GameObject.Find("���").GetComponent<TMP_Text>();

        Debug.Log("view : �� ����");
    }

    public void ������(player.GameData ���ӵ�����)
    {
        Debug.Log("[   �� ����   ]");
        //���ӵ����� ��
        ��������� = ���ӵ�����;

        ���͸�����();
        �����̻��º���();
        �ð�����();
        ��纯��();
    }

    void ���͸�����()
    {
        if (���͸� != null)
        {
            for (int i = 0; i < 5; i++)
                if (i < ���������.���͸�)
                    ���͸�[i].SetActive(true);
                else
                    ���͸�[i].SetActive(false);

            if (���������.���͸� > 3)
                for (int i = 0; i < ���͸�.Length; i++)
                    ���͸�[i].GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/battery/green");
            else if (���������.���͸� > 1)
                for (int i = 0; i < ���������.���͸�; i++)
                    ���͸�[i].GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/battery/yellow");
            else
                ���͸�[0].GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/battery/red");

            Debug.Log("- ���͸� ���� �Ϸ�");
        }
    }

    void �����̻��º���()
    {
        
        Animator anim = ������.GetComponent<Animator>();
        anim.SetBool(���������.�������, true);

        Debug.Log("- ������ ���� ���� �Ϸ�");
    }

    void �ð�����()
    {
        if (���������.���ð��� != ���������.���ð���)//�ð� ���� �ڷ�ƾ ����
        {
            //�ش�rt = �ؿʹ�.GetComponent<RectTransform>();

            if (���������.���ð��� == player.�ð���.��)
            {
                if (!�ش� && !���)
                {
                    if (�ڷ�ƾ != null)
                        StopCoroutine(�ڷ�ƾ);
                    �ڷ�ƾ = StartCoroutine(������());
                }
            }
            else if (���������.���ð��� == player.�ð���.��)
            {
                if (!�ش� && !���)
                {
                    if (�ڷ�ƾ != null)
                        StopCoroutine(�ڷ�ƾ);
                    StartCoroutine(������());
                }
            }
        }
        else //�ð��� �̹��� ��ġ ����
        {
            if (���������.���ð��� == player.�ð���.��)
            {
                float y = �ش�rt.rect.height / 4;
                �ش�rt.anchoredPosition = new Vector3(0, y, 0);

                int Y = (int)(���t.localScale.y * -10f);
                ���t.position = new Vector3(0, Y, -1);

                Debug.Log("- �ð� ���� �Ϸ�");
            }
            else
                Debug.Log("- �ð� ���� �Ϸ�");
        }


    }
    IEnumerator ������()
    {
        while (true)
        {
            //��� �̵�
            //�Ҽ� ��� ���� ������ int�� ��ȯ�ؼ� ���
            int �����ġ = (int)(���t.position.y * 10f);
            int ����ǥ = (int)(���t.localScale.y * 100f);

            if (�����ġ < ����ǥ)
                ���t.position = Vector2.MoveTowards(���t.position, new Vector2(0, ����ǥ/10), ����̼�);
            else ��� = true;

            //��&�� ����
            if (�ش�rt.anchoredPosition.y > (�ش�rt.rect.height / -4))
                �ش�rt.anchoredPosition = Vector2.MoveTowards(�ش�rt.anchoredPosition, new Vector2(0, (�ش�rt.rect.height / -4)), �ش��̼�);
            else �ش� = true;

            if (�ش� && ���)
            {
                �ش� = false;
                ��� = false;
                Debug.Log("- �ð� ���� �Ϸ�");
                yield break;
            }
            else
                yield return new WaitForSecondsRealtime(0.01f);
        }
    }
    IEnumerator ������()
    {
        while (true)
        {
            //��� �̵�
            //�Ҽ� ��� ���� ������ int�� ��ȯ�ؼ� ���
            int �����ġ = (int)(���t.position.y * 10f);
            int ����ǥ = (int)(���t.localScale.y * -100f);

            if (�����ġ > ����ǥ)
                ���t.position = Vector2.MoveTowards(���t.position, new Vector2(0, ����ǥ/10), ����̼�);
            else
                ��� = true;

            //��&�� ����
            if (�ش�rt.anchoredPosition.y < (�ش�rt.rect.height / 4))
                �ش�rt.anchoredPosition = Vector2.MoveTowards(�ش�rt.anchoredPosition, new Vector2(0, (�ش�rt.rect.height / 4)), �ش��̼�);
            else �ش� = true;

            if (�ش� && ���)
            {
                �ش� = false;
                ��� = false;
                Debug.Log("- �ð� ���� �Ϸ�");
                yield break;
            }
            else
                yield return new WaitForSecondsRealtime(0.01f);
        }
    }

    void ��纯��()
    {
        
        ���.text = ���������.���;

        Debug.Log("- ��� ���� �Ϸ�");
    }
}
