using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayManager : ScenesManager
{
    public enum LEVELSTATE { STATE_START, STATE_TREENPC1, STATE_TREENPC2, STATE_END }

    private LEVELSTATE m_levelState = LEVELSTATE.STATE_START;

    private Player m_player;
    private GameObject m_playPanel;
    private GameObject m_overPanel;
    private PlayDialog m_dialog;

    private GameObject m_door;
    private Dictionary<string, Sprite> m_profileSpr = new Dictionary<string, Sprite>();


    public LEVELSTATE LevelState { get => m_levelState; set => m_levelState = value; }
    public PlayDialog Dialog => m_dialog;

    public Dictionary<string, Sprite> ProfileSpr => m_profileSpr;
    private Dictionary<string, AudioClip> m_effect = new Dictionary<string, AudioClip>();
    public Dictionary<string, AudioClip> Effect => m_effect;

    public Player Player => m_player;

    public PlayManager() : base()
    {
        m_sceneLevel = GameManager.SCENE.SCENE_PLAY;
        m_sceneName = "MainScene";
    }

    protected override void Load_Resource()
    {
        // 이미지
        m_profileSpr.Add("Portrait_NPC_Happy", GameManager.Ins.Load<Sprite>("1_Graphic/UI/Chating/Portrait_NPC_Happy"));
        m_profileSpr.Add("Portrait_NPC_Sad",   GameManager.Ins.Load<Sprite>("1_Graphic/UI/Chating/Portrait_NPC_Sad"));
        m_profileSpr.Add("Portrait_Player",    GameManager.Ins.Load<Sprite>("1_Graphic/UI/Chating/Portrait_Player"));
        m_profileSpr.Add("Portrait_Slime",     GameManager.Ins.Load<Sprite>("1_Graphic/UI/Chating/Portrait_Slime"));
        m_profileSpr.Add("Portrait_Tree",      GameManager.Ins.Load<Sprite>("1_Graphic/UI/Chating/Portrait_Tree"));
        m_profileSpr.Add("Portrait_Wolf",      GameManager.Ins.Load<Sprite>("1_Graphic/UI/Chating/Portrait_Wolf"));

        // 사운드
        // 플레이어
        m_effect.Add("Player_Melee",       GameManager.Ins.Load<AudioClip>("2_Sound/SFX/1_Player/Input/Attack/Melee"));
        m_effect.Add("Player_Ranged",      GameManager.Ins.Load<AudioClip>("2_Sound/SFX/1_Player/Input/Attack/Ranged"));
        m_effect.Add("Player_Melee_Dash",  GameManager.Ins.Load<AudioClip>("2_Sound/SFX/1_Player/Input/Attack/MA_Dash"));
        m_effect.Add("Player_Melee_Move",  GameManager.Ins.Load<AudioClip>("2_Sound/SFX/1_Player/Input/Attack/MA_Move"));
        m_effect.Add("Player_Ranged_Dash", GameManager.Ins.Load<AudioClip>("2_Sound/SFX/1_Player/Input/Attack/RA_Dash"));
        m_effect.Add("Player_Ranged_Move", GameManager.Ins.Load<AudioClip>("2_Sound/SFX/1_Player/Input/Attack/RA_Move"));

        m_effect.Add("Player_Dash", GameManager.Ins.Load<AudioClip>("2_Sound/SFX/1_Player/Input/Etc/Dash"));

        m_effect.Add("Player_Walk_0", GameManager.Ins.Load<AudioClip>("2_Sound/SFX/1_Player/Input/Etc/Walk/Walk_0"));
        m_effect.Add("Player_Walk_1", GameManager.Ins.Load<AudioClip>("2_Sound/SFX/1_Player/Input/Etc/Walk/Walk_1"));
        m_effect.Add("Player_Walk_2", GameManager.Ins.Load<AudioClip>("2_Sound/SFX/1_Player/Input/Etc/Walk/Walk_2"));
        m_effect.Add("Player_Walk_3", GameManager.Ins.Load<AudioClip>("2_Sound/SFX/1_Player/Input/Etc/Walk/Walk_3"));
        m_effect.Add("Player_Walk_4", GameManager.Ins.Load<AudioClip>("2_Sound/SFX/1_Player/Input/Etc/Walk/Walk_4"));

        m_effect.Add("Player_Hit",  GameManager.Ins.Load<AudioClip>("2_Sound/SFX/1_Player/Damaged/Hit"));
        m_effect.Add("Player_Dead", GameManager.Ins.Load<AudioClip>("2_Sound/SFX/1_Player/Damaged/Dead"));

        // NPC
        m_effect.Add("Npc_Tree", GameManager.Ins.Load<AudioClip>("2_Sound/SFX/3_Npc/TreeTalk"));

        // 몬스터
        m_effect.Add("Slime_Idle",   GameManager.Ins.Load<AudioClip>("2_Sound/SFX/2_Monster/Slime/idle"));
        m_effect.Add("Slime_Move",   GameManager.Ins.Load<AudioClip>("2_Sound/SFX/2_Monster/Slime/Move"));
        m_effect.Add("Slime_Attack", GameManager.Ins.Load<AudioClip>("2_Sound/SFX/2_Monster/Slime/Attack"));
        m_effect.Add("Slime_Hit",    GameManager.Ins.Load<AudioClip>("2_Sound/SFX/2_Monster/Slime/Hit"));
        m_effect.Add("Slime_Death",  GameManager.Ins.Load<AudioClip>("2_Sound/SFX/2_Monster/Slime/Death"));

        m_effect.Add("Mushroom_Walk",   GameManager.Ins.Load<AudioClip>("2_Sound/SFX/2_Monster/Mushroom/Walk"));
        m_effect.Add("Mushroom_Attack", GameManager.Ins.Load<AudioClip>("2_Sound/SFX/2_Monster/Mushroom/Attack"));
        m_effect.Add("Mushroom_Hit",    GameManager.Ins.Load<AudioClip>("2_Sound/SFX/2_Monster/Mushroom/Hit"));
        m_effect.Add("Mushroom_Death",  GameManager.Ins.Load<AudioClip>("2_Sound/SFX/2_Monster/Mushroom/Death"));

        m_effect.Add("KingSlime_Idle",       GameManager.Ins.Load<AudioClip>("2_Sound/SFX/2_Monster/KingSlime/idle"));
        m_effect.Add("KingSlime_Move",       GameManager.Ins.Load<AudioClip>("2_Sound/SFX/2_Monster/KingSlime/Move"));
        m_effect.Add("KingSlime_GroundSlam", GameManager.Ins.Load<AudioClip>("2_Sound/SFX/2_Monster/KingSlime/GroundSlam"));
        m_effect.Add("KingSlime_Summon",     GameManager.Ins.Load<AudioClip>("2_Sound/SFX/2_Monster/KingSlime/Summon"));
        m_effect.Add("KingSlime_Berserk",    GameManager.Ins.Load<AudioClip>("2_Sound/SFX/2_Monster/KingSlime/Berserk"));
        m_effect.Add("KingSlime_PhaseShift", GameManager.Ins.Load<AudioClip>("2_Sound/SFX/2_Monster/KingSlime/PhaseShift"));
        m_effect.Add("KingSlime_Hit",        GameManager.Ins.Load<AudioClip>("2_Sound/SFX/2_Monster/KingSlime/Hit"));
        m_effect.Add("KingSlime_Death",      GameManager.Ins.Load<AudioClip>("2_Sound/SFX/2_Monster/KingSlime/Death"));

        // UI
        m_effect.Add("UI_Button", GameManager.Ins.Load<AudioClip>("2_Sound/SFX/4_UI/Button"));
    }

    public override void Enter_Game()
    {
        base.Enter_Game();
    }

    protected override void Load_Scene()
    {
        // 초기화
        GameObject gameObject = GameObject.Find("Player");
        m_player = gameObject.GetComponent<Player>();

        GameObject canvas = GameObject.Find("Canvas");
        m_playPanel = canvas.transform.Find("Panel_Play").gameObject;
        m_overPanel = canvas.transform.Find("Panel_Over").gameObject;
        m_dialog = canvas.transform.Find("Panel_Dialog").gameObject.GetComponent<PlayDialog>();
        m_dialog.Initialize();

        m_levelState = LEVELSTATE.STATE_START;

        // 보스 출구 벽 생성
        m_door = GameManager.Ins.LoadCreate("4_Prefab/3_Map/Door");
        m_door.transform.position = new Vector3(137.5f, -28.52f, 0f);

        GameManager.Ins.UI.Start_FadeIn(1f, Color.black, () => Start_Game());
    }

    protected override void Start_Game()
    {
        base.Start_Game();
    }

    public override void Update_Game()
    {
    }

    public override void LateUpdate_Game()
    {
    }

    public override void Over_Game()
    {
        m_overPanel.SetActive(true);
    }

    public override void Exit_Game()
    {
    }

    public override void Set_Pause(bool pause)
    {
        if (m_player != null)
            m_player.Set_Pause(pause);
    }

    public void Clear_Game()
    {
        GameManager.Ins.Destroy(m_door);
    }

    public void Restart_Game()
    {
        m_overPanel.SetActive(false);
        m_player.Restart_Player(m_levelState);

        GameManager.Ins.Sound.Play_AudioSourceBGM("Play_Basic", true, 1f);
        GameManager.Ins.UI.Start_FadeIn(1f, Color.black, () => Start_Game());
    }
}
