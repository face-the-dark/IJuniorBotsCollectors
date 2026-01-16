using Base;
using TMPro;
using UnityEngine;

namespace UI
{
    public class ResourceBaseView : MonoBehaviour
    {
        private const string StartCount = "0";
        
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private ResourceBase _resourceBase;

        private void Start() => 
            _text.text = StartCount;

        private void OnEnable() => 
            _resourceBase.ScoreChanged += UpdateText;

        private void OnDisable() => 
            _resourceBase.ScoreChanged -= UpdateText;

        private void UpdateText(int count) => 
            _text.text = count.ToString();
    }
}