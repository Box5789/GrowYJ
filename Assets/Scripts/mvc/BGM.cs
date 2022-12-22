using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BGM : MonoBehaviour
{
    public AudioMixer �ͼ�;
    public Slider �����̴�;
    public Button ���ҰŹ�ư;

    [SerializeField] private float ����;

    public static BGM instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += SoundInit;
    }


    public void SoundInit(Scene scene, LoadSceneMode mode)
    {
        GameObject �Ͻ������г� = GameObject.Find("Canvas").transform.Find("�Ͻ������г�").gameObject;
        �����̴� = �Ͻ������г�.transform.Find("Slider").gameObject.GetComponent<Slider>();
        //���ҰŹ�ư = �Ͻ������г�.transform.Find("���Ұ�").gameObject.GetComponent<Button>();

        �����̴�.onValueChanged.AddListener(delegate { AudioControl(); });
        //���ҰŹ�ư.onClick.AddListener(delegate { ���Ұ�(); });

        �����̴�.value = ����;
        �ͼ�.SetFloat("���", ����);
    }

    public void AudioControl()
    {
        if (�����̴�.value == �����̴�.minValue)
        {
            //���Ұ�();
            ���� = �����̴�.minValue;
            �ͼ�.SetFloat("���", -80);
            �����̴�.value = �����̴�.minValue;
        }
        else
        {
            ���� = �����̴�.value;
            �ͼ�.SetFloat("���", ����);
        }
    }

    public void ���Ұ�()
    {
        ���� = �����̴�.minValue;
        �ͼ�.SetFloat("���", -80);
        �����̴�.value = �����̴�.minValue;
    }
}
