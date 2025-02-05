    using UnityEngine;
    using TMPro;
using UnityEngine.UI;

public class EmailManager : MonoBehaviour
    {
        //Reference to the text field that will display the mail content
        public  TextMeshProUGUI mailTextField;

        public string[] allMails;
        public GameState gameState;

        public Button[] allMailButtons;

        private float time = 0f;

        // this funtion is called from the mail buttons in the email panel
        public void ShowMail(int index)
        {
            if (index < 0 || index >= allMails.Length)
            {
                Debug.LogError("Invalid index");
                return;
            }

            mailTextField.text = allMails[index];
        }

        public void Update()
        {   
            time += Time.deltaTime;

            // set the conditions for the mail buttons to be active 
            allMailButtons[0].gameObject.SetActive(time > 5f);
            allMailButtons[1].gameObject.SetActive(time > 20f);
            allMailButtons[2].gameObject.SetActive(gameState.photoEntry);
            allMailButtons[3].gameObject.SetActive(gameState.sprayedEntry);
            allMailButtons[4].gameObject.SetActive(gameState.photoIntruder);
            allMailButtons[5].gameObject.SetActive(gameState.photoVentilation);
            allMailButtons[6].gameObject.SetActive(gameState.photoWaste);
            allMailButtons[7].gameObject.SetActive(gameState.photoSpider);
            allMailButtons[8].gameObject.SetActive(gameState.photoEggs);
            allMailButtons[9].gameObject.SetActive(gameState.smelledWarriors);
            allMailButtons[10].gameObject.SetActive(gameState.sprayedSpider);
            allMailButtons[11].gameObject.SetActive(gameState.photoQueen);
            allMailButtons[12].gameObject.SetActive(gameState.smelledSex);
            allMailButtons[13].gameObject.SetActive(gameState.photoFlight);
        }
    }
