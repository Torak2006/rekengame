using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public List<Question> questions = new List<Question>();
    private int currentQuestionIndex = 0;
    private int score = 0;
    private int goedBeantwoord = 0;
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI scoreText;
    public GameObject answerPrefab;
    public Transform answersParent;
    public GameObject eindScherm;
    public TextMeshProUGUI eindTekst;
    private bool kanAntwoorden = true;

    void Start()
    {
        eindScherm.SetActive(false);
        CreateQuestions();
        ShuffleQuestions();
        ShowQuestion();
        UpdateScore();
    }

    public void AnswerSelected(int answer, AnswerItem item)
    {
        if (!kanAntwoorden) return;
        kanAntwoorden = false;

        Question q = questions[currentQuestionIndex];
        if (answer == q.correctAnswer)
        {
            score += 100;
            goedBeantwoord++;
            item.ToonKleur(true);
            UpdateScore();
        }
        else
        {
            item.ToonKleur(false);
        }

        currentQuestionIndex++;
        Invoke("VolgendeVraag", 1.5f);
    }

    void UpdateScore()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    void VolgendeVraag()
    {
        kanAntwoorden = true;
        ShowQuestion();
    }

    void ToonEindScherm()
    {
        eindScherm.SetActive(true);

        bool gewonnen = goedBeantwoord >= 5;
        string resultaat = gewonnen ? " Gewonnen!" : " Verloren!";

        eindTekst.text = resultaat + "\n\n" +
                         "Score: " + score + "\n" +
                         "Goed beantwoord: " + goedBeantwoord + " / " + questions.Count;
    }

  

    public void SluitAf() 
    {
        Debug.Log("Spel afgesloten");
        Application.Quit();
    }

    void CreateQuestions()
    {
        int aantalVragen = 10;
        for (int i = 0; i < aantalVragen; i++)
        {
            questions.Add(GenerateRandomQuestion());
        }
    }

    Question GenerateRandomQuestion()
    {
        int a = Random.Range(1, 11);
        int b = Random.Range(1, 11);
        int type = Random.Range(0, 3);
        int correctAnswer = 0;
        string questionText = "";

        switch (type)
        {
            case 0:
                correctAnswer = a + b;
                questionText = $"{a} + {b} = ?";
                break;
            case 1:
                correctAnswer = a - b;
                questionText = $"{a} - {b} = ?";
                break;
            case 2:
                correctAnswer = a * b;
                questionText = $"{a} × {b} = ?";
                break;
        }

        List<int> answers = new List<int>();
        answers.Add(correctAnswer);

        while (answers.Count < 5)
        {
            int fakeAnswer = correctAnswer + Random.Range(-10, 11);
            if (!answers.Contains(fakeAnswer))
                answers.Add(fakeAnswer);
        }

        for (int i = 0; i < answers.Count; i++)
        {
            int rnd = Random.Range(i, answers.Count);
            int temp = answers[i];
            answers[i] = answers[rnd];
            answers[rnd] = temp;
        }

        return new Question
        {
            questionText = questionText,
            correctAnswer = correctAnswer,
            answers = answers.ToArray()
        };
    }

    void ShuffleQuestions()
    {
        for (int i = 0; i < questions.Count; i++)
        {
            int randomIndex = Random.Range(i, questions.Count);
            Question temp = questions[i];
            questions[i] = questions[randomIndex];
            questions[randomIndex] = temp;
        }
    }

    void ShowQuestion()
    {
        if (currentQuestionIndex >= questions.Count)
        {
            ToonEindScherm();
            return;
        }

        Question q = questions[currentQuestionIndex];
        questionText.text = q.questionText;

        foreach (Transform child in answersParent)
        {
            Destroy(child.gameObject);
        }

        float spacing = 0.4f;
        float startX = -1.996f;
        float startY = -0.692f;
        float startZ = 0f;

        for (int i = 0; i < q.answers.Length; i++)
        {
            GameObject obj = Instantiate(answerPrefab, answersParent);
            float zPos = startZ + i * spacing;
            obj.transform.localPosition = new Vector3(startX, startY, zPos);
            AnswerItem item = obj.GetComponent<AnswerItem>();
            item.SetAnswer(q.answers[i]);
        }
    }
}