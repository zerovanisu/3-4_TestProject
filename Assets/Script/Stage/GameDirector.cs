using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
    public static GameDirector Instance;

    [Header("プレイヤー")]
    [SerializeField]
    public GameObject Player;

    [Header("チュートリアルUI")]
    [SerializeField]
    GameObject Tutorial_UI;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        Player.GetComponent<PlayerManager>().Frieze = true;
    }

    private void Update()
    {
        if(Tutorial_UI.activeSelf == true)
        {

        }

        if(Input.GetButtonDown("Button_A"))
        {
            Tutorial_UI.SetActive(false);

            Player.GetComponent<PlayerManager>().Frieze = false;
        }
    }

    //ダメージ計算(((攻撃力(+装備) + (技の補正値 * 0.75) / 2 + 10)　-　((防御力(+装備) * 補正値) * 0.5))小数点未満切り捨て)
    //戻り値(ダメージ,攻撃力,攻撃補正値,防御力,防御補正値)
    public int Damage(int DamagePoint, int Atc, float A_Skill, int Body, float B_Skill)
    {

        DamagePoint = Mathf.FloorToInt((Atc + (A_Skill * 0.75f) / 2 + 10) - ((Body * B_Skill) * 0.75f));

        Debug.Log(DamagePoint);

        return DamagePoint;
    }
}
