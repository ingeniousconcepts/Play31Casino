using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Blackjack game logic class,all key logic put here.
/// include Chips management,game rule, poker deal,etc.
/// </summary>
public class Game1 : RefSingleton<Game1> {

    const int DEFAULT_MONEY = 5000;

    enum RESULT
    {
        NONE,
        BANKER_WIN,
        PLAYER_WIN,
    }

    int money;
    public int Money
    {
        get
        {
            return money;
        }
        set
        {
            if (money != value)
            {
                money = value;
                PlayerPrefs.SetInt("Money", value);
                PlayerPrefs.Save();
                ChipsPanel.RefreshChips();
                totalMonty.text = money.ToString();
            }
        }
    }

    public int Chips;

    int lastChips = 0;

    public Text totalMonty;
    public Button btnCancel;
    public Button btnDeal;

    public UIChipsPanel1 ChipsPanel;
    public UIButtonsPanel buttonsPanel;

    public Dealer dealer;

    public CardsSlot bankerSlot;
    public CardsSlot playerSlot;


    public CanvasGroup playerScorePanel;
    public Text playerScore;

    public CanvasGroup bankerScorePanel;
    public Text bankerScore;


    public Text winMoney;
    public GameObject winPanel;
    public GameObject lossPanel;
    public GameObject winBJ;
    public GameObject rewardPanel;
    public Text rewardMoney;

    public Toggle musicSwitch;
    public Text playerResultDetails;
    public Text Title;
    public Text aiResultDetails;
    
    public bool status = false;
    public static Game1 instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.
    public CardsSlot[] Players;
    private int turn = 0;
    public GameObject[] Timers;
    // Use this for initialization
    void Start() {
        this.InitGame();
    }

    // Update is called once per frame
    void Update() {

    }

    /// <summary>
    /// Exit this game.
    /// </summary>
    public void Exit()
    {
        SceneManager.LoadScene(0);
    }
    public void Awake()
    {
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

 
}

    /// <summary>
    /// Inits the game.
    /// </summary>
	/// 
    void InitGame()
    {
        PlayerPrefs.SetInt("Money", DEFAULT_MONEY);
        PlayerPrefs.Save();

        this.Money = PlayerPrefs.GetInt("Money", DEFAULT_MONEY);
        this.totalMonty.text = this.money.ToString();

        Chip1.Instance.gameObject.SetActive(false);
        this.ShowDealButtons(false);

        this.winPanel.SetActive(false);
        this.rewardPanel.SetActive(false);
    }

    /// <summary>
    /// Adds the Chips.
    /// </summary>
    /// <param name="Chip1">Chip1.</param>
    public void AddChips(int Chip1)
    {
        this.Chips += Chip1;
        this.Money -= Chip1;
        this.ShowDealButtons(true);
    }

    /// <summary>
    /// Clears the Chips.
    /// </summary>
    public void ClearChips()
    {
        SoundManager.Instance.PlayButton();
        this.Money += this.Chips;
        this.Chips = 0;
        Chip1.Instance.Clear();
        this.ShowDealButtons(false);
    }

    /// <summary>
    /// Shows the deal buttons.
    /// </summary>
    /// <param name="show">If set to <c>true</c> show.</param>
    public void ShowDealButtons(bool show)
    {
        this.btnDeal.gameObject.SetActive(show);
        this.btnCancel.gameObject.SetActive(show);
    }

    /// <summary>
    /// start game.
    /// </summary>
    public void GameStart()
    {
        SoundManager.Instance.PlayButton();
        this.ShowDealButtons(false);
        this.ChipsPanel.HidePanel();
        StartCoroutine(GameLoop());
    }

    /// <summary>
    /// main game loop.
    /// </summary>
    /// <returns>The loop.</returns>
    IEnumerator GameLoop()
    {
        Chip1.Instance.FlyToSide();
        yield return new WaitForSeconds(1f);

        Game1.Instance.playerSlot.AddCard(dealer.Deal(),false);
        yield return new WaitForSeconds(0.5f);
        Game1.Instance.playerSlot.AddCard(dealer.Deal(),false);
        yield return new WaitForSeconds(0.5f);

        Game1.Instance.bankerSlot.AddCard(dealer.Deal(),false);
        yield return new WaitForSeconds(0.5f);
        Game1.Instance.bankerSlot.AddCard(dealer.Deal(), false);
        yield return new WaitForSeconds(0.5f);

        if(!this.CheckResult())
        {
            buttonsPanel.ShowButtons();
        }
    }

