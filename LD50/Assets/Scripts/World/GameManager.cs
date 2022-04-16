using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;


public enum FlowerTypes
{
    NONE,
    DOUBLE_JUMP,
    DASH,
    ATTACK
}


public class GameManager
{
    //private static GameManager instance;

    public static UIManager uiManager;
    public static List<FlowerTypes> achivedFlowers = new List<FlowerTypes>();

    public static FlowerTypes lastFlower = FlowerTypes.NONE;

    public static Player Player;


    public async static void GameOver()
    {
        //Debug.Log("GAME OVER");

        await Task.Delay(2000);

        //if (uiManager != null)
        //    uiManager.ShowDialogue("Game Over");

        //await Task.Delay(200);

        RestartLevel();
    }

    public static void SetUIManager(UIManager uiManager)
    {
        GameManager.uiManager = uiManager;
    }

    public async static void SetPlayer(Player Player)
    {
        GameManager.Player = Player;

        await Task.Delay(100);

        ShowInitText();
    }

    public async static void ShowInitText()
    {
        var text = "";

        if (lastFlower == FlowerTypes.NONE)
            text = "Rumour has it that if someone were to acquire three mythical flowers hidden in the land, they would stop aging. And so the king set on a quest to find these flowers, for he desired to remain young and powerful";
        else if (lastFlower == FlowerTypes.DASH)
            text = "As the king set on his quest for the next flower, he felt age was affecting his agility.";
        else if (lastFlower == FlowerTypes.DOUBLE_JUMP)
            text = "Upon starting his next journey, the king realized he couldn't jump as high as he used to.";
        else if (lastFlower == FlowerTypes.ATTACK)
            text = "Age was taking its toll on the king... he was so weakened he couldn't even swing his blade.";

        if (uiManager != null)
            uiManager.ShowDialogue(text);
    }

    public static void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public async static void FlowerAchived(FlowerTypes type)
    {
        achivedFlowers.Add(type);

        //await Task.Delay(200);

        if (uiManager != null)
            uiManager.ShowDialogue("And so, the king found one of the mythical flowers.");

        await Task.Delay(2000);

        lastFlower = type;
        RestartLevel();

    }

}
