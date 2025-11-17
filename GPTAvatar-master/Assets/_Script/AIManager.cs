//If you've bought the Salsa Suite plugin and installed it, you should uncomment the next line to enable lipsyncing.
//If you don't have it, leave it commented; el proyecto compilará sin lipsync ni movimientos de ojos.
//#define CRAZY_MINNOW_PRESENT

// SIN SALSA: no usamos CrazyMinnow.SALSA ni defines.

using SimpleJSON;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using static OpenAITextCompletionManager;

public class AIManager : MonoBehaviour
{
    public MicRecorder _microPhoneScript;
    string _openAI_APIKey;
    string _openAI_APIModel;
    string _googleAPIkey;
    string _elevenLabsAPIkey;

    public GameObject _visuals;
    AudioSource _audioSourceToUse = null;

    Vector2 vTextOverlayPos = new Vector2(Screen.width * 0.58f, (float)Screen.height - ((float)Screen.height * 0.4f));
    Vector2 vStatusOverlayPos = new Vector2(Screen.width * 0.44f, (float)Screen.height - ((float)Screen.height * 1.1f));

    public TMPro.TextMeshProUGUI _dialogText;
    public TMPro.TextMeshProUGUI _statusText;

    Queue<GTPChatLine> _chatHistory = new Queue<GTPChatLine>();

    public Button _recordButton;

    Friend _activeFriend;
    Animator _animator = null;

    public TMPro.TextMeshProUGUI _friendNameGUI;

    private void OnDestroy()
    {
    }

    public void SetActiveFriend(Friend newFriend)
    {
        _activeFriend = newFriend;
        if (newFriend == null) return;

        // Audio por defecto
        _audioSourceToUse = gameObject.GetComponent<AudioSource>();
        _friendNameGUI.text = _activeFriend._name;

        if (_friendNameGUI.text == "Unset")
        {
            _dialogText.text = "Before running this, edit the config_template.txt file to set your API keys, then rename the file to config.txt!";
            return;
        }

        _dialogText.text = "Click Start for the character to introduce themselves.";
        _statusText.text = "";

        ForgetStuff();

        List<GameObject> objs = new List<GameObject>();
        RTUtil.AddObjectsToListByNameIncludingInactive(_visuals, "char_visual", true, objs);

        foreach (GameObject obj in objs)
        {
            obj.SetActive(false);
        }

        // Encender el visual que necesitamos
        var activeVisual = RTUtil.FindInChildrenIncludingInactive(_visuals, "char_visual_" + _activeFriend._visual);
        if (activeVisual != null)
        {
            activeVisual.SetActive(true);
        }

        // SIN SALSA: solo usamos el Animator para flags Listening/Talking
        _animator = activeVisual != null ? activeVisual.GetComponentInChildren<Animator>() : null;

        SetListening(false);
    }

    void SetListening(bool bNew)
    {
        if (_animator != null)
        {
            _animator.SetBool("Listening", bNew);
        }
    }

    void SetTalking(bool bNew)
    {
        if (_animator != null)
        {
            _animator.SetBool("Talking", bNew);
        }
    }

    public void SetGoogleAPIKey(string key)
    {
        _googleAPIkey = key;
    }

    public void SetOpenAI_APIKey(string key)
    {
        _openAI_APIKey = key;
    }

    public void SetOpenAI_Model(string model)
    {
        _openAI_APIModel = model;
    }

    public void SetElevenLabsAPIKey(string key)
    {
        _elevenLabsAPIkey = key;
    }

    public string GetAdvicePrompt()
    {
        return _activeFriend._advicePrompt;
    }

    public void ModFriend(int mod)
    {
        int curFriendIndex = _activeFriend._index;

        int newFriendIndex = (curFriendIndex + mod) % Config.Get().GetFriendCount();
        if (newFriendIndex < 0) newFriendIndex = Config.Get().GetFriendCount() - 1;
        SetActiveFriend(Config.Get().GetFriendByIndex(newFriendIndex));
    }

    public void PlayClickSound()
    {
        RTMessageManager.Get().Schedule(0, RTAudioManager.Get().PlayEx, "muffled_bump", 0.5f, 1.0f, false, 0.0f);
    }

    public void OnPreviousFriend()
    {
        PlayClickSound();
        ModFriend(-1);
    }

    public void OnNextFriend()
    {
        PlayClickSound();
        ModFriend(1);
    }

    void Start()
    {
    }

    public int CountWords(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return 0;
        }

