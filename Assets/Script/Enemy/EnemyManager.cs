using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("�ő�HP")]
    [SerializeField]
    public int Enemy_HPMax;

    [Header("���݂�HP")]
    [SerializeField]
    public int Enemy_HP;

    [Header("�U����")]
    [SerializeField]
    public int Enemy_Attack;

    [Header("�h���")]
    [SerializeField]
    public int Enemy_Body;

    [Header("����")]
    [SerializeField]
    float Speed_E;

    public int DamagePoint;
    public bool Frieze;//��~�t���O

    Renderer Renderer_E;

    void Start()
    {
        Enemy_HP = Enemy_HPMax;
        Renderer_E = GetComponent<SpriteRenderer>();
        Frieze = true;//��ʓ��ɓ���܂œ����n�߂Ȃ��悤�ɂ���
    }

    void Update()
    {
        if(Renderer_E.isVisible)
        {
            Frieze = false;
        }

        //��{���̒��őS�Ă̏������s��(��~��Ԃ𔽉f�����邽��)
        if(Frieze == false)
        {
            //�҂���[��
            if (Enemy_HP <= 0)
            {
                Sound_Manager.Instance.PlaySE(SE.Deth, 0.2f, 0);
                Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //�v���C���[�ɓ���������_���[�W��^����
        if(collision.gameObject.tag == "Player")
        {
            GameObject Player = collision.gameObject;

            //�v���C���[�����G�łȂ��Ȃ�_���[�W��^����
            if(Player.GetComponent<PlayerManager>().Muteking == false)
            {
                Sound_Manager.Instance.PlaySE(SE.Damage_P,1,0);

                //����̃X�e�[�^�X���擾
                int P_Body = Player.GetComponent<PlayerManager>().Player_Body;

                //�Q�[���}�l�[�W���[�̎��Ń_���v���s��
                DamagePoint = GameDirector.Instance.Damage(DamagePoint, Enemy_Attack, 1, P_Body, 1);

                //�v�Z���ʂŏo���_���[�W������̗̑͂����
                Player.GetComponent<PlayerManager>().Player_HP -= DamagePoint;

                //�v���C���[�̔�e����
                Player.GetComponent<PlayerManager>().Damageing = true;
            }
        }
    }
}
