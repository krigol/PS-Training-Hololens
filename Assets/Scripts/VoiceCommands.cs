using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class VoiceCommands : MonoBehaviour {
    KeywordRecognizer keywordRecognizer;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

	// Use this for initialization
	void Start () {
        keywords.Add("Reset blocks", () =>
        {
            BroadcastMessage("OnResetBlock");
        });
        keywords.Add("Shoot ball", () =>
        {
            BroadcastMessage("OnShoot");
        });
        keywords.Add("Close app", () =>
        {
            BroadcastMessage("CloseApp");
        });
        keywords.Add("Get cubes", () =>
        {
            BroadcastMessage("GetCubes");
        });

        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());

        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();
	}

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        Action keywordAction;
        if (keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();
        }
    }
}
