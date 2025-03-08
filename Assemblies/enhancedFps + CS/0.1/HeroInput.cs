using System;
using UnityEngine;

// Token: 0x020000DB RID: 219
[Serializable]
public class HeroInput : MonoBehaviour
{
	// Token: 0x06000302 RID: 770 RVA: 0x00003E12 File Offset: 0x00002012
	public HeroInput()
	{
		this.kickPowerMax = StaticVariables.kickPowerMax;
		this.kickCurvePowerMax = StaticVariables.kickCurvePowerMax + StaticVariables.kickCurvePowerBottomEdge;
	}

	// Token: 0x06000303 RID: 771 RVA: 0x00003E36 File Offset: 0x00002036
	public virtual void FixedUpdate()
	{
		if (!this.doneThisFrame)
		{
			this.isFromFixedUpdate = true;
			this.UpdateNow();
			this.isFromFixedUpdate = false;
			this.doneThisFrame = true;
		}
	}

	// Token: 0x06000304 RID: 772 RVA: 0x00003E63 File Offset: 0x00002063
	public virtual void Update()
	{
		this.frameTimeElapsed += Time.unscaledDeltaTime;
		this.UpdateNow();
	}

	// Token: 0x06000305 RID: 773 RVA: 0x00041BA8 File Offset: 0x0003FDA8
	public virtual void LateUpdate()
	{
		if (GrassManager.instance)
		{
			GrassManager.instance.firstRenderer.sharedMaterial.SetVector("_CameraRotationPoint", this.transform.position);
		}
		if (GameManager.instance && GlobalManager.instance.GetKeyDown(Keys.Emotion) && ShopManager.instance.PlayerHasItem(-1, BASManager.instance.basPlayer.currentEmotionID))
		{
			if (Network.isClient)
			{
				NetworkManager.instance.networkView1.RPC("AQ", RPCMode.Server, new object[] { BASManager.instance.basPlayer.currentEmotionID });
			}
			else
			{
				ShopManager.instance.StartEmotion(GameManager.instance.localPlayerIndex, BASManager.instance.basPlayer.currentEmotionID);
			}
		}
	}

