using System;
using UnityEngine;

[Serializable]
public class HeroInput : MonoBehaviour
{
	public HeroController heroControllerScript;

	public NetworkInputExchange networkInputExchange;

	[NonSerialized]
	public bool bothHorizontal;

	[NonSerialized]
	public bool bothVertical;

	[NonSerialized]
	public int newInputHorizontal;

	[NonSerialized]
	public int newInputVertical;

	private float kickPower;

	private float kickPowerMax;

	private float kickPowerMin;

	private float kickCurvePower;

	private float kickCurvePowerMax;

	private float kickCurvePowerTmp;

	private bool canIncrease;

	private bool isFromFixedUpdate;

	private bool doneThisFrame;

	private int msecOffset;

	private float frameTimeElapsed;

	private ulong prevMsec;

	public HeroInput()
	{
		kickPowerMax = StaticVariables.kickPowerMax;
		kickCurvePowerMax = StaticVariables.kickCurvePowerMax + StaticVariables.kickCurvePowerBottomEdge;
	}

	public override void FixedUpdate()
	{
		if (!doneThisFrame)
		{
			isFromFixedUpdate = true;
			UpdateNow();
			isFromFixedUpdate = false;
			doneThisFrame = true;
		}
	}

	public override void Update()
	{
		frameTimeElapsed += Time.unscaledDeltaTime;
		UpdateNow();
	}

