using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Windows.Speech;
using static RSAAPITest;

public class DPXVoiceCommands : MonoBehaviour
{
    KeywordRecognizer keywordRecognizer;
    Dictionary<string, Action> keywords = new Dictionary<string, Action>();
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Adding keywords to recognizer"); 
        keywords.Add("increase frequency", () =>
        {
            Debug.Log(gameObject);
            gameObject.GetComponent<ChangeFrequency>().IncreaseFrequency();
        });
        keywords.Add("decrease frequency", () =>
        {
            Debug.Log(gameObject);
            gameObject.GetComponent<ChangeFrequency>().DecreaseFrequency();
        });
        keywords.Add("increase span", () =>
        {
            Debug.Log(gameObject);
            gameObject.GetComponent<ChangeSpan>().IncreaseSpan();
        });
        keywords.Add("decrease span", () =>
        {
            Debug.Log(gameObject);
            gameObject.GetComponent<ChangeSpan>().DecreaseSpan();
        });
        keywords.Add("increase amplitude", () =>
        {
            Debug.Log(gameObject);
            gameObject.GetComponent<ChangeRefLevel>().IncreaseRefLevel();
        });
        keywords.Add("decrease amplitude", () =>
        {
            Debug.Log(gameObject);
            gameObject.GetComponent<ChangeRefLevel>().DecreaseRefLevel();
        });
        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += ChangeSetting_OnPhraseRecognized;
        keywordRecognizer.Start();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Recognizes phrases in the form '<[frequency, span, amplitude]> <up/down>'
    private void ChangeSetting_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        Debug.Log("phrase recognized: " + args.text);
        if (keywords.TryGetValue(args.text, out Action keywordAction))
        {
            keywordAction.Invoke();
        }
    }
}
