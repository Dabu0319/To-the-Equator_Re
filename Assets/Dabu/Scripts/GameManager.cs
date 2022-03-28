using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameStatus
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

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        //StartCoroutine(EnterWind());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }
    }

    public IEnumerator EnterWind()
    {
        gameStatus = GameStatus.Wind;
        blizzard.SetActive(true);

        yield return new WaitForSeconds(5f);
        StartCoroutine(ExitWind());
    }

    public IEnumerator ExitWind()
    {
        gameStatus = GameStatus.Normal;
        blizzard.SetActive(false);

        yield return new WaitForSeconds(5f);
        StartCoroutine(EnterWind());
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
