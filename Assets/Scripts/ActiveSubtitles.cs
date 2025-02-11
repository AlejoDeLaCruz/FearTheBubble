using UnityEngine;
using TMPro;
using System.Collections;

public class ActiveSubtitles : MonoBehaviour
{
    [Header("Configuración de Texto")]
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField][TextArea(3, 5)] private string fullText;
    [SerializeField] private float timeBetweenLetters = 0.05f;

    private string currentText = "";
    private bool isTyping = false;

    private void OnEnable()
    {
        StartDialogue();
    }

    public void StartDialogue()
    {
        if (!isTyping && dialogueText != null)
        {
            currentText = "";
            StartCoroutine(TypeText());
        }
    }

    private IEnumerator TypeText()
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in fullText)
        {
            // Se añade la letra directamente sin aplicar ningún tag de efecto.
            currentText += letter;
            dialogueText.text = currentText;

            // Espera entre letras
            yield return new WaitForSecondsRealtime(timeBetweenLetters);
        }

        isTyping = false;
    }

    public void SkipDialogue()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.text = fullText;
            isTyping = false;
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        dialogueText.text = "";
    }
}