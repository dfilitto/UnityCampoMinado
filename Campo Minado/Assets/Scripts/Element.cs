using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element : MonoBehaviour {

    [SerializeField]
    private AudioClip audioClick; // Som do click
    [SerializeField]
    private AudioClip audioBomb; //som da bomba
    [SerializeField]
    private bool mine; //se o elemento é uma mina
    [SerializeField]
    private Sprite[] emptyTextures; //imagens dos blocos 1 - 8
    [SerializeField]
    private Sprite mineTexture; //imagem da mina
    [SerializeField]
    private Sprite defaultTexture; //imagem da mina
    private AudioSource playSound; //player (toca as músicas)
    // Use this for initialization
    void Start () {
        playSound = gameObject.GetComponent<AudioSource>();
        mine = Random.value < 0.15;

        //registrar bloco matriz de blocos
        int x = (int)transform.position.x;
        int y = (int)transform.position.y;
        GameController.elements[x, y] = this;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool IsMine()
    {
        return this.mine;
    }
    public void LoadTexture(int adjacentCount)
    {
        if (mine)
        {
            GetComponent<SpriteRenderer>().sprite = mineTexture;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = emptyTextures[adjacentCount];
        }
    }

    public void RestartDefault()
    {
        GetComponent<SpriteRenderer>().sprite = defaultTexture;
    }
    private void OnMouseUpAsButton()
    {
        if (GameController.GameState == "Play")
        {
            
            if (mine)
            {
                // tela de game over
                playSound.clip = audioBomb;
                playSound.Play();
                this.LoadTexture(0);
                //print("Game Over!!!");
                GameController.UncoverMines();
                GameController.GameState = "Game Over";
            }
            else
            {
                playSound.clip = audioClick;
                playSound.Play();
                int x = (int)transform.position.x;
                int y = (int)transform.position.y;
                //lógica proximidade da mina
                int p = GameController.AdjacentMines(x, y);
                this.LoadTexture(p);

                GameController.FFuncover(x, y, new bool[GameController.w, GameController.h]);

                if (GameController.isFinished())
                {
                    //print("You Win!!!!!");
                    GameController.GameState = "Win";
                }

            }
        }
    }

    public bool isCovered()
    {
        bool flag = false;
        if (GetComponent<SpriteRenderer>().sprite.texture.name == "default")
        {
            flag = true;
        }
        return flag;
    }
}
