using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Utility;

namespace Game
{
    [Serializable]
    public class BestScoreData
    {
        public int Score { get; set; }
    }

    [Serializable]
    public struct AddScoreDelay
    {
        [Range(0.01f, 0.1f)] public float min;
        [Range(0.5f, 1f)] public float max;
    }

    public class Score : MonoBehaviour
    {
        private const string BestScoreKey = "bsdat";
        
        [SerializeField] private AddScoreDelay addScoreDelay;
        
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private TMP_Text bestScoreText;
        [field: SerializeField] public int LineCompletedScore { get; private set; } = 10;
        [field: SerializeField] public int SquarePlacedScore { get; private set; } = 1;

        private bool _animationRunning;

        private BestScoreData _bestScoreData = new();

        public int CurrentScore { get; private set; }
        public bool NewBestScore { get; private set; }

        public int BestScore => _bestScoreData.Score;

        private void Awake()
        {
            if (BinaryDataStream.Exist(BestScoreKey)) StartCoroutine(ReadDataFile());
        }

        private void Start()
        {
            CurrentScore = 0;
            UpdateScoreText(CurrentScore);
        }

        private void OnEnable()
        {
            GameEvents.AddScore += AddScore;
            GameEvents.GameOver += SaveBestScore;
        }

        private void OnDisable()
        {
            GameEvents.AddScore -= AddScore;
            GameEvents.GameOver -= SaveBestScore;
        }

        private IEnumerator ReadDataFile()
        {
            _bestScoreData = BinaryDataStream.Read<BestScoreData>(BestScoreKey);
            yield return new WaitForEndOfFrame();
            UpdateBestScoreText(_bestScoreData.Score);
        }

        private void SaveBestScore()
        {
            BinaryDataStream.Save(_bestScoreData, BestScoreKey);
        }

        private void AddScore(int score)
        {
            CurrentScore += score;
            if (CurrentScore > _bestScoreData.Score)
            {
                _bestScoreData.Score = CurrentScore;
                NewBestScore = true;
            }

            if (!_animationRunning)
                StartCoroutine(AddingScoreAnimation());
        }

        private IEnumerator AddingScoreAnimation()
        {
            var tempScore = Convert.ToInt32(scoreText.text);
            var pointsToAdd = CurrentScore - tempScore;

            _animationRunning = true;

            while (tempScore < CurrentScore)
            {
                tempScore++;
                var waitTime = Mathf.Lerp(addScoreDelay.min, addScoreDelay.max,
                    (float)(tempScore - (CurrentScore - pointsToAdd)) / pointsToAdd);
                yield return new WaitForSeconds(waitTime);
                if (NewBestScore) UpdateBestScoreText(tempScore);
                UpdateScoreText(tempScore);
            }

            _animationRunning = false;
        }

        private void UpdateScoreText(int score)
        {
            scoreText.text = score.ToString();
        }

        private void UpdateBestScoreText(int score)
        {
            bestScoreText.text = score.ToString();
        }
    }
}