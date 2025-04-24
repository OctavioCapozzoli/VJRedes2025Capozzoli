using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TagManager
{
    #region Character_Tags

    public const string PLAYER_TAG = "Player";
    public const string ENEMY_TAG = "Enemy";

    #endregion

    #region Scene_Management_Tags
    public const string LEVELS_MANAGER_TAG = "Levels_Manager";
    public const string MAIN_MENU_SCREEN_TAG = "Main_Menu";
    public const string GAME_SCREEN_TAG = "Game";
    public const string WIN_SCREEN_TAG = "Win";
    public const string GAME_OVER_SCREEN_TAG = "Game_Over";
    #endregion

    #region Utilities_Tags
    public const string GAME_MANAGER_TAG = "Game_Manager";
    public const string MAIN_CAMERA_TAG = "MainCamera";
    public const string SPAWNPOINT_TAG = "SpawnPoint";
    #endregion
}
