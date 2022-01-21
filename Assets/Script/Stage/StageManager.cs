using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    [Header("�J�ڂ̑��x")]
    [SerializeField]
    float Scene_Speed;

    [Header("�v���C���[")]
    [SerializeField]
    GameObject Player;

    string Scene_Name;

    bool Scene_Change, Changeing;

    void Start()
    {
        
    }

    void Update()
    {
        //�S�[����{�^���������ꂽ��J�ڊJ�n
        if(Input.GetButtonDown("Button_A") && Scene_Change == true)
        {
            Changeing = true;
        }

        //�J�ڏ���
        if(Changeing == true)
        {
            //���݂̃V�[���ɂ���Ď��ɐi�߂�V�[����ς���
            switch (Scene_Name)
            {
                case "Stage1":
                    SceneManager.LoadScene("Boss_Stage");
                    break;

                case "Boss_Stage":
                    SceneManager.LoadScene("Clear_Scene");
                    break;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //�v���C���[�ɃN���A��`����
            Player.GetComponent<PlayerManager>().Player_Clear = true;


            //���݂̃V�[�������擾
            Scene_Name = SceneManager.GetActiveScene().name;

            Scene_Change = true;
        }

    }
}