    /// <summary>
    /// Checks game result.
    /// </summary>
    /// <returns><c>true</c>, if result was checked, <c>false</c> otherwise.</returns>
    /// <param name="final">If set to <c>true</c> final.</param>
	/// 
	/// Hand Sum Value is showing here
    bool CheckResult(bool final= false)
    {
        int bankerPoint = bankerSlot.Point;
        int playerPoint = playerSlot.Point;

		this.ShowScore(1, true, bankerPoint);

        this.ShowScore(2, true, playerPoint);
        
        if (bankerPoint == playerPoint && bankerPoint == 31)
        {
            ShowResult(RESULT.NONE);
            //Tie
            return true;
        }

        if (bankerPoint == 31)
        {

			ShowResult(RESULT.BANKER_WIN, bankerPoint,"AI SUM is 31");
			print ("AI Wins");
            return true;
        }

        if(playerPoint == 31)
        {
			ShowResult(RESULT.PLAYER_WIN, playerPoint,"Player SUM is 31");
			print ("Player Wins");
            return true;
        }

		if (playerPoint == 14) {

			ShowResult(RESULT.PLAYER_WIN, playerPoint,"Player SUM is 14");
			print ("Player Wins");
			return true;
		}

		if (bankerPoint == 14) {
			
			ShowResult(RESULT.BANKER_WIN, bankerPoint,"AI SUM is 14");
			print ("AI Wins");
			return true;
		}

        if (bankerPoint > 31 && playerPoint > 31)
        {
			ShowResult(RESULT.NONE, bankerPoint);
			print ("Tie");
            return true;
        }

        if(bankerPoint>31)
        {
			ShowResult(RESULT.PLAYER_WIN, playerPoint,"Player2 Burst");
			print ("Player2 Burst");
            return true;
        }

        if (playerPoint > 31)
        {
			ShowResult(RESULT.BANKER_WIN, bankerPoint,"Player1 Burst");
			print ("Player Burst");
            return true;
        }

        if(final)
        {
            if (bankerPoint == playerPoint)
            {
                ShowResult(RESULT.NONE);
                //TIE
                return true;
            }
            if(bankerPoint > playerPoint)
            {
                ShowResult(RESULT.BANKER_WIN, bankerPoint,"Player2 Hand Value is Greater");
				print ("Player2 Hand Value is Greater");
            }
            else
            {
                ShowResult(RESULT.PLAYER_WIN, playerPoint,"Player Hand Value is Greater");
				print ("Player Hand Value is Greater");
            }
            return true;
        }

        return false;
    }
    
   
   
    
    /// <summary>
    /// Shows game result.
    /// </summary>
    /// <param name="result">Result.</param>
    /// <param name="point">Point.</param>
	void ShowResult(RESULT result, int point=0,string msg = "")
    {
        //        Debug.LogFormat("{0},{1}", result, point);
        //		if (msg != "") {
        //			if (result == RESULT.PLAYER_WIN) {
        ////				print ("Player Wins");
        //				winPanel.SetActive (true);
        //				playerResultDetails.text = "\n"+"Player value :"+playerScore.text+"\n"+"AI Value :"+bankerScore.text;;
        //			} else if (result == RESULT.BANKER_WIN) {
        //				lossPanel.SetActive (true);
        //				aiResultDetails.text = "Player value :"+playerScore.text+"\n"+"AI Value :"+bankerScore.text;;
        //			}
        //
        //		} else {
        status = true;
        buttonsPanel.HideButtons();
        if (result == RESULT.PLAYER_WIN) {
//				print ("Player Wins");
				winPanel.SetActive (true);
                Title.text = "Player 1 is Winner";
				playerResultDetails.text = msg+"\n";

		} else if (result == RESULT.BANKER_WIN) {
                Title.text = "Player 2 is Winner";
                 winPanel.SetActive (true);
                playerResultDetails.text = msg+"\n";

			}
//		}
//		StartCoroutine( ResultProcess(result, point));

    }


    /// <summary>
    /// Result process.
    /// </summary>
    /// <param name="result">Result.</param>
    /// <param name="point">Point.</param>
    IEnumerator ResultProcess(RESULT result,int point)
    {
        bankerSlot.FlipAll();
        this.ShowScore(1, true, bankerSlot.Point);
        this.ShowScore(2, true, playerSlot.Point);

        buttonsPanel.HideButtons();
        yield return new WaitForSeconds(1f);

        if (result == RESULT.BANKER_WIN)
        {
            SoundManager.Instance.PlayFailed();
            Chip1.Instance.FlyToBanker();
        }
        else
        {
            Chip1.Instance.FlyToPlayer();
            int addMoney = 0;

            if (result == RESULT.PLAYER_WIN)
            {
                SoundManager.Instance.PlayWin();
                if (point == 31)
                {
                    addMoney = (int)(this.Chips * 1.5);
                }
                else
                    addMoney = this.Chips;
            }
            if (addMoney > 0)
            {
                this.ShowWinChips(addMoney, point == 31);
                this.Money += (this.Chips + addMoney);
                yield return new WaitForSeconds(2f);
            }
            if (result == RESULT.NONE)
                this.Money += this.Chips;
        }

        this.lastChips = this.Chips;
        this.Chips = 0;
        if (this.lastChips > this.Money)
        {
            this.lastChips = 0;
        }
        playerSlot.RemoveAll();
        bankerSlot.RemoveAll();
        ChipsPanel.ShowPanel();
//        ShowScore(0, false, 0);
        if (this.Money == 0)
        {
            yield return new WaitForSeconds(2f);
            this.ShowRewards(500);
            yield return new WaitForSeconds(2f);
        }
        if(this.lastChips >0)
        {
            yield return new WaitForSeconds(1f);
            Chip1.Instance.ShowChips(null, new Vector3(640, 0, 0));
            this.AddChips(this.lastChips);
        }
		ShowScore (1,false,0);
		ShowScore (2,false,0);
        yield return null;
    }

