using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class FPSDisplayer : MonoBehaviour
{
    public GUISkin skin;

    private string fpsString;

    private float updatePeriod;

    private float nextUpdate;

    private float frames;

    private float fps;

    private float prevFps;

    private Dictionary<string, string> configData = new Dictionary<string, string>();

    public int maxFps;

    public int vSync;

    public FPSDisplayer()
    {
        this.updatePeriod = 0.1f;
    }

    private void Awake()
    {
    }

    public virtual void Main()
    {
    }

    public virtual void OnGUI()
    {
        if (GameManager.instance.showFps)
        {
            GUI.set_skin(this.skin);
            GUI.Label(new Rect((float)(Screen.get_width() - 250), 0f, 250f, 50f), this.fpsString, "LabelBigWhite");
        }
    }

    private void ReadConfigFile(string path)
    {
        try
        {
            string[] strArray = File.ReadAllLines(path);
            for (int i = 0; i < (int)strArray.Length; i++)
            {
                string[] strArray1 = strArray[i].Split(new char[] { '=' });
                if ((int)strArray1.Length == 2)
                {
                    string str = strArray1[0].Trim();
                    string str1 = strArray1[1].Trim();
                    this.configData.set_Item(str, str1);
                }
            }
        }
        catch (Exception exception)
        {
        }
    }

    private void Start()
    {
        this.ReadConfigFile(string.Concat(Application.get_streamingAssetsPath(), "/config.txt"));
        if (this.configData.ContainsKey("maxFps"))
        {
            this.maxFps = int.Parse(this.configData.get_Item("maxFps"));
        }
        if (this.configData.ContainsKey("vSync"))
        {
            this.vSync = int.Parse(this.configData.get_Item("vSync"));
        }
        Application.set_targetFrameRate(this.maxFps);
        QualitySettings.set_vSyncCount(this.vSync);
    }

    public virtual void Update()
    {
        if (GameManager.instance.showFps)
        {
            this.frames += 1f;
            if (Time.get_unscaledTime() >= this.nextUpdate)
            {
                this.prevFps = this.fps;
                this.fps = Mathf.Round(this.frames / (this.updatePeriod + (Time.get_unscaledTime() - this.nextUpdate)));
                if (this.prevFps != this.fps || this.prevFps == 0f)
                {
                    this.fpsString = string.Concat("FPS: ", this.fps);
                }
                this.nextUpdate = Time.get_unscaledTime() + this.updatePeriod;
                this.frames = 0f;
            }
        }
    }
}
