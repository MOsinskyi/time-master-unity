using System;
using System.Collections.Generic;
using Sound;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public enum ButtonType
    {
        Close,
        Replay,
        Settings
    }

    public enum ToggleType
    {
        Sound,
        Music
    }
    
    [Serializable]
    public class SettingButton
    {
        public ButtonType type;
        public Button button;
    }
    
    [Serializable]
    public class SettingToggle
    {
        public ToggleType type;
        public Toggle toggle;
    }
    
    
    public class SettingsScreen : MonoBehaviour
    {
        [SerializeField] private GameObject settingsPanel;
        
        [SerializeField] private List<SettingToggle> toggles = new();
        [SerializeField] private List<SettingButton> buttons = new();

        private void Start()
        {
            foreach (var button in buttons)
            {
                switch (button.type)
                {
                    case ButtonType.Close:
                        button.button.onClick.AddListener(CloseSettings);
                        break;
                    case ButtonType.Replay:
                        button.button.onClick.AddListener(Replay);
                        break;
                    case ButtonType.Settings:
                        button.button.onClick.AddListener(OpenSettings);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                
                button.button.onClick.AddListener(() => SoundManager.Instance.PlaySfx(Config.AudioName.ButtonClick));
            }

            foreach (var toggle in toggles)
            {
                switch (toggle.type)
                {
                    case ToggleType.Sound:
                        toggle.toggle.onValueChanged.AddListener(SoundManager.Instance.ToggleSfx);
                        toggle.toggle.isOn = SoundManager.Instance.SfxStatus;
                        break;
                    case ToggleType.Music:
                        toggle.toggle.onValueChanged.AddListener(SoundManager.Instance.ToggleMusic);
                        toggle.toggle.isOn = SoundManager.Instance.SoundStatus;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                
                toggle.toggle.onValueChanged.AddListener(_ => SoundManager.Instance.PlaySfx(Config.AudioName.ButtonClick));
            }
        }

        private void OpenSettings()
        {
            settingsPanel.SetActive(true);
        }

        private void CloseSettings()
        {
            settingsPanel.SetActive(false);
        }

        private void Replay()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
