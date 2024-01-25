using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIcontroller : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject mainGame;
    public GameObject gameOver;
    public GameObject gameWin;
    public GameObject bg;
    public GameObject dialogue;

    // Start is called before the first frame update
    void Start()
    {
        StartMenu();
    }
//basically just a bunch of methods to set the different ui screens. based on known transitions i probably didn't need to setactive all of them but better safe than sorry 
    public void StartMenu()
    {
        mainMenu.SetActive(true);
        mainGame.SetActive(false);
        gameOver.SetActive(false);
        gameWin.SetActive(false);
        bg.SetActive(true);
        dialogue.SetActive(false);
    }
    public void Instruction()
    {
        mainMenu.SetActive(false);
        mainGame.SetActive(false);
        gameOver.SetActive(false);
        gameWin.SetActive(false);
        bg.SetActive(false);
        dialogue.SetActive(true);
    }
    public void GameMenu()
    {
        mainMenu.SetActive(false);
        mainGame.SetActive(true);
        gameOver.SetActive(false);
        gameWin.SetActive(false);
        bg.SetActive(false);
        dialogue.SetActive(false);
    }
    public void LoseMenu()
    {
        mainMenu.SetActive(false);
        mainGame.SetActive(false);
        gameOver.SetActive(true);
        gameWin.SetActive(false);
        bg.SetActive(true);
        dialogue.SetActive(false);
    }
    public void WinMenu()
    {
        mainMenu.SetActive(false);
        mainGame.SetActive(false);
        gameOver.SetActive(false);
        gameWin.SetActive(true);
        bg.SetActive(true);
        dialogue.SetActive(false);
    }
}
