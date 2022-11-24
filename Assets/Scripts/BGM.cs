using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BGM : MonoBehaviour
{
    public AudioMixer 믹서;
    public Slider 슬라이더;
    public Button 음소거버튼;

    [SerializeField] private float 볼륨;

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
        GameObject 일시정지패널 = GameObject.Find("Canvas").transform.Find("일시정지패널").gameObject;
        슬라이더 = 일시정지패널.transform.Find("Slider").gameObject.GetComponent<Slider>();
        //음소거버튼 = 일시정지패널.transform.Find("음소거").gameObject.GetComponent<Button>();

        슬라이더.onValueChanged.AddListener(delegate { AudioControl(); });
        //음소거버튼.onClick.AddListener(delegate { 음소거(); });

        슬라이더.value = 볼륨;
        믹서.SetFloat("브금", 볼륨);
    }

    public void AudioControl()
    {
        if (슬라이더.value == 슬라이더.minValue)
        {
            //음소거();
            볼륨 = 슬라이더.minValue;
            믹서.SetFloat("브금", -80);
            슬라이더.value = 슬라이더.minValue;
        }
        else
        {
            볼륨 = 슬라이더.value;
            믹서.SetFloat("브금", 볼륨);
        }
    }

    public void 음소거()
    {
        볼륨 = 슬라이더.minValue;
        믹서.SetFloat("브금", -80);
        슬라이더.value = 슬라이더.minValue;
    }
}
