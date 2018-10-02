using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    // Use this for initialization
    [SerializeField]
    private Text msgGameOver;
    [SerializeField]
    private Text msgSpace;
    [SerializeField]
    private GameObject block;
    [SerializeField]
    private AudioClip audioWin;
    [SerializeField]
    private AudioClip audioGameOver;

    public static int w = 10; //largura
    public static int h = 13; //altura

    public static Element[,] elements = new Element[w, h]; // todos os blocos que possuo
    public static string GameState; //Stop, Play, Game Over, Win
    private AudioSource playSound;
    void Start () {
        this.CreateCamp();
        GameController.GameState = "Stop";
        playSound = gameObject.GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        if (GameController.GameState == "Game Over")
        {
            msgGameOver.gameObject.active = true;
            msgGameOver.text = "Game Over!!!";
            GameController.GameState = "Stop";
            playSound.clip = audioGameOver;
            playSound.Play();
            print("Game Over!!!");
        }
        if (GameController.GameState == "Win")
        {
            msgGameOver.gameObject.active = true;
            msgGameOver.text = "Win!!!";
            GameController.GameState = "Stop";
            playSound.clip = audioWin;
            playSound.Play();
            print("Win!!!");
        }
        if (GameController.GameState == "Stop")
        {
            msgSpace.gameObject.active = true;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartGame();
            }
        }
    }
    private void StartGame()
    {
        msgSpace.gameObject.active = false;
        msgGameOver.gameObject.active = false;
        foreach (Element item in elements)
        {
            item.RestartDefault();
        }

        GameController.GameState = "Play";
    }
    //cria o campo minado
    private void CreateCamp()
    {
        for (int i = 0; i < w; i++)
        {

            for (int j = 0; j < h; j++)
            {
                Instantiate(block, new Vector3(i, j, 0f), Quaternion.identity);
            }
        }
    }
    //exibir todas as minas quando ocorrer o game over
    public static void UncoverMines()
    {
        foreach (Element item in elements)
        {
            if(item.IsMine())
            {
                item.LoadTexture(0);
            }
        }
    }
    //descobrir se o elemento da pos x,y é uma mina ou não
    public static bool MineAt(int x, int y)
    {
        bool flag = false;
        if(x >=0 && y >=0 && x < w && y < h)
        {
            flag = elements[x, y].IsMine();
        }
        return flag;
    }
    public static int AdjacentMines(int x, int y)
    {
        int count = 0;
        if (MineAt(x, y+1)) count++; //top
        if (MineAt(x+1, y+1)) count++; //top - right
        if (MineAt(x+1, y)) count++; //right
        if (MineAt(x+1, y-1)) count++; //bottom - right
        if (MineAt(x, y-1)) count++; //bottom
        if (MineAt(x-1, y-1)) count++; //botton left
        if (MineAt(x-1, y)) count++; ;//left
        if (MineAt(x-1, y+1)) count++; //top-left
        return count;
    }
    public static void FFuncover(int x, int y, bool[,] visited)
    {
        if (x >= 0 && y >=0 && x < w && y < h)
        {
            //Ja visitou o bloco? Pare
            if (visited[x, y])
                return;

            //altera a imagem do bloco
            elements[x, y].LoadTexture(AdjacentMines(x,y));

            //esta perto de uma mina? Pare
            if (AdjacentMines(x,y) > 0)
                return;

            visited[x, y] = true;

            //recursividade
            FFuncover(x-1, y, visited);
            FFuncover(x+1, y, visited);
            FFuncover(x, y-1, visited);
            FFuncover(x, y+1, visited);
        }
    }

    public static bool isFinished()
    {
        foreach (Element item in elements)
        {
            if (item.isCovered() && !item.IsMine())
                return false;
        }
        return true;
    }
}