	// Token: 0x06000306 RID: 774 RVA: 0x00041C90 File Offset: 0x0003FE90
	public virtual void UpdateNow()
	{
		if (this.doneThisFrame)
		{
			this.doneThisFrame = false;
			return;
		}
		float num = this.frameTimeElapsed;
		bool flag = false;
		if (PredictionManager.instance.globalMsec != this.prevMsec)
		{
			flag = true;
			this.prevMsec = PredictionManager.instance.globalMsec;
			this.frameTimeElapsed = 0f;
			PredictionManager.instance.inputSent = false;
		}
		this.msecOffset = 0;
		ulong globalMsec = PredictionManager.instance.globalMsec;
		bool flag2 = this.isFromFixedUpdate;
		PredictionManager.instance.FindFinishedIndex(PredictionManager.instance.globalMsec + (ulong)((long)this.msecOffset));
		if (Cursor.visible || (GameManager.instance.isMenuOpened && !GlobalManager.instance.GetKey(Keys.ShowMenu) && !GameManager.instance.isMenuTab) || GameManager.instance.isGameChatOpened)
		{
			this.bothHorizontal = false;
			this.bothVertical = false;
			this.newInputHorizontal = 0;
			this.newInputVertical = 0;
			if (this.networkInputExchange)
			{
				if (this.heroControllerScript)
				{
					this.heroControllerScript.inputMouseX = 0f;
					this.heroControllerScript.inputMouseY = 0f;
					this.heroControllerScript.inputFire = (ButtonState)0;
				}
				this.networkInputExchange.inputHorizontal = 0;
				this.networkInputExchange.inputVertical = 0;
				this.networkInputExchange.inputJump = (ButtonState)0;
				this.networkInputExchange.kickPower = 0f;
				this.networkInputExchange.kickCurvePower = 0f;
				this.networkInputExchange.kickIncrease = false;
			}
			else if (this.heroControllerScript)
			{
				this.heroControllerScript.inputMouseX = 0f;
				this.heroControllerScript.inputMouseY = 0f;
				this.heroControllerScript.inputHorizontal = 0;
				this.heroControllerScript.inputVertical = 0;
				this.heroControllerScript.inputJump = (ButtonState)0;
				this.heroControllerScript.inputFire = (ButtonState)0;
			}
			float lastRotationY = PredictionManager.instance.lastRotationY;
			Vector3 eulerAngles = base.transform.eulerAngles;
			eulerAngles.y = lastRotationY;
			base.transform.eulerAngles = eulerAngles;
			if (this.heroControllerScript)
			{
				this.heroControllerScript.rotationY = PredictionManager.instance.lastRotationY;
			}
			PredictionManager.instance.AddLocalPlayerInput(0, 0, PredictionManager.instance.lastRotationY, (ButtonState)0, 0f, 0f, GlobalManager.instance.oldShift, null, false, Vector3.zero, this.msecOffset);
			return;
		}
		if (GlobalManager.instance.GetKey(Keys.MoveLeft) && GlobalManager.instance.GetKey(Keys.MoveRight))
		{
			if (!this.bothHorizontal)
			{
				this.newInputHorizontal *= -1;
			}
			this.bothHorizontal = true;
		}
		else
		{
			this.bothHorizontal = false;
			if (GlobalManager.instance.GetKey(Keys.MoveLeft))
			{
				this.newInputHorizontal = -1;
			}
			else if (GlobalManager.instance.GetKey(Keys.MoveRight))
			{
				this.newInputHorizontal = 1;
			}
			else
			{
				this.newInputHorizontal = 0;
			}
		}
		if (GlobalManager.instance.GetKey(Keys.MoveBack) && GlobalManager.instance.GetKey(Keys.MoveForward))
		{
			if (!this.bothVertical)
			{
				this.newInputVertical *= -1;
			}
			this.bothVertical = true;
		}
		else
		{
			this.bothVertical = false;
			if (GlobalManager.instance.GetKey(Keys.MoveBack))
			{
				this.newInputVertical = -1;
			}
			else if (GlobalManager.instance.GetKey(Keys.MoveForward))
			{
				this.newInputVertical = 1;
			}
			else
			{
				this.newInputVertical = 0;
			}
		}
		if (this.heroControllerScript)
		{
			this.heroControllerScript.inputMouseY = GlobalManager.instance.GetMouseGamepadAxisRaw(MouseAxis.Y) * StaticVariables.sensitivityFactor;
			this.heroControllerScript.inputMouseX = GlobalManager.instance.GetMouseGamepadAxisRaw(MouseAxis.X) * StaticVariables.sensitivityFactor;
			if (GlobalManager.instance.usingGamepad)
			{
				this.heroControllerScript.inputMouseX = Input.GetAxisRaw("Horizontal") * StaticVariables.sensitivityFactor;
			}
			float lastRotationY2 = PredictionManager.instance.lastRotationY;
			Vector3 eulerAngles2 = base.transform.eulerAngles;
			eulerAngles2.y = lastRotationY2;
			base.transform.eulerAngles = eulerAngles2;
			float num2 = base.transform.eulerAngles.y + this.heroControllerScript.inputMouseX * GlobalManager.instance.mouseSpeed;
			Vector3 eulerAngles3 = base.transform.eulerAngles;
			eulerAngles3.y = num2;
			base.transform.eulerAngles = eulerAngles3;
			float num3 = base.transform.eulerAngles.y % 360f;
			Vector3 eulerAngles4 = base.transform.eulerAngles;
			eulerAngles4.y = num3;
			base.transform.eulerAngles = eulerAngles4;
			this.heroControllerScript.useRotationY = true;
			this.heroControllerScript.rotationY = base.transform.eulerAngles.y;
			PredictionManager.instance.lastRotationY = base.transform.eulerAngles.y;
			if (GameManager.instance.isBasketball || GameManager.instance.isHockey || GameManager.instance.isHandball)
			{
				GameManager.instance.BasketballServiceFixedUpdate(false);
			}
			this.heroControllerScript.inputFire = (ButtonState)0;
			if (GlobalManager.instance.GetKeyDown(Keys.Kick))
			{
				this.heroControllerScript.inputFire = this.heroControllerScript.inputFire | ButtonState.Down;
			}
			if (GlobalManager.instance.GetKeyUp(Keys.Kick))
			{
				this.heroControllerScript.inputFire = this.heroControllerScript.inputFire | ButtonState.Up;
			}
			if (GlobalManager.instance.GetKey(Keys.Kick))
			{
				this.heroControllerScript.inputFire = this.heroControllerScript.inputFire | ButtonState.Pressed;
			}
			if ((this.heroControllerScript.inputFire & ButtonState.Down) != (ButtonState)0 && !GameManager.instance.isPaused)
			{
				this.kickPower = this.kickPowerMin;
				this.canIncrease = true;
				GameManager.instance.showKickBar = true;
				GameManager.instance.kickBarTimer = 0f;
				this.kickCurvePower = 0f;
				this.kickCurvePowerTmp = 0f;
			}
			if (flag && this.canIncrease && (this.heroControllerScript.inputFire & ButtonState.Pressed) != (ButtonState)0 && !GameManager.instance.isPaused && flag && GlobalManager.instance.GetKey(Keys.VerticalKick) && !GameManager.instance.isRacingBall && !GameManager.instance.isHockey && !GameManager.instance.isHandball && (!GlobalManager.instance.GetKey(Keys.CurveLeft) || !GlobalManager.instance.GetKey(Keys.CurveRight)))
			{
				if (GlobalManager.instance.GetKey(Keys.CurveLeft) && GlobalManager.instance.curveMode == CurveMode.Mouse)
				{
					this.kickCurvePowerTmp = -this.kickCurvePowerMax * (float)((GlobalManager.instance.curveMode != CurveMode.Mouse) ? (-1) : 1);
					this.kickCurvePowerTmp *= 1.05f;
				}
				if (GlobalManager.instance.GetKey(Keys.CurveRight) && GlobalManager.instance.curveMode == CurveMode.Mouse)
				{
					this.kickCurvePowerTmp = this.kickCurvePowerMax * (float)((GlobalManager.instance.curveMode != CurveMode.Mouse) ? (-1) : 1);
					this.kickCurvePowerTmp *= 1.05f;
				}
				if (GlobalManager.instance.GetKey(Keys.CurveLeft) && GlobalManager.instance.curveMode == CurveMode.QE)
				{
					this.kickCurvePowerTmp = -this.kickCurvePowerMax * (float)((GlobalManager.instance.curveMode != CurveMode.QE) ? (-1) : 1);
					this.kickCurvePowerTmp *= 2f;
				}
				if (GlobalManager.instance.GetKey(Keys.CurveRight) && GlobalManager.instance.curveMode == CurveMode.QE)
				{
					this.kickCurvePowerTmp = this.kickCurvePowerMax * (float)((GlobalManager.instance.curveMode != CurveMode.QE) ? (-1) : 1);
					this.kickCurvePowerTmp *= 2f;
				}
				else
				{
					if (GlobalManager.instance.GetKey(Keys.CurveLeft))
					{
						this.kickCurvePowerTmp = -this.kickCurvePowerMax * (float)((GlobalManager.instance.curveMode != CurveMode.Mouse && GlobalManager.instance.curveMode != CurveMode.QE) ? (-1) : 1);
						this.kickCurvePowerTmp *= 2f;
					}
					if (GlobalManager.instance.GetKey(Keys.CurveRight) && GlobalManager.instance.curveMode == CurveMode.Mouse)
					{
						this.kickCurvePowerTmp = this.kickCurvePowerMax * (float)((GlobalManager.instance.curveMode != CurveMode.Mouse && GlobalManager.instance.curveMode != CurveMode.QE) ? (-1) : 1);
						this.kickCurvePowerTmp *= 2f;
					}
				}
			}
			if (this.canIncrease && (this.heroControllerScript.inputFire & ButtonState.Pressed) != (ButtonState)0 && !GameManager.instance.isPaused && !GameManager.instance.isRacingBall && (GlobalManager.instance.curveMode == CurveMode.Mouse || GlobalManager.instance.curveMode == CurveMode.MouseReversed))
			{
				this.kickCurvePowerTmp -= this.heroControllerScript.inputMouseX * GlobalManager.instance.mouseSpeed * 1.5f * (float)((GlobalManager.instance.curveMode != CurveMode.Mouse) ? (-1) : 1);
			}
			if (this.networkInputExchange)
			{
				this.networkInputExchange.kickIncrease = false;
			}
			if (flag && this.canIncrease && (this.heroControllerScript.inputFire & ButtonState.Pressed) != (ButtonState)0 && !GameManager.instance.isPaused)
			{
				this.kickPower += StaticVariables.kickPowerIncrease * num;
				if (this.kickPower > this.kickPowerMax)
				{
					this.kickPower = this.kickPowerMax;
					this.canIncrease = false;
				}
				GameManager.instance.showKickBar = true;
				GameManager.instance.kickBarTimer = 0f;
				if (this.networkInputExchange)
				{
					this.networkInputExchange.kickIncrease = true;
				}
				float num4 = 250f;
				if (!GameManager.instance.isRacingBall)
				{
					if (GlobalManager.instance.GetKey(Keys.CurveLeft))
					{
						this.kickCurvePowerTmp -= num4 * num * (float)((GlobalManager.instance.curveMode != CurveMode.QE && GlobalManager.instance.curveMode != CurveMode.Mouse) ? (-1) : 1);
					}
					if (GlobalManager.instance.GetKey(Keys.CurveRight))
					{
						this.kickCurvePowerTmp += num4 * num * (float)((GlobalManager.instance.curveMode != CurveMode.QE && GlobalManager.instance.curveMode != CurveMode.Mouse) ? (-1) : 1);
					}
				}
				this.kickCurvePower = this.kickCurvePowerTmp;
				if (Mathf.Abs(this.kickCurvePower) <= StaticVariables.kickCurvePowerBottomEdge)
				{
					this.kickCurvePower = 0f;
				}
				if (this.kickCurvePower > StaticVariables.kickCurvePowerBottomEdge)
				{
					this.kickCurvePower -= StaticVariables.kickCurvePowerBottomEdge;
					if (this.kickCurvePower < 0f)
					{
						this.kickCurvePower = 0f;
					}
				}
				else if (this.kickCurvePower < -StaticVariables.kickCurvePowerBottomEdge)
				{
					this.kickCurvePower += StaticVariables.kickCurvePowerBottomEdge;
					if (this.kickCurvePower > 0f)
					{
						this.kickCurvePower = 0f;
					}
				}
				if (this.kickCurvePower < -this.kickCurvePowerMax)
				{
					this.kickCurvePower = -this.kickCurvePowerMax;
				}
				if (this.kickCurvePower > this.kickCurvePowerMax)
				{
					this.kickCurvePower = this.kickCurvePowerMax;
				}
			}
			if (flag)
			{
				if (this.kickPower > 0f)
				{
					GameManager.instance.kickPower = this.kickPower;
					GameManager.instance.kickCurvePower = this.kickCurvePower;
				}
				this.KickService();
			}
		}
		if (flag || GameManager.instance.isPaused)
		{
			if (this.networkInputExchange)
			{
				this.networkInputExchange.inputHorizontal = this.newInputHorizontal;
				this.networkInputExchange.inputVertical = this.newInputVertical;
				this.networkInputExchange.inputJump = (ButtonState)0;
				if (GlobalManager.instance.GetKeyDown(Keys.Jump))
				{
					this.networkInputExchange.inputJump = this.networkInputExchange.inputJump | ButtonState.Down;
				}
				if (GlobalManager.instance.GetKeyUp(Keys.Jump))
				{
					this.networkInputExchange.inputJump = this.networkInputExchange.inputJump | ButtonState.Up;
				}
				if (GlobalManager.instance.GetKey(Keys.Jump))
				{
					this.networkInputExchange.inputJump = this.networkInputExchange.inputJump | ButtonState.Pressed;
				}
				if (this.heroControllerScript)
				{
					bool flag3 = false;
					Vector3[] array = new Vector3[StaticVariables.ballObjectsMax];
					Vector3 vector = default(Vector3);
					if (PredictionManager.instance.finishedIndexInfo.index >= 0 && PredictionManager.instance.finishedIndexInfo.equal && GameManager.instance.localPlayerIndex >= 0)
					{
						flag3 = true;
						for (int i = 0; i < StaticVariables.ballObjectsMax; i++)
						{
							array[i] = PredictionManager.instance.finishedBallStates[PredictionManager.instance.finishedIndexInfo.index].ballStateHelper[i].position;
						}
						vector = PredictionManager.instance.finishedPlayers[GameManager.instance.localPlayerIndex].playerStates[PredictionManager.instance.finishedIndexInfo.index].position;
					}
					PredictionManager.instance.AddLocalPlayerInput(this.networkInputExchange.inputHorizontal, this.networkInputExchange.inputVertical, base.transform.eulerAngles.y, this.networkInputExchange.inputJump, this.networkInputExchange.kickPower, this.networkInputExchange.kickCurvePower, GlobalManager.instance.oldShift, array, flag3, vector, this.msecOffset);
					this.networkInputExchange.kickPower = 0f;
					this.networkInputExchange.kickCurvePower = 0f;
					return;
				}
			}
			else if (this.heroControllerScript)
			{
				this.heroControllerScript.inputHorizontal = this.newInputHorizontal;
				this.heroControllerScript.inputVertical = this.newInputVertical;
				this.heroControllerScript.inputJump = (ButtonState)0;
				if (GlobalManager.instance.GetKeyDown(Keys.Jump))
				{
					this.heroControllerScript.inputJump = this.heroControllerScript.inputJump | ButtonState.Down;
				}
				if (GlobalManager.instance.GetKeyUp(Keys.Jump))
				{
					this.heroControllerScript.inputJump = this.heroControllerScript.inputJump | ButtonState.Up;
				}
				if (GlobalManager.instance.GetKey(Keys.Jump))
				{
					this.heroControllerScript.inputJump = this.heroControllerScript.inputJump | ButtonState.Pressed;
				}
			}
		}
	}

