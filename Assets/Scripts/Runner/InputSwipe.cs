using System;
using Interface;
using UnityEngine;

namespace Runner
{
    public class InputSwipe : IInput
    {
        private const int _SwipeThreshold = 100;

        public event Action onJump;
        public event Action<int> onTurnSide;

        public void Handle()
        {
            switch (ReadGesture())
            {
                case Gesture.SwipeLeft:
                    onTurnSide.Invoke(-1);
                    break;
                case Gesture.SwipeRight:
                    onTurnSide.Invoke(1);
                    break;
                case Gesture.SwipeUp:
                    onJump.Invoke();
                    break;
            }
        }

        private enum Gesture
        {
            None, Tap, SwipeLeft, SwipeRight, SwipeUp, SwipeDown
        }


        private bool _isDragging = false;
        private Vector2 _startTouch;
        private Vector2 _swipeDelta;


        private Gesture ReadGesture()
        {
            var actualGesture = Gesture.None;

            #region ForPC

#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
            {
                actualGesture = Gesture.Tap;
                _isDragging = true;
                _startTouch = Input.mousePosition;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                _isDragging = false;
                actualGesture = Gesture.None;
                ResetTouch();
            }
#endif

            #endregion

            if (Input.touches.Length > 0)
            {
                if (Input.touches[0].phase == TouchPhase.Began)
                {
                    actualGesture = Gesture.Tap;
                    _isDragging = true;
                    _startTouch = Input.touches[0].position;
                }
                else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
                {
                    _isDragging = false;
                    ResetTouch();
                }
            }

            _swipeDelta = Vector2.zero;
            if (_isDragging)
            {
                if (Input.touches.Length < 0)
                    _swipeDelta = Input.touches[0].position - _startTouch;
                else if (Input.GetMouseButton(0))
                    _swipeDelta = (Vector2)Input.mousePosition - _startTouch;
            }


            if (_swipeDelta.magnitude > _SwipeThreshold)
            {
                float x = _swipeDelta.x;
                float y = _swipeDelta.y;
                if (Mathf.Abs(x) > Mathf.Abs(y))
                {

                    if (x < 0)
                        actualGesture = Gesture.SwipeLeft;
                    else
                        actualGesture = Gesture.SwipeRight;
                }
                else
                {

                    if (y < 0)
                        actualGesture = Gesture.SwipeDown;
                    else
                        actualGesture = Gesture.SwipeUp;
                }

                ResetTouch();
            }

            return actualGesture;
        }

        private void ResetTouch()
        {
            _startTouch = _swipeDelta = Vector2.zero;
            _isDragging = false;
        }
    }
}
