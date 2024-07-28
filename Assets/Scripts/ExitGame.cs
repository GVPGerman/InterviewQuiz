using TMPro;
using UnityEngine;

public class ExitGame : InputDataInTable
{
    [SerializeField] private TextMeshProUGUI _countCorectAnswer;

    public void LeaveGame()
    {
        Application.Quit();
    }

    protected void ConclusionCountCorectAnswer()
    {
        _countCorectAnswer.text = $"{countCorectAnswer} из {questions.Count}";
    }
}