	// Token: 0x06000307 RID: 775 RVA: 0x00042A40 File Offset: 0x00040C40
	public virtual void KickService()
	{
		if (this.kickPower > (float)0 && ((this.heroControllerScript.inputFire & ButtonState.Pressed) == (ButtonState)0 || this.kickPower >= this.kickPowerMax))
		{
			if (this.kickPower > this.kickPowerMax)
			{
				this.kickPower = this.kickPowerMax;
			}
			if (this.kickPower < (float)0)
			{
				this.kickPower = (float)0;
			}
			bool flag = false;
			if (GlobalManager.instance.GetKey(Keys.VerticalKick) && !GameManager.instance.isRacingBall && !GameManager.instance.isHockey && !GameManager.instance.isHandball)
			{
				flag = true;
				if ((GlobalManager.instance.curveMode == CurveMode.Mouse || GlobalManager.instance.curveMode == CurveMode.MouseReversed) && this.newInputHorizontal != 0 && this.kickPower >= this.kickPowerMax && !GlobalManager.instance.GetKey(Keys.CurveLeft) && !GlobalManager.instance.GetKey(Keys.CurveRight))
				{
					this.kickCurvePower = (float)0;
				}
			}
			else if (GameManager.instance.isUrbanBeta)
			{
			}
			if (Network.isClient)
			{
				this.networkInputExchange.kickPower = ((!flag) ? this.kickPower : (-this.kickPower));
				this.networkInputExchange.kickCurvePower = this.kickCurvePower;
				GameManager.instance.drawVerticalArrow = flag;
			}
			else
			{
				bool flag2 = false;
				if (this.heroControllerScript.canKick != 0)
				{
					for (int i = 0; i < GameManager.instance.currentBallsCount; i++)
					{
						if ((this.heroControllerScript.canKick & (1 << i)) != 0)
						{
							this.heroControllerScript.KickNowServerOneBall((!flag) ? this.kickPower : (-this.kickPower), this.kickCurvePower, GlobalManager.instance.oldShift, i);
							flag2 = true;
						}
					}
				}
				if (flag2)
				{
					this.heroControllerScript.kickPowerHoldTimer = (float)(-1);
					GameManager.instance.drawVerticalArrow = flag;
				}
				else
				{
					this.heroControllerScript.kickPowerHoldTimer = (float)0;
					this.heroControllerScript.kickPowerHoldPower = ((!flag) ? this.kickPower : (-this.kickPower));
					this.heroControllerScript.kickPowerHoldCurvePower = this.kickCurvePower;
					this.heroControllerScript.kickHoldOldShift = GlobalManager.instance.oldShift;
					GameManager.instance.drawVerticalArrow = flag;
				}
			}
			this.canIncrease = false;
			this.kickPower = (float)0;
			this.kickCurvePower = (float)0;
		}
		if (this.kickPower >= this.kickPowerMax)
		{
			this.kickPower = (float)0;
		}
	}

