using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Random = System.Random;

namespace Dialog.Scripts
{
    /// <summary>
    /// Handles the NPC interaction with a player.
    /// Takes in a text file with a conversation to be displayed.
    /// Passes the file to the DialogManger to parse.
    /// </summary>
    public class TutDialogBehaviour : DialogBehaviour
    {
        [Header("ShopDialogBehaviour Child Vars")]
        [SerializeField] [Tooltip("Has a list of greetings messages picked at random.")]
        private List<TextAsset> greetingTextFiles; 
        [SerializeField] [Tooltip("Has a list of farewell messages picked at random.")]
        private List<TextAsset> farewellTextFiles;
        
        [SerializeField] private GameObject uiPanelsTalking;
        [SerializeField] private GameObject uiPanelsShopInfo;
        [SerializeField] private TextMeshProUGUI descDisplay;
        // [SerializeField] private Animator nextPageIcon; // stretch goal: bouncing continue animator

        public UnityEvent enableGrab;
        
        // used to pick greeting and farewell
        private Random rand = new();
        private int[] last3Greetings = new int[3];
        private int[] last3Farewells = new int[3];

        protected override void Awake()
        {
            doneTalking = true;
            
            // testing, uncomment for production
            // ShowShopInterface();
        }

        /// <summary>
        /// Passes the dialog to display and the text panel to the
        /// DialogManager to parse and display.
        /// Triggered by interacting with NPCs (Unity Event).
        /// </summary>
        public override void StartDialog()
        {
            isConvoOngoing = true;
            
            // decide which conversation to show
            int randIntGreet = rand.Next(greetingTextFiles.Count);
            // don't show the player the past 3 conversations they heard before
            while (ConversationIsStale(randIntGreet, last3Greetings))
            {
                randIntGreet = rand.Next(greetingTextFiles.Count);
            }

            convoQueue = dialogParser.ParseTextFileAsQueue(greetingTextFiles[randIntGreet]);
            
            uiPanelsTalking.SetActive(true); // open dialog panel
            currSpeech = convoQueue.Dequeue();
            StartCoroutine(TypeCurrSpeech(currSpeech.speech, descDisplay, currSpeech.speed));
            
        }

        private bool ConversationIsStale(int curr, int[] prevSelections)
        {
            return prevSelections.Contains(curr);
        }

        /// <summary>
        /// Enters 'buying mode', that shows the player how much
        /// each item costs when grabbed, their cash, and the total bill.
        /// </summary>
        public void ShowShopInterface()
        {
            uiPanelsShopInfo.SetActive(true);
            // UpdateCartTotal(0);
        }
        
        /// <summary>
        /// Closes the text panel and stops talking.
        ///
        /// Triggered by Cancel input action.
        /// Triggered by the final convo in the queue
        /// </summary>
        public override void EndDialog()
        {
            StopCoroutine(nameof(TypeCurrSpeech));
            enableGrab.Invoke(); // triggers shopMgr::EnableGrab
        }

        /// <summary>
        /// Says goodbye to the player. Triggered by Checkout Unity Event.
        ///
        /// Different from merely closing the dialog pane
        /// </summary>
        public IEnumerator EndInteraction()
        {
            int randIntFarewell = rand.Next(greetingTextFiles.Count);
            while (ConversationIsStale(randIntFarewell, last3Farewells))
            {
                randIntFarewell = rand.Next(greetingTextFiles.Count);
            }

            convoQueue = dialogParser.ParseTextFileAsQueue(farewellTextFiles[randIntFarewell]);
            currSpeech = convoQueue.Dequeue();
            yield return StartCoroutine(TypeCurrSpeech(currSpeech.speech, descDisplay, currSpeech.speed));
            EndDialog();
        }
    }
}
