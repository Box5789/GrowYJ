using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CutPlay : MonoBehaviour
{
    public Sprite[] ToonImages;
    public Image CutScene;
    public GameObject PopUp;
    public GameObject SkipBtn;
    private int CutNum;
    private int WholeNum;

    void Start()
    {
        ToonImages = Resources.LoadAll<Sprite>("Sprite/Cartoon");
        WholeNum = ToonImages.Length - 1;
        CutNum = 0;
        InvokeRepeating("ChangeCut", 0, 2);
        if (CutNum == WholeNum)
        {
            CancelInvoke("ChangeCut");
            PlayerPrefs.SetInt("See", 1);
            SceneManager.LoadScene("Play Scene");
        }

        if (PlayerPrefs.GetInt("See") == 1)
        {
            SkipBtn.SetActive(true);
        }
    }
    
    void ChangeCut(int Num)
    {
        CutScene.sprite = ToonImages[Num];
        CutNum++;
    }

    public void Skip()
    {
        PopUp.SetActive(true);
    }
    
    public void NoSkip()
    {
        PopUp.SetActive(false);
    }
    
    public void YesSkip()
    {
        SceneManager.LoadScene("Play Scene");
    }
    
    //실제론 안쓸것. PlayerPrefs 초기화 용도
    public void SetPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}
