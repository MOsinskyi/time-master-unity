using System.Collections;
using System.Globalization;
using Sound;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ReviveScreen : MonoBehaviour
    {
        [SerializeField] private float reviveTime = 5f;
        [SerializeField] private GameObject revivePanel;
        [SerializeField] private TMP_Text cooldownText;
        [SerializeField] private Image radialCooldownImage;

        private void Start()
        {
            radialCooldownImage.fillAmount = 1f;
            cooldownText.text = reviveTime.ToString(CultureInfo.CurrentCulture);
        }

        private void OnEnable()
        {
            GameEvents.StartReviveCooldown += ShowReviveScreen;
        }

        private void OnDisable()
        {
            GameEvents.StartReviveCooldown -= ShowReviveScreen;
        }

        private void ShowReviveScreen()
        {
            revivePanel.SetActive(true);
            SoundManager.Instance.StopMusic();
            StartCoroutine(CooldownRoutine());
        }

        private IEnumerator CooldownRoutine()
        {
            var timeLeft = reviveTime;
            while (timeLeft > 0)
            {
                yield return new WaitForSeconds(1f);
                timeLeft -= 1f;
                cooldownText.text = timeLeft.ToString(CultureInfo.CurrentCulture);
                radialCooldownImage.fillAmount -= 1f / reviveTime;
                SoundManager.Instance.PlaySfx(Config.AudioName.CooldownTimer);
            }
            
            revivePanel.SetActive(false);
            GameEvents.GameOver.Invoke();
        }
    }
}
