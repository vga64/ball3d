using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Boo.Lang;
using Boo.Lang.Runtime;
using CompilerGenerated;
using NATTraversal;
using Open.Nat;
using Steamworks;
using UnityEngine;
using UnityScript.Lang;

// Token: 0x020000C7 RID: 199
[Serializable]
public class GlobalManager : MonoBehaviour
{
    // Token: 0x06000275 RID: 629 RVA: 0x00033C1C File Offset: 0x00031E1C
    public GlobalManager()
    {
        this.defFilePath = string.Empty;
        this.dedicatedRankPass = string.Empty;
        this.dedicatedGTVPass = string.Empty;
        this.dedicatedDefTeamMax = 3;
        this.dedicatedUseOvertime = true;
        this.dedicatedAdminPassword = string.Empty;
        this.dedicatedVoteStadiumEnabled = true;
        this.botLevel = BotLevel.Beginner;
        this.topRanksGetTimer = -1f;
        this.yourRankIndex = -1;
        this.mouseSpeed = StaticVariables.defaultMouseSpeed;
        this.playerNamesEnabled = true;
        this.teamShirtsCode = string.Empty;
        this.aaLevel = 2;
        this.curveMode = CurveMode.QE;
        this.tribunesEnabled = 1;
        this.grass3DEnabled = true;
        this.cameraFov = StaticVariables.defaultCameraFov;
        this.vSync = 1;
        this.maxFps = 1;
        this.isSoundOn = true;
        this.isAmbientOn = true;
        this.isPlayerNamesOn = true;
        this.teamShirtsCodeOption = string.Empty;
        this.isPredictionOn = true;
        this.isGrass3DEnabled = true;
        this.localBasketBallIndex = 50;
        this.defKeys = new KeyCode[]
        {
            KeyCode.A,
            KeyCode.D,
            KeyCode.W,
            KeyCode.S,
            KeyCode.Space,
            KeyCode.Mouse0,
            KeyCode.LeftShift,
            KeyCode.Q,
            KeyCode.E,
            KeyCode.P,
            KeyCode.LeftAlt,
            KeyCode.H,
            KeyCode.Return,
            KeyCode.Backspace,
            KeyCode.V,
            KeyCode.F
        };
        this.keyNames = new string[]
        {
            "Move Left",
            "Move Right",
            "Move Forward",
            "Move Back",
            "Jump",
            "Kick",
            "Vertical Kick",
            "Curve Left",
            "Curve Right",
            "Pause",
            "Show Names",
            "Hide Effects",
            "Chat",
            "Team Chat",
            "Show Menu",
            "Emotion"
        };
        this.activeKeyIndex = -1;
        this.usePredictionTmp = true;
        this.steamTicketString = string.Empty;
        this.newsNumber = -1;
        this.masterServerFailedTimer = -1f;
        this.timeSinceTheLastRankGame = 99999f;
        this.jugglingCeilOffsetMax = 150f;
        this.jugglingCeilTimerFactor = 2f;
        this.jugglingCountDownMax = 3;
        this.jugglingAfterTime = 2;
        this.jugglingGravity = 600f;
        this.jugglingBallRadius = 31f;
        this.jugglingPointerRadius = 7.5f;
        this.jugglingBallDump = 0.99f;
        this.jugglingBallBounceDump = 0.7f;
        this.jugglingKickFactor = 1000f;
        this.jugglingIdleSpeed = 5f;
        this.jugglingBounceIdleSpeed = 100f;
        this.jugglingMaxSpeed = 3000f;
        this.jugglingCursorVisible = true;
        this.dontClickTime = 2f;
    }

    // Token: 0x06000277 RID: 631 RVA: 0x00033EE0 File Offset: 0x000320E0
    public virtual void Awake()
    {
        if (!this.isDedicated)
        {
            this.natHelper = new NATHelper();
            this.natHelper.findNatDevice(null);
        }
        int num = 0;
        num = PlayerPrefs.GetInt("firstRun20", 1);
        PlayerPrefs.SetInt("firstRun20", 0);
        if (!this.isDedicated)
        {
            PlayerPrefs.Save();
        }
        if (num == 1)
        {
            this.firstRun = true;
        }
        else
        {
            this.firstRun = false;
        }
        if (!GlobalManager.instance)
        {
            GlobalManager.instance = this;
            if (!this.isDedicated || this.cameraDedicated)
            {
            }
            UnityEngine.Object.DontDestroyOnLoad(this.gameObject);
            this.LoadCountryNames();
            this.boxStyle = new GUIStyle();
            this.topRankPlayers = new CTopRankPlayer[1000];
            for (int i = 0; i < Extensions.get_length(this.topRankPlayers); i++)
            {
                this.topRankPlayers[i] = new CTopRankPlayer();
            }
            this.keys = new KeyCode[Extensions.get_length(this.keyNames)];
            this.keysTmp = new KeyCode[Extensions.get_length(this.keyNames)];
            if (GlobalManager.instance && GlobalManager.instance.isDedicated)
            {
                this.menuBG = null;
            }
            this.jugglingRect = new Rect((float)Screen.width * 0.5f - (float)480 + (float)670, (float)Screen.height * 0.5f - (float)300 + (float)100, (float)250, (float)350);
        }
        else
        {
            UnityEngine.Object.Destroy(this.gameObject);
        }
    }

    // Token: 0x06000278 RID: 632 RVA: 0x0003408C File Offset: 0x0003228C
    public virtual void Start()
    {
        if (!Application.isWebPlayer)
        {
            this.defFilePath = PlayerPrefs.GetString("defFilePath", string.Empty);
        }
        if (!this.isDedicated)
        {
            Shader.WarmupAllShaders();
        }
        this.LoadOptions();
        StaticVariables.guideLongTricksString = "Welcome to Ball 3D: Soccer Online!\n" + "\n" + "^7Low Kick: \n" + "^0Jump before the kick.\n" + "\n" + "^7Block / Goalkeeper: \n" + "^0To block the ball more effectively, keep clicking " + "^4" + this.GetBetterKeyName(Keys.Kick) + "^0 all the time.\n" + "\n" + "^7Spin Kick: \n" + "^4" + this.GetBetterKeyName(Keys.VerticalKick) + "^0 + " + "^4" + this.GetBetterKeyName(Keys.CurveLeft) + "^0 or " + "^4" + this.GetBetterKeyName(Keys.CurveRight) + "^0 + " + "^4" + this.GetBetterKeyName(Keys.Kick) + "\n" + "^0Don't load the kick bar, just click " + "^4" + this.GetBetterKeyName(Keys.Kick) + "^0 as fast as possible.\n" + "\n" + "^7Power Shot: \n" + "^0Two players have to kick the ball at the same time. You can watch it at the end of this replay: \n";
        if (SteamManager.Initialized)
        {
            this.getAuthSessionTicketResponse = Callback<GetAuthSessionTicketResponse_t>.Create(new Callback<GetAuthSessionTicketResponse_t>.DispatchDelegate(this.OnGetAuthSessionTicketResponse));
            this.microTxnAuthorizationResponse = Callback<MicroTxnAuthorizationResponse_t>.Create(new Callback<MicroTxnAuthorizationResponse_t>.DispatchDelegate(this.OnMicroTxnAuthorizationResponse));
            this.gameOverlayActivated = Callback<GameOverlayActivated_t>.Create(new Callback<GameOverlayActivated_t>.DispatchDelegate(this.OnGameOverlayActivated));
            SteamUserStats.RequestCurrentStats();
        }
        else
        {
            Debug.Log("steam not initialized");
        }
    }

    // Token: 0x06000279 RID: 633 RVA: 0x00034278 File Offset: 0x00032478
    public virtual void OnApplicationQuit()
    {
        if (this.natHelper && !this.isDedicated)
        {
            this.natHelper.stopPortForwarding();
        }
    }

    // Token: 0x0600027A RID: 634 RVA: 0x000342AC File Offset: 0x000324AC
    public virtual IEnumerator UnloadAssets()
    {
        return new GlobalManager.$UnloadAssets$1859(this).GetEnumerator();
    }

    // Token: 0x0600027B RID: 635 RVA: 0x000342BC File Offset: 0x000324BC
    public virtual void Update()
    {
        if (this.isDedicated && GameManager.instance && this.masterServerFailedTimer >= (float)0)
        {
            this.masterServerFailedTimer += Time.unscaledDeltaTime;
            for (int i = 1; i < Extensions.get_length(GameManager.instance.players); i++)
            {
                if (GameManager.instance.players[i].exist)
                {
                    this.masterServerFailedTimer = (float)0;
                }
            }
            if (this.masterServerFailedTimer > 180f)
            {
                Network.Disconnect(200);
                Application.LoadLevel("MenuServerList");
            }
        }
        if (!this.isDedicated && this.natHelper && this.natHelper.isDoneFindingNATDevice && !this.portForwardingDone)
        {
            this.portForwardingDone = true;
            this.natHelper.mapPort(StaticVariables.networkPort, StaticVariables.networkPort, 0, NATTraversal.Protocol.Both, "Ball 3D map port", $adaptor$__GlobalManager_Update$callable0$532_144__$Action$0.Adapt(new __GlobalManager_Update$callable0$532_144__(this.OnPortMappingDone)));
        }
        if (this.topRanksGetTimer >= (float)0)
        {
            this.topRanksGetTimer += Time.unscaledDeltaTime;
        }
        if (!Screen.fullScreen && (Screen.width < 960 || Screen.height < 600))
        {
            Screen.SetResolution(960, 600, false);
        }
        if (!GlobalManager.instance.isDedicated)
        {
            if (this.connectingInProgress && !GameManager.instance)
            {
                QualitySettings.antiAliasing = 0;
            }
            else
            {
                QualitySettings.antiAliasing = this.aaLevel;
            }
            QualitySettings.vSyncCount = this.vSync;
            if (this.vSync == 0)
            {
                Application.targetFrameRate = this.maxFps;
            }
            else
            {
                Application.targetFrameRate = -1;
            }
        }
        if (this.GetKey(Keys.MoveLeft))
        {
            if (this.horizontalAxis >= (float)0)
            {
                this.horizontalAxis = (float)0;
            }
            this.horizontalAxis -= Time.unscaledDeltaTime * 3f;
            if (this.horizontalAxis < -1f)
            {
                this.horizontalAxis = -1f;
            }
        }
        else if (this.GetKey(Keys.MoveRight))
        {
            if (this.horizontalAxis <= (float)0)
            {
                this.horizontalAxis = (float)0;
            }
            this.horizontalAxis += Time.unscaledDeltaTime * 3f;
            if (this.horizontalAxis > 1f)
            {
                this.horizontalAxis = 1f;
            }
        }
        else if (this.horizontalAxis > (float)0)
        {
            this.horizontalAxis -= Time.unscaledDeltaTime * 3f;
            if (this.horizontalAxis < (float)0)
            {
                this.horizontalAxis = (float)0;
            }
        }
        else if (this.horizontalAxis < (float)0)
        {
            this.horizontalAxis += Time.unscaledDeltaTime * 3f;
            if (this.horizontalAxis > (float)0)
            {
                this.horizontalAxis = (float)0;
            }
        }
        if (this.GetKey(Keys.MoveBack))
        {
            if (this.verticalAxis >= (float)0)
            {
                this.verticalAxis = (float)0;
            }
            this.verticalAxis -= Time.unscaledDeltaTime * 3f;
            if (this.verticalAxis < -1f)
            {
                this.verticalAxis = -1f;
            }
        }
        else if (this.GetKey(Keys.MoveForward))
        {
            if (this.verticalAxis <= (float)0)
            {
                this.verticalAxis = (float)0;
            }
            this.verticalAxis += Time.unscaledDeltaTime * 3f;
            if (this.verticalAxis > 1f)
            {
                this.verticalAxis = 1f;
            }
        }
        else if (this.verticalAxis > (float)0)
        {
            this.verticalAxis -= Time.unscaledDeltaTime * 3f;
            if (this.verticalAxis < (float)0)
            {
                this.verticalAxis = (float)0;
            }
        }
        else if (this.verticalAxis < (float)0)
        {
            this.verticalAxis += Time.unscaledDeltaTime * 3f;
            if (this.verticalAxis > (float)0)
            {
                this.verticalAxis = (float)0;
            }
        }
        this.SetPressedKey();
    }

    // Token: 0x0600027C RID: 636 RVA: 0x000346C4 File Offset: 0x000328C4
    public virtual IEnumerator LoadTopRank()
    {
        return new GlobalManager.$LoadTopRank$1865(this).GetEnumerator();
    }

    // Token: 0x0600027D RID: 637 RVA: 0x000346D4 File Offset: 0x000328D4
    public virtual bool ShowTopRanks(Vector2 offset)
    {
        if (this.topRanksGetTimer >= (float)0)
        {
            this.yourRankIndex = -1;
            int i;
            for (i = 0; i < Extensions.get_length(this.topRankPlayers); i++)
            {
                if (BASManager.instance.basPlayer.loginStatus == LoginStatus.Logged && this.topRankPlayers[i].nick.ToLower() == BASManager.instance.basPlayer.login)
                {
                    this.yourRankIndex = i;
                }
            }
            int num = 15;
            int num2 = 100;
            string lhs = string.Empty;
            i = this.topRankScrollPosition;
            while (i < this.topRankScrollPosition + num && i < num2)
            {
                if (this.yourRankIndex == i)
                {
                    lhs = "^4";
                }
                else
                {
                    lhs = "^0";
                }
                GUI.Label(new Rect((float)0 + offset.x, (float)(40 + (i - this.topRankScrollPosition) * 30) + offset.y, (float)300, (float)30), StaticVariables.ColorsToHTML(lhs + (i + 1) + ".", false));
                GUI.Label(new Rect((float)30 + offset.x, (float)(43 + (i - this.topRankScrollPosition) * 30) + offset.y, (float)30, (float)30), this.topRankPlayers[i].flagTexture);
                GUI.Label(new Rect((float)50 + offset.x, (float)(36 + (i - this.topRankScrollPosition) * 30) + offset.y, (float)33, (float)32), BASManager.instance.rankIconsSmall[BASManager.instance.RankPointsToRankNumber(this.topRankPlayers[i].rankPoints)]);
                GUI.Label(new Rect((float)84 + offset.x, (float)(40 + (i - this.topRankScrollPosition) * 30) + offset.y, (float)190, (float)30), StaticVariables.ColorsToHTML(lhs + this.topRankPlayers[i].nick, false));
                GUI.Label(new Rect((float)294 + offset.x, (float)(40 + (i - this.topRankScrollPosition) * 30) + offset.y, (float)85, (float)30), StaticVariables.ColorsToHTML(lhs + this.topRankPlayers[i].rankPoints + " (" + this.topRankPlayers[i].rankGames + ")", false));
                i++;
            }
            string rhs = "1000+";
            if (this.yourRankIndex >= 0)
            {
                rhs = string.Empty + (this.yourRankIndex + 1);
            }
            if (BASManager.instance.basPlayer.loginStatus == LoginStatus.Logged)
            {
                GUI.Label(new Rect((float)135 + offset.x, (float)492 + offset.y, (float)190, (float)30), "Your position: " + rhs);
            }
            this.topRankScrollPosition = (int)GUI.VerticalScrollbar(new Rect((float)385 + offset.x, (float)40 + offset.y, (float)50, (float)450), (float)this.topRankScrollPosition, (float)num, (float)0, (float)num2);
        }
        return false;
    }

