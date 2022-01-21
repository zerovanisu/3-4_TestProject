using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [Header("フェードする画像")]
    [SerializeField]
    private Image FeadImage;

    [Header("フェードの速度")]
    [SerializeField]
    private float FeadSpeed;

    [Header("フェードの状態")]
    [SerializeField]
    private string Feading;

    [Header("遷移時間")]
    [SerializeField]
    private float Scene_Time;

    private float Collar_R, Collar_G, Collar_B, Coller_A, Scene_Count;

    bool Starting;

    void Start()
    {
        //RGBを一度保存する
        Collar_R = FeadImage.GetComponent<Image>().color.r;
        Collar_G = FeadImage.GetComponent<Image>().color.g;
        Collar_B = FeadImage.GetComponent<Image>().color.b;
        Coller_A = FeadImage.GetComponent<Image>().color.a;

        Scene_Count = Scene_Time;
        Feading = "フェードアウト";

        FeadImage.gameObject.SetActive(true);

        Sound_Manager.Instance.PlayBGM(BGM.Title,0.8f);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Button_A"))
        {
            Sound_Manager.Instance.PlaySE(SE.Select,1f,0f);
            Starting = true;
        }
    }

    private void FixedUpdate()
    {
        if(Starting == true)
        {
            Feading = "フェードイン";
            
            //フェードが終わったら時間差でシーン切り替え
            if(FeadImage.color.a >= 1)
            {
                Scene_Count -= Time.deltaTime;
            }
        }

        if(Scene_Count <= 0)
        {
            SceneManager.LoadScene("Stage1");
        }

        Fead();
    }
    
    //フェード処理
    void Fead()
    {
        //透明度が0より上で左クリックされた状態の時
        if (Coller_A > 0 && Feading == "フェードアウト")
        {
            Coller_A -= FeadSpeed;
        }
        //透明度が1より下で右クリックされた状態の時
        else if (Coller_A < 1 && Feading == "フェードイン")
        {
            Coller_A += FeadSpeed;
        }

        //色の反映
        FeadImage.GetComponent<Image>().color = new Color(Collar_R, Collar_G, Collar_B, Coller_A);
    }
}
