using UnityEngine;
using System.Collections.Generic;

namespace Crab.Utils
{
    public class Delay
    {
        private int length;
        private float endTime;
        private bool started;

        public Delay(int length, bool start = true)
        {
            this.length = length;
            if (start)
                Start();

            started = start;
        }
        public Delay(float seconds, bool start = true)
        {
            length = (int)(seconds*1000);
            if (start)
                Start();

            started = start;
        }

        public bool Over()
        {
            if (!started)
                return false;

            return Time.time * 1000 >= endTime;
        }

        public float TimeRemaining() {
            return IsStarted() ? (float)endTime / 1000 - Time.time : 0;
        }

        public void Start(int newLength = -1)
        {
            if (newLength != -1) {
                length = newLength;
            }


            started = true;
            endTime = length + Time.time * 1000;
        }

        //Start the delay with x seconds
        public void Start(float newLength)
        {
            Start((int)(newLength * 1000));
        }

        public void Reset()
        {
            started = false;
        }

        public void End()
        {
            started = true;
            endTime = 0;
        }

        public bool IsStarted() {
            return started;
        }
    }
}