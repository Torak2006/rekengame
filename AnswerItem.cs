using UnityEngine;
using TMPro;

public class AnswerItem : MonoBehaviour
{
    public TextMeshPro answerText;
    private int answerValue;
    private Renderer meshRenderer;

    void Awake()
    {
        meshRenderer = GetComponent<Renderer>();
    }

    public void SetAnswer(int value)
    {
        answerValue = value;
        answerText.text = value.ToString();
    }

    public void OnClick()
    {
        GameManager gm = FindObjectOfType<GameManager>();
        gm.AnswerSelected(answerValue, this); 
    }

    public void ToonKleur(bool correct)
    {
        if (correct)
            meshRenderer.material.color = Color.green;
        else
            meshRenderer.material.color = Color.red;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("left hand") || other.CompareTag("right hand"))
        {
            GameManager gm = FindObjectOfType<GameManager>();
            gm.AnswerSelected(answerValue, this);
        }
    }
}