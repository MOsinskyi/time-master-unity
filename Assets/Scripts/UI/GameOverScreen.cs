using System.Collections;
using System.Collections.Generic;
using Game;
using Sound;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

internal enum Styles
{
    Default,
    NewHighScore
}

namespace UI
{
    public class GameOverScreen : MonoBehaviour
    {
        [SerializeField] private List<GameObject> styles;
        [SerializeField] private List<Button> playButtons;
        [SerializeField] private TMP_Text currentScore;
        [SerializeField] private TMP_Text highScoresStyle1;
        [SerializeField] private TMP_Text highScoresStyle2;

        [SerializeField] private float addScoreDelay;

        private bool _animationRunning;

        private Score _score;
        
        private void Start()
        {
            BindPlayButtons();
            ChangeStateOfPlayButtons(false);

            _score = FindAnyObjectByType<Score>();
        }

        private void OnEnable()
        {
            GameEvents.GameOver += ShowGameOverScreen;
        }

        private void OnDisable()
        {
            GameEvents.GameOver -= ShowGameOverScreen;
        }

        private void PlayButtonSound()
        {
            SoundManager.Instance.PlaySfx(Config.AudioName.ButtonClick);
        }

        private void BindPlayButtons()
        {
            foreach (var button in playButtons)
            {
                button.onClick.AddListener(PlayButtonSound);
                button.onClick.AddListener(PlayButton);
            }
        }

        private void ChangeStateOfPlayButtons(bool state = true)
        {
            foreach (var button in playButtons)
                button.gameObject.SetActive(state);
        }

        private IEnumerator AddScoreAnimation(int score, TMP_Text scoreText)
        {
            var tempScore = 0;
            _animationRunning = true;
            SoundManager.Instance.PlaySfx(Config.AudioName.ScoreCounting, true);
            while (tempScore < score)
            {
                tempScore++;
                scoreText.text = tempScore.ToString();
                if (Input.GetMouseButtonDown(0))
                    break;
                yield return new WaitForSeconds(addScoreDelay);
            }
            
            scoreText.text = score.ToString();

            SoundManager.Instance.StopSfx();

            _animationRunning = false;
            ChangeStateOfPlayButtons();
            SoundManager.Instance.PlaySfx(Config.AudioName.Pop);
        }

        private void ShowGameOverScreen()
        {
            if (_score.NewBestScore)
            {
                styles[(int)Styles.NewHighScore].SetActive(true);
                StartCoroutine(AddScoreAnimation(_score.BestScore, highScoresStyle2));
            }
            else
            {
                styles[(int)Styles.Default].SetActive(true);
                StartCoroutine(AddScoreAnimation(_score.CurrentScore, currentScore));
                highScoresStyle1.text = _score.BestScore.ToString();
            }
        }

        private void PlayButton()
        {
            if (!_animationRunning)
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}