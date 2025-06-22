
    using CleverCrow.Fluid.Dialogues.Graphs;
    using UnityEngine;

    public class GameController: MonoBehaviour
    {
        [SerializeField] private DialogueGraph IntroDialog;
        [SerializeField] private DialogueGraph MainDialog;
        private void Awake()
        {
            Debug.Log($"Game controller Awake");
            EventBus.Broadcast(new StartDialogEvent() {dialogue = IntroDialog});
        }
    }
