using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [Header("�t�F�[�h����摜")]
    [SerializeField]
    private Image FeadImage;

    [Header("�t�F�[�h�̑��x")]
    [SerializeField]
    private float FeadSpeed;

    [Header("�t�F�[�h�̏��")]
    [SerializeField]
    private string Feading;

    [Header("�J�ڎ���")]
    [SerializeField]
    private float Scene_Time;

    private float Collar_R, Collar_G, Collar_B, Coller_A, Scene_Count;

    bool Starting;

    void Start()
    {
        //RGB����x�ۑ�����
        Collar_R = FeadImage.GetComponent<Image>().color.r;
        Collar_G = FeadImage.GetComponent<Image>().color.g;
        Collar_B = FeadImage.GetComponent<Image>().color.b;
        Coller_A = FeadImage.GetComponent<Image>().color.a;

        Scene_Count = Scene_Time;
        Feading = "�t�F�[�h�A�E�g";

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
            Feading = "�t�F�[�h�C��";
            
            //�t�F�[�h���I������玞�ԍ��ŃV�[���؂�ւ�
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
    
    //�t�F�[�h����
    void Fead()
    {
        //�����x��0����ō��N���b�N���ꂽ��Ԃ̎�
        if (Coller_A > 0 && Feading == "�t�F�[�h�A�E�g")
        {
            Coller_A -= FeadSpeed;
        }
        //�����x��1��艺�ŉE�N���b�N���ꂽ��Ԃ̎�
        else if (Coller_A < 1 && Feading == "�t�F�[�h�C��")
        {
            Coller_A += FeadSpeed;
        }

        //�F�̔��f
        FeadImage.GetComponent<Image>().color = new Color(Collar_R, Collar_G, Collar_B, Coller_A);
    }
}
