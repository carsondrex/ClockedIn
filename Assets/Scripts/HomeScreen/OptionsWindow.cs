using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.UI;

public class OptionsWindow : MonoBehaviour
{

    public AudioMixer audioMixer;
    public TMP_Dropdown graphicsDropdown;
    public TMP_Dropdown resolutionDropdown;
    Resolution[] resolutions;

    void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();

        int currentResIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResIndex = i;
            }

        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
        graphicsDropdown.value = PlayerPrefs.GetInt("QualityLevel", 5);
        graphicsDropdown.RefreshShownValue();
    }

    //0 for medium, 1 for high, 2 for ultra
    public void SetQuality(int qualityIndex)
    {
        int setIndex = 0;
        if (qualityIndex == 0)
        {
            setIndex = 2;
        }
        else if (qualityIndex == 1)
        {
            setIndex = 3;
        }
        else if (qualityIndex == 2)
        {
            setIndex = 5;
        }
        QualitySettings.SetQualityLevel(setIndex);
        PlayerPrefs.SetInt("QualityLevel", setIndex);
    }

}
