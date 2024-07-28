using System.Data;
using TMPro;
using UnityEngine;

public class InputDataInTable : QuestionManager
{
    [SerializeField] protected TMP_InputField fullNameUser;

    protected int countCorectAnswer = 0;

    protected double CountResult()
    {
        double oneHandredProcent = 100;

        double oneProcent = questions.Count / oneHandredProcent;
        double procentPassed = (double) countCorectAnswer / oneProcent;

        return procentPassed;
    }

    protected void InputDate(double procent)
    {
        if(IsLastQuestion())
        {
            CountResult();

            string dateComplete = $"{System.DateTime.Now.Day}:{System.DateTime.Now.Month}:{System.DateTime.Now.Year}";

            IDbCommand command = dbConnection.CreateCommand();

            string fullNameUser = this.fullNameUser.text;
            
            string query = $"INSERT INTO UserResultTest(FullName, DateCompletion, ProccentPassed) VALUES ('{fullNameUser}', '{dateComplete}', '{procent}%')";

            command.CommandText = query;

            command.ExecuteNonQuery();

            ConnectionClose();

            Debug.Log("Подключение прервано.");
        }
    }
}
