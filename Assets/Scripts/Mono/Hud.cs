using System;
using TMPro;
using UnityEngine;

namespace Mono
{
    public class Hud : MonoBehaviour
    {
        [SerializeField] private TMP_Text _scoreTxt;
        [SerializeField] private TMP_Text _coinsTxt;
        [SerializeField] private GameObject _failScreen;

        public event Action onRestartClick;
        
        
        public int Score
        {
            set
            {
                
            }
        }
        
        public int Coins
        {
            set
            {
                _coinsTxt?.SetText( value.ToString());
            }
        }

        public bool GameFail
        {
            set
            {
                _failScreen?.SetActive(value);
            }
        }

        public void OnButton_Restart() => onRestartClick?.Invoke();
    }
}