    // Token: 0x0600027E RID: 638 RVA: 0x00034A54 File Offset: 0x00032C54
    public virtual bool ShowTopRanksSmall(Vector2 offset)
    {
        if (BASManager.instance && BASManager.instance.basPlayer != null && this.topRankPlayers != null)
        {
            GUILayout.BeginArea(new Rect((float)Screen.width * 0.5f - (float)480, (float)Screen.height * 0.5f - (float)300, (float)960, (float)600));
            if (this.topRanksGetTimer >= (float)0)
            {
                this.yourRankIndex = -1;
                int i;
                for (i = 0; i < Extensions.get_length(this.topRankPlayers); i++)
                {
                    if (BASManager.instance.basPlayer.loginStatus == LoginStatus.Logged && this.topRankPlayers[i] != null && !string.IsNullOrEmpty(this.topRankPlayers[i].nick) && this.topRankPlayers[i].nick.ToLower() == BASManager.instance.basPlayer.login)
                    {
                        this.yourRankIndex = i;
                    }
                }
                int num = 12;
                int num2 = 100;
                string lhs = string.Empty;
                GUI.Label(new Rect((float)75 + offset.x, offset.y + (float)10, (float)150, (float)30), "Best Players");
                i = this.topRankScrollPosition;
                while (i < this.topRankScrollPosition + num && i < num2)
                {
                    if (this.yourRankIndex == i)
                    {
                        lhs = "^4";
                    }
                    else
                    {
                        lhs = "^0";
                    }
                    GUI.Label(new Rect((float)0 + offset.x, (float)(40 + (i - this.topRankScrollPosition) * 30) + offset.y, (float)300, (float)30), StaticVariables.ColorsToHTML(lhs + (i + 1) + ".", false));
                    GUI.Label(new Rect((float)30 + offset.x, (float)(43 + (i - this.topRankScrollPosition) * 30) + offset.y, (float)30, (float)30), this.topRankPlayers[i].flagTexture);
                    GUI.Label(new Rect((float)50 + offset.x, (float)(36 + (i - this.topRankScrollPosition) * 30) + offset.y, (float)33, (float)32), BASManager.instance.rankIconsSmall[BASManager.instance.RankPointsToRankNumber(this.topRankPlayers[i].rankPoints)]);
                    GUI.Label(new Rect((float)84 + offset.x, (float)(40 + (i - this.topRankScrollPosition) * 30) + offset.y, (float)70, (float)30), StaticVariables.ColorsToHTML(lhs + this.topRankPlayers[i].nick, false));
                    GUI.Label(new Rect((float)164 + offset.x, (float)(40 + (i - this.topRankScrollPosition) * 30) + offset.y, (float)85, (float)30), StaticVariables.ColorsToHTML(lhs + this.topRankPlayers[i].rankPoints, false));
                    i++;
                }
                string rhs = "1000+";
                if (this.yourRankIndex >= 0)
                {
                    rhs = string.Empty + (this.yourRankIndex + 1);
                }
                if (BASManager.instance.basPlayer.loginStatus == LoginStatus.Logged)
                {
                    GUI.Label(new Rect((float)40 + offset.x, (float)402 + offset.y, (float)150, (float)30), "Your position: " + rhs);
                }
                this.topRankScrollPosition = (int)GUI.VerticalScrollbar(new Rect((float)385 + offset.x - (float)175, (float)40 + offset.y, (float)50, (float)350), (float)this.topRankScrollPosition, (float)num, (float)0, (float)num2);
                GUILayout.EndArea();
            }
        }
        return false;
    }

    // Token: 0x0600027F RID: 639 RVA: 0x00034E7C File Offset: 0x0003307C
    public virtual void JugglingMiniGameLateUpdate()
    {
        Cursor.visible = this.jugglingCursorVisible;
    }

    // Token: 0x06000280 RID: 640 RVA: 0x00034E8C File Offset: 0x0003308C
    public virtual void JugglingMiniGameGUI()
    {
        GUI.Box(new Rect(this.jugglingRect.x, this.jugglingRect.y + this.jugglingCeilOffset, this.jugglingRect.width, this.jugglingRect.height - this.jugglingCeilOffset), string.Empty, "BoxTransparent");
        if (Input.mousePosition.x >= this.jugglingRect.x && Input.mousePosition.x < this.jugglingRect.x + this.jugglingRect.width && (float)Screen.height - Input.mousePosition.y >= this.jugglingRect.y && (float)Screen.height - Input.mousePosition.y < this.jugglingRect.y + this.jugglingRect.height)
        {
            this.jugglingCursorVisible = false;
            GUI.DrawTexture(new Rect(Input.mousePosition.x - (float)this.jugglingPointer.width * 0.5f, (float)Screen.height - Input.mousePosition.y - (float)this.jugglingPointer.height * 0.5f, (float)this.jugglingPointer.width, (float)this.jugglingPointer.height), this.jugglingPointer);
        }
        else
        {
            this.jugglingCursorVisible = true;
        }
        GUI.DrawTexture(new Rect(this.jugglingBallPosition.x - (float)this.jugglingBall.width * 0.5f, this.jugglingBallPosition.y - (float)this.jugglingBall.height * 0.5f, (float)this.jugglingBall.width, (float)this.jugglingBall.height), this.jugglingBall);
        GUILayout.BeginArea(this.jugglingRect);
        if (this.jugglingGameState == 1)
        {
            GUILayout.Space((float)50);
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            GUILayout.Label(string.Empty + (UnityBuiltins.parseInt((float)this.jugglingCountDownMax - this.jugglingTimer1) + 1), "LabelTitle", new GUILayoutOption[0]);
            GUILayout.EndHorizontal();
        }
        if (this.jugglingGameState == 2 && this.dontClickTimer >= (float)0)
        {
            GUILayout.Space((float)50);
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            GUILayout.Label("DO NOT CLICK!", "LabelTitle", new GUILayoutOption[0]);
            GUILayout.EndHorizontal();
        }
        if (this.jugglingGameState == 4)
        {
            GUILayout.Space((float)50);
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            GUILayout.Label((!this.jugglingIsNewRecord) ? "TRY AGAIN..." : "NEW RECORD!", "LabelTitle", new GUILayoutOption[0]);
            GUILayout.EndHorizontal();
        }
        GUILayout.EndArea();
        GUILayout.BeginArea(new Rect(this.jugglingRect.x, this.jugglingRect.y - (float)25, this.jugglingRect.width, this.jugglingRect.height + (float)25 + (float)40));
        GUILayout.Label("Waiting for a server? Play this...", new GUILayoutOption[0]);
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal(new GUILayoutOption[0]);
        GUILayout.Label(string.Empty + this.jugglingScore, "LabelTitle", new GUILayoutOption[0]);
        GUILayout.FlexibleSpace();
        GUILayout.Label(string.Empty + this.jugglingHighScore, "LabelTitle", new GUILayoutOption[0]);
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    // Token: 0x06000281 RID: 641 RVA: 0x00035240 File Offset: 0x00033440
    public virtual void JugglingMiniGameUpdate()
    {
        this.jugglingRect = new Rect((float)Screen.width * 0.5f - (float)480 + (float)670, (float)Screen.height * 0.5f - (float)300 + (float)100, (float)250, (float)350);
        if (this.jugglingGameState == 0)
        {
            this.jugglingBallPosition = new Vector2(this.jugglingRect.x + this.jugglingRect.width * 0.5f, this.jugglingRect.y + this.jugglingRect.height * 0.5f);
            this.jugglingTimer1 = (float)0;
            this.jugglingCeilTimer = (float)0;
            this.jugglingCeilOffset = (float)0;
            this.dontClickTimer = -1f;
            this.jugglingScore = 0;
            this.jugglingGameState = 1;
        }
        if (this.jugglingGameState == 1 && this.jugglingTimer1 >= (float)this.jugglingCountDownMax)
        {
            this.jugglingBallVelocity.y = -300f;
            this.jugglingBallVelocity.x = 600f * UnityEngine.Random.value - 300f;
            this.jugglingGameState = 2;
        }
        if (this.jugglingGameState == 2)
        {
            if (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2))
            {
                this.dontClickTimer = (float)0;
            }
            if (this.dontClickTimer >= (float)0)
            {
                this.dontClickTimer += Time.deltaTime;
                if (this.dontClickTimer >= this.dontClickTime)
                {
                    this.dontClickTimer = -1f;
                }
            }
        }
        if (this.jugglingGameState == 3)
        {
            this.jugglingTimer1 = (float)0;
            if (this.jugglingScore > this.jugglingHighScore)
            {
                this.jugglingHighScore = this.jugglingScore;
                this.jugglingIsNewRecord = true;
                PlayerPrefs.SetInt("jugglingHighScore", this.jugglingHighScore);
                PlayerPrefs.Save();
            }
            else
            {
                this.jugglingIsNewRecord = false;
            }
            this.jugglingGameState = 4;
        }
        if (this.jugglingGameState == 4 && this.jugglingTimer1 >= (float)this.jugglingAfterTime)
        {
            this.jugglingGameState = 0;
        }
        this.jugglingTimer1 += Time.deltaTime;
        this.jugglingPrevMousePosition = Input.mousePosition;
    }

    // Token: 0x06000282 RID: 642 RVA: 0x00035480 File Offset: 0x00033680
    public virtual void JugglingMiniGameFixedUpdate()
    {
        if (this.jugglingGameState == 2 || this.jugglingGameState == 4)
        {
            if (this.jugglingGameState == 2)
            {
                this.jugglingCeilTimer += Time.deltaTime;
            }
            this.jugglingCeilOffset = Mathf.Min(this.jugglingCeilTimer * this.jugglingCeilTimerFactor, this.jugglingCeilOffsetMax);
            Vector2 b = new Vector2(Input.mousePosition.x, (float)Screen.height - Input.mousePosition.y);
            Vector2 vector = new Vector2(this.jugglingPrevMousePosition.x, (float)Screen.height - this.jugglingPrevMousePosition.y);
            Vector2 vector2 = (this.jugglingBallPosition - b) / Time.deltaTime;
            vector2.x += UnityEngine.Random.value * 20f - 10f;
            this.jugglingBallVelocity.y = this.jugglingBallVelocity.y + this.jugglingGravity * Time.deltaTime;
            bool flag = false;
            if (Vector2.Distance(this.jugglingBallPosition, b) <= this.jugglingBallRadius + this.jugglingPointerRadius)
            {
                if (!this.jugglingPrevFrameKick)
                {
                    this.jugglingBallVelocity += vector2.normalized * this.jugglingKickFactor;
                    this.jugglingPrevFrameKick = true;
                    flag = true;
                    if (this.jugglingGameState == 2)
                    {
                        this.jugglingScore++;
                    }
                }
            }
            else
            {
                this.jugglingPrevFrameKick = false;
            }
            if (this.jugglingBallPosition.x + this.jugglingBallRadius >= this.jugglingRect.x + this.jugglingRect.width)
            {
                this.jugglingBallPosition.x = this.jugglingRect.x + this.jugglingRect.width - this.jugglingBallRadius - (float)1;
                this.jugglingBallVelocity.x = this.jugglingBallVelocity.x * (-1f * this.jugglingBallBounceDump);
            }
            else if (this.jugglingBallPosition.x - this.jugglingBallRadius <= this.jugglingRect.x)
            {
                this.jugglingBallPosition.x = this.jugglingRect.x + this.jugglingBallRadius + (float)1;
                this.jugglingBallVelocity.x = this.jugglingBallVelocity.x * (-1f * this.jugglingBallBounceDump);
            }
            if (this.jugglingBallPosition.y + this.jugglingBallRadius >= this.jugglingRect.y + this.jugglingRect.height)
            {
                this.jugglingBallPosition.y = this.jugglingRect.y + this.jugglingRect.height - this.jugglingBallRadius - (float)1;
                if (!flag)
                {
                    if (this.jugglingBallVelocity.y < this.jugglingBounceIdleSpeed)
                    {
                        this.jugglingBallVelocity.y = (float)0;
                    }
                    else
                    {
                        this.jugglingBallVelocity.y = this.jugglingBallVelocity.y * (-1f * this.jugglingBallBounceDump);
                    }
                    if (this.jugglingGameState == 2)
                    {
                        this.jugglingGameState = 3;
                    }
                }
            }
            else if (this.jugglingBallPosition.y - this.jugglingBallRadius <= this.jugglingRect.y + this.jugglingCeilOffset)
            {
                this.jugglingBallPosition.y = this.jugglingRect.y + this.jugglingCeilOffset + this.jugglingBallRadius + (float)1;
                this.jugglingBallVelocity.y = this.jugglingBallVelocity.y * (-1f * this.jugglingBallBounceDump);
            }
            this.jugglingBallVelocity *= this.jugglingBallDump;
            if (this.jugglingBallVelocity.x > (float)0)
            {
                if (this.jugglingBallVelocity.x > this.jugglingMaxSpeed)
                {
                    this.jugglingBallVelocity.x = this.jugglingMaxSpeed;
                }
                else if (this.jugglingBallVelocity.x < this.jugglingIdleSpeed)
                {
                    this.jugglingBallVelocity.x = (float)0;
                }
            }
            else if (this.jugglingBallVelocity.x < (float)0)
            {
                if (this.jugglingBallVelocity.x < -this.jugglingMaxSpeed)
                {
                    this.jugglingBallVelocity.x = -this.jugglingMaxSpeed;
                }
                else if (this.jugglingBallVelocity.x > -this.jugglingIdleSpeed)
                {
                    this.jugglingBallVelocity.x = (float)0;
                }
            }
            if (this.jugglingBallVelocity.y > (float)0)
            {
                if (this.jugglingBallVelocity.y > this.jugglingMaxSpeed)
                {
                    this.jugglingBallVelocity.y = this.jugglingMaxSpeed;
                }
                else if (this.jugglingBallVelocity.y < this.jugglingIdleSpeed)
                {
                    this.jugglingBallVelocity.y = (float)0;
                }
            }
            else if (this.jugglingBallVelocity.y < (float)0 && this.jugglingBallVelocity.y < -this.jugglingMaxSpeed)
            {
                this.jugglingBallVelocity.y = -this.jugglingMaxSpeed;
            }
            this.jugglingBallPosition += this.jugglingBallVelocity * Time.deltaTime;
        }
    }

