using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : ExitGame
{
    [SerializeField] private TextMeshProUGUI _textCounterQuestion;
    [SerializeField] private Button[] _answerButtons;
    [SerializeField] protected TextMeshProUGUI questionText;

    [SerializeField] private  int _timeDelay = 2;

    [SerializeField] private GameObject _endGameAnimation;
    [SerializeField] private GameObject _startGameButton;

    private List<string> allAnswer = new List<string>();

    private int _countQuestion = 1;
    private int _selectedAnswer;

    protected Question currentQuestion;

    public void StartGame()
    {
        if (fullNameUser.text == "")
        {
            fullNameUser.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;

            if (PlayerPrefs.GetInt("Completed", 0) == 1)
            {

            }
        }
        else
        {
            animatorStartGame.SetTrigger("TriggerActivator");

            currentQuestion = GetCurrentQuestion();

            DisplayAll();
        }
    }

    public void CheckAnswer(int selectedAnswer)
    {
        ButtonsInteract(false);

        _selectedAnswer = selectedAnswer;
        bool isCorrect = currentQuestion.CorrectAnswer == _answerButtons[selectedAnswer].GetComponentInChildren<TextMeshProUGUI>().text;

        if (isCorrect)
            countCorectAnswer++;

        _answerButtons[selectedAnswer].GetComponent<Image>().color = isCorrect ? Color.green : Color.red;

        for (int i = 0; i < _answerButtons.Length; i++)
        {
            if (_answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text == currentQuestion.CorrectAnswer)
            {
                _answerButtons[i].GetComponent<Image>().color = Color.green;
            }
        }

        NextColumn();

        Invoke(nameof(NextQuestion), _timeDelay);
    }

    private void NextQuestion()
    {
        if (IsLastQuestion())
        {
            ConclusionCountCorectAnswer();

            _endGameAnimation.GetComponent<Animator>().SetTrigger("EndGame");

            double result = CountResult();
            InputDate(result);

            PlayerPrefs.SetInt("Completed", 1);
            PlayerPrefs.Save();
        }
        else
        {
            _countQuestion++;

            _answerButtons[_selectedAnswer].GetComponent<Image>().color = Color.white;

            currentQuestion = GetCurrentQuestion();
            DisplayAll();

            ButtonsInteract(true);
        }
    }

    protected void DisplayAll()
    {
        foreach (Button button in _answerButtons)
        {
            _textCounterQuestion.text = $"{_countQuestion}/{questions.Count}";

            button.GetComponent<Image>().color = Color.white;

            DisplayQuestion();
            DisplayAnswer();
        }
    }

    private void DisplayQuestion()
    {
        questionText.text = currentQuestion.QuestionText;
    }

    private void DisplayAnswer()
    {
        allAnswer.AddRange(currentQuestion.IncorrectAnswers);
        allAnswer.Add(currentQuestion.CorrectAnswer);

        for (int i = 0; i < _answerButtons.Length; i++)
        {
            int randomNumberAnswer = Random.Range(0, allAnswer.Count);

            _answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = allAnswer[randomNumberAnswer];

            allAnswer.RemoveAt(randomNumberAnswer);
        }
    }

    public void ButtonsInteract(bool isAntaractble)
    {
        for (int i = 0; i < _answerButtons.Length; i++)
        {
            _answerButtons[i].interactable = isAntaractble;
        }
    }
}