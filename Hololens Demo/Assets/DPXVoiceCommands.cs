using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class DPXVoiceCommands : MonoBehaviour
{
    //KeywordRecognizer keywordRecognizer;
    //Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();
    // Start is called before the first frame update
    void Start()
    {
        /*keywords.Add("change frequency", () => {
            ChangeFrequency cfChange = (ChangeFrequency)GameObject.Find("frequency").GetComponent(typeof(ChangeFrequency));
            //cfChange.UpdateFrequency();
        });
        keywords.Add("change reference level", () => {
            ChangeRefLevel refLevelChange = (ChangeRefLevel)GameObject.Find("refLevel").GetComponent(typeof(ChangeRefLevel));
            refLevelChange.UpdateRefLevel();
        });
        keywords.Add("change span", () => {
            ChangeSpan spanChange = (ChangeSpan)GameObject.Find("span").GetComponent(typeof(ChangeSpan));
            spanChange.UpdateSpan();
        });
        keywords.Add("change bandwidth", () => {
            ChangeRBW rbwChange = (ChangeRBW)GameObject.Find("rbw").GetComponent(typeof(ChangeRBW));
            //rbwChange.UpdateRBW();
        });
        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += ChangeSetting_OnPhraseRecognized;
        keywordRecognizer.Start();*/
    }

    // Update is called once per frame
    void Update()
    {

    }

    /*private void ChangeSetting_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        System.Action keywordAction;

        if (keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();
        }
    }*/
}
