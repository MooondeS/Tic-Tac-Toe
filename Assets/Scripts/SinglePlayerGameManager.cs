using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SinglePlayerGameManager : GameManager
{
    //AI
    public bool GametypeBot = true;
    public TextMeshProUGUI botThinkingText;
    bool botMove;
    int randMove;

    void Start()
    {
        baseColor = undoButton.buttonImage.color;
        base.StartGame();
    }

    void Update()
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
    public override void Pressed(MarkButtonScript buttonOnClick)
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

                if (winCondition())
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
    }

    public override void PlayAgain()
    {
        StopAllCoroutines();
        botThinkingText.text = string.Empty;

        foreach (MarkButtonScript item in list_Slots)
        {
            item.markImage.enabled = true;
            item.thisButton.enabled = true;
        }

        base.StartGame();
    }

    private void pauseToPress()
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
    private void resumePress()
    {
        foreach (MarkButtonScript item in list_Slots)
        {
            if (!historyPlacement.Contains(item))
            {
                item.markImage.enabled = true;
                item.thisButton.enabled = true;
            }
        }
    }
    #endregion



    #region AI Behaviors
    public override IEnumerator ChangeTurn()
    {
        if (playerTurn)
        {
            txt_whosTurn.text = "PLAYER 1'S TURN";

            foreach (MarkButtonScript item in list_Slots)
            {
                if (!historyPlacement.Contains(item))
                {
                    item.markImage.sprite = img_X;
                }
            }
        }

        //Bot's turn and change sprite to X marks
        else
        {
            txt_whosTurn.text = "EASY BOT'S TURN";
            pauseToPress();

            foreach (MarkButtonScript item in list_Slots)
            {
                if (!historyPlacement.Contains(item))
                {
                    item.markImage.sprite = img_O;
                }
            }

            StartCoroutine(BotThinking());
        }

        yield return null;
    }

    IEnumerator BotThinking()
    {
        txt_whosTurn.text = "EASY BOT'S TURN";
        //Disable all availables buttons
        
        yield return new WaitForSeconds(0.4f);
        botThinkingText.text += ".";
        yield return new WaitForSeconds(0.4f);
        botThinkingText.text += ".";
        yield return new WaitForSeconds(0.4f);
        botThinkingText.text += ".";
        yield return new WaitForSeconds(0.4f);

        EasyAI();

        botThinkingText.text = string.Empty;
    }

    private void EasyAI()
    {
        if (!playerTurn && !gamedoneCheck)
        {
            Random:
            randMove = Random.Range(0, list_Slots.Count);

            if (!historyPlacement.Contains(list_Slots[randMove]))
            {
                int buttonIndex = list_Slots.IndexOf(list_Slots[randMove]);
                list_Slots[randMove].markImage.enabled = true;
                blockStateCheck[buttonIndex] = State.O;
                playerTurn = true;
                historyPlacement.Add(list_Slots[randMove]);
                list_Slots[randMove].thisButton.enabled = false;
                resumePress();

                if (winCondition())
                {
                    txt_whosTurn.text = "Easy Bot Wins";    

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
            else goto Random; //Random again if [] slot is disabled; 
        }
    }
    #endregion
}
