using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameDirector : MonoBehaviour
{
    public static GameDirector Instance;

    [Header("�v���C���[")]
    [SerializeField]
    public GameObject Player;

    [Header("�J�ڂ̑��x")]
    [SerializeField]
    float Change_TimeMax;

    [Header("�`���[�g���A��UI")]
    [SerializeField]
    GameObject Tutorial_UI;

    [Header("���߂��ׂ�UI")]
    [SerializeField]
    GameObject Gameover_UI;

    [Header("�N���AUI")]
    [SerializeField]
    GameObject Clear_UI;

    string Scene_Name;
    float Change_Time;
    bool Changeing, Sound_End;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        Scene_Name = SceneManager.GetActiveScene().name;

        switch (Scene_Name)
        {
            case "Stage1":
                Sound_Manager.Instance.PlayBGM(BGM.Stage_1,0.8f);
                break;
            case "Boss_Stage":
                Sound_Manager.Instance.PlayBGM(BGM.Boss,0.3f);
                break;
        }

        Tutorial_UI.SetActive(true);
        Gameover_UI.SetActive(false);
        Clear_UI.SetActive(false);

        Change_Time = Change_TimeMax;
        Changeing = Sound_End = false;
    }

    private void Update()
    {
        //�`���[�g���A���\����
        if(Tutorial_UI.activeSelf == true)
        {
            Player.GetComponent<PlayerManager>().Frieze = true;

            //�{�^���������ꂽ��Q�[���J�n
            if (Input.GetButtonDown("Button_A"))
            {
                Sound_Manager.Instance.PlaySE(SE.Select, 1, 0);

                Tutorial_UI.SetActive(false);

                Player.GetComponent<PlayerManager>().Frieze = false;
            }
        }

        if(Sound_End == false)
        {
            if(Player.GetComponent<PlayerManager>().Player_Clear == true)
            {
                Sound_Manager.Instance.PlayBGM(BGM.Clear,0.5f);
                Sound_End = true;
            }
            else if (Player.GetComponent<PlayerManager>().Player_Deth == true)
            {
                Sound_Manager.Instance.PlayBGM(BGM.GameOver, 0.5f);
                Sound_End = true;
            }
        }

        GameOver();
        GameClear();        
    }

    void GameOver()
    {
        //�Q�[���I�[�o�[���
        if (Player.GetComponent<PlayerManager>().Player_Deth == true)
        {
            Gameover_UI.SetActive(true);

            //�{�^���������ď����o������J��
            if (Input.GetButtonDown("Button_A") && Changeing == false)
            {
                Sound_Manager.Instance.PlaySE(SE.Select, 1, 0);
                Changeing = true;
            }
            if (Changeing == true)
            {
                Change_Time -= Time.deltaTime;
            }

            //���݂̃V�[���������[�h����
            if (Change_Time <= 0)
            {
                string Now_Scene = SceneManager.GetActiveScene().name;
                SceneManager.LoadScene(Now_Scene);
            }
        }
        else
        {
            Gameover_UI.SetActive(false);
        }
    }
    void GameClear()
    {
        //�Q�[���N���A���
        if (Player.GetComponent<PlayerManager>().Player_Clear == true)
        {
            Clear_UI.SetActive(true);

            //�{�^�����������班���҂�
            if (Input.GetButtonDown("Button_A") && Changeing == false)
            {
                Sound_Manager.Instance.PlaySE(SE.Select, 1, 0);
                Changeing = true;
            }
            if (Changeing == true)
            {
                Change_Time -= Time.deltaTime;
            }

            //�����҂�����J��
            if (Change_Time <= 0)
            {

                /*//���݂̃V�[�������擾
                Scene_Name = SceneManager.GetActiveScene().name;

                //���݂̃V�[���ɂ���Ď��ɐi�߂�V�[����ς���
                switch (Scene_Name)
                {
                    case "Stage1":
                        SceneManager.LoadScene("Boss_Stage");
                        break;

                    case "Boss_Stage":
                        SceneManager.LoadScene("Clear_Scene");
                        break;
                }*/

                //�^�C�g���ɖ߂�
                SceneManager.LoadScene("Title_Scene");
            }
        }
        else
        {
            Clear_UI.SetActive(false);
        }
    }

    //�_���[�W�v�Z(((�U����(+����) + (�Z�̕␳�l * 0.75) / 2 + 10)�@-�@((�h���(+����) * �␳�l) * 0.5))�����_�����؂�̂�)
    //�߂�l(�_���[�W,�U����,�U���␳�l,�h���,�h��␳�l)
    public int Damage(int DamagePoint, int Atc, float A_Skill, int Body, float B_Skill)
    {

        DamagePoint = Mathf.FloorToInt((Atc + (A_Skill * 0.75f) / 2 + 10) - ((Body * B_Skill) * 0.75f));

        Debug.Log(DamagePoint);

        return DamagePoint;
    }
}
