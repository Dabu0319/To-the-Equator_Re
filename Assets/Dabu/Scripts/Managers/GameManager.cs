using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public  enum GameStatus
{
    Normal,
    Wind,
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameStatus gameStatus = GameStatus.Normal;


    public float windForce = 5.0f;

    public GameObject blizzard;
    public GameObject heavyWind;
    public GameObject normalWind;

    
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (gameStatus == GameStatus.Wind)
        {
            StartCoroutine(EnterWind(0f, 3f,3f));
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }
    }

    public IEnumerator EnterWind(float startDelay, float windTime, float windStopTime)
    {
        yield return new WaitForSeconds(startDelay);
        gameStatus = GameStatus.Wind;
        //blizzard.SetActive(true);
        normalWind.SetActive(false);
        heavyWind.SetActive(true);

        yield return new WaitForSeconds(windTime);
        StartCoroutine(ExitWind(startDelay,windTime,windStopTime));
    }

    public IEnumerator ExitWind(float startDelay, float windTime, float windStopTime)
    {
        gameStatus = GameStatus.Normal;
        //blizzard.SetActive(false);

        normalWind.SetActive(true);
        heavyWind.SetActive(false);
        
        yield return new WaitForSeconds(windStopTime);
        StartCoroutine(EnterWind(0,windTime,windStopTime));
    }

    public void StopWind()
    {
        StopAllCoroutines();
        gameStatus = GameStatus.Normal;
    }
    
    


    #region 关卡SceneManager相关

    
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    #endregion


}