	// Token: 0x06000308 RID: 776 RVA: 0x000024E5 File Offset: 0x000006E5
	public virtual void Main()
	{
	}

	// Token: 0x040007B3 RID: 1971
	public HeroController heroControllerScript;

	// Token: 0x040007B4 RID: 1972
	public NetworkInputExchange networkInputExchange;

	// Token: 0x040007B5 RID: 1973
	[NonSerialized]
	public bool bothHorizontal;

	// Token: 0x040007B6 RID: 1974
	[NonSerialized]
	public bool bothVertical;

	// Token: 0x040007B7 RID: 1975
	[NonSerialized]
	public int newInputHorizontal;

	// Token: 0x040007B8 RID: 1976
	[NonSerialized]
	public int newInputVertical;

	// Token: 0x040007B9 RID: 1977
	private float kickPower;

	// Token: 0x040007BA RID: 1978
	private float kickPowerMax;

	// Token: 0x040007BB RID: 1979
	private float kickPowerMin;

	// Token: 0x040007BC RID: 1980
	private float kickCurvePower;

	// Token: 0x040007BD RID: 1981
	private float kickCurvePowerMax;

	// Token: 0x040007BE RID: 1982
	private float kickCurvePowerTmp;

	// Token: 0x040007BF RID: 1983
	private bool canIncrease;

	// Token: 0x040007C0 RID: 1984
	private bool isFromFixedUpdate;

	// Token: 0x040007C1 RID: 1985
	private bool doneThisFrame;

	// Token: 0x040007C2 RID: 1986
	private int msecOffset;

	// Token: 0x040007C3 RID: 1987
	private float frameTimeElapsed;

	// Token: 0x040007C4 RID: 1988
	private ulong prevMsec;
}
