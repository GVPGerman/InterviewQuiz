using System.Collections.Generic;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine;

public class QuestionManager : MonoBehaviour
{
    [SerializeField] private GameObject _startGameAnimation;
    protected Animator animatorStartGame;

    private string _pathToDatabase;
    private IDataReader _reader;
    protected IDbConnection dbConnection;

    private List<string> _correctAnswers = new List<string>();
    private List<List<string>> _incorrectAnswers = new List<List<string>>();
    protected List<string> questions = new List<string>();

    protected int _currentIndex = 0;

    private void Start()
    {
        _pathToDatabase = "Data Source=D:/programer/Create/Diplom/InterviewQuiz/Assets/StreamingAssets/Diplom";

        Debug.Log("Подключение прошло успешно. Путь: " + _pathToDatabase);

        dbConnection = new SqliteConnection(_pathToDatabase); 
        dbConnection.Open();

        IDbCommand command = dbConnection.CreateCommand();
        command.CommandText = "SELECT * FROM answerandquestion";

        _reader = command.ExecuteReader();

        while (_reader.Read())
        {
            InputData();
        }

        StartAnimationGame();
    }

    private void StartAnimationGame()
    {
        animatorStartGame = _startGameAnimation.GetComponent<Animator>();
    }


    private void InputData()
    {
        questions.Add(_reader["Question"].ToString());
        _correctAnswers.Add(_reader["TrueAnswer"].ToString());

        string[] incorrectAnswersArray = _reader["FalseAnswer"].ToString().Split(',');
        _incorrectAnswers.Add(new List<string>(incorrectAnswersArray));
    }

    protected Question GetCurrentQuestion()
    {
        string question = questions[_currentIndex];
        string correctAnswer = _correctAnswers[_currentIndex];
        List<string> incorrectAnswersForCurrentQuestion = _incorrectAnswers[_currentIndex];

        return new Question(question, correctAnswer, incorrectAnswersForCurrentQuestion);
    }

    protected void NextColumn()
    {
        _currentIndex++;
    }

    protected bool IsLastQuestion()
    {
        return _currentIndex == questions.Count;
    }

    protected void ConnectionClose()
    {
        _reader.Close();
        _reader = null;
        dbConnection.Close();
        dbConnection = null;
    }
}

public class Question
{
    public string QuestionText { get; private set; }
    public string CorrectAnswer { get; private set; }
    public List<string> IncorrectAnswers { get; private set; }

    public Question(string questionText, string correctAnswer, List<string> incorrectAnswers)
    {
        QuestionText = questionText;
        CorrectAnswer = correctAnswer;
        IncorrectAnswers = incorrectAnswers;
    }
}