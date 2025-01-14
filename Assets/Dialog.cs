using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialog : MonoBehaviour
{
    [SerializeField] GameObject dialogbox;
    [SerializeField] TextMeshProUGUI textMeshPro;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TypeWriter(string Text, float TimeBettweenChars, float Endtime)
    {
        StartCoroutine(TypeWriteText(Text, TimeBettweenChars, Endtime));
    }

    IEnumerator TypeWriteText(string Text, float TimeBettweenChars, float Endtime)
    {
        dialogbox.SetActive(true);
        string TypedText = "";
        foreach (char c in Text)
        {
            TypedText += c;
            textMeshPro.text = TypedText;
            yield return new WaitForSeconds(TimeBettweenChars);
        }
        yield return new WaitForSeconds(Endtime);
        dialogbox.SetActive(false);
    }
}
