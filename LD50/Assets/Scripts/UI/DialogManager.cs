using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{

    UIManager uiMan;
    Text dialogueText;
    Button button;

    // Start is called before the first frame update
    void Start()
    {
        uiMan = GetComponentInParent<UIManager>();
        button = GetComponentInChildren<Button>();
        dialogueText = GetComponentInChildren<Text>();
        button.onClick.AddListener(() => uiMan.QuitDialaguePause());
        gameObject.SetActive(false);

        //StartDialogue();
    }

    public void SetText(string dialogue)
    {
        dialogueText.text = dialogue;
    }
    

    void StartDialogue()
    {
        uiMan.DialaguePause();
    }
}
