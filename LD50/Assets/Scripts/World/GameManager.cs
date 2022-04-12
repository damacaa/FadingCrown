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
            text = "After obtaining the flower below the land, the king could not move as quickly as before";
        else if (lastFlower == FlowerTypes.DOUBLE_JUMP)
            text = "When the king obtained the flower in the sky, he stopped being able to jump as high as before";
        else if (lastFlower == FlowerTypes.ATTACK)
            text = "The king finally got to the flower after encountering a myriad of enemies, but he became so weak that he couldn't even swing his blade.";

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
            uiManager.ShowDialogue("After great endeavors, our King hopes he can find the peace he longes...");

        await Task.Delay(2000);

        lastFlower = type;
        RestartLevel();

    }

}