	public override void LateUpdate()
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		if (Object.op_Implicit((Object)(object)GrassManager.instance))
		{
			GrassManager.instance.firstRenderer.sharedMaterial.SetVector("_CameraRotationPoint", Vector4.op_Implicit(((Component)this).transform.position));
		}
		if (Object.op_Implicit((Object)(object)GameManager.instance) && GlobalManager.instance.GetKeyDown(Keys.Emotion) && ShopManager.instance.PlayerHasItem(-1, BASManager.instance.basPlayer.currentEmotionID))
		{
			if (Network.isClient)
			{
				NetworkManager.instance.networkView1.RPC("AQ", (RPCMode)0, new object[1] { BASManager.instance.basPlayer.currentEmotionID });
			}
			else
			{
				ShopManager.instance.StartEmotion(GameManager.instance.localPlayerIndex, BASManager.instance.basPlayer.currentEmotionID);
			}
		}
	}

	public virtual void UpdateNow()
	{
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_024e: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0412: Unknown result type (might be due to invalid IL or missing references)
		//IL_0417: Unknown result type (might be due to invalid IL or missing references)
		//IL_0428: Unknown result type (might be due to invalid IL or missing references)
		//IL_0435: Unknown result type (might be due to invalid IL or missing references)
		//IL_044d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0452: Unknown result type (might be due to invalid IL or missing references)
		//IL_0463: Unknown result type (might be due to invalid IL or missing references)
		//IL_0482: Unknown result type (might be due to invalid IL or missing references)
		//IL_049c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c8c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cc5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c21: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c26: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c64: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c69: Unknown result type (might be due to invalid IL or missing references)
		if (doneThisFrame)
		{
			doneThisFrame = false;
			return;
		}
		float num = frameTimeElapsed;
		bool flag = false;
		if (PredictionManager.instance.globalMsec != prevMsec)
		{
			flag = true;
			prevMsec = PredictionManager.instance.globalMsec;
			frameTimeElapsed = 0f;
			PredictionManager.instance.inputSent = false;
		}
		msecOffset = 0;
		_ = PredictionManager.instance.globalMsec;
		_ = isFromFixedUpdate;
		PredictionManager.instance.FindFinishedIndex(PredictionManager.instance.globalMsec + (ulong)msecOffset);
		if (Cursor.visible || (GameManager.instance.isMenuOpened && !GlobalManager.instance.GetKey(Keys.ShowMenu) && !GameManager.instance.isMenuTab) || GameManager.instance.isGameChatOpened)
		{
			bothHorizontal = false;
			bothVertical = false;
			newInputHorizontal = 0;
			newInputVertical = 0;
			if (Object.op_Implicit((Object)(object)networkInputExchange))
			{
				if (Object.op_Implicit((Object)(object)heroControllerScript))
				{
					heroControllerScript.inputMouseX = 0f;
					heroControllerScript.inputMouseY = 0f;
					heroControllerScript.inputFire = (ButtonState)0;
				}
				networkInputExchange.inputHorizontal = 0;
				networkInputExchange.inputVertical = 0;
				networkInputExchange.inputJump = (ButtonState)0;
				networkInputExchange.kickPower = 0f;
				networkInputExchange.kickCurvePower = 0f;
				networkInputExchange.kickIncrease = false;
			}
			else if (Object.op_Implicit((Object)(object)heroControllerScript))
			{
				heroControllerScript.inputMouseX = 0f;
				heroControllerScript.inputMouseY = 0f;
				heroControllerScript.inputHorizontal = 0;
				heroControllerScript.inputVertical = 0;
				heroControllerScript.inputJump = (ButtonState)0;
				heroControllerScript.inputFire = (ButtonState)0;
			}
			float lastRotationY = PredictionManager.instance.lastRotationY;
			Vector3 eulerAngles = ((Component)this).transform.eulerAngles;
			eulerAngles.y = lastRotationY;
			((Component)this).transform.eulerAngles = eulerAngles;
			if (Object.op_Implicit((Object)(object)heroControllerScript))
			{
				heroControllerScript.rotationY = PredictionManager.instance.lastRotationY;
			}
			PredictionManager.instance.AddLocalPlayerInput(0, 0, PredictionManager.instance.lastRotationY, (ButtonState)0, 0f, 0f, GlobalManager.instance.oldShift, null, usePlayerPosition: false, Vector3.zero, msecOffset);
			return;
		}
		if (GlobalManager.instance.GetKey(Keys.MoveLeft) && GlobalManager.instance.GetKey(Keys.MoveRight))
		{
			if (!bothHorizontal)
			{
				newInputHorizontal *= -1;
			}
			bothHorizontal = true;
		}
		else
		{
			bothHorizontal = false;
			if (GlobalManager.instance.GetKey(Keys.MoveLeft))
			{
				newInputHorizontal = -1;
			}
			else if (GlobalManager.instance.GetKey(Keys.MoveRight))
			{
				newInputHorizontal = 1;
			}
			else
			{
				newInputHorizontal = 0;
			}
		}
		if (GlobalManager.instance.GetKey(Keys.MoveBack) && GlobalManager.instance.GetKey(Keys.MoveForward))
		{
			if (!bothVertical)
			{
				newInputVertical *= -1;
			}
			bothVertical = true;
		}
		else
		{
			bothVertical = false;
			if (GlobalManager.instance.GetKey(Keys.MoveBack))
			{
				newInputVertical = -1;
			}
			else if (GlobalManager.instance.GetKey(Keys.MoveForward))
			{
				newInputVertical = 1;
			}
			else
			{
				newInputVertical = 0;
			}
		}
		if (Object.op_Implicit((Object)(object)heroControllerScript))
		{
			heroControllerScript.inputMouseY = GlobalManager.instance.GetMouseGamepadAxisRaw(MouseAxis.Y) * StaticVariables.sensitivityFactor;
			heroControllerScript.inputMouseX = GlobalManager.instance.GetMouseGamepadAxisRaw(MouseAxis.X) * StaticVariables.sensitivityFactor;
			if (GlobalManager.instance.usingGamepad)
			{
				heroControllerScript.inputMouseX = Input.GetAxisRaw("Horizontal") * StaticVariables.sensitivityFactor;
			}
			float lastRotationY2 = PredictionManager.instance.lastRotationY;
			Vector3 eulerAngles2 = ((Component)this).transform.eulerAngles;
			eulerAngles2.y = lastRotationY2;
			((Component)this).transform.eulerAngles = eulerAngles2;
			float y = ((Component)this).transform.eulerAngles.y + heroControllerScript.inputMouseX * GlobalManager.instance.mouseSpeed;
			Vector3 eulerAngles3 = ((Component)this).transform.eulerAngles;
			eulerAngles3.y = y;
			((Component)this).transform.eulerAngles = eulerAngles3;
			float y2 = ((Component)this).transform.eulerAngles.y % 360f;
			Vector3 eulerAngles4 = ((Component)this).transform.eulerAngles;
			eulerAngles4.y = y2;
			((Component)this).transform.eulerAngles = eulerAngles4;
			heroControllerScript.useRotationY = true;
			heroControllerScript.rotationY = ((Component)this).transform.eulerAngles.y;
			PredictionManager.instance.lastRotationY = ((Component)this).transform.eulerAngles.y;
			if (GameManager.instance.isBasketball || GameManager.instance.isHockey || GameManager.instance.isHandball)
			{
				GameManager.instance.BasketballServiceFixedUpdate(updateTransform: false);
			}
			heroControllerScript.inputFire = (ButtonState)0;
			if (GlobalManager.instance.GetKeyDown(Keys.Kick))
			{
				heroControllerScript.inputFire |= ButtonState.Down;
			}
			if (GlobalManager.instance.GetKeyUp(Keys.Kick))
			{
				heroControllerScript.inputFire |= ButtonState.Up;
			}
			if (GlobalManager.instance.GetKey(Keys.Kick))
			{
				heroControllerScript.inputFire |= ButtonState.Pressed;
			}
			if ((heroControllerScript.inputFire & ButtonState.Down) != 0 && !GameManager.instance.isPaused)
			{
				kickPower = kickPowerMin;
				canIncrease = true;
				GameManager.instance.showKickBar = true;
				GameManager.instance.kickBarTimer = 0f;
				kickCurvePower = 0f;
				kickCurvePowerTmp = 0f;
			}
			if (flag && canIncrease && (heroControllerScript.inputFire & ButtonState.Pressed) != 0 && !GameManager.instance.isPaused && flag && GlobalManager.instance.GetKey(Keys.VerticalKick) && !GameManager.instance.isRacingBall && !GameManager.instance.isHockey && !GameManager.instance.isHandball && (!GlobalManager.instance.GetKey(Keys.CurveLeft) || !GlobalManager.instance.GetKey(Keys.CurveRight)))
			{
				if (GlobalManager.instance.GetKey(Keys.CurveLeft) && GlobalManager.instance.curveMode == CurveMode.Mouse)
				{
					kickCurvePowerTmp = (0f - kickCurvePowerMax) * (float)((GlobalManager.instance.curveMode == CurveMode.Mouse) ? 1 : (-1));
					kickCurvePowerTmp *= 1.05f;
				}
				if (GlobalManager.instance.GetKey(Keys.CurveRight) && GlobalManager.instance.curveMode == CurveMode.Mouse)
				{
					kickCurvePowerTmp = kickCurvePowerMax * (float)((GlobalManager.instance.curveMode == CurveMode.Mouse) ? 1 : (-1));
					kickCurvePowerTmp *= 1.05f;
				}
				if (GlobalManager.instance.GetKey(Keys.CurveLeft) && GlobalManager.instance.curveMode == CurveMode.QE)
				{
					kickCurvePowerTmp = (0f - kickCurvePowerMax) * (float)((GlobalManager.instance.curveMode == CurveMode.QE) ? 1 : (-1));
					kickCurvePowerTmp *= 2f;
				}
				if (GlobalManager.instance.GetKey(Keys.CurveRight) && GlobalManager.instance.curveMode == CurveMode.QE)
				{
					kickCurvePowerTmp = kickCurvePowerMax * (float)((GlobalManager.instance.curveMode == CurveMode.QE) ? 1 : (-1));
					kickCurvePowerTmp *= 2f;
				}
				else
				{
					if (GlobalManager.instance.GetKey(Keys.CurveLeft))
					{
						kickCurvePowerTmp = (0f - kickCurvePowerMax) * (float)((GlobalManager.instance.curveMode == CurveMode.Mouse || GlobalManager.instance.curveMode == CurveMode.QE) ? 1 : (-1));
						kickCurvePowerTmp *= 2f;
					}
					if (GlobalManager.instance.GetKey(Keys.CurveRight) && GlobalManager.instance.curveMode == CurveMode.Mouse)
					{
						kickCurvePowerTmp = kickCurvePowerMax * (float)((GlobalManager.instance.curveMode == CurveMode.Mouse || GlobalManager.instance.curveMode == CurveMode.QE) ? 1 : (-1));
						kickCurvePowerTmp *= 2f;
					}
				}
			}
			if (canIncrease && (heroControllerScript.inputFire & ButtonState.Pressed) != 0 && !GameManager.instance.isPaused && !GameManager.instance.isRacingBall && (GlobalManager.instance.curveMode == CurveMode.Mouse || GlobalManager.instance.curveMode == CurveMode.MouseReversed))
			{
				kickCurvePowerTmp -= heroControllerScript.inputMouseX * GlobalManager.instance.mouseSpeed * 1.5f * (float)((GlobalManager.instance.curveMode == CurveMode.Mouse) ? 1 : (-1));
			}
			if (Object.op_Implicit((Object)(object)networkInputExchange))
			{
				networkInputExchange.kickIncrease = false;
			}
			if (flag && canIncrease && (heroControllerScript.inputFire & ButtonState.Pressed) != 0 && !GameManager.instance.isPaused)
			{
				kickPower += StaticVariables.kickPowerIncrease * num;
				if (kickPower > kickPowerMax)
				{
					kickPower = kickPowerMax;
					canIncrease = false;
				}
				GameManager.instance.showKickBar = true;
				GameManager.instance.kickBarTimer = 0f;
				if (Object.op_Implicit((Object)(object)networkInputExchange))
				{
					networkInputExchange.kickIncrease = true;
				}
				float num2 = 250f;
				if (!GameManager.instance.isRacingBall)
				{
					if (GlobalManager.instance.GetKey(Keys.CurveLeft))
					{
						kickCurvePowerTmp -= num2 * num * (float)((GlobalManager.instance.curveMode == CurveMode.QE || GlobalManager.instance.curveMode == CurveMode.Mouse) ? 1 : (-1));
					}
					if (GlobalManager.instance.GetKey(Keys.CurveRight))
					{
						kickCurvePowerTmp += num2 * num * (float)((GlobalManager.instance.curveMode == CurveMode.QE || GlobalManager.instance.curveMode == CurveMode.Mouse) ? 1 : (-1));
					}
				}
				kickCurvePower = kickCurvePowerTmp;
				if (Mathf.Abs(kickCurvePower) <= StaticVariables.kickCurvePowerBottomEdge)
				{
					kickCurvePower = 0f;
				}
				if (kickCurvePower > StaticVariables.kickCurvePowerBottomEdge)
				{
					kickCurvePower -= StaticVariables.kickCurvePowerBottomEdge;
					if (kickCurvePower < 0f)
					{
						kickCurvePower = 0f;
					}
				}
				else if (kickCurvePower < 0f - StaticVariables.kickCurvePowerBottomEdge)
				{
					kickCurvePower += StaticVariables.kickCurvePowerBottomEdge;
					if (kickCurvePower > 0f)
					{
						kickCurvePower = 0f;
					}
				}
				if (kickCurvePower < 0f - kickCurvePowerMax)
				{
					kickCurvePower = 0f - kickCurvePowerMax;
				}
				if (kickCurvePower > kickCurvePowerMax)
				{
					kickCurvePower = kickCurvePowerMax;
				}
			}
			if (flag)
			{
				if (kickPower > 0f)
				{
					GameManager.instance.kickPower = kickPower;
					GameManager.instance.kickCurvePower = kickCurvePower;
				}
				KickService();
			}
		}
		if (!flag && !GameManager.instance.isPaused)
		{
			return;
		}
		if (Object.op_Implicit((Object)(object)networkInputExchange))
		{
			networkInputExchange.inputHorizontal = newInputHorizontal;
			networkInputExchange.inputVertical = newInputVertical;
			networkInputExchange.inputJump = (ButtonState)0;
			if (GlobalManager.instance.GetKeyDown(Keys.Jump))
			{
				networkInputExchange.inputJump |= ButtonState.Down;
			}
			if (GlobalManager.instance.GetKeyUp(Keys.Jump))
			{
				networkInputExchange.inputJump |= ButtonState.Up;
			}
			if (GlobalManager.instance.GetKey(Keys.Jump))
			{
				networkInputExchange.inputJump |= ButtonState.Pressed;
			}
			if (!Object.op_Implicit((Object)(object)heroControllerScript))
			{
				return;
			}
			bool usePlayerPosition = false;
			Vector3[] array = (Vector3[])(object)new Vector3[StaticVariables.ballObjectsMax];
			Vector3 playerPosition = default(Vector3);
			if (PredictionManager.instance.finishedIndexInfo.index >= 0 && PredictionManager.instance.finishedIndexInfo.equal && GameManager.instance.localPlayerIndex >= 0)
			{
				usePlayerPosition = true;
				for (int i = 0; i < StaticVariables.ballObjectsMax; i++)
				{
					array[i] = PredictionManager.instance.finishedBallStates[PredictionManager.instance.finishedIndexInfo.index].ballStateHelper[i].position;
				}
				playerPosition = PredictionManager.instance.finishedPlayers[GameManager.instance.localPlayerIndex].playerStates[PredictionManager.instance.finishedIndexInfo.index].position;
			}
			PredictionManager.instance.AddLocalPlayerInput(networkInputExchange.inputHorizontal, networkInputExchange.inputVertical, ((Component)this).transform.eulerAngles.y, networkInputExchange.inputJump, networkInputExchange.kickPower, networkInputExchange.kickCurvePower, GlobalManager.instance.oldShift, array, usePlayerPosition, playerPosition, msecOffset);
			networkInputExchange.kickPower = 0f;
			networkInputExchange.kickCurvePower = 0f;
		}
		else if (Object.op_Implicit((Object)(object)heroControllerScript))
		{
			heroControllerScript.inputHorizontal = newInputHorizontal;
			heroControllerScript.inputVertical = newInputVertical;
			heroControllerScript.inputJump = (ButtonState)0;
			if (GlobalManager.instance.GetKeyDown(Keys.Jump))
			{
				heroControllerScript.inputJump |= ButtonState.Down;
			}
			if (GlobalManager.instance.GetKeyUp(Keys.Jump))
			{
				heroControllerScript.inputJump |= ButtonState.Up;
			}
			if (GlobalManager.instance.GetKey(Keys.Jump))
			{
				heroControllerScript.inputJump |= ButtonState.Pressed;
			}
		}
	}

	public override void KickService()
	{
		if (!(kickPower <= 0f) && ((heroControllerScript.inputFire & ButtonState.Pressed) == 0 || !(kickPower < kickPowerMax)))
		{
			if (!(kickPower <= kickPowerMax))
			{
				kickPower = kickPowerMax;
			}
			if (!(kickPower >= 0f))
			{
				kickPower = 0f;
			}
			bool flag = false;
			if (GlobalManager.instance.GetKey(Keys.VerticalKick) && !GameManager.instance.isRacingBall && !GameManager.instance.isHockey && !GameManager.instance.isHandball)
			{
				flag = true;
				if ((GlobalManager.instance.curveMode == CurveMode.Mouse || GlobalManager.instance.curveMode == CurveMode.MouseReversed) && newInputHorizontal != 0 && !(kickPower < kickPowerMax) && !GlobalManager.instance.GetKey(Keys.CurveLeft) && !GlobalManager.instance.GetKey(Keys.CurveRight))
				{
					kickCurvePower = 0f;
				}
			}
			else if (!GameManager.instance.isUrbanBeta)
			{
			}
			if (Network.isClient)
			{
				networkInputExchange.kickPower = ((!flag) ? kickPower : (0f - kickPower));
				networkInputExchange.kickCurvePower = kickCurvePower;
				GameManager.instance.drawVerticalArrow = flag;
			}
			else
			{
				bool flag2 = false;
				if (heroControllerScript.canKick != 0)
				{
					for (int i = 0; i < GameManager.instance.currentBallsCount; i++)
					{
						if ((heroControllerScript.canKick & (1 << i)) != 0)
						{
							heroControllerScript.KickNowServerOneBall((!flag) ? kickPower : (0f - kickPower), kickCurvePower, GlobalManager.instance.oldShift, i);
							flag2 = true;
						}
					}
				}
				if (flag2)
				{
					heroControllerScript.kickPowerHoldTimer = -1f;
					GameManager.instance.drawVerticalArrow = flag;
				}
				else
				{
					heroControllerScript.kickPowerHoldTimer = 0f;
					heroControllerScript.kickPowerHoldPower = ((!flag) ? kickPower : (0f - kickPower));
					heroControllerScript.kickPowerHoldCurvePower = kickCurvePower;
					heroControllerScript.kickHoldOldShift = GlobalManager.instance.oldShift;
					GameManager.instance.drawVerticalArrow = flag;
				}
			}
			canIncrease = false;
			kickPower = 0f;
			kickCurvePower = 0f;
		}
		if (!(kickPower < kickPowerMax))
		{
			kickPower = 0f;
		}
	}

	public override void Main()
	{
	}
}