    // Token: 0x06000283 RID: 643 RVA: 0x000359C4 File Offset: 0x00033BC4
    public virtual void ShowPlayer(Rect rect, CPlayer player, int index, int originalIndex)
    {
        this.boxStyle.normal.background = null;
        int num = -1;
        if ((player.basPlayer.loginStatus == LoginStatus.Logged || originalIndex < 0) && player.basPlayer.currentBadgeID >= 0 && player.basPlayer.currentBadgeID < Extensions.get_length(ShopManager.instance.items) && (originalIndex < 0 || !Network.isServer || ShopManager.instance.PlayerHasItem(originalIndex, player.basPlayer.currentBadgeID)))
        {
            num = ShopManager.instance.items[player.basPlayer.currentBadgeID].objectIndex;
        }
        if (num >= 0 && num < Extensions.get_length(ShopManager.instance.badges))
        {
            this.boxStyle.normal.background = (Texture2D)ShopManager.instance.badges[num];
        }
        GUILayout.BeginArea(rect);
        if (num >= 0 && num < Extensions.get_length(ShopManager.instance.badges))
        {
            GUILayout.Box(new GUIContent(string.Empty, index.ToString()), this.boxStyle, new GUILayoutOption[]
            {
                GUILayout.ExpandHeight(true)
            });
        }
        else
        {
            GUILayout.Box(new GUIContent(string.Empty, index.ToString()), "BoxTransparent", new GUILayoutOption[]
            {
                GUILayout.ExpandHeight(true)
            });
        }
        GUILayout.EndArea();
        this.boxStyle.normal.background = (Texture2D)this.readyFrame;
        GUILayout.BeginArea(rect);
        if (player.isReady && (player.team == Team.Red || player.team == Team.Blue) && GameManager.instance && (GameManager.instance.gameStatus == GameStatus.Warmup || GameManager.instance.gameStatus == GameStatus.Countdown))
        {
            GUILayout.Box(new GUIContent(string.Empty, index.ToString()), this.boxStyle, new GUILayoutOption[]
            {
                GUILayout.ExpandHeight(true)
            });
        }
        GUILayout.EndArea();
        GUILayout.BeginArea(new Rect(rect.x, rect.y, rect.width + (float)10, rect.height + (float)2));
        GUILayout.BeginHorizontal(new GUILayoutOption[0]);
        string text;
        if (originalIndex == 0)
        {
            text = "LabelHost";
        }
        else if (player.isAdmin)
        {
            text = "LabelAdmin";
        }
        else
        {
            text = "LabelPlayer";
        }
        GUILayout.Space((float)30);
        string a = string.Empty;
        if ((player.basPlayer.loginStatus == LoginStatus.Logged || player.basPlayer.loginStatus == LoginStatus.Null) && player.basPlayer.rank == Rank.Admin)
        {
            a = "A";
        }
        else if (player.basPlayer.loginStatus == LoginStatus.Logged || player.basPlayer.loginStatus == LoginStatus.Null)
        {
            a = string.Empty;
        }
        else if (player.basPlayer.loginStatus == LoginStatus.Checking || player.basPlayer.loginStatus == LoginStatus.TryAgain)
        {
            a = "?";
        }
        else
        {
            a = "G";
        }
        if (player.basPlayer.rank == Rank.Bot)
        {
            a = string.Empty;
        }
        if (a != string.Empty)
        {
            GUILayout.BeginVertical(new GUILayoutOption[0]);
            GUILayout.FlexibleSpace();
            if (a == "A")
            {
                GUILayout.Label(new GUIContent(BASManager.instance.iconAdmin), text, new GUILayoutOption[0]);
            }
            else if (a == "?")
            {
                GUILayout.Label(new GUIContent(BASManager.instance.iconChecking), text, new GUILayoutOption[0]);
            }
            else if (a == "G")
            {
                GUILayout.Label(new GUIContent(BASManager.instance.iconGuest), text, new GUILayoutOption[0]);
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
        }
        if (player.basPlayer.proLevel >= 1 && player.basPlayer.proLevel <= 4 && a != "A")
        {
            GUILayout.BeginVertical(new GUILayoutOption[0]);
            GUILayout.FlexibleSpace();
            GUILayout.Label(new GUIContent(BASManager.instance.proIcons[player.basPlayer.proLevel - 1]), text, new GUILayoutOption[0]);
            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
        }
        if (player.basPlayer.rank == Rank.Bot)
        {
            GUILayout.BeginVertical(new GUILayoutOption[0]);
            GUILayout.FlexibleSpace();
            GUILayout.Label(new GUIContent(BASManager.instance.botIcon), text, new GUILayoutOption[0]);
            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
        }
        bool allowBlack = false;
        GUILayout.BeginVertical(new GUILayoutOption[0]);
        int num2 = 100;
        if (player.basPlayer.proLevel >= 1 && player.basPlayer.proLevel <= 4 && a != "A")
        {
            num2 = 88;
        }
        if (player.basPlayer.rank == Rank.Bot)
        {
            num2 = 88;
        }
        GUILayout.FlexibleSpace();
        GUILayout.Space((float)3);
        GUILayout.BeginHorizontal(new GUILayoutOption[0]);
        GUILayout.Space((float)5);
        if (player.basPlayer.loginStatus == LoginStatus.Logged && player.basPlayer.proLevel >= 1 && player.basPlayer.proLevel <= 4)
        {
            allowBlack = true;
            string bgcolor = StaticVariables.GetBGColor(player.nick);
            GUILayout.Label(new GUIContent(StaticVariables.ColorsToHTML(bgcolor + StaticVariables.ColorsToHTML(player.nick, true), false, false, true), index.ToString()), "LabelPlayer", new GUILayoutOption[]
            {
                GUILayout.Width((float)num2)
            });
        }
        else
        {
            GUILayout.Label(new GUIContent(StaticVariables.ColorsToHTML(player.nick, true), index.ToString()), "LabelPlayerBlack", new GUILayoutOption[]
            {
                GUILayout.Width((float)num2)
            });
        }
        GUILayout.EndHorizontal();
        if (Convert.ToInt32(Time.realtimeSinceStartup * 0.75f) % 2 == 0)
        {
            text = "LabelPlayer";
        }
        GUILayout.Space((float)-23);
        if (text == "LabelPlayer")
        {
            GUILayout.Label(new GUIContent(StaticVariables.ColorsToHTML(player.nick, false, false, allowBlack), index.ToString()), text, new GUILayoutOption[]
            {
                GUILayout.Width((float)num2)
            });
        }
        else
        {
            GUILayout.Label(new GUIContent(StaticVariables.ColorsToHTML(player.nick, true), index.ToString()), text, new GUILayoutOption[]
            {
                GUILayout.Width((float)num2)
            });
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
        if (a == "A")
        {
            GUILayout.Space((float)-127);
            GUILayout.Label(new GUIContent(BASManager.instance.frameAdmin), "LabelPlayer", new GUILayoutOption[0]);
        }
        else if (a == "?" || a == "G")
        {
            GUILayout.Space((float)-10);
        }
        else
        {
            GUILayout.Space((float)5);
        }
        if (player.basPlayer.proLevel >= 1 && player.basPlayer.proLevel <= 4 && a != "A")
        {
            GUILayout.Space((float)-13);
        }
        if (player.basPlayer.rank == Rank.Bot)
        {
            GUILayout.Space((float)-13);
        }
        if (a == "A")
        {
            GUILayout.Space((float)-5);
        }
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical(new GUILayoutOption[0]);
        GUILayout.FlexibleSpace();
        if (player.ping > 999)
        {
            player.ping = 999;
        }
        string lhs = StaticVariables.PingToColorStr(player.ping);
        string text2 = player.ping.ToString();
        if (Convert.ToInt32(Time.realtimeSinceStartup * 0.75f) % 2 != 0)
        {
            lhs = "^0";
            if (player.basPlayer.rankPoints >= 0)
            {
                text2 = string.Empty + player.basPlayer.rankPoints;
            }
            else
            {
                text2 = string.Empty + 1000;
            }
        }
        GUILayout.Space((float)3);
        GUILayout.BeginHorizontal(new GUILayoutOption[0]);
        GUIStyle style = this.skin.GetStyle("LabelPlayerBlack");
        TextAnchor alignment = style.alignment;
        style.alignment = TextAnchor.MiddleRight;
        GUILayout.Space((float)5);
        GUILayout.Label(new GUIContent(StaticVariables.ColorsToHTML(text2, true), index.ToString()), "LabelPlayerBlack", new GUILayoutOption[]
        {
            GUILayout.Width((float)40)
        });
        style.alignment = alignment;
        GUILayout.EndHorizontal();
        GUILayout.Space((float)-23);
        GUIStyle style2 = this.skin.GetStyle("LabelPlayer");
        TextAnchor alignment2 = style2.alignment;
        style2.alignment = TextAnchor.MiddleRight;
        GUILayout.Label(new GUIContent(StaticVariables.ColorsToHTML(lhs + text2, false), index.ToString()), "LabelPlayer", new GUILayoutOption[]
        {
            GUILayout.Width((float)40)
        });
        style2.alignment = alignment2;
        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
        GUILayout.Space((float)20);
        if (GameManager.instance && GameManager.instance.localPlayer != null && GameManager.instance.localPlayer.isAdmin && index != -2 && index >= 0)
        {
            GUILayout.Space((float)-20);
            GUILayout.FlexibleSpace();
            GUILayout.BeginVertical(new GUILayoutOption[0]);
            GUILayout.FlexibleSpace();
            string tooltip = " ";
            if (GameManager.instance.players[index].team == Team.Red)
            {
                tooltip = "RedList";
            }
            else if (GameManager.instance.players[index].team == Team.Blue)
            {
                tooltip = "BlueList";
            }
            else if (GameManager.instance.players[index].team == Team.Spec)
            {
                tooltip = "SpecList";
            }
            if (!GlobalManager.instance.isBotGame && GUILayout.Button(new GUIContent("...", tooltip), new GUILayoutOption[0]))
            {
                (GameManager.instance.menuObject.GetComponent("MenuGame") as MenuGame).playerOptions = true;
                (GameManager.instance.menuObject.GetComponent("MenuGame") as MenuGame).playerOptionsIndex = index;
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
            GUILayout.Space((float)17);
        }
        GUILayout.EndHorizontal();
        if (Convert.ToInt32(Time.realtimeSinceStartup * 0.75f) % 2 == 0)
        {
            GUI.Label(new Rect((float)10, (float)8, (float)20, (float)15), new GUIContent(player.flagTexture), text);
        }
        else
        {
            GUI.Label(new Rect((float)0, (float)0, (float)36, (float)32), new GUIContent(BASManager.instance.rankIconsSmall[BASManager.instance.RankPointsToRankNumber(player.basPlayer.rankPoints)]), text);
        }
        GUILayout.EndArea();
    }

    // Token: 0x06000284 RID: 644 RVA: 0x0003652C File Offset: 0x0003472C
    public virtual int StadiumIndexToFilter(int stadiumIndex)
    {
        return (stadiumIndex != 46 && stadiumIndex != 47 && stadiumIndex != 48 && stadiumIndex != 49) ? ((stadiumIndex != 54 && stadiumIndex != 55 && stadiumIndex != 56 && stadiumIndex != 69) ? ((stadiumIndex != 77) ? ((stadiumIndex != 81 && stadiumIndex != 82 && stadiumIndex != 38) ? ((stadiumIndex != 6 && stadiumIndex != 14 && stadiumIndex != 21 && stadiumIndex != 27 && stadiumIndex != 28 && stadiumIndex != 33 && stadiumIndex != 34 && stadiumIndex != 39 && stadiumIndex != 75 && stadiumIndex != 76) ? ((stadiumIndex != 83) ? ((stadiumIndex != 37) ? ((stadiumIndex != 84) ? ((stadiumIndex != 85) ? ((stadiumIndex != 87 && stadiumIndex != 92) ? 1 : 12) : 11) : 10) : 6) : 8) : 4) : 5) : 3) : 2) : 7;
    }

    // Token: 0x06000285 RID: 645 RVA: 0x00036660 File Offset: 0x00034860
    public virtual int ServiceSportsList(Rect rect, Vector2 frameStart, int filterNumber, bool hideAllGames, bool isSmall, bool hideRanking)
    {
        GUILayout.BeginArea(rect);
        GUILayout.BeginHorizontal(new GUILayoutOption[0]);
        int num = 82;
        int num2 = 64;
        if (isSmall)
        {
            num = 40;
            num2 = 32;
        }
        int[] array = new int[]
        {
            0,
            1,
            2,
            4,
            9,
            10,
            5,
            6,
            11,
            0,
            7,
            8,
            3
        };
        int num3 = array[filterNumber];
        frameStart += new Vector2((float)((num + 4) * num3), (float)0);
        if (isSmall)
        {
            frameStart.x -= (float)30;
        }
        if (!isSmall)
        {
            frameStart.x -= (float)47;
        }
        GUILayout.FlexibleSpace();
        if (!hideAllGames && GUILayout.Button((!isSmall) ? "All Games" : "All", new GUILayoutOption[]
        {
            GUILayout.Width((float)num),
                                              GUILayout.Height((float)num2)
        }))
        {
            filterNumber = 0;
        }
        if (GUILayout.Button(new GUIContent((!isSmall) ? StaticVariables.sportsShortNames[0] : string.Empty, this.filterSmallIcons[0]), new GUILayoutOption[]
        {
            GUILayout.Width((float)num),
                             GUILayout.Height((float)num2)
        }))
        {
            filterNumber = 1;
        }
        if (GUILayout.Button(new GUIContent((!isSmall) ? StaticVariables.sportsShortNames[1] : string.Empty, this.filterSmallIcons[1]), new GUILayoutOption[]
        {
            GUILayout.Width((float)num),
                             GUILayout.Height((float)num2)
        }))
        {
            filterNumber = 2;
        }
        if (GUILayout.Button(new GUIContent((!isSmall) ? StaticVariables.sportsShortNames[11] : string.Empty, this.filterSmallIcons[11]), new GUILayoutOption[]
        {
            GUILayout.Width((float)num),
                             GUILayout.Height((float)num2)
        }))
        {
            filterNumber = 12;
        }
        if (GUILayout.Button(new GUIContent((!isSmall) ? StaticVariables.sportsShortNames[2] : string.Empty, this.filterSmallIcons[2]), new GUILayoutOption[]
        {
            GUILayout.Width((float)num),
                             GUILayout.Height((float)num2)
        }))
        {
            filterNumber = 3;
        }
        if (GUILayout.Button(new GUIContent((!isSmall) ? StaticVariables.sportsShortNames[5] : string.Empty, this.filterSmallIcons[5]), new GUILayoutOption[]
        {
            GUILayout.Width((float)num),
                             GUILayout.Height((float)num2)
        }))
        {
            filterNumber = 6;
        }
        if (GUILayout.Button(new GUIContent((!isSmall) ? StaticVariables.sportsShortNames[6] : string.Empty, this.filterSmallIcons[6]), new GUILayoutOption[]
        {
            GUILayout.Width((float)num),
                             GUILayout.Height((float)num2)
        }))
        {
            filterNumber = 7;
        }
        if (GUILayout.Button(new GUIContent((!isSmall) ? StaticVariables.sportsShortNames[9] : string.Empty, this.filterSmallIcons[9]), new GUILayoutOption[]
        {
            GUILayout.Width((float)num),
                             GUILayout.Height((float)num2)
        }))
        {
            filterNumber = 10;
        }
        if (GUILayout.Button(new GUIContent((!isSmall) ? StaticVariables.sportsShortNames[10] : string.Empty, this.filterSmallIcons[10]), new GUILayoutOption[]
        {
            GUILayout.Width((float)num),
                             GUILayout.Height((float)num2)
        }))
        {
            filterNumber = 11;
        }
        if (GUILayout.Button(new GUIContent((!isSmall) ? StaticVariables.sportsShortNames[3] : string.Empty, this.filterSmallIcons[3]), new GUILayoutOption[]
        {
            GUILayout.Width((float)num),
                             GUILayout.Height((float)num2)
        }))
        {
            filterNumber = 4;
        }
        if (GUILayout.Button(new GUIContent((!isSmall) ? StaticVariables.sportsShortNames[4] : string.Empty, this.filterSmallIcons[4]), new GUILayoutOption[]
        {
            GUILayout.Width((float)num),
                             GUILayout.Height((float)num2)
        }))
        {
            filterNumber = 5;
        }
        if (isSmall)
        {
            string text = "All Games";
            if (filterNumber > 0)
            {
                text = StaticVariables.sportsLongNames[filterNumber - 1];
            }
            GUILayout.Button(text, "ButtonStatic2", new GUILayoutOption[]
            {
                GUILayout.Width((float)130),
                             GUILayout.Height((float)num2)
            });
        }
        GUILayout.FlexibleSpace();
        GUI.DrawTexture(new Rect(frameStart.x, frameStart.y, (float)(num - 10), (float)(num2 - 10)), this.frameSelection);
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
        return filterNumber;
    }

    // Token: 0x06000286 RID: 646 RVA: 0x00036B3C File Offset: 0x00034D3C
    public virtual void LoadCountryNames()
    {
        TextAsset textAsset = Resources.Load("countryNames") as TextAsset;
        string[] array = textAsset.text.Split("\n".ToCharArray());
        this.countryNames = new string[(int)((float)Extensions.get_length(array) * 0.5f + (float)1)];
        this.countryCodes = new string[(int)((float)Extensions.get_length(array) * 0.5f + (float)1)];
        this.countryTextures = new Texture[(int)((float)Extensions.get_length(array) * 0.5f + (float)1)];
        this.countryNames[0] = "Auto";
        this.countryCodes[0] = string.Empty;
        this.countryTextures[0] = (Texture)Resources.Load("Flags/empty", typeof(Texture));
        int num = 0;
        while (num + 1 < Extensions.get_length(array))
        {
            this.countryNames[(int)((float)num * 0.5f + (float)1)] = array[num];
            this.countryCodes[(int)((float)num * 0.5f + (float)1)] = array[num + 1];
            this.countryTextures[(int)((float)num * 0.5f + (float)1)] = (Texture)Resources.Load("Flags/" + this.countryCodes[(int)((float)num * 0.5f + (float)1)], typeof(Texture));
            num += 2;
        }
    }

    // Token: 0x06000287 RID: 647 RVA: 0x00036C8C File Offset: 0x00034E8C
    public virtual IEnumerator FreeMemoryHelper()
    {
        return new GlobalManager.$FreeMemoryHelper$1872().GetEnumerator();
    }

    // Token: 0x06000288 RID: 648 RVA: 0x00036C98 File Offset: 0x00034E98
    public virtual void ChangeBotGamePlayerPosition(FieldPosition fieldPosition)
    {
        if (fieldPosition == FieldPosition.GK)
        {
            GameManager.instance.players[0].basPlayer.fieldPosition = FieldPosition.GK;
            GameManager.instance.players[1].basPlayer.fieldPosition = FieldPosition.AT;
        }
        else
        {
            GameManager.instance.players[0].basPlayer.fieldPosition = FieldPosition.AT;
            GameManager.instance.players[1].basPlayer.fieldPosition = FieldPosition.GK;
        }
    }

    // Token: 0x06000289 RID: 649 RVA: 0x00036D10 File Offset: 0x00034F10
    public virtual void InitOptions()
    {
        this.optionsTab = 0;
        this.sensitivity = this.mouseSpeed;
        this.isInvertMouseY = this.invertMouseY;
        this.isSoundOn = AudioManager.instance.soundEnabled;
        this.isAmbientOn = AudioManager.instance.ambientEnabled;
        this.isPlayerNamesOn = this.playerNamesEnabled;
        this.tribunesEnabledOption = this.tribunesEnabled;
        this.teamShirtsCodeOption = this.teamShirtsCode;
        this.isGrass3DEnabled = this.grass3DEnabled;
        this.cameraFovOption = this.cameraFov;
        this.useTopDownCameraOption = this.useTopDownCamera;
        if (this.aaLevel == 4)
        {
            this.aaLevelOption = 2;
        }
        else if (this.aaLevel == 2)
        {
            this.aaLevelOption = 1;
        }
        else
        {
            this.aaLevelOption = 0;
        }
        this.vSyncOption = this.vSync;
        this.maxFpsOption = this.maxFps;
        this.curveModeOption = (int)this.curveMode;
        this.isSpeedometer = ShopManager.instance.showSpeedometer;
        if (PredictionManager.instance)
        {
            this.predictionLevel = PredictionManager.instance.clientLagFrames;
            this.isPredictionOn = PredictionManager.instance.usePrediction;
        }
        else
        {
            this.isPredictionOn = this.usePredictionTmp;
            this.predictionLevel = this.clientLagFramesTmp;
        }
        for (int i = 0; i < Extensions.get_length(this.keys); i++)
        {
            this.keysTmp[i] = this.keys[i];
        }
        this.sfxVolumeOption = (float)((AudioManager.instance.sfxVolume + (double)0.001f) * (double)100);
        this.ambientVolumeOption = (float)((AudioManager.instance.ambientVolume + (double)0.001f) * (double)100);
        this.menuMusicVolumeOption = (float)((AudioManager.instance.menuMusicVolume + (double)0.001f) * (double)100);
        this.prevCameraFov = this.cameraFov;
        this.prevUseTopDownCamera = this.useTopDownCamera;
        this.prevSFXVolume = (float)AudioManager.instance.sfxVolume;
        this.prevAmbientVolume = (float)AudioManager.instance.ambientVolume;
        this.prevMenuMusicVolume = (float)AudioManager.instance.menuMusicVolume;
        this.prevSoundEnabled = AudioManager.instance.soundEnabled;
    }

    // Token: 0x0600028A RID: 650 RVA: 0x00036F40 File Offset: 0x00035140
    public virtual bool Options(bool runFromGamePlay)
    {
        if (this.activeKeyIndex >= 0)
        {
            GUI.enabled = false;
        }
        bool result = false;
        GUI.Box(new Rect((float)Screen.width * 0.5f - (float)300 - (float)10, (float)Screen.height * 0.5f - (float)235 - (float)10, (float)620, (float)490), string.Empty);
        GUILayout.BeginArea(new Rect((float)Screen.width * 0.5f - (float)300, (float)Screen.height * 0.5f - (float)235, (float)600, (float)470));
        GUILayout.Label("Options", "LabelTitle", new GUILayoutOption[0]);
        GUILayout.Space((float)5);
        this.optionsTab = GUILayout.SelectionGrid(this.optionsTab, new string[]
        {
            "Video",
            "Audio",
            "Controls",
            "Keys",
            "Game",
            "Info"
        }, 5, new GUILayoutOption[0]);
        GUILayout.Space((float)30);
        if (this.optionsTab == 0)
        {
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            GUILayout.Label("Anti Aliasing:", "LabelRight", new GUILayoutOption[]
            {
                GUILayout.Width((float)292)
            });
            GUILayout.Space((float)5);
            this.aaLevelOption = GUILayout.SelectionGrid(this.aaLevelOption, new string[]
            {
                "Off",
                "x2",
                "x4"
            }, 3, new GUILayoutOption[0]);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space((float)10);
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            GUILayout.Label("Fullscreen:", "LabelRight", new GUILayoutOption[]
            {
                GUILayout.Width((float)292)
            });
            GUILayout.Space((float)5);
            if (GUILayout.Button("Change", new GUILayoutOption[]
            {
                GUILayout.Width((float)70)
            }))
            {
                if (true)
                {
                    if (runFromGamePlay && GameManager.instance)
                    {
                        GameManager.instance.menuGameComponent.fullscreenOptions = true;
                        GameManager.instance.menuGameComponent.options = false;
                    }
                    else
                    {
                        result = true;
                        this.serverListMenuState = MenuState.FullscreenOptions;
                    }
                    ShopManager.instance.selectedResolution = ShopManager.instance.currentSelectedResolution;
                    ShopManager.instance.showFullscreenDialog = ShopManager.instance.currentShowFullscreenDialog;
                }
                else
                {
                    ShopManager.instance.StartShop(ShopManager.fullscreenID, false, true);
                }
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space((float)10);
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            GUILayout.Label("Hide Shot Effects:", "LabelRight", new GUILayoutOption[]
            {
                GUILayout.Width((float)292)
            });
            GUILayout.Space((float)5);
            if (GUILayout.Button("Change", new GUILayoutOption[]
            {
                GUILayout.Width((float)70)
            }))
            {
                if (runFromGamePlay && GameManager.instance)
                {
                    GameManager.instance.menuGameComponent.options = false;
                }
                else
                {
                    result = true;
                    this.serverListMenuState = MenuState.HideShotEffectsOptions;
                }
                if (BASManager.instance.basPlayer.loginStatus == LoginStatus.Logged && BASManager.instance.basPlayer.proLevel > 0 && BASManager.instance.basPlayer.proExpDateTime > BASManager.instance.serverTime)
                {
                    ShopManager.instance.muteEffects = ShopManager.instance.currentMuteEffects;
                    if (runFromGamePlay && GameManager.instance)
                    {
                        GameManager.instance.menuGameComponent.muteEffectsOptions = true;
                    }
                }
                else
                {
                    ShopManager.instance.StartShop(ShopManager.pro1ID, false, true);
                }
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space((float)10);
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            GUILayout.Label("Show Audience:", "LabelRight", new GUILayoutOption[]
            {
                GUILayout.Width((float)292)
            });
            GUILayout.Space((float)5);
            string text = string.Empty;
            if (this.tribunesEnabledOption == 1)
            {
                text += "On";
            }
            else
            {
                text += "Off";
            }
            if (GUILayout.Button(text, new GUILayoutOption[]
            {
                GUILayout.Width((float)40)
            }))
            {
                if (this.tribunesEnabledOption == 1)
                {
                    this.tribunesEnabledOption = 0;
                }
                else
                {
                    this.tribunesEnabledOption = 1;
                }
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space((float)10);
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            GUILayout.Label("Grass 3D:", "LabelRight", new GUILayoutOption[]
            {
                GUILayout.Width((float)292)
            });
            GUILayout.Space((float)5);
            text = string.Empty;
            if (this.isGrass3DEnabled)
            {
                text += "On";
            }
            else
            {
                text += "Off";
            }
            if (GUILayout.Button(text, new GUILayoutOption[]
            {
                GUILayout.Width((float)40)
            }))
            {
                this.isGrass3DEnabled = !this.isGrass3DEnabled;
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space((float)10);
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            GUILayout.Label(StaticVariables.ColorsToHTML("Top Down Camera:", false), "LabelRight", new GUILayoutOption[]
            {
                GUILayout.Width((float)292)
            });
            GUILayout.Space((float)5);
            text = string.Empty;
            if (this.useTopDownCameraOption)
            {
                text += "On";
            }
            else
            {
                text += "Off";
            }
            if (GUILayout.Button(text, new GUILayoutOption[]
            {
                GUILayout.Width((float)40)
            }))
            {
                if (true)
                {
                    this.useTopDownCameraOption = !this.useTopDownCameraOption;
                    if (runFromGamePlay && GameManager.instance)
                    {
                        this.useTopDownCamera = this.useTopDownCameraOption;
                    }
                }
                else
                {
                    this.useTopDownCameraOption = false;
                    this.useTopDownCamera = false;
                    ShopManager.instance.StartShop(ShopManager.cameraID, false, true);
                }
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space((float)10);
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            if (this.useTopDownCameraOption)
            {
                GUI.enabled = false;
            }
            GUILayout.Label("Camera FOV:", "LabelRight", new GUILayoutOption[]
            {
                GUILayout.Width((float)292)
            });
            GUILayout.Space((float)5);
            this.cameraFovOption = GUILayout.HorizontalSlider(this.cameraFovOption, StaticVariables.minFov, StaticVariables.maxFov, new GUILayoutOption[]
            {
                GUILayout.Width((float)130)
            });
            this.cameraFovOption = (float)UnityBuiltins.parseInt(this.cameraFovOption);
            if (runFromGamePlay && GameManager.instance)
            {
                this.cameraFov = this.cameraFovOption;
            }
            GUILayout.Space((float)5);
            GUILayout.Label(string.Empty + this.cameraFovOption, new GUILayoutOption[0]);
            GUILayout.FlexibleSpace();
            if (this.useTopDownCameraOption)
            {
                GUI.enabled = true;
            }
            GUILayout.EndHorizontal();
            GUILayout.Space((float)10);
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            GUILayout.Label("VSync:", "LabelRight", new GUILayoutOption[]
            {
                GUILayout.Width((float)292)
            });
            GUILayout.Space((float)5);
            text = string.Empty;
            if (this.vSyncOption == 1)
            {
                text += "On";
            }
            else
            {
                text += "Off";
            }
            if (GUILayout.Button(text, new GUILayoutOption[]
            {
                GUILayout.Width((float)40)
            }))
            {
                if (this.vSyncOption == 1)
                {
                    this.vSyncOption = 0;
                }
                else
                {
                    this.vSyncOption = 1;
                }
            }
            if (this.vSyncOption == 0)
            {
                GUILayout.Space((float)10);
                GUILayout.Label("Max FPS:", "LabelRight", new GUILayoutOption[0]);
                GUILayout.Space((float)5);
                this.maxFpsOption = (int)GUILayout.HorizontalSlider((float)this.maxFpsOption, 30, 540, new GUILayoutOption[]
                {
                    GUILayout.Width((float)130)
                });
                this.maxFpsOption = UnityBuiltins.parseInt(this.maxFpsOption);
                this.maxFpsOption = UnityBuiltins.parseInt(this.maxFpsOption) / 30 * 30;
                GUILayout.Space((float)5);
                GUILayout.Label(string.Empty + this.maxFpsOption, new GUILayoutOption[0]);
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
        else if (this.optionsTab == 1)
        {
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            GUILayout.Label("Sound:", "LabelRight", new GUILayoutOption[]
            {
                GUILayout.Width((float)292)
            });
            GUILayout.Space((float)5);
            string text = string.Empty;
            if (this.isSoundOn)
            {
                text += "On";
            }
            else
            {
                text += "Off";
            }
            if (GUILayout.Button(text, new GUILayoutOption[]
            {
                GUILayout.Width((float)40)
            }))
            {
                this.isSoundOn = !this.isSoundOn;
            }
            AudioManager.instance.soundEnabled = this.isSoundOn;
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space((float)10);
            if (!this.isSoundOn)
            {
                GUI.enabled = false;
            }
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            GUILayout.Label("SFX Volume:", "LabelRight", new GUILayoutOption[]
            {
                GUILayout.Width((float)292)
            });
            GUILayout.Space((float)5);
            this.sfxVolumeOption = GUILayout.HorizontalSlider(this.sfxVolumeOption, (float)0, 100f, new GUILayoutOption[]
            {
                GUILayout.Width((float)130)
            });
            this.sfxVolumeOption = (float)UnityBuiltins.parseInt(this.sfxVolumeOption);
            if (true)
            {
                AudioManager.instance.sfxVolume = (double)(this.sfxVolumeOption * 0.01f);
            }
            GUILayout.Space((float)5);
            GUILayout.Label(string.Empty + this.sfxVolumeOption, new GUILayoutOption[0]);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space((float)10);
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            GUILayout.Label("Audience Volume:", "LabelRight", new GUILayoutOption[]
            {
                GUILayout.Width((float)292)
            });
            GUILayout.Space((float)5);
            this.ambientVolumeOption = GUILayout.HorizontalSlider(this.ambientVolumeOption, (float)0, 100f, new GUILayoutOption[]
            {
                GUILayout.Width((float)130)
            });
            this.ambientVolumeOption = (float)UnityBuiltins.parseInt(this.ambientVolumeOption);
            if (true)
            {
                AudioManager.instance.ambientVolume = (double)(this.ambientVolumeOption * 0.01f);
            }
            GUILayout.Space((float)5);
            GUILayout.Label(string.Empty + this.ambientVolumeOption, new GUILayoutOption[0]);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space((float)10);
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            GUILayout.Label("Menu Music Volume:", "LabelRight", new GUILayoutOption[]
            {
                GUILayout.Width((float)292)
            });
            GUILayout.Space((float)5);
            this.menuMusicVolumeOption = GUILayout.HorizontalSlider(this.menuMusicVolumeOption, (float)0, 100f, new GUILayoutOption[]
            {
                GUILayout.Width((float)130)
            });
            this.menuMusicVolumeOption = (float)UnityBuiltins.parseInt(this.menuMusicVolumeOption);
            if (true)
            {
                AudioManager.instance.menuMusicVolume = (double)(this.menuMusicVolumeOption * 0.01f);
            }
            GUILayout.Space((float)5);
            GUILayout.Label(string.Empty + this.menuMusicVolumeOption, new GUILayoutOption[0]);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space((float)10);
            if (!this.isSoundOn)
            {
                GUI.enabled = true;
            }
        }
        else if (this.optionsTab == 2)
        {
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            GUILayout.Label("Mouse sensitivity:", "LabelRight", new GUILayoutOption[]
            {
                GUILayout.Width((float)292)
            });
            GUILayout.Space((float)5);
            this.sensitivity = GUILayout.HorizontalSlider(this.sensitivity, StaticVariables.minSensitivity, StaticVariables.maxSensitivity, new GUILayoutOption[]
            {
                GUILayout.Width((float)130)
            });
            GUILayout.Space((float)5);
            GUILayout.Label(string.Empty + this.sensitivity, new GUILayoutOption[0]);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space((float)10);
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            GUILayout.Label("Invert Mouse Y Axis:", "LabelRight", new GUILayoutOption[]
            {
                GUILayout.Width((float)292)
            });
            GUILayout.Space((float)5);
            string text = string.Empty;
            if (this.isInvertMouseY)
            {
                text += "On";
            }
            else
            {
                text += "Off";
            }
            if (GUILayout.Button(text, new GUILayoutOption[]
            {
                GUILayout.Width((float)40)
            }))
            {
                this.isInvertMouseY = !this.isInvertMouseY;
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space((float)10);
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            GUILayout.Label("Curve Ball:", "LabelRight", new GUILayoutOption[]
            {
                GUILayout.Width((float)292)
            });
            GUILayout.Space((float)5);
            if (GUILayout.Button(GlobalManager.instance.arrowLeft, new GUILayoutOption[0]))
            {
                this.curveModeOption--;
                if (this.curveModeOption < 0)
                {
                    this.curveModeOption = 0;
                }
            }
            if (GUILayout.Button(GlobalManager.instance.arrowRight, new GUILayoutOption[0]))
            {
                this.curveModeOption++;
                if (this.curveModeOption > 3)
                {
                    this.curveModeOption = 3;
                }
            }
            GUILayout.Space((float)5);
            string text2 = string.Empty;
            if (this.curveModeOption == 0)
            {
                text2 = "Mouse Rotation";
            }
            else if (this.curveModeOption == 1)
            {
                text2 = "Mouse Rotation (Reversed)";
            }
            else if (this.curveModeOption == 2)
            {
                text2 = "Keyboard: QE keys";
            }
            else if (this.curveModeOption == 3)
            {
                text2 = "Keyboard: QE keys (Reversed)";
            }
            GUILayout.Label(text2, new GUILayoutOption[]
            {
                GUILayout.Width((float)219)
            });
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
        else if (this.optionsTab == 3)
        {
            int i = 0;
            while ((float)i < (float)Extensions.get_length(this.keyNames) * 0.5f)
            {
                GUILayout.BeginHorizontal(new GUILayoutOption[0]);
                GUILayout.Space((float)50);
                GUILayout.Label(this.keyNames[i] + ":", "LabelRight", new GUILayoutOption[]
                {
                    GUILayout.Width((float)100)
                });
                GUILayout.Space((float)10);
                string text3 = this.GetBetterKeyName((Keys)i, true) + string.Empty;
                if (this.keysTmp[i] == KeyCode.None)
                {
                    text3 = "---";
                }
                if (this.activeKeyIndex == i)
                {
                    GUI.enabled = true;
                    if (Convert.ToInt32(Time.realtimeSinceStartup * 10f) % 2 == 0)
                    {
                        text3 = "???";
                    }
                    else
                    {
                        text3 = string.Empty;
                    }
                }
                if (this.activeKeyIndex == -1)
                {
                    if (GUILayout.Button(text3, new GUILayoutOption[]
                    {
                        GUILayout.Width((float)100)
                    }))
                    {
                        this.activeKeyIndex = i;
                    }
                }
                else
                {
                    GUILayout.Box(text3, "BoxTransparent", new GUILayoutOption[]
                    {
                        GUILayout.Width((float)100),
                                  GUILayout.Height((float)24)
                    });
                }
                if (this.activeKeyIndex == i)
                {
                    GUI.enabled = false;
                }
                int num = i + UnityBuiltins.parseInt((float)Extensions.get_length(this.keyNames) * 0.5f - 0.1f) + 1;
                if (num < Extensions.get_length(this.keyNames))
                {
                    GUILayout.Space((float)50);
                    GUILayout.Label(this.keyNames[num] + ":", "LabelRight", new GUILayoutOption[]
                    {
                        GUILayout.Width((float)100)
                    });
                    GUILayout.Space((float)10);
                    text3 = this.GetBetterKeyName((Keys)num, true) + string.Empty;
                    if (this.keysTmp[num] == KeyCode.None)
                    {
                        text3 = "---";
                    }
                    if (this.activeKeyIndex == num)
                    {
                        GUI.enabled = true;
                        if (Convert.ToInt32(Time.realtimeSinceStartup * 10f) % 2 == 0)
                        {
                            text3 = "???";
                        }
                        else
                        {
                            text3 = string.Empty;
                        }
                    }
                    if (this.activeKeyIndex == -1)
                    {
                        if (GUILayout.Button(text3, new GUILayoutOption[]
                        {
                            GUILayout.Width((float)100)
                        }))
                        {
                            this.activeKeyIndex = num;
                        }
                    }
                    else
                    {
                        GUILayout.Box(text3, "BoxTransparent", new GUILayoutOption[]
                        {
                            GUILayout.Width((float)100),
                                      GUILayout.Height((float)24)
                        });
                    }
                    if (this.activeKeyIndex == num)
                    {
                        GUI.enabled = false;
                    }
                }
                GUILayout.EndHorizontal();
                GUILayout.Space((float)10);
                i++;
            }
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Restore Defaults", new GUILayoutOption[]
            {
                GUILayout.Width((float)150)
            }))
            {
                this.SetDefaultKeysTmp();
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
        else if (this.optionsTab == 4)
        {
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            GUILayout.Label("Pro Options:", "LabelRight", new GUILayoutOption[]
            {
                GUILayout.Width((float)292)
            });
            GUILayout.Space((float)5);
            if (GUILayout.Button("Change", new GUILayoutOption[]
            {
                GUILayout.Width((float)70)
            }))
            {
                if (BASManager.instance.basPlayer.loginStatus == LoginStatus.Logged && BASManager.instance.basPlayer.proLevel > 0 && BASManager.instance.basPlayer.proExpDateTime > BASManager.instance.serverTime)
                {
                    if (runFromGamePlay)
                    {
                        if (GameManager.instance)
                        {
                            GameManager.instance.menuGameComponent.proOptions = true;
                            GameManager.instance.menuGameComponent.options = false;
                        }
                    }
                    else
                    {
                        result = true;
                        this.serverListMenuState = MenuState.ProOptions;
                    }
                    ShopManager.instance.selectedProCC = BASManager.instance.proCC;
                    ShopManager.instance.selectedProGoalMessage = BASManager.instance.proGoalMessage;
                    ShopManager.instance.selectedProHudNumber = BASManager.instance.proHudNumber;
                    ShopManager.instance.selectedCCScrollPosition = 0;
                }
                else
                {
                    ShopManager.instance.StartShop(ShopManager.pro1ID, false, true);
                }
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space((float)10);
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            GUILayout.Label("Shirt Number:", "LabelRight", new GUILayoutOption[]
            {
                GUILayout.Width((float)292)
            });
            GUILayout.Space((float)5);
            if (GUILayout.Button("Change", new GUILayoutOption[]
            {
                GUILayout.Width((float)70)
            }))
            {
                if (ShopManager.instance.PlayerHasItem(-1, ShopManager.shirtNumberID))
                {
                    ShopManager.instance.selectedShirtNumber = BASManager.instance.basPlayer.currentShirtNumber;
                    if (ShopManager.instance.selectedShirtNumber < 0 || ShopManager.instance.selectedShirtNumber > 99)
                    {
                        ShopManager.instance.selectedShirtNumber = 0;
                    }
                    if (runFromGamePlay)
                    {
                        if (GameManager.instance)
                        {
                            GameManager.instance.menuGameComponent.shirtNumberOptions = true;
                            GameManager.instance.menuGameComponent.options = false;
                        }
                    }
                    else
                    {
                        result = true;
                        this.serverListMenuState = MenuState.ShirtNumberOptions;
                    }
                }
                else
                {
                    ShopManager.instance.StartShop(ShopManager.shirtNumberID, false, true);
                }
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space((float)10);
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            GUILayout.Label("Show Player Names:", "LabelRight", new GUILayoutOption[]
            {
                GUILayout.Width((float)292)
            });
            GUILayout.Space((float)5);
            string text = string.Empty;
            if (this.isPlayerNamesOn)
            {
                text += "On";
            }
            else
            {
                text += "Off";
            }
            if (GUILayout.Button(text, new GUILayoutOption[]
            {
                GUILayout.Width((float)40)
            }))
            {
                this.isPlayerNamesOn = !this.isPlayerNamesOn;
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space((float)10);
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            GUILayout.Label("Mute List:", "LabelRight", new GUILayoutOption[]
            {
                GUILayout.Width((float)292)
            });
            GUILayout.Space((float)5);
            if (runFromGamePlay && GameManager.instance)
            {
                if (GUILayout.Button("Change", new GUILayoutOption[]
                {
                    GUILayout.Width((float)70)
                }) && runFromGamePlay && GameManager.instance)
                {
                    GameManager.instance.menuGameComponent.changeMute = true;
                    GameManager.instance.menuGameComponent.options = false;
                }
            }
            else
            {
                GUILayout.Label("Run the game to change this option!", new GUILayoutOption[0]);
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space((float)10);
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            GUILayout.Label("Team Shirts Code:", "LabelRight", new GUILayoutOption[]
            {
                GUILayout.Width((float)292)
            });
            GUILayout.Space((float)5);
            this.teamShirtsCodeOption = GUILayout.TextField(this.teamShirtsCodeOption, 4, new GUILayoutOption[]
            {
                GUILayout.Width((float)70)
            });
            this.teamShirtsCodeOption = this.teamShirtsCodeOption.ToUpper();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space((float)10);
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            GUILayout.Label("Create Team Shirts Code (website):", "LabelRight", new GUILayoutOption[]
            {
                GUILayout.Width((float)292)
            });
            GUILayout.Space((float)5);
            if (GUILayout.Button("Create", new GUILayoutOption[]
            {
                GUILayout.Width((float)70)
            }))
            {
                StaticVariables.OpenURLTop(StaticVariables.domain + "/?url=teams");
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space((float)10);
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            GUILayout.Label("Antilag:", "LabelRight", new GUILayoutOption[]
            {
                GUILayout.Width((float)292)
            });
            GUILayout.Space((float)5);
            text = string.Empty;
            if (this.isPredictionOn)
            {
                text += "On";
            }
            else
            {
                text += "Off";
            }
            if (GUILayout.Button(text, new GUILayoutOption[]
            {
                GUILayout.Width((float)70)
            }))
            {
                this.isPredictionOn = !this.isPredictionOn;
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space((float)10);
            if (!this.isPredictionOn)
            {
                GUI.enabled = false;
            }
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            GUILayout.Label("Frames Of Latency:", "LabelRight", new GUILayoutOption[]
            {
                GUILayout.Width((float)292)
            });
            GUILayout.Space((float)5);
            GUILayout.Space((float)5);
            this.predictionLevel = (int)GUILayout.HorizontalSlider((float)this.predictionLevel, (float)0, (float)9, new GUILayoutOption[]
            {
                GUILayout.Width((float)130)
            });
            this.predictionLevel = UnityBuiltins.parseInt(this.predictionLevel);
            GUILayout.Space((float)5);
            GUILayout.Label(string.Empty + this.predictionLevel, new GUILayoutOption[0]);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            if (!this.isPredictionOn)
            {
                GUI.enabled = true;
            }
        }
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal(new GUILayoutOption[0]);
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Cancel", new GUILayoutOption[]
        {
            GUILayout.Height((float)30),
                             GUILayout.Width((float)150)
        }))
        {
            result = true;
            this.serverListMenuState = MenuState.Main;
            if (true)
            {
                this.cameraFov = this.prevCameraFov;
                this.useTopDownCamera = this.prevUseTopDownCamera;
                AudioManager.instance.sfxVolume = (double)this.prevSFXVolume;
                AudioManager.instance.ambientVolume = (double)this.prevAmbientVolume;
                AudioManager.instance.menuMusicVolume = (double)this.prevMenuMusicVolume;
                AudioManager.instance.soundEnabled = this.prevSoundEnabled;
            }
        }
        GUILayout.Space((float)10);
        if (GUILayout.Button("OK", new GUILayoutOption[]
        {
            GUILayout.Height((float)30),
                             GUILayout.Width((float)150)
        }))
        {
            result = true;
            this.serverListMenuState = MenuState.Main;
            AudioManager.instance.sfxVolume = (double)(this.sfxVolumeOption * 0.01f);
            AudioManager.instance.ambientVolume = (double)(this.ambientVolumeOption * 0.01f);
            AudioManager.instance.menuMusicVolume = (double)(this.menuMusicVolumeOption * 0.01f);
            GlobalManager.instance.mouseSpeed = this.sensitivity;
            GlobalManager.instance.invertMouseY = this.isInvertMouseY;
            AudioManager.instance.SetSoundState(this.isSoundOn);
            AudioManager.instance.SetAmbientState(AudioManager.instance.sfxVolume != (double)0);
            AudioManager.instance.SetMusicState(AudioManager.instance.menuMusicVolume != (double)0);
            GlobalManager.instance.playerNamesEnabled = this.isPlayerNamesOn;
            GlobalManager.instance.tribunesEnabled = this.tribunesEnabledOption;
            GlobalManager.instance.teamShirtsCode = this.teamShirtsCodeOption;
            GlobalManager.instance.grass3DEnabled = this.isGrass3DEnabled;
            GlobalManager.instance.cameraFov = this.cameraFovOption;
            GlobalManager.instance.useTopDownCamera = this.useTopDownCameraOption;
            if (this.aaLevelOption == 2)
            {
                GlobalManager.instance.aaLevel = 4;
            }
            else if (this.aaLevelOption == 1)
            {
                GlobalManager.instance.aaLevel = 2;
            }
            else
            {
                GlobalManager.instance.aaLevel = 0;
            }
            this.vSync = this.vSyncOption;
            this.maxFps = this.maxFpsOption;
            GlobalManager.instance.curveMode = (CurveMode)this.curveModeOption;
            ShopManager.instance.showSpeedometer = this.isSpeedometer;
            if (PredictionManager.instance)
            {
                PredictionManager.instance.clientLagFrames = this.predictionLevel;
                if (PredictionManager.instance.usePrediction != this.isPredictionOn)
                {
                    PredictionManager.instance.ClearFinished();
                    PredictionManager.instance.ClearOneFrame();
                    PredictionManager.instance.localPlayerMinusFrames = 0;
                }
                PredictionManager.instance.usePrediction = this.isPredictionOn;
            }
            else
            {
                this.clientLagFramesTmp = this.predictionLevel;
                this.usePredictionTmp = this.isPredictionOn;
            }
            for (int i = 0; i < Extensions.get_length(this.keys); i++)
            {
                this.keys[i] = this.keysTmp[i];
            }
            ShopManager.instance.SavePrefs();
            GlobalManager.instance.SaveOptions();
            if (runFromGamePlay && GameManager.instance)
            {
                if (Network.isClient)
                {
                    global::NetworkManager.instance.networkView1.RPC("Y", RPCMode.Server, new object[]
                    {
                        StaticVariables.CipherCode(this.teamShirtsCode)
                    });
                }
                else
                {
                    GameManager.instance.ChangeTeamShirtsCodeAt(0, StaticVariables.CipherCode(this.teamShirtsCode));
                }
            }
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
        GUI.enabled = true;
        return result;
    }

    // Token: 0x0600028B RID: 651 RVA: 0x00038954 File Offset: 0x00036B54
    public virtual void LoadOptions()
    {
        this.mouseSpeed = PlayerPrefs.GetFloat("mouseSpeed", StaticVariables.defaultMouseSpeed);
        this.cameraFov = PlayerPrefs.GetFloat("cameraFov", StaticVariables.defaultCameraFov);
        int num = 0;
        num = PlayerPrefs.GetInt("invertMouseY", 0);
        if (num == 1)
        {
            this.invertMouseY = true;
        }
        else
        {
            this.invertMouseY = false;
        }
        int num2 = 0;
        num2 = PlayerPrefs.GetInt("soundEnabled", 1);
        if (GlobalManager.instance.isDedicated)
        {
            num2 = 0;
        }
        if (num2 == 1)
        {
            AudioManager.instance.SetSoundState(true);
        }
        else
        {
            AudioManager.instance.SetSoundState(false);
        }
        int num3 = 0;
        num3 = PlayerPrefs.GetInt("playerNamesEnabled_", 1);
        if (num3 == 1)
        {
            this.playerNamesEnabled = true;
        }
        else
        {
            this.playerNamesEnabled = false;
        }
        int num4 = 0;
        num4 = PlayerPrefs.GetInt("aaLevel", 2);
        this.aaLevel = num4;
        if (this.aaLevel > 4)
        {
            this.aaLevel = 4;
        }
        int num5 = 0;
        num5 = PlayerPrefs.GetInt("vSync", 1);
        this.vSync = num5;
        this.maxFps = PlayerPrefs.GetInt("maxFps", (int)120);
        this.curveMode = (CurveMode)PlayerPrefs.GetInt("curveMode", 2);
        this.tribunesEnabled = PlayerPrefs.GetInt("tribunesEnabled21", 1);
        if (GlobalManager.instance.isDedicated)
        {
            this.tribunesEnabled = 0;
        }
        this.teamShirtsCode = PlayerPrefs.GetString("teamShirtsCode", string.Empty);
        int num6 = 0;
        num6 = PlayerPrefs.GetInt("usePrediction", 1);
        if (num6 == 1)
        {
            this.usePredictionTmp = true;
        }
        else
        {
            this.usePredictionTmp = false;
        }
        this.clientLagFramesTmp = PlayerPrefs.GetInt("clientLagFrames", 3);
        if (PredictionManager.instance)
        {
            PredictionManager.instance.usePrediction = this.usePredictionTmp;
            PredictionManager.instance.clientLagFrames = this.clientLagFramesTmp;
        }
        if (GlobalManager.instance.isTutorial)
        {
            this.tribunesEnabled = 0;
            AudioManager.instance.SetAmbientState(false);
        }
        int num7 = 0;
        num7 = PlayerPrefs.GetInt("grass3DEnabled", 1);
        if (GlobalManager.instance.isDedicated)
        {
            num7 = 0;
        }
        if (num7 == 1)
        {
            this.grass3DEnabled = true;
        }
        else
        {
            this.grass3DEnabled = false;
        }
        int num8 = 0;
        num8 = PlayerPrefs.GetInt("useTopDownCamera", 0);
        if (GlobalManager.instance.isDedicated)
        {
            num8 = 0;
        }
        if (num8 == 1)
        {
            this.useTopDownCamera = true;
        }
        else
        {
            this.useTopDownCamera = false;
        }
        this.localBallIndex = PlayerPrefs.GetInt("localBallIndex", 0);
        this.localBasketBallIndex = PlayerPrefs.GetInt("localBasketBallIndex", 50);
        this.isSpaceKeyFree = true;
        for (int i = 0; i < Extensions.get_length(this.keyNames); i++)
        {
            this.keys[i] = (KeyCode)PlayerPrefs.GetInt(this.keyNames[i] + string.Empty, (int)this.defKeys[i]);
            if (this.keys[i] == KeyCode.Space)
            {
                this.isSpaceKeyFree = false;
            }
        }
        AudioManager.instance.sfxVolume = (double)PlayerPrefs.GetInt("sfxVolume", UnityBuiltins.parseInt((StaticVariables.defSFXVolume + (double)0.001f) * 100.0)) * 0.01;
        AudioManager.instance.ambientVolume = (double)PlayerPrefs.GetInt("ambientVolume", UnityBuiltins.parseInt((StaticVariables.defAmbientVolume + (double)0.001f) * 100.0)) * 0.01;
        AudioManager.instance.menuMusicVolume = (double)PlayerPrefs.GetInt("menuMusicVolume", UnityBuiltins.parseInt((StaticVariables.defMenuMusicVolume + (double)0.001f) * 100.0)) * 0.01;
        if (AudioManager.instance.sfxVolume == (double)0)
        {
            AudioManager.instance.SetAmbientState(false);
        }
        else
        {
            AudioManager.instance.SetAmbientState(true);
        }
        if (AudioManager.instance.menuMusicVolume == (double)0)
        {
            AudioManager.instance.SetMusicState(false);
        }
        else
        {
            AudioManager.instance.SetMusicState(true);
        }
        this.tutorialDone = PlayerPrefs.GetInt("tutorialDone", 0);
        this.jugglingHighScore = PlayerPrefs.GetInt("jugglingHighScore", 0);
    }

    // Token: 0x0600028C RID: 652 RVA: 0x00038D90 File Offset: 0x00036F90
    public virtual void SaveOptions()
    {
        if (!GlobalManager.instance.isDedicated)
        {
            PlayerPrefs.SetFloat("mouseSpeed", this.mouseSpeed);
            PlayerPrefs.SetFloat("cameraFov", this.cameraFov);
            int value = 0;
            if (this.invertMouseY)
            {
                value = 1;
            }
            else
            {
                value = 0;
            }
            PlayerPrefs.SetInt("invertMouseY", value);
            int value2 = 0;
            if (AudioManager.instance.soundEnabled)
            {
                value2 = 1;
            }
            else
            {
                value2 = 0;
            }
            PlayerPrefs.SetInt("soundEnabled", value2);
            int value3 = 0;
            if (this.playerNamesEnabled)
            {
                value3 = 1;
            }
            else
            {
                value3 = 0;
            }
            PlayerPrefs.SetInt("playerNamesEnabled_", value3);
            PlayerPrefs.SetInt("aaLevel", this.aaLevel);
            PlayerPrefs.SetInt("vSync", this.vSync);
            PlayerPrefs.SetInt("maxFps", this.maxFps);
            PlayerPrefs.SetInt("curveMode", (int)this.curveMode);
            if (!GlobalManager.instance.isTutorial)
            {
                PlayerPrefs.SetInt("tribunesEnabled21", this.tribunesEnabled);
            }
            PlayerPrefs.SetString("teamShirtsCode", this.teamShirtsCode);
            if (PredictionManager.instance)
            {
                this.usePredictionTmp = PredictionManager.instance.usePrediction;
                this.clientLagFramesTmp = PredictionManager.instance.clientLagFrames;
            }
            int value4 = 0;
            if (this.usePredictionTmp)
            {
                value4 = 1;
            }
            else
            {
                value4 = 0;
            }
            PlayerPrefs.SetInt("usePrediction", value4);
            PlayerPrefs.SetInt("clientLagFrames", this.clientLagFramesTmp);
            int value5 = 0;
            if (this.grass3DEnabled)
            {
                value5 = 1;
            }
            else
            {
                value5 = 0;
            }
            PlayerPrefs.SetInt("grass3DEnabled", value5);
            int value6 = 0;
            if (this.useTopDownCamera)
            {
                value6 = 1;
            }
            else
            {
                value6 = 0;
            }
            PlayerPrefs.SetInt("useTopDownCamera", value6);
            PlayerPrefs.SetInt("localBallIndex", this.localBallIndex);
            PlayerPrefs.SetInt("localBasketBallIndex", this.localBasketBallIndex);
            this.isSpaceKeyFree = true;
            for (int i = 0; i < Extensions.get_length(this.keyNames); i++)
            {
                PlayerPrefs.SetInt(this.keyNames[i] + string.Empty, (int)this.keys[i]);
                if (this.keys[i] == KeyCode.Space)
                {
                    this.isSpaceKeyFree = false;
                }
            }
            PlayerPrefs.SetInt("sfxVolume", UnityBuiltins.parseInt((AudioManager.instance.sfxVolume + (double)0.001f) * 100.0));
            PlayerPrefs.SetInt("ambientVolume", UnityBuiltins.parseInt((AudioManager.instance.ambientVolume + (double)0.001f) * 100.0));
            PlayerPrefs.SetInt("menuMusicVolume", UnityBuiltins.parseInt((AudioManager.instance.menuMusicVolume + (double)0.001f) * 100.0));
            PlayerPrefs.SetInt("tutorialDone", this.tutorialDone);
            PlayerPrefs.Save();
        }
    }

    // Token: 0x0600028D RID: 653 RVA: 0x00039078 File Offset: 0x00037278
    public virtual void SetPressedKey()
    {
        if (this.activeKeyIndex >= 0)
        {
            for (int i = 0; i <= 329; i++)
            {
                if (Input.GetKey((KeyCode)i))
                {
                    if (i == 27 || i == 9)
                    {
                        this.activeKeyIndex = -1;
                    }
                    else
                    {
                        this.ClearAllKeysWithKeyCode((KeyCode)i);
                        this.keysTmp[this.activeKeyIndex] = (KeyCode)i;
                        this.activeKeyIndex = -1;
                    }
                    return;
                }
            }
            for (int i = 0; i < 7; i++)
            {
                if (Input.GetMouseButton(i))
                {
                    this.ClearAllKeysWithKeyCode(KeyCode.Mouse0 + i);
                    this.keysTmp[this.activeKeyIndex] = KeyCode.Mouse0 + i;
                    this.activeKeyIndex = -1;
                    break;
                }
            }
        }
    }

    // Token: 0x0600028E RID: 654 RVA: 0x0003913C File Offset: 0x0003733C
    public virtual void ClearAllKeysWithKeyCode(KeyCode keyCode)
    {
        for (int i = 0; i < Extensions.get_length(this.keysTmp); i++)
        {
            if (this.keysTmp[i] == keyCode)
            {
                this.keysTmp[i] = KeyCode.None;
            }
        }
    }

    // Token: 0x0600028F RID: 655 RVA: 0x0003917C File Offset: 0x0003737C
    public virtual void SetDefaultKeysTmp()
    {
        for (int i = 0; i < Extensions.get_length(this.keyNames); i++)
        {
            this.keysTmp[i] = this.defKeys[i];
        }
    }

    // Token: 0x06000290 RID: 656 RVA: 0x000391B8 File Offset: 0x000373B8
    public virtual bool GetKeyTmp(Keys key)
    {
        return (this.keys[(int)key] < KeyCode.Mouse0 || this.keys[(int)key] > KeyCode.Mouse6) ? Input.GetKey(this.keys[(int)key]) : Input.GetMouseButton(this.keys[(int)key] - KeyCode.Mouse0);
    }

    // Token: 0x06000291 RID: 657 RVA: 0x00039214 File Offset: 0x00037414
    public virtual bool GetKeyDownTmp(Keys key)
    {
        return (this.keys[(int)key] < KeyCode.Mouse0 || this.keys[(int)key] > KeyCode.Mouse6) ? Input.GetKeyDown(this.keys[(int)key]) : Input.GetMouseButtonDown(this.keys[(int)key] - KeyCode.Mouse0);
    }

    // Token: 0x06000292 RID: 658 RVA: 0x00039270 File Offset: 0x00037470
    public virtual bool GetKeyUpTmp(Keys key)
    {
        return (this.keys[(int)key] < KeyCode.Mouse0 || this.keys[(int)key] > KeyCode.Mouse6) ? Input.GetKeyUp(this.keys[(int)key]) : Input.GetMouseButtonUp(this.keys[(int)key] - KeyCode.Mouse0);
    }

    // Token: 0x06000293 RID: 659 RVA: 0x000392CC File Offset: 0x000374CC
    public virtual bool GetKey(Keys key)
    {
        bool result;
        if (this.GetKeyTmp(key))
        {
            result = true;
        }
        else if (key == Keys.Jump && this.isSpaceKeyFree && Input.GetKey(KeyCode.Space))
        {
            result = true;
        }
        else
        {
            if (this.usingGamepad && key == Keys.MoveBack)
            {
                if (Input.GetAxisRaw("Vertical") > 0.9f)
                {
                    return true;
                }
            }
            else if (this.usingGamepad && key == Keys.MoveForward && Input.GetAxisRaw("Vertical") < -0.9f)
            {
                return true;
            }
            result = false;
        }
        return result;
    }

    // Token: 0x06000294 RID: 660 RVA: 0x00039370 File Offset: 0x00037570
    public virtual bool GetKeyDown(Keys key)
    {
        bool result;
        if (this.GetKeyDownTmp(key))
        {
            result = true;
        }
        else if (key == Keys.Jump && this.isSpaceKeyFree && Input.GetKeyDown(KeyCode.Space))
        {
            result = true;
        }
        else
        {
            if (this.usingGamepad && key == Keys.MoveBack)
            {
                if (Input.GetAxisRaw("Vertical") > 0.9f)
                {
                    return true;
                }
            }
            else if (this.usingGamepad && key == Keys.MoveForward && Input.GetAxisRaw("Vertical") < -0.9f)
            {
                return true;
            }
            result = false;
        }
        return result;
    }

    // Token: 0x06000295 RID: 661 RVA: 0x00039414 File Offset: 0x00037614
    public virtual bool GetKeyUp(Keys key)
    {
        return this.GetKeyUpTmp(key) || (key == Keys.Jump && this.isSpaceKeyFree && Input.GetKeyUp(KeyCode.Space));
    }

    // Token: 0x06000296 RID: 662 RVA: 0x00039450 File Offset: 0x00037650
    public virtual float GetKeyAxis(KeyAxis keyAxis)
    {
        return (keyAxis != KeyAxis.Horizontal) ? ((keyAxis != KeyAxis.Vertical) ? ((float)0) : this.verticalAxis) : this.horizontalAxis;
    }

    // Token: 0x06000297 RID: 663 RVA: 0x00039480 File Offset: 0x00037680
    public virtual float GetKeyAxisRaw(KeyAxis keyAxis)
    {
        float keyAxis2 = this.GetKeyAxis(keyAxis);
        return (keyAxis2 >= (float)0) ? ((keyAxis2 <= (float)0) ? ((float)0) : 1f) : -1f;
    }

    // Token: 0x06000298 RID: 664 RVA: 0x000394C0 File Offset: 0x000376C0
    public virtual float GetMouseGamepadAxisRaw(MouseAxis mouseAxis)
    {
        return (mouseAxis != MouseAxis.X) ? ((mouseAxis != MouseAxis.Y) ? ((float)0) : ((!this.usingGamepad) ? Input.GetAxisRaw("Mouse Y") : ((float)0))) : ((!this.usingGamepad) ? Input.GetAxisRaw("Mouse X") : ((float)0));
    }

    // Token: 0x06000299 RID: 665 RVA: 0x00039530 File Offset: 0x00037730
    public virtual string GetBetterKeyName(Keys key)
    {
        return this.GetBetterKeyName(key, false);
    }

    // Token: 0x0600029A RID: 666 RVA: 0x0003953C File Offset: 0x0003773C
    public virtual string GetBetterKeyName(Keys key, bool useTmp)
    {
        KeyCode[] array = this.keys;
        if (useTmp)
        {
            array = this.keysTmp;
        }
        string result = array[(int)key] + string.Empty;
        if (array[(int)key] == KeyCode.Mouse0)
        {
            result = "LMB";
        }
        else if (array[(int)key] == KeyCode.Mouse1)
        {
            result = "RMB";
        }
        else if (array[(int)key] == KeyCode.Mouse2)
        {
            result = "MMB";
        }
        if (array[(int)key] >= KeyCode.JoystickButton0 && array[(int)key] <= KeyCode.JoystickButton19)
        {
            result = "Pad" + (array[(int)key] - KeyCode.JoystickButton0);
        }
        return result;
    }

    // Token: 0x0600029B RID: 667 RVA: 0x000395E8 File Offset: 0x000377E8
    public virtual void CheckUsingGamepad()
    {
        this.usingGamepad = false;
        for (int i = 0; i < Extensions.get_length(this.keys); i++)
        {
            if (this.keys[i] >= KeyCode.JoystickButton0 && this.keys[i] <= KeyCode.JoystickButton19)
            {
                this.usingGamepad = true;
                break;
            }
        }
    }

    // Token: 0x0600029C RID: 668 RVA: 0x00039648 File Offset: 0x00037848
    public virtual void GetAuthSesstionTicketResponse()
    {
        this.steamTicketString = string.Empty;
        if (SteamManager.Initialized)
        {
            AppId_t appID = SteamUtils.GetAppID();
            Debug.Log("app_id: " + appID);
            if (this.ticketBytes == null)
            {
                this.ticketBytes = new byte[2000];
            }
            BASManager.instance.wwwTimer = (float)0;
            if (!RuntimeServices.EqualityOperator(this.hAuthTicket, null))
            {
                SteamUser.CancelAuthTicket(this.hAuthTicket);
            }
            this.getAuthSessionTicketResponse = Callback<GetAuthSessionTicketResponse_t>.Create(new Callback<GetAuthSessionTicketResponse_t>.DispatchDelegate(this.OnGetAuthSessionTicketResponse));
            this.microTxnAuthorizationResponse = Callback<MicroTxnAuthorizationResponse_t>.Create(new Callback<MicroTxnAuthorizationResponse_t>.DispatchDelegate(this.OnMicroTxnAuthorizationResponse));
            this.hAuthTicket = SteamUser.GetAuthSessionTicket(this.ticketBytes, Extensions.get_length(this.ticketBytes), out this.outTicketLength);
            Debug.Log("hAuthTicket: " + this.hAuthTicket);
        }
        Debug.Log("GetAuthSesstionTicketResponse(), length: " + this.outTicketLength);
    }

    // Token: 0x0600029D RID: 669 RVA: 0x0003975C File Offset: 0x0003795C
    public virtual void OnGetAuthSessionTicketResponse(GetAuthSessionTicketResponse_t callback)
    {
        byte[] array = new byte[(int)this.outTicketLength];
        int num = 0;
        while ((long)num < (long)this.outTicketLength)
        {
            array[num] = this.ticketBytes[num];
            num++;
        }
        if (callback.m_eResult == EResult.k_EResultOK)
        {
            this.steamTicketString = BitConverter.ToString(array).Replace("-", string.Empty);
            this.StartCoroutine(BASManager.instance.LoginSteam(this.steamTicketString));
        }
        else
        {
            BASManager.instance.loginFinishStatus = LoginFinishStatus.ConnectionProblem;
        }
    }

    // Token: 0x0600029E RID: 670 RVA: 0x000397F0 File Offset: 0x000379F0
    public virtual void OnMicroTxnAuthorizationResponse(MicroTxnAuthorizationResponse_t callback)
    {
        if (callback.m_bAuthorized == 1)
        {
            BASManager.instance.lastTransactionCoinsID = (int)callback.m_ulOrderID;
            if (ShopManager.instance.menuState == ShopMenuState.BuyCoinsFinish)
            {
                BASManager.instance.buyCoinsFinishStatus = BuyCoinsFinishStatus.Buying;
                BASManager.instance.wwwTimer = (float)0;
                ShopManager.instance.BuyCoinsFinish();
            }
        }
        else if (ShopManager.instance.menuState == ShopMenuState.BuyCoinsFinish)
        {
            BASManager.instance.buyCoinsFinishStatus = BuyCoinsFinishStatus.Null;
            if (BASManager.instance.lastOptionID == ShopManager.nickID)
            {
                ShopManager.instance.menuState = ShopMenuState.MainShop;
            }
            else
            {
                ShopManager.instance.menuState = ShopMenuState.BuyCoins;
            }
        }
    }

    // Token: 0x0600029F RID: 671 RVA: 0x000398A8 File Offset: 0x00037AA8
    public virtual void OnGameOverlayActivated(GameOverlayActivated_t callback)
    {
        if (callback.m_bActive == 0)
        {
            this.StartCoroutine(BASManager.instance.GetInformationFromServer(false));
        }
    }

    // Token: 0x060002A0 RID: 672 RVA: 0x000398D8 File Offset: 0x00037AD8
    public virtual void OnPortMappingDone(Mapping mapping, bool isError)
    {
        if (isError)
        {
            Debug.Log("Port mapping failed");
        }
        else
        {
            Debug.Log("Port " + mapping.PublicPort + " mapped (" + mapping.Protocol + ")");
        }
    }

    // Token: 0x060002A1 RID: 673 RVA: 0x00039938 File Offset: 0x00037B38
    public virtual IEnumerator LoadNews()
    {
        return new GlobalManager.$LoadNews$1873(this).GetEnumerator();
    }

    // Token: 0x060002A2 RID: 674 RVA: 0x00039948 File Offset: 0x00037B48
    public virtual void OnFailedToConnectToMasterServer(NetworkConnectionError info)
    {
        if (this.isDedicated)
        {
            this.masterServerFailedTimer = (float)0;
        }
    }

    // Token: 0x060002A3 RID: 675 RVA: 0x00039960 File Offset: 0x00037B60
    public virtual void SetAchievement(string name)
    {
        if (SteamManager.Initialized)
        {
            SteamUserStats.SetAchievement(name);
            bool flag = SteamUserStats.StoreStats();
        }
    }

    // Token: 0x060002A4 RID: 676 RVA: 0x00039988 File Offset: 0x00037B88
    public virtual void OpenSteamBallStoreItemID(int itemID)
    {
        if (SteamManager.Initialized)
        {
            SteamFriends.ActivateGameOverlayToWebPage("https://store.steampowered.com/itemstore/485610/detail/" + itemID + "/");
        }
    }

    // Token: 0x060002A5 RID: 677 RVA: 0x000399C0 File Offset: 0x00037BC0
    public virtual void OpenSteamBallStore()
    {
        if (SteamManager.Initialized)
        {
            SteamFriends.ActivateGameOverlayToWebPage("https://store.steampowered.com/itemstore/485610/browse/?filter=all");
        }
    }

    // Token: 0x060002A6 RID: 678 RVA: 0x000399E4 File Offset: 0x00037BE4
    public virtual void OpenSteamBallStoreMainItems()
    {
        if (SteamManager.Initialized)
        {
            SteamFriends.ActivateGameOverlayToWebPage("https://store.steampowered.com/itemstore/485610/browse/?filter=Main%20Items");
        }
    }

    // Token: 0x060002A7 RID: 679 RVA: 0x00039A08 File Offset: 0x00037C08
    public virtual void OpenSteamBallMarket()
    {
        if (SteamManager.Initialized)
        {
            SteamFriends.ActivateGameOverlayToWebPage("https://steamcommunity.com/market/search?appid=485610");
        }
    }

    // Token: 0x060002A8 RID: 680 RVA: 0x00039A2C File Offset: 0x00037C2C
    public virtual void OpenSteamBallMarketBoxSearch()
    {
        if (SteamManager.Initialized)
        {
            SteamFriends.ActivateGameOverlayToWebPage("https://steamcommunity.com/market/search?appid=485610&q=box");
        }
    }

    // Token: 0x060002A9 RID: 681 RVA: 0x00039A50 File Offset: 0x00037C50
    public virtual void OpenSteamBallInventory()
    {
        if (SteamManager.Initialized)
        {
            SteamFriends.ActivateGameOverlayToWebPage("http://steamcommunity.com/profiles/" + SteamUser.GetSteamID().ToString() + "/inventory/#485610");
        }
    }

    // Token: 0x060002AA RID: 682 RVA: 0x00039A90 File Offset: 0x00037C90
    public virtual void SteamTriggerFirstTimeDrop()
    {
        this.StartCoroutine(SharpHelper.SteamTriggerFirstTimeDrop());
    }

    // Token: 0x060002AB RID: 683 RVA: 0x00039AA0 File Offset: 0x00037CA0
    public virtual void SteamTriggerNormalDrop()
    {
        SharpHelper.SteamTriggerNormalDrop();
    }

    // Token: 0x060002AC RID: 684 RVA: 0x00039AA8 File Offset: 0x00037CA8
    public virtual void Main()
    {
    }

    // Token: 0x04000670 RID: 1648
    public GUISkin skin;

    // Token: 0x04000671 RID: 1649
    public Texture[] filterSmallIcons;

    // Token: 0x04000672 RID: 1650
    public Texture frameSelection;

    // Token: 0x04000673 RID: 1651
    public Texture menuBG;

    // Token: 0x04000674 RID: 1652
    public Texture readyFrame;

    // Token: 0x04000675 RID: 1653
    public GameObject cameraDedicated;

    // Token: 0x04000676 RID: 1654
    public Texture arrowLeft;

    // Token: 0x04000677 RID: 1655
    public Texture arrowRight;

    // Token: 0x04000678 RID: 1656
    public Texture unusualsoftLogo;

    // Token: 0x04000679 RID: 1657
    public Texture teamInvIcon;

    // Token: 0x0400067A RID: 1658
    public Texture newsIcon;

    // Token: 0x0400067B RID: 1659
    public Texture eslLogo;

    // Token: 0x0400067C RID: 1660
    public Texture newIcon;

    // Token: 0x0400067D RID: 1661
    public Texture[] gameModeScreens;

    // Token: 0x0400067E RID: 1662
    public Texture jugglingBall;

    // Token: 0x0400067F RID: 1663
    public Texture jugglingPointer;

    // Token: 0x04000680 RID: 1664
    [NonSerialized]
    public string defFilePath;

    // Token: 0x04000681 RID: 1665
    [NonSerialized]
    public bool isDedicated;

    // Token: 0x04000682 RID: 1666
    [NonSerialized]
    public string dedicatedRankPass;

    // Token: 0x04000683 RID: 1667
    [NonSerialized]
    public string dedicatedGTVPass;

    // Token: 0x04000684 RID: 1668
    [NonSerialized]
    public int dedicatedDefTeamMax;

    // Token: 0x04000685 RID: 1669
    [NonSerialized]
    public int dedicatedStadiumIndex;

    // Token: 0x04000686 RID: 1670
    [NonSerialized]
    public int dedicatedBallIndex;

    // Token: 0x04000687 RID: 1671
    [NonSerialized]
    public int dedicatedTimeLimit;

    // Token: 0x04000688 RID: 1672
    [NonSerialized]
    public int dedicatedScoreLimit;

    // Token: 0x04000689 RID: 1673
    [NonSerialized]
    public int dedicatedSendRate;

    // Token: 0x0400068A RID: 1674
    [NonSerialized]
    public bool dedicatedUseOvertime;

    // Token: 0x0400068B RID: 1675
    [NonSerialized]
    public string dedicatedAdminPassword;

    // Token: 0x0400068C RID: 1676
    [NonSerialized]
    public bool dedicatedVoteStadiumEnabled;

    // Token: 0x0400068D RID: 1677
    [NonSerialized]
    public bool firstRun;

    // Token: 0x0400068E RID: 1678
    [NonSerialized]
    public bool isTutorial;

    // Token: 0x0400068F RID: 1679
    [NonSerialized]
    public bool isBotGame;

    // Token: 0x04000690 RID: 1680
    [NonSerialized]
    public BotLevel botLevel;

    // Token: 0x04000691 RID: 1681
    [NonSerialized]
    public bool isPlayerGK;

    // Token: 0x04000692 RID: 1682
    [NonSerialized]
    public float secondRefreshTimer;

    // Token: 0x04000693 RID: 1683
    [NonSerialized]
    public string[] countryNames;

    // Token: 0x04000694 RID: 1684
    [NonSerialized]
    public string[] countryCodes;

    // Token: 0x04000695 RID: 1685
    [NonSerialized]
    public Texture[] countryTextures;

    // Token: 0x04000696 RID: 1686
    [NonSerialized]
    public GUIStyle boxStyle;

    // Token: 0x04000697 RID: 1687
    [NonSerialized]
    public float topRanksGetTimer;

    // Token: 0x04000698 RID: 1688
    [NonSerialized]
    public CTopRankPlayer[] topRankPlayers;

    // Token: 0x04000699 RID: 1689
    [NonSerialized]
    public int topRankScrollPosition;

    // Token: 0x0400069A RID: 1690
    [NonSerialized]
    public int yourRankIndex;

    // Token: 0x0400069B RID: 1691
    [NonSerialized]
    public float mouseSpeed;

    // Token: 0x0400069C RID: 1692
    [NonSerialized]
    public bool invertMouseY;

    // Token: 0x0400069D RID: 1693
    [NonSerialized]
    public bool playerNamesEnabled;

    // Token: 0x0400069E RID: 1694
    [NonSerialized]
    public string teamShirtsCode;

    // Token: 0x0400069F RID: 1695
    [NonSerialized]
    public int aaLevel;

    // Token: 0x040006A0 RID: 1696
    [NonSerialized]
    public CurveMode curveMode;

    // Token: 0x040006A1 RID: 1697
    [NonSerialized]
    public int tribunesEnabled;

    // Token: 0x040006A2 RID: 1698
    [NonSerialized]
    public bool oldShift;

    // Token: 0x040006A3 RID: 1699
    [NonSerialized]
    public bool grass3DEnabled;

    // Token: 0x040006A4 RID: 1700
    [NonSerialized]
    public float cameraFov;

    // Token: 0x040006A5 RID: 1701
    [NonSerialized]
    public float prevCameraFov;

    // Token: 0x040006A6 RID: 1702
    [NonSerialized]
    public int optionsTab;

    // Token: 0x040006A7 RID: 1703
    [NonSerialized]
    public int vSync;

    // Token: 0x040006A8 RID: 1704
    [NonSerialized]
    public float prevSFXVolume;

    // Token: 0x040006A9 RID: 1705
    [NonSerialized]
    public float prevAmbientVolume;

    // Token: 0x040006AA RID: 1706
    [NonSerialized]
    public float prevMenuMusicVolume;

    // Token: 0x040006AB RID: 1707
    [NonSerialized]
    public bool prevSoundEnabled;

    // Token: 0x040006AC RID: 1708
    [NonSerialized]
    public bool useTopDownCamera;

    // Token: 0x040006AD RID: 1709
    [NonSerialized]
    public bool prevUseTopDownCamera;

    // Token: 0x040006AE RID: 1710
    [NonSerialized]
    public float sensitivity;

    // Token: 0x040006AF RID: 1711
    [NonSerialized]
    public bool isInvertMouseY;

    // Token: 0x040006B0 RID: 1712
    [NonSerialized]
    public bool isSoundOn;

    // Token: 0x040006B1 RID: 1713
    [NonSerialized]
    public bool isAmbientOn;

    // Token: 0x040006B2 RID: 1714
    [NonSerialized]
    public bool isPlayerNamesOn;

    // Token: 0x040006B3 RID: 1715
    [NonSerialized]
    public string teamShirtsCodeOption;

    // Token: 0x040006B4 RID: 1716
    [NonSerialized]
    public int aaLevelOption;

    // Token: 0x040006B5 RID: 1717
    [NonSerialized]
    public int tribunesEnabledOption;

    // Token: 0x040006B6 RID: 1718
    [NonSerialized]
    public int curveModeOption;

    // Token: 0x040006B7 RID: 1719
    [NonSerialized]
    public bool isSpeedometer;

    // Token: 0x040006B8 RID: 1720
    [NonSerialized]
    public int predictionLevel;

    // Token: 0x040006B9 RID: 1721
    [NonSerialized]
    public bool isPredictionOn;

    // Token: 0x040006BA RID: 1722
    [NonSerialized]
    public bool isGrass3DEnabled;

    // Token: 0x040006BB RID: 1723
    [NonSerialized]
    public float cameraFovOption;

    // Token: 0x040006BC RID: 1724
    [NonSerialized]
    public int vSyncOption;
    public int maxFps;
    public int maxFpsOption;
    // Token: 0x040006BD RID: 1725
    [NonSerialized]
    public float sfxVolumeOption;

    // Token: 0x040006BE RID: 1726
    [NonSerialized]
    public float ambientVolumeOption;

    // Token: 0x040006BF RID: 1727
    [NonSerialized]
    public float menuMusicVolumeOption;

    // Token: 0x040006C0 RID: 1728
    [NonSerialized]
    public bool useTopDownCameraOption;

    // Token: 0x040006C1 RID: 1729
    [NonSerialized]
    public int localBallIndex;

    // Token: 0x040006C2 RID: 1730
    [NonSerialized]
    public int localBasketBallIndex;

    // Token: 0x040006C3 RID: 1731
    [NonSerialized]
    public MenuState serverListMenuState;

    // Token: 0x040006C4 RID: 1732
    [NonSerialized]
    public KeyCode[] keys;

    // Token: 0x040006C5 RID: 1733
    [NonSerialized]
    public KeyCode[] keysTmp;

    // Token: 0x040006C6 RID: 1734
    [NonSerialized]
    public KeyCode[] defKeys;

    // Token: 0x040006C7 RID: 1735
    [NonSerialized]
    public string[] keyNames;

    // Token: 0x040006C8 RID: 1736
    [NonSerialized]
    public int activeKeyIndex;

    // Token: 0x040006C9 RID: 1737
    [NonSerialized]
    public bool isSpaceKeyFree;

    // Token: 0x040006CA RID: 1738
    [NonSerialized]
    public int clientLagFramesTmp;

    // Token: 0x040006CB RID: 1739
    [NonSerialized]
    public bool usePredictionTmp;

    // Token: 0x040006CC RID: 1740
    [NonSerialized]
    public Callback<GetAuthSessionTicketResponse_t> getAuthSessionTicketResponse;

    // Token: 0x040006CD RID: 1741
    [NonSerialized]
    public Callback<MicroTxnAuthorizationResponse_t> microTxnAuthorizationResponse;

    // Token: 0x040006CE RID: 1742
    [NonSerialized]
    public Callback<GameOverlayActivated_t> gameOverlayActivated;

    // Token: 0x040006CF RID: 1743
    [NonSerialized]
    public string steamTicketString;

    // Token: 0x040006D0 RID: 1744
    [NonSerialized]
    public int tutorialDone;

    // Token: 0x040006D1 RID: 1745
    [NonSerialized]
    public bool connectingInProgress;

    // Token: 0x040006D2 RID: 1746
    [NonSerialized]
    public bool usingGamepad;

    // Token: 0x040006D3 RID: 1747
    [NonSerialized]
    public bool backFromOnline;

    // Token: 0x040006D4 RID: 1748
    private float horizontalAxis;

    // Token: 0x040006D5 RID: 1749
    private float verticalAxis;

    // Token: 0x040006D6 RID: 1750
    [NonSerialized]
    public NATHelper natHelper;

    // Token: 0x040006D7 RID: 1751
    [NonSerialized]
    public bool portForwardingDone;

    // Token: 0x040006D8 RID: 1752
    [NonSerialized]
    public CNews[] news;

    // Token: 0x040006D9 RID: 1753
    [NonSerialized]
    public CStream[] streams;

    // Token: 0x040006DA RID: 1754
    [NonSerialized]
    public int newsNumber;

    // Token: 0x040006DB RID: 1755
    [NonSerialized]
    public string[] newsStr;

    // Token: 0x040006DC RID: 1756
    [NonSerialized]
    public float masterServerFailedTimer;

    // Token: 0x040006DD RID: 1757
    [NonSerialized]
    public float timeSinceTheLastRankGame;

    // Token: 0x040006DE RID: 1758
    [NonSerialized]
    public static GlobalManager instance;

    // Token: 0x040006DF RID: 1759
    private double sum;

    // Token: 0x040006E0 RID: 1760
    private double cnt;

    // Token: 0x040006E1 RID: 1761
    private Rect jugglingRect;

    // Token: 0x040006E2 RID: 1762
    private int jugglingScore;

    // Token: 0x040006E3 RID: 1763
    [NonSerialized]
    public int jugglingHighScore;

    // Token: 0x040006E4 RID: 1764
    private Vector2 jugglingBallPosition;

    // Token: 0x040006E5 RID: 1765
    private Vector2 jugglingBallVelocity;

    // Token: 0x040006E6 RID: 1766
    private int jugglingGameState;

    // Token: 0x040006E7 RID: 1767
    private float jugglingTimer1;

    // Token: 0x040006E8 RID: 1768
    private float jugglingCeilTimer;

    // Token: 0x040006E9 RID: 1769
    private float jugglingCeilOffset;

    // Token: 0x040006EA RID: 1770
    private float jugglingCeilOffsetMax;

    // Token: 0x040006EB RID: 1771
    private float jugglingCeilTimerFactor;

    // Token: 0x040006EC RID: 1772
    private int jugglingCountDownMax;

    // Token: 0x040006ED RID: 1773
    private int jugglingAfterTime;

    // Token: 0x040006EE RID: 1774
    private float jugglingGravity;

    // Token: 0x040006EF RID: 1775
    private float jugglingBallRadius;

    // Token: 0x040006F0 RID: 1776
    private float jugglingPointerRadius;

    // Token: 0x040006F1 RID: 1777
    private float jugglingBallDump;

    // Token: 0x040006F2 RID: 1778
    private float jugglingBallBounceDump;

    // Token: 0x040006F3 RID: 1779
    private float jugglingKickFactor;

    // Token: 0x040006F4 RID: 1780
    private float jugglingIdleSpeed;

    // Token: 0x040006F5 RID: 1781
    private float jugglingBounceIdleSpeed;

    // Token: 0x040006F6 RID: 1782
    private float jugglingMaxSpeed;

    // Token: 0x040006F7 RID: 1783
    private Vector2 jugglingPrevMousePosition;

    // Token: 0x040006F8 RID: 1784
    private bool jugglingCursorVisible;

    // Token: 0x040006F9 RID: 1785
    private bool jugglingIsNewRecord;

    // Token: 0x040006FA RID: 1786
    private bool jugglingPrevFrameKick;

    // Token: 0x040006FB RID: 1787
    private float dontClickTimer;

    // Token: 0x040006FC RID: 1788
    private float dontClickTime;

    // Token: 0x040006FD RID: 1789
    private uint outTicketLength;

    // Token: 0x040006FE RID: 1790
    private byte[] ticketBytes;

    // Token: 0x040006FF RID: 1791
    private HAuthTicket hAuthTicket;

    // Token: 0x020000C8 RID: 200
    [CompilerGenerated]
    [Serializable]
    internal sealed class $UnloadAssets$1859 : GenericGenerator<object>
    {
        // Token: 0x060002AD RID: 685 RVA: 0x00039AAC File Offset: 0x00037CAC
        public $UnloadAssets$1859(GlobalManager self_)
        {
            this.$self_$1864 = self_;
        }

        // Token: 0x060002AE RID: 686 RVA: 0x00039ABC File Offset: 0x00037CBC
        public override IEnumerator<object> GetEnumerator()
        {
            return new GlobalManager.$UnloadAssets$1859.$(this.$self_$1864);
        }

        // Token: 0x04000700 RID: 1792
        internal GlobalManager $self_$1864;
    }

    // Token: 0x020000CA RID: 202
    [CompilerGenerated]
    [Serializable]
    internal sealed class $LoadTopRank$1865 : GenericGenerator<WWW>
    {
        // Token: 0x060002B1 RID: 689 RVA: 0x00039C58 File Offset: 0x00037E58
        public $LoadTopRank$1865(GlobalManager self_)
        {
            this.$self_$1871 = self_;
        }

        // Token: 0x060002B2 RID: 690 RVA: 0x00039C68 File Offset: 0x00037E68
        public override IEnumerator<WWW> GetEnumerator()
        {
            return new GlobalManager.$LoadTopRank$1865.$(this.$self_$1871);
        }

        // Token: 0x04000705 RID: 1797
        internal GlobalManager $self_$1871;
    }

    // Token: 0x020000CC RID: 204
    [CompilerGenerated]
    [Serializable]
    internal sealed class $FreeMemoryHelper$1872 : GenericGenerator<WaitForSeconds>
    {
        // Token: 0x060002B6 RID: 694 RVA: 0x00039EE0 File Offset: 0x000380E0
        public override IEnumerator<WaitForSeconds> GetEnumerator()
        {
            return new GlobalManager.$FreeMemoryHelper$1872.$();
        }
    }

    // Token: 0x020000CE RID: 206
    [CompilerGenerated]
    [Serializable]
    internal sealed class $LoadNews$1873 : GenericGenerator<WWW>
    {
        // Token: 0x060002B9 RID: 697 RVA: 0x00039F54 File Offset: 0x00038154
        public $LoadNews$1873(GlobalManager self_)
        {
            this.$self_$1884 = self_;
        }

        // Token: 0x060002BA RID: 698 RVA: 0x00039F64 File Offset: 0x00038164
        public override IEnumerator<WWW> GetEnumerator()
        {
            return new GlobalManager.$LoadNews$1873.$(this.$self_$1884);
        }

        // Token: 0x0400070B RID: 1803
        internal GlobalManager $self_$1884;
    }
}
