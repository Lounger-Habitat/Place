using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginSceneSetting : MonoBehaviour
{
    [Header("新春模式 New!")]
    public bool newSpring=true;
    [Header("创作模式")]
    public bool create=true;
    [Header("涂鸦模式")]
    public bool graffiti=true;
    [Header("竞赛模式")]
    public bool competition=true;


    public GameObject newSpringObj;
    public GameObject createObj;
    public GameObject graffitiObj;
    public GameObject competitionObj;
    // Start is called before the first frame update
    void Start()
    {
        newSpringObj.SetActive(newSpring);
        createObj.SetActive(create);
        graffitiObj.SetActive(graffiti);
        competitionObj.SetActive(competition);
    }

  
}
