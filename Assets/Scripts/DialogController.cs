using System.Collections.Generic;
using UnityEngine;
using CleverCrow.Fluid.Dialogues;
using CleverCrow.Fluid.Dialogues.Choices;
using CleverCrow.Fluid.Dialogues.Graphs;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DialogController : MonoBehaviour
{ 
    private DialogueController _controller;
    [SerializeField] private TMP_Text _dialogBox;
    [SerializeField] private TMP_Text _speakerNameTextBox;
    [SerializeField] private GameObject _dialogUIParent;
    [SerializeField] private Image _dialogBoxBackground;
    [SerializeField] private Transform _choicesContainer;
    [SerializeField] private Image _speakerImage;
    
    [SerializeField] private DialogueGraph _dialogueGraph;

    [SerializeField] private GameObject _dialogChoicePrefab;

    private bool dialogActive;

    private List<GameObject> _spawnedChoices = new();
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var database = new DatabaseInstanceExtended();
        _controller = new DialogueController(database);
        _controller.Events.Speak.AddListener(HandleSpeak);
        _controller.Events.Begin.AddListener(HandleDialogBegin);
        _controller.Events.Choice.AddListener(HandleChoices);
        _controller.Events.End.AddListener(HandleDialogEnd);

        ClearText();
        _controller.Play(_dialogueGraph);
        
        EventBus.Subscribe<StartDialogEvent>(HandleStartDialog);
    }

    private void ClearText()
    {
        _speakerNameTextBox.text = "";
        _dialogBox.text = "";
        _speakerImage.enabled = false;
    }

    private void HandleChoices(IActor speaker, string message, List<IChoice> choices)
    {
        foreach (var choice in choices)
        {
            var button = Instantiate(_dialogChoicePrefab, _choicesContainer);
            button.GetComponentInChildren<TextMeshProUGUI>().text = choice.Text;
            button.GetComponentInChildren<Button>().onClick.AddListener(() =>
            {
                _controller.SelectChoice(choices.IndexOf(choice));
                ClearChoices();
            });
            
            _spawnedChoices.Add(button);
        }
        
        HandleSpeak(speaker, message);
    }

    private void ClearChoices()
    {
        foreach (var choice in _spawnedChoices)
        {
            Destroy(choice);
        }
    }

    private void HandleDialogEnd()
    {
        _dialogUIParent.SetActive(false);
        ClearText();
    }

    private void HandleDialogBegin()
    {
        _dialogUIParent.SetActive(true);
        dialogActive = true;
    }

    private void HandleStartDialog(StartDialogEvent obj)
    {
        Debug.Log($"Starting dialogue: {obj.dialogue}");
        _controller.Play(obj.dialogue);
    }

    private void HandleSpeak(IActor speaker, string message)
    {
        _dialogBoxBackground.enabled = true;
        if (speaker != null)
        {
            _speakerNameTextBox.text = speaker.DisplayName;
            _speakerImage.enabled = true;
        }
        else
        {
            _speakerNameTextBox.text = "";
            _speakerImage.enabled = false;
        }
        
        _dialogBox.text = message;
    }

    // Update is called once per frame
    void Update()
    {
        if (!dialogActive) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _controller.Next();
        }
    }
}
