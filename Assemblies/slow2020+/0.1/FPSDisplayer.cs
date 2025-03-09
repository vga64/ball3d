using System;
using UnityEngine;
using UnityScript.Lang;

// Token: 0x02000099 RID: 153
[Serializable]
public class FPSDisplayer : MonoBehaviour
{
	// Token: 0x0600019D RID: 413 RVA: 0x00003532 File Offset: 0x00001732
	public FPSDisplayer()
	{
		this.updatePeriod = 0.1f;
		this.latenyUpdateTime = 1f;
	}

	// Token: 0x0600019E RID: 414 RVA: 0x00019094 File Offset: 0x00017294
	public virtual void Update()
	{
		GameManager.instance.showFps = true;
		if (!GameManager.instance.leave && Input.GetKeyDown(KeyCode.F3))
		{
			GameManager.instance.leave = true;
			Network.Disconnect(10);
		}
		if (GameManager.instance)
		{
			GlobalManager.instance.infoString = string.Empty;
			this.frames += 1f;
			if (Time.unscaledTime >= this.nextUpdate)
			{
				this.prevFps = this.fps;
				this.fps = Mathf.Round(this.frames / (this.updatePeriod + (Time.unscaledTime - this.nextUpdate)));
				this.nextUpdate = Time.unscaledTime + this.updatePeriod;
				this.frames = 0f;
			}
			if (GlobalManager.instance.infoFps)
			{
				GlobalManager.instance.infoString = GlobalManager.instance.infoString + "FPS: " + this.fps.ToString("F0") + "\n";
			}
			if (GlobalManager.instance.infoPlayerSpeed && !ReplayManager.instance.isPlaying)
			{
				float num = 0f;
				if (GameManager.instance.localPlayer != null && GameManager.instance.localPlayer.cacheRigidbody)
				{
					num = new Vector2(GameManager.instance.localPlayer.cacheRigidbody.velocity.z, GameManager.instance.localPlayer.cacheRigidbody.velocity.x).magnitude;
				}
				GlobalManager.instance.infoString = GlobalManager.instance.infoString + "Player Speed: " + num.ToString("F2") + " u/s\n";
			}
			if (GlobalManager.instance.infoSlowdown && !ReplayManager.instance.isPlaying)
			{
				float num2 = 1f;
				if (GameManager.instance.localPlayer != null && GameManager.instance.localPlayer.heroControllerComponent)
				{
					num2 = GameManager.instance.localPlayer.heroControllerComponent.closeBallSlowDownFactor;
				}
				GlobalManager.instance.infoString = GlobalManager.instance.infoString + "Slowdown: " + ((1f - num2) * 100f).ToString("F2") + "%\n";
			}
			if (GlobalManager.instance.infoPing && !ReplayManager.instance.isPlaying)
			{
				float num3 = 0f;
				if (Network.isClient)
				{
					num3 = (float)Network.GetLastPing(Network.connections[0]);
				}
				GlobalManager.instance.infoString = GlobalManager.instance.infoString + "Ping: " + UnityBuiltins.parseInt(num3 + 0.5f).ToString("F0") + " ms\n";
			}
			if (GlobalManager.instance.infoLatency && !ReplayManager.instance.isPlaying)
			{
				if (Network.isClient)
				{
					if (GameManager.instance.localPlayer != null && GameManager.instance.localPlayer.team != Team.Spec)
					{
						this.latencyUpdateTimer += Time.unscaledDeltaTime;
						if (this.latencyUpdateTimer >= this.latenyUpdateTime)
						{
							this.latencyUpdateTimer = 0f;
							this.latency = (PredictionManager.instance.globalMsec - PredictionManager.instance.localPlayerMsec) * 0.01666f * 1000f;
						}
					}
					else
					{
						this.latency = 0f;
					}
				}
				else
				{
					this.latency = 0f;
				}
				if (GameManager.instance.localPlayer != null && GameManager.instance.localPlayer.team != Team.Spec)
				{
					GlobalManager.instance.infoString = GlobalManager.instance.infoString + "Latency: " + UnityBuiltins.parseInt(this.latency + 0.5f).ToString("F0") + " ms\n";
				}
				else
				{
					GlobalManager.instance.infoString = GlobalManager.instance.infoString + "Latency: -\n";
				}
			}
			if (GlobalManager.instance.infoNetwork && !ReplayManager.instance.isPlaying)
			{
				int num4 = 0;
				int num5 = 0;
				if (Network.isClient || Network.isServer)
				{
					num4 = StaticVariables.serverSendRate;
					num5 = StaticVariables.clientSendRate;
				}
				GlobalManager.instance.infoString = string.Concat(new object[]
				{
					GlobalManager.instance.infoString,
					"Server Sendrate: ",
					num4,
					"\n"
				});
				if (Network.isClient && GameManager.instance.localPlayer != null && GameManager.instance.localPlayer.team != Team.Spec)
				{
					GlobalManager.instance.infoString = string.Concat(new object[]
					{
						GlobalManager.instance.infoString,
						"Client Sendrate: ",
						num5,
						"\n"
					});
					return;
				}
				GlobalManager.instance.infoString = GlobalManager.instance.infoString + "Client Sendrate: -\n";
			}
		}
	}

	// Token: 0x0600019F RID: 415 RVA: 0x00019594 File Offset: 0x00017794
	public virtual void OnGUI()
	{
		if (GameManager.instance && GlobalManager.instance.infoString != string.Empty)
		{
			GUI.skin = this.skin;
			GUI.Label(new Rect((float)(Screen.width - 250), 0f, 250f, 300f), GlobalManager.instance.infoString, "infoPanel");
		}
	}

	// Token: 0x060001A0 RID: 416 RVA: 0x000024E5 File Offset: 0x000006E5
	public virtual void Main()
	{
	}

	// Token: 0x04000441 RID: 1089
	public GUISkin skin;

	// Token: 0x04000442 RID: 1090
	private float updatePeriod;

	// Token: 0x04000443 RID: 1091
	private float nextUpdate;

	// Token: 0x04000444 RID: 1092
	private float frames;

	// Token: 0x04000445 RID: 1093
	private float fps;

	// Token: 0x04000446 RID: 1094
	private float prevFps;

	// Token: 0x04000447 RID: 1095
	private float latency;

	// Token: 0x04000448 RID: 1096
	private float latencyUpdateTimer;

	// Token: 0x04000449 RID: 1097
	private float latenyUpdateTime;

	// Token: 0x0400044A RID: 1098
	public GUIStyle infoPanel;
}
