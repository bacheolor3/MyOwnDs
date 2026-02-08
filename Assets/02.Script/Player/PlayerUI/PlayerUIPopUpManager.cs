using System.Collections;
using TMPro;
using UnityEngine;


namespace TSG
{
    public class PlayerUIPopUpManager : MonoBehaviour
    {
        [Header("유다이 뜨게 하기")]
        [SerializeField] GameObject youDiedPopUpGameObject;
        [SerializeField] TextMeshProUGUI youDiedPopUpBackgroundText;
        [SerializeField] TextMeshProUGUI youDiedPopUpText;
        [SerializeField] CanvasGroup youDiedPopUpCanvasGroup;   // 알파에서 시간 지날수록 페이드 되게 해줌

        public void SendYouDiedPopUp()
        {
            // 전조 효과 실행

            youDiedPopUpGameObject.SetActive(true);
            youDiedPopUpBackgroundText.characterSpacing = 0;
            // 팝업을 늘리고
            StartCoroutine(StretchPopUpTextOverTime(youDiedPopUpBackgroundText, 8, 19f));
            // 페이드 인 띄우고
            StartCoroutine(FadeInPopUpOverTime(youDiedPopUpCanvasGroup, 5));
            // 기다리다가, 그리고 페이드 아웃 나오게
            StartCoroutine(WaitThenFadeOutPopUpOverTime(youDiedPopUpCanvasGroup, 2, 5));
        }
        
        private IEnumerator StretchPopUpTextOverTime(TextMeshProUGUI text, float duration, float stretchAmount)
        {
            if(duration > 0f)
            {
                text.characterSpacing = 0;  // 글자간 간격을 리셋해준다
                float timer = 0;
                yield return null;

                while(timer < duration)
                {
                    timer = timer + Time.deltaTime;
                    text.characterSpacing = Mathf.Lerp(text.characterSpacing, stretchAmount, duration * (Time.deltaTime / 20));
                    yield return null;
                }
            }
        }

        private IEnumerator FadeInPopUpOverTime(CanvasGroup canvas, float duration)
        {
            if(duration > 0)
            {
                canvas.alpha = 0;
                float timer = 0;

                yield return null;

                while(timer < duration)
                {
                    timer = timer + Time.deltaTime;
                    canvas.alpha = Mathf.Lerp(canvas.alpha, 1, duration * Time.deltaTime);
                    yield return null;
                }
            }
            canvas.alpha = 1;

            yield return null;
        }

        private IEnumerator WaitThenFadeOutPopUpOverTime(CanvasGroup canvas, float duration, float delay)
        {
            if(duration > 0)
            {
                while(delay > 0)
                {
                    delay = delay - Time.deltaTime;
                    yield return null;
                }
                canvas.alpha = 1;
                float timer = 0;

                yield return null;

                while(timer < duration)
                {
                    timer = timer + Time.deltaTime;
                    canvas.alpha = Mathf.Lerp(canvas.alpha, 0, duration * Time.deltaTime);
                    yield return null;
                }
            }
            canvas.alpha = 0;

            yield return null;
        }
    }
}