        string[] words = input.Split(new char[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
        return words.Length;
    }

    string GetBasePrompt()
    {
        return _activeFriend._basePrompt;
    }

    string GetDirectionPrompt()
    {
        return _activeFriend._directionPrompt;
    }

    void TrimHistoryIfNeeded()
    {
        int tokenSize = CountWords(GetBasePrompt());
        int historyTokenSize = 0;

        foreach (GTPChatLine line in _chatHistory)
        {
            historyTokenSize += CountWords(line._content);
        }

        int maxTokenUseForPromptsAndHistory = tokenSize + _activeFriend._friendTokenMemory;

        if (tokenSize + historyTokenSize > maxTokenUseForPromptsAndHistory)
        {
            while (tokenSize + historyTokenSize > maxTokenUseForPromptsAndHistory && _chatHistory.Count > 0)
            {
                GTPChatLine line = _chatHistory.Dequeue();
                historyTokenSize -= CountWords(line._content);

                if (_chatHistory.Count == 0) break;
                line = _chatHistory.Dequeue();
                historyTokenSize -= CountWords(line._content);

                if (_chatHistory.Count == 0) break;
                line = _chatHistory.Dequeue();
                historyTokenSize -= CountWords(line._content);
            }
        }

        Debug.Log("Prompt tokens: " + tokenSize + " History token size:" + historyTokenSize);
    }

    void GetGPT3Text(string question)
    {
        OpenAITextCompletionManager textCompletionScript = gameObject.GetComponent<OpenAITextCompletionManager>();
        Queue<GTPChatLine> lines = new Queue<GTPChatLine>();

        lines.Enqueue(new GTPChatLine("system", GetBasePrompt()));

        TrimHistoryIfNeeded();

        foreach (GTPChatLine line in _chatHistory)
        {
            lines.Enqueue(line);
        }

        lines.Enqueue(new GTPChatLine("system", GetDirectionPrompt()));
        lines.Enqueue(new GTPChatLine("user", question));

        string json = textCompletionScript.BuildChatCompleteJSON(
            lines,
            _activeFriend._maxTokensToGenerate,
            _activeFriend._temperature,
            _openAI_APIModel
        );

        RTDB db = new RTDB();
        db.Set("question", question);
        db.Set("role", "user");

        textCompletionScript.SpawnChatCompleteRequest(json, OnGPT3TextCompletedCallback, db, _openAI_APIKey);
        UpdateStatusText(RTUtil.ConvertSansiToUnityColors("(AI is thinking) You said: `$" + question + "``"), 20);
    }

    void OnGPT3TextCompletedCallback(RTDB db, JSONObject jsonNode)
    {
        if (jsonNode == null)
        {
            Debug.Log("Got callback! Data: " + db.ToString());
            UpdateStatusText(db.GetString("msg"));
            return;
        }

        string reply = jsonNode["choices"][0]["message"]["content"];
        if (reply.Length < 5)
        {
            Debug.Log("Error parsing reply: " + reply);
            db.Set("english", "Error. I don't know what to say.");
            db.Set("japanese", "エラーです。なんて言っていいのかわからない。");
            SayText(db);
            return;
        }

        db.Set("english", reply);
        db.Set("japanese", reply);

        SayText(db);

        _chatHistory.Enqueue(new GTPChatLine(db.GetString("role"), db.GetString("question")));
        _chatHistory.Enqueue(new GTPChatLine("assistant", reply));
    }

    void SayText(RTDB db)
    {
        string text = db.GetString(_activeFriend._language);
        string json;
        int sampleRate = 22050;

        if (_activeFriend._elevenLabsVoice.Length > 1 && _elevenLabsAPIkey.Length > 1)
        {
            string countryCode = _activeFriend._elevenLabsVoice.Substring(0, 5);
            ElevenLabsTextToSpeechManager ttsScript = gameObject.GetComponent<ElevenLabsTextToSpeechManager>();
            json = ttsScript.BuildTTSJSON(text, _activeFriend._elevenlabsStability);
            ttsScript.SpawnTTSRequest(json, OnTTSCompletedCallbackElevenLabs, db, _elevenLabsAPIkey, _activeFriend._elevenLabsVoice);
            UpdateStatusText("Clearing throat...", 20);
        }
        else if (_activeFriend._googleVoice.Length > 1 && _googleAPIkey.Length > 1)
        {
            string countryCode = _activeFriend._googleVoice.Substring(0, 5);
            GoogleTextToSpeechManager ttsScript = gameObject.GetComponent<GoogleTextToSpeechManager>();
            json = ttsScript.BuildTTSJSON(text, countryCode, _activeFriend._googleVoice, sampleRate, _activeFriend._pitch, _activeFriend._speed);
            ttsScript.SpawnTTSRequest(json, OnTTSCompletedCallback, db, _googleAPIkey);
            UpdateStatusText("Clearing throat...", 20);
        }
        else
        {
            // Sin TTS configurado: solo mostramos el texto
            UpdateDialogText(db.GetString("japanese"));
            UpdateStatusText("");
        }
    }

    void OnTTSCompletedCallback(RTDB db, byte[] wavData)
    {
        if (wavData == null)
        {
            Debug.Log("Error getting wav: " + db.GetString("msg"));
        }
        else
        {
            GoogleTextToSpeechManager ttsScript = gameObject.GetComponent<GoogleTextToSpeechManager>();
            AudioSource audioSource = _audioSourceToUse;
            audioSource.clip = ttsScript.MakeAudioClipFromWavFileInMemory(wavData);
            audioSource.Play();
        }

        UpdateDialogText(db.GetString("japanese"));
        UpdateStatusText("");
    }

    void OnTTSCompletedCallbackElevenLabs(RTDB db, AudioClip clip)
    {
        if (clip == null)
        {
            Debug.Log("Error getting wav: " + db.GetString("msg"));
        }
        else
        {
            ElevenLabsTextToSpeechManager ttsScript = gameObject.GetComponent<ElevenLabsTextToSpeechManager>();
            AudioSource audioSource = _audioSourceToUse;
            audioSource.clip = clip;
            audioSource.Play();
        }

        UpdateDialogText(db.GetString("japanese"));
        UpdateStatusText("");
    }

    public void ProcessMicAudioByFileName(string fAudioFileName)
    {
        OpenAISpeechToTextManager speechToTextScript = gameObject.GetComponent<OpenAISpeechToTextManager>();

        byte[] fileBytes = System.IO.File.ReadAllBytes(fAudioFileName);
        string prompt = "";

        RTDB db = new RTDB();

        // Construir prompt con algo de historial
        foreach (GTPChatLine line in _chatHistory)
        {
            prompt += line._content + "\n";
            if (prompt.Length > 180)
            {
                break;
            }
        }

        if (prompt == "")
        {
            prompt = _activeFriend._basePrompt;
        }

        speechToTextScript.SpawnSpeechToTextRequest(prompt, OnSpeechToTextCompletedCallback, db, _openAI_APIKey, fileBytes);
        UpdateStatusText("Understanding speech...", 20);
    }

    void OnSpeechToTextCompletedCallback(RTDB db, JSONObject jsonNode)
    {
        if (jsonNode == null)
        {
            Debug.Log("Got callback! Data: " + db.ToString());
            UpdateStatusText(db.GetString("msg"));
            return;
        }

        string reply = jsonNode["text"];
        UpdateStatusText("Heard: " + reply);
        GetGPT3Text(reply);
    }

    public void ToggleRecording()
    {
        if (!_microPhoneScript.IsRecording())
        {
            StopTalking();
            Debug.Log("Recording started");
            _recordButton.GetComponent<Image>().color = Color.red;
            _microPhoneScript.StartRecording();
            PlayClickSound();
            SetListening(true);
        }
        else
        {
            _recordButton.GetComponent<Image>().color = Color.white;
            PlayClickSound();
            string outputFileName = Application.temporaryCachePath + "/temp.wav";
            _microPhoneScript.StopRecordingAndProcess(outputFileName);
            SetListening(false);
        }
    }

    public void OnStopButton()
    {
        PlayClickSound();
        StopTalking();
    }

    public void OnCopyButton()
    {
        PlayClickSound();
        string text = _dialogText.text;
        if (text.Length > 0)
        {
            GUIUtility.systemCopyBuffer = text;
            UpdateStatusText("Copied to clipboard");
        }
        else
        {
            UpdateStatusText("Nothing to copy");
        }
    }

    public void OnAdviceButton()
    {
        ForgetStuff();
        PlayClickSound();

        OpenAITextCompletionManager textCompletionScript = gameObject.GetComponent<OpenAITextCompletionManager>();
        Queue<GTPChatLine> lines = new Queue<GTPChatLine>();
        lines.Enqueue(new GTPChatLine("system", GetBasePrompt()));

        TrimHistoryIfNeeded();

        foreach (GTPChatLine line in _chatHistory)
        {
            lines.Enqueue(line);
        }

        string question = GetAdvicePrompt();

        lines.Enqueue(new GTPChatLine("system", GetDirectionPrompt()));
        lines.Enqueue(new GTPChatLine("system", question));

        string json = textCompletionScript.BuildChatCompleteJSON(
            lines,
            _activeFriend._maxTokensToGenerate,
            _activeFriend._temperature,
            _openAI_APIModel
        );

        RTDB db = new RTDB();
        db.Set("role", "system");
        db.Set("question", question);

        textCompletionScript.SpawnChatCompleteRequest(json, OnGPT3TextCompletedCallback, db, _openAI_APIKey);
        UpdateStatusText(RTUtil.ConvertSansiToUnityColors("Thinking..."), 20);
        UpdateDialogText("");
    }

    public void StopTalking()
    {
        if (_audioSourceToUse != null)
        {
            _audioSourceToUse.Stop();
        }
        SetTalking(false);
    }

    public void ForgetStuff()
    {
        _chatHistory.Clear();
        StopTalking();
    }

    public void OnForgetButton()
    {
        PlayClickSound();
        ForgetStuff();
    }

    public int GetFriendIndex()
    {
        if (_activeFriend == null)
            return 0;
        else
            return _activeFriend._index;
    }

    void UpdateStatusText(string msg, float timer = 3)
    {
        _statusText.text = msg;
    }

    void UpdateDialogText(string msg)
    {
        _dialogText.text = msg;
    }

    private void Update()
    {
        if (_audioSourceToUse != null)
        {
            SetTalking(_audioSourceToUse.isPlaying);
        }
    }
}
