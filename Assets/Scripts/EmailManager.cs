using UnityEngine;
using TMPro;

public class EmailManager : MonoBehaviour
{
    //Reference to the text field that will display the mail content
    [SerializeField] private  TextMeshProUGUI mailTextField;

    public string[] allMails;

    public void ShowMail(int index)
    {
        if (index < 0 || index >= allMails.Length)
        {
            Debug.LogError("Invalid index");
            return;
        }

        mailTextField.text = allMails[index];
    }
}
