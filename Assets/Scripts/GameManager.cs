using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Text UI")]
    public TextMeshProUGUI txt_player1score;
    public TextMeshProUGUI txt_player2score;
    public TextMeshProUGUI txt_whosTurn;
    protected int player1score;
    protected int player2score;
    public bool playerTurn;

    [Header("Check Slots")]
    public List<MarkButtonScript> list_Slots;
    public List<MarkButtonScript> historyPlacement;
    protected State[] blockStateCheck = new State[9];

    [Header("Game States")]
    public bool gamedoneCheck;
    public Winner e_winner;

    [Header("Markers")]
    public UndoButtonScript undoButton;
    public Sprite img_X, img_O;
    public GameObject winLine;
    private int rand;
    
    protected Color baseColor;

    private void Start()
    {
        baseColor = undoButton.buttonImage.color;
        StartGame();
    }

    private void Update()
    {
        if (historyPlacement.Count == 0)
        {
            undoButton.undoText.color = new Color(0.21f, 0.21f, 0.21f, 1);
            undoButton.buttonSetting.enabled = false;
            undoButton.buttonImage.color = new Color(baseColor.r, baseColor.g, baseColor.b, 0);
        }
        else
        {
            undoButton.undoText.color = Color.white;
            undoButton.buttonSetting.enabled = true;
            undoButton.buttonImage.color = new Color(baseColor.r, baseColor.g, baseColor.b, 1);
        }
    }
    
    #region Buttons
    public virtual void Pressed(MarkButtonScript buttonOnClick)
    {
        int buttonIndex = list_Slots.IndexOf(buttonOnClick);

        if (playerTurn == true)
        {
            if (buttonOnClick.thisButton.enabled)
            {
                blockStateCheck[buttonIndex] = State.X;

                playerTurn = false;
                historyPlacement.Add(buttonOnClick);
                buttonOnClick.thisButton.enabled = false;

                if(winCondition()) 
                {
                    txt_whosTurn.text = "Player 1 Wins";

                    gamedoneCheck = true;
                    e_winner = Winner.Player1;

                    player1score++;
                    txt_player1score.text = player1score.ToString();

                    stopAlltheButton();
                    winLineRotation();
                }
                else StartCoroutine(ChangeTurn());

                //Press all but not win = draw 
                if (historyPlacement.Count == list_Slots.Count)
                {
                    if (!winCondition()) 
                    { 
                        txt_whosTurn.text = "Draws!"; 
                        gamedoneCheck = true; 
                    }
                }
            }
        }

        else
        {   
            if (buttonOnClick.thisButton.enabled)
            {
                blockStateCheck[buttonIndex] = State.O;

                playerTurn = true;
                historyPlacement.Add(buttonOnClick);
                buttonOnClick.thisButton.enabled = false;

                if (winCondition())
                {
                    txt_whosTurn.text = "Player 2 Wins";

                    gamedoneCheck = true;
                    e_winner = Winner.Player2;

                    player2score++;
                    txt_player2score.text = player2score.ToString();

                    stopAlltheButton();
                    winLineRotation();
                }

                else StartCoroutine(ChangeTurn());

                //Press all but not win = draw 
                if (historyPlacement.Count == list_Slots.Count)
                {
                    if (!winCondition())
                    {
                        txt_whosTurn.text = "Draws!";
                        gamedoneCheck = true;
                    }
                }
            }
        }
    }

    public void StartGame()
    {
        historyPlacement.Clear();
        blockStateCheck = new State[9];
        winLine.SetActive(false);

        gamedoneCheck = false;
        e_winner = Winner.None;

        rand = Random.Range(0, 2);

        if (rand == 1) playerTurn = true;
        else playerTurn = false;

        StartCoroutine(ChangeTurn());
    }

    public virtual void PlayAgain()
    {
        foreach (MarkButtonScript item in list_Slots)
        {
            item.markImage.enabled = true;
            item.thisButton.enabled = true;
        }

        StartGame();
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void UndoPressed()
    {
        winLine.SetActive(false);

        if (gamedoneCheck)
        {
            if(e_winner == Winner.Player1)
            {
                player1score--;
                txt_player1score.text = player1score.ToString();
            }

            else if (e_winner == Winner.Player2)
            {
                player2score--;
                txt_player2score.text = player2score.ToString();
            }

            e_winner = Winner.None;
            gamedoneCheck = false;
        }

        foreach (MarkButtonScript item in list_Slots)
        {
            if (!historyPlacement.Contains(item))
            {
                item.thisButton.enabled = true;
                item.markImage.enabled = true;
            }
        }

        historyPlacement[historyPlacement.Count - 1].thisButton.enabled = true;

        int lastIndexSlot = list_Slots.IndexOf(historyPlacement[historyPlacement.Count - 1]);

        blockStateCheck[lastIndexSlot] = State.None; 

        historyPlacement.RemoveAt(historyPlacement.Count - 1);

        if (playerTurn) playerTurn = false;
        else playerTurn = true;
        StartCoroutine(ChangeTurn());
    }
    #endregion

    #region Rules
    public virtual IEnumerator ChangeTurn()
    {
        if (playerTurn)
        {
            txt_whosTurn.text = "PLAYER 1'S TURN";
            for (int slots = 0; slots < list_Slots.Count; slots++)
            {
                if (list_Slots[slots].thisButton.enabled)
                {
                    Image x_Slot = list_Slots[slots].markImage;

                    x_Slot.sprite = img_X;
                }
            }
        }

        else
        {
            txt_whosTurn.text = "PLAYER 2'S TURN";
            for (int slots = 0; slots < list_Slots.Count; slots++)
            {
                if (list_Slots[slots].thisButton.enabled)
                {
                    Image o_Slot = list_Slots[slots].markImage;

                    o_Slot.sprite = img_O;
                }
            }
        }
        yield return null;
    }

    protected void winLineRotation()
    {
        winLine.SetActive(true);

        if(checkWin(0, 1, 2) || checkWin(3, 4, 5) || checkWin(6, 7, 8))
        {
            winLine.transform.eulerAngles = new Vector3 (0, 0, 90);
            winLine.transform.localScale = new Vector3(1, 1, 1);

            if (checkWin(0, 1, 2)) winLine.transform.position = list_Slots[1].transform.position;
            else if (checkWin(3, 4, 5)) winLine.transform.position = list_Slots[4].transform.position;
            else if (checkWin(6, 7, 8)) winLine.transform.position = list_Slots[7].transform.position;
        }

        if(checkWin(0, 3, 6) || checkWin(1, 4, 7) || checkWin(2, 5, 8))
        {
            winLine.transform.eulerAngles = new Vector3(0, 0, 0);
            winLine.transform.localScale = new Vector3(1, 1, 1);

            if (checkWin(0, 3, 6)) winLine.transform.position = list_Slots[3].transform.position;
            else if (checkWin(1, 4, 7)) winLine.transform.position = list_Slots[4].transform.position;
            else if (checkWin(2, 5, 8)) winLine.transform.position = list_Slots[5].transform.position;
        }

        if(checkWin(0, 4, 8))
        {
            winLine.transform.eulerAngles = new Vector3(0, 0, 45);
            winLine.transform.position = list_Slots[4].transform.position;
            winLine.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        }

        if (checkWin(2, 4, 6))
        {
            winLine.transform.eulerAngles = new Vector3(0, 0, -45);
            winLine.transform.position = list_Slots[4].transform.position;
            winLine.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        }
    }

    protected void stopAlltheButton()
    {
        foreach (MarkButtonScript item in list_Slots)
        {
            if (!historyPlacement.Contains(item))
            {
                item.thisButton.enabled = false;
                item.markImage.enabled = false;
            }
        }
    }

    protected bool winCondition()
    {
        bool gamewon = (checkWin(0, 1, 2) || checkWin(3, 4, 5) || checkWin(6, 7, 8) || checkWin(0, 3, 6) 
            || checkWin(1, 4, 7) || checkWin(2, 5, 8) || checkWin(0, 4, 8) || checkWin(2, 4, 6));

        if (gamewon)
        {
            stopAlltheButton();    
        }

        return gamewon;
    }

    bool checkWin(int a, int b, int c)
    {
        if (blockStateCheck[a] != State.None)
        {
            bool winning = blockStateCheck[a] == blockStateCheck[b] && blockStateCheck[a] == blockStateCheck[c];

            return winning;
        }

        else return false;
    }
    #endregion
}

public enum State { None, X, O };
public enum Winner { None, Player1, Player2 };