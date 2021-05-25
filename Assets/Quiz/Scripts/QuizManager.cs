using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizManager : MonoBehaviour
{
#pragma warning disable 649
    //ref to the QuizGameUI script
    [SerializeField] private QuizGameUI quizGameUI;
    //ref to the scriptableobject file
    [SerializeField] private List<QuizDataScriptable> quizDataList;
    [SerializeField] private float timeInSeconds;
#pragma warning restore 649
    public int maxHealth = 100;
    public int currentHealth;
    private string currentCategory = "";
    private int correctAnswerCount = 0;
    private int WrongAnswerCount = 0;
    //questions data
    private List<Question> questions;
    //current question data
    private Question selectedQuetion = new Question();
    private int gameScore;
    private int lifesRemaining;
    private float currentTime;
    private QuizDataScriptable dataScriptable;
  //  public HealthBar healthBar;
    public BrickFillerAnimation brickfill;
    private GameStatus gameStatus = GameStatus.NEXT;

    public GameStatus GameStatus { get { return gameStatus; } }

    public List<QuizDataScriptable> QuizData { get => quizDataList; }
    void Start()
    {
        currentHealth = 0;
       // healthBar.SetMaxHealth(maxHealth);
    }

    public void StartGame(int categoryIndex, string category)
    {
        currentCategory = category;
        correctAnswerCount = 0;
        WrongAnswerCount = 0;
        gameScore = 0;
       // lifesRemaining = 4;
        currentTime = timeInSeconds;
        //set the questions data
        questions = new List<Question>();
        dataScriptable = quizDataList[categoryIndex];
        questions.AddRange(dataScriptable.questions);
        //select the question
        SelectQuestion();
        gameStatus = GameStatus.PLAYING;

    }
    public void PauseGame()
    {
        Time.timeScale = 0;
        quizGameUI.GamePausePanel.SetActive(true);
    }

    public void ResumeGame()
    {
        quizGameUI.GamePausePanel.SetActive(false);
        Time.timeScale = 1;
        
    }
    /// <summary>
    /// Method used to randomly select the question form questions data
    /// </summary>
    public void SelectQuestion()
    {
        //get the random number
        int val = UnityEngine.Random.Range(0, questions.Count);
        //set the selectedQuetion
        selectedQuetion = questions[val];
        //send the question to quizGameUI
        quizGameUI.SetQuestion(selectedQuetion);

        questions.RemoveAt(val);
    }
    void TakeDamage(int bricknum)
    {
       // currentHealth += damage;

       // healthBar.SetHealth(currentHealth);
       brickfill.Setbricks(bricknum);
       // PlayerPrefs.SetInt("wrong", bricknum);
        quizGameUI.questionAudio.PlayOneShot(quizGameUI.brickssound);

    }
    private void Update()
    {
        if (gameStatus == GameStatus.PLAYING)
        {
            currentTime -= Time.deltaTime;
            SetTime(currentTime);
        }
    }

    void SetTime(float value)
    {
        TimeSpan time = TimeSpan.FromSeconds(currentTime);                       //set the time value
        quizGameUI.TimerText.text = time.ToString("mm':'ss");   //convert time to Time format

        if (currentTime <= 0)
        {
            //Game Over
            GameEnd();
        }
    }

    /// <summary>
    /// Method called to check the answer is correct or not
    /// </summary>
    /// <param name="selectedOption">answer string</param>
    /// <returns></returns>
    public bool Answer(string selectedOption)
    {
        //set default to false
        bool correct = false;
        //if selected answer is similar to the correctAns
        if (selectedQuetion.correctAns == selectedOption)
        {
            //Yes, Ans is correct
            correctAnswerCount++;
            correct = true;
            gameScore += 1;
            quizGameUI.ScoreText.text = " Correct  :  " + gameScore;
            Debug.Log("its correct ans");
            Invoke("SelectQuestion", 0.4f);
        }
        else
        {
            //No, Ans is wrong
            //Reduce Life
            // lifesRemaining--;
           // TakeDamage(20);
            WrongAnswerCount++;
            quizGameUI.WrongText.text = " Wrong   :  " + WrongAnswerCount;
            
            // quizGameUI.ReduceLife(lifesRemaining);
            if (WrongAnswerCount >= 4) { GameEnd(); }
            TakeDamage(WrongAnswerCount);

            if (lifesRemaining == 0)
            {
               // GameEnd();
            }
        }

        if (gameStatus == GameStatus.PLAYING)
        {
            if (questions.Count > 0)
            {
                if (true)
                {
                    Debug.Log("quizmanager" + PlayerPrefs.GetInt("missed"));
                    //call SelectQuestion method again after 1s
                   // Invoke("SelectQuestion", 0.4f);
                    
                }
                else {
                    // questions.RemoveAt(val);
                    PlayerPrefs.SetInt("missed", 1);
                }
            }
            else
            {
                GameEnd();
            }
        }
        //return the value of correct bool
        return correct;
    }

    public void GameEnd()
    {
        gameStatus = GameStatus.NEXT;
        quizGameUI.GameOverPanel.SetActive(true);
        quizGameUI.GamePausePanel.SetActive(false);
        //fi you want to save only the highest score then compare the current score with saved score and if more save the new score
        //eg:- if correctAnswerCount > PlayerPrefs.GetInt(currentCategory) then call below line
        if(Time.timeScale == 0)
        { Time.timeScale = 1; }
        //Save the score
        PlayerPrefs.SetInt(currentCategory, correctAnswerCount); //save the score for this category
    }
}

//Datastructure for storeing the quetions data
[System.Serializable]
public class Question
{
    public string questionInfo;         //question text
    public QuestionType questionType;   //type
    public Sprite questionImage;        //image for Image Type
    public AudioClip audioClip;         //audio for audio type
    public UnityEngine.Video.VideoClip videoClip;   //video for video type
    public List<string> options;        //options to select
    public string correctAns;           //correct option
}

[System.Serializable]
public enum QuestionType
{
    TEXT,
    IMAGE,
    AUDIO,
    VIDEO
}

[SerializeField]
public enum GameStatus
{
    PLAYING,
    NEXT
}