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
    List<string> keywords = new List<string> { "increase frequency", "decrease frequency", "increase span", "decrease span", "increase amplitude", "decrease amplitude"};
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Adding keywords to recognizer");
        keywordRecognizer = new KeywordRecognizer(keywords.ToArray());
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
        if (args.text.Contains("frequency"))
        {
            ChangeFrequency cf = GameObject.Find("Plane").GetComponent<ChangeFrequency>();
            if (args.text.Contains("up")) {
				cf.IncreaseFrequency();
			}
			else if (args.text.Contains("down")) {
				cf.DecreaseFrequency();
			}
        }
        else if (args.text.Contains("span"))
        {
            ChangeSpan csp = GameObject.Find("Plane").GetComponent<ChangeSpan>();
            if (args.text.Contains("up")) {
				csp.IncreaseSpan();
			}
			else if (args.text.Contains("down")) {
				csp.DecreaseSpan();
			}
        }
        else if (args.text.Contains("amplitude")) {
            ChangeRefLevel crl = GameObject.Find("Plane").GetComponent<ChangeRefLevel>();
            if (args.text.Contains("up")) {
				crl.IncreaseRefLevel();
			}
			else if (args.text.Contains("down")) {
				crl.DecreaseRefLevel();
			}
        }
    }
}
