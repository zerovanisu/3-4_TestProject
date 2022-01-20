using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Sound_Manager.Instance.PlayBGM(BGM.Boss);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
