using UnityEngine;

public static class PlayerData
{
    public static int coin = 0;
    public static bool isAiMode = false;

    public static void load()
    {
        coin = PlayerPrefs.GetInt("coin",0);
        isAiMode = PlayerPrefs.GetInt("aimode",0) == 1; //1 = true 0 = false
    }
    public static void save()
    {
        PlayerPrefs.SetInt("coin",coin);
        PlayerPrefs.SetInt("aimode",isAiMode ? 1 : 0);
    }

}
