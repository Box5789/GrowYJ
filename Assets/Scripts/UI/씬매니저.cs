using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class 씬매니저 : MonoBehaviour
{
    public void 영준이키우기()
    {
        SceneManager.LoadScene("Play Scene");
    }

    public void 메인으로()
    {
        SceneManager.LoadScene("Start Scene");
    }

    public void 이벤트결과창()
    {
        //SceneManager.LoadScene("");
    }

    public void 일주일일기()
    {
        //SceneManager.LoadScene("");
    }
}