    /// <summary>
    /// Show  win Chips.
    /// </summary>
    /// <param name="Chip1">Chip1.</param>
    /// <param name="blackjack">If set to <c>true</c> blackjack.</param>
    void ShowWinChips(int Chip1,bool blackjack)
    {
        this.winMoney.text = "+" + Chip1.ToString();
        this.winPanel.transform.localScale = Vector3.zero;
        this.winPanel.SetActive(true);
        this.winBJ.SetActive(blackjack);
//        LeanTween.scale(this.winPanel, Vector3.one, 0.2f).setEase(LeanTweenType.easeInBack).setOnComplete(()=>{
//            LeanTween.delayedCall(this.gameObject, 1f, () => {
//                LeanTween.scale(this.winPanel, Vector3.zero, 0.2f).setEase(LeanTweenType.easeInBack);
//            });
//        });

    }

    /// <summary>
    /// Show rewards.
    /// </summary>f
    /// <param name="money">Money.</param>
    void ShowRewards(int money)
    {
        this.rewardMoney.text = money.ToString();
        this.rewardPanel.transform.localScale = Vector3.zero;
        this.rewardPanel.SetActive(true);
        LeanTween.scale(this.rewardPanel, Vector3.one, 0.2f).setEase(LeanTweenType.easeInBack).setOnComplete(() => {
            LeanTween.delayedCall(this.gameObject, 1f, () => {
                LeanTween.scale(this.rewardPanel, Vector3.zero, 0.2f).setEase(LeanTweenType.easeInBack);
                this.Money += money;
            });
        });

    }

    /// <summary>
    /// On Hit button clicked
    /// </summary>
    public void Hit()
    {
        SoundManager.Instance.PlayButton();
        Players[turn].FlipAll();
        StartCoroutine(HitProcess());
        //playerSlot.FlipAll();
        
    }

    /// <summary>
    /// ON Stand button clicked
    /// </summary>
    public void Stand()
    {
        SoundManager.Instance.PlayButton();
        Players[turn].FlipAll();
        Timers[turn].SetActive(false);
        turn++;
        if (turn <= Timers.Length - 1)
            Timers[turn].SetActive(true);

       if(turn ==Timers.Length) { 
            CheckResult(true);
            print("print="+turn);
        }

    }

    /// <summary>
    /// Stand process.
    /// </summary>
    /// <returns>The process.</returns>
   

    /// <summary>
    /// ON Double button clicked
    /// </summary>
    public void Double()
    {
        
    }

    /// <summary>
    /// Oo Split button clicked
    /// </summary>
    public void Split()
    {
        
    }

    /// <summary>
    /// Hit process.
    /// </summary>
    /// <returns>The process.</returns>
    IEnumerator HitProcess()
    {

        Players[turn].AddCard(dealer.Deal());
        yield return new WaitForSeconds(0.5f);

        int point = Players[turn].Point;
        this.ShowScore(2, true, point);
        if (point >= 31)
        {//player end
            
            this.Final();
        }
        else
            this.CheckResult();

    }


   

    /// <summary>
    /// Final result check
    /// </summary>
    void Final()
    {
        this.CheckResult(true);
    }

    /// <summary>
    /// Show the score.
    /// </summary>
    /// <param name="side">Side.</param>
    /// <param name="show">If set to <c>true</c> show.</param>
    /// <param name="score">Score.</param>
    void ShowScore(int side,bool show, int score)
    {
        if (side == 1 && show)
            this.bankerScore.text = score.ToString();
        if (side == 2 && show)
            this.playerScore.text = score.ToString();
        if(side == 0 || side == 1)
        {
            if(!this.bankerScorePanel.alpha.Equals(show ? 1f:0f))
            {
                LeanTween.alphaCanvas(this.bankerScorePanel, show ? 1f : 0f, 0.2f);
                
            }
        }
        if (side == 0 || side == 2)
        {
            if (!this.playerScorePanel.alpha.Equals(show ? 1f : 0f))
            {
                LeanTween.alphaCanvas(this.playerScorePanel, show ? 1f : 0f, 0.2f);
                Timers[turn].SetActive(true);
            }
        }
    }

    public void RecycleCard(Card card)
    {
        this.dealer.Recycle(card);
    }
}
