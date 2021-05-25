using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class QuizGameUI : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] private QuizManager quizManager;               //ref to the QuizManager script
    [SerializeField] private CategoryBtnScript categoryBtnPrefab;
    [SerializeField] private GameObject scrollHolder,fillwater;
    [SerializeField] private Text scoreText, timerText,wrongText;
    [SerializeField] private GameObject gameOverPanel, gamePausePanel, mainMenu, gamePanel;
    [SerializeField] private Color correctCol, wrongCol, normalCol; //color of buttons
    [SerializeField] public AudioSource questionAudio;             //audio source for audio clip
    [SerializeField] private Text questionInfoText;                 //text to show question
    [SerializeField] private List<Button> options;                  //options button reference
#pragma warning restore 649
    public AudioClip correctaudio,wrongaudio,fillwaterclip,brickssound;
    private float audioLength;          //store audio length
    private Question question;          //store current question data
    private bool answered = false;      //bool to keep track if answered or not

    public Text TimerText { get => timerText; }                     //getter
    public Text ScoreText { get => scoreText; }
    public Text WrongText { get => wrongText; }
    //getter
    public GameObject GameOverPanel { get => gameOverPanel; }                     //getter
    public GameObject GamePausePanel { get => gamePausePanel; }
    private void Start()
    {
        //add the listner to all the buttons
        for (int i = 0; i < options.Count; i++)
        {
            Button localBtn = options[i];
            localBtn.onClick.AddListener(() => OnClick(localBtn));
        }

        CreateCategoryButtons();

    }
    
    /// <summary>
    /// Method which populate the question on the screen
    /// </summary>
    /// <param name="question"></param>
    public void SetQuestion(Question question)
    {
        //set the question
        this.question = question;
        //check for questionType
        switch (question.questionType)
        {
            case QuestionType.TEXT:
              //     //switch so we can add animations with video and images in future
                break;
            case QuestionType.AUDIO:
                questionAudio.transform.gameObject.SetActive(true);         //activate questionAudio
                
                audioLength = question.audioClip.length;                    //set audio clip
                StartCoroutine(PlayAudio());                                //start Coroutine
                break;
            
        }

        questionInfoText.text = question.questionInfo;                      //set the question text

        //suffle the list of options
        List<string> ansOptions = ShuffleList.ShuffleListItems<string>(question.options);

        //assign options to respective option buttons
        for (int i = 0; i < options.Count; i++)
        {
            //set the child text
            options[i].GetComponentInChildren<Text>().text = ansOptions[i];
            options[i].name = ansOptions[i];    //set the name of button
            options[i].image.color = normalCol; //set color of button to normal
        }

        answered = false;                       

    }



    /// <summary>
    /// IEnumerator to repeate the audio after some time
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayAudio()
    {
        //if questionType is audio
        if (question.questionType == QuestionType.AUDIO)
        {
            //PlayOneShot
            questionAudio.PlayOneShot(question.audioClip);
            //wait for few seconds
            yield return new WaitForSeconds(audioLength + 0.5f);
            //play again
            StartCoroutine(PlayAudio());
        }
        else //if questionType is not audio
        {
            //stop the Coroutine
            StopCoroutine(PlayAudio());
            //return null
            yield return null;
        }
    }
    private void selectque() {
        quizManager.SelectQuestion();
    }
    /// <summary>
    /// Method assigned to the buttons
    /// </summary>
    /// <param name="btn">ref to the button object</param>
    void OnClick(Button btn)
    {
        Debug.Log("button is clicked");
        if (quizManager.GameStatus == GameStatus.PLAYING)
        {
            //if answered is false
            if (!answered)
            {
                //set answered true
                answered = true;
                //get the bool value
                bool val = quizManager.Answer(btn.name);
                Debug.Log("btn name is" + val);
                //if its true
                if (val)
                {
                    PlayerPrefs.SetInt("correct", 1);
                    PlayerPrefs.SetInt("missed", 0);
                    //set color to correct
                    // Invoke("selectque", 0.4f);
                    //btn.image.color = correctCol;
                    questionAudio.PlayOneShot(correctaudio);
                    StartCoroutine(BlinkImg(btn.image));
                  
                    // btn.gameObject.SetActive(false);
                }
                else
                {
                    answered = false;
                    //else set it to wrong color
                    fillwater.SetActive(true);
                    btn.image.color = wrongCol;
               //     yield return new WaitForSeconds(0.5f);
                    questionAudio.PlayOneShot(wrongaudio);
                    PlayerPrefs.SetInt("missed", 1);
                    //Debug.Log(PlayerPrefs.GetInt("missed"));
                   
                }
            }
        }
    }

    /// <summary>
    /// Method to create Category Buttons dynamically
    /// </summary>
    void CreateCategoryButtons()
    {
        //we loop through all the available catgories in our QuizManager
        for (int i = 0; i < quizManager.QuizData.Count; i++)
        {
            //Create new CategoryBtn
            CategoryBtnScript categoryBtn = Instantiate(categoryBtnPrefab, scrollHolder.transform);
            //Set the button default values
            categoryBtn.SetButton(quizManager.QuizData[i].categoryName, quizManager.QuizData[i].questions.Count);
            int index = i;
            //Add listner to button which calls CategoryBtn method
            categoryBtn.Btn.onClick.AddListener(() => CategoryBtn(index, quizManager.QuizData[index].categoryName));
        }
    }

    //Method called by Category Button
    private void CategoryBtn(int index, string category)
    {
        quizManager.StartGame(index, category); //start the game
        mainMenu.SetActive(false);              //deactivate mainMenu
        gamePanel.SetActive(true);              //activate game panel
       // StartCoroutine("DoCheck");
    }
    IEnumerator DoCheck()
    {
        for (; ; )
        {
            // execute block of code here
            GameObject ops = ObjectPooler.SharedInstance.GetPooledObject();
            if (ops != null)
            {
                ops.SetActive(true);
            }
            yield return new WaitForSeconds(3.1f);
        }
    }
    //this give blink effect [if needed use or dont use]
    IEnumerator BlinkImg(Image img)
    {
        for (int i = 0; i < 2; i++)
        {
            img.color = Color.white;
            yield return new WaitForSeconds(0.1f);
            img.color = correctCol;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void RestryButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void endgamebutton()
    {
        SceneManager.LoadScene("Start");
    }
    public void OnClick22()
    {
        Debug.Log("button is clicked"+ EventSystem.current.currentSelectedGameObject.name);
        if (quizManager.GameStatus == GameStatus.PLAYING)
        {
            
            //if answered is false
            if (!answered)
            {
                //set answered true
                answered = true;
                //get the bool value
                bool val = quizManager.Answer(EventSystem.current.currentSelectedGameObject.name);
                Debug.Log("btn name is" + val);
                //if its true
                if (val)
                {
                    PlayerPrefs.SetInt("correct", 1);
                    PlayerPrefs.SetInt("missed", 0);
                    //set color to correct
                    // Invoke("selectque", 0.4f);
                    //btn.image.color = correctCol;
                    questionAudio.PlayOneShot(correctaudio);
                    //StartCoroutine(BlinkImg(EventSystem.current.currentSelectedGameObject.image));

                    // btn.gameObject.SetActive(false);
                }
                else
                {
                    answered = false;
                    //else set it to wrong color
                    fillwater.SetActive(true);
                  //  EventSystem.current.currentSelectedGameObject.image.color = wrongCol;
                    //     yield return new WaitForSeconds(0.5f);
                    questionAudio.PlayOneShot(wrongaudio);
                    PlayerPrefs.SetInt("missed", 1);
                    //Debug.Log(PlayerPrefs.GetInt("missed"));

                }
            }
        }
    }


}
