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
            {
                Start();
            }
            started = start;
        }

        public bool Over()
        {
            if (!started)
                return false;

            return Time.time * 1000 >= endTime;
        }

        public void Start(int newLength = -1)
        {
            if (newLength != -1) {
                length = newLength;
            }


            started = true;
            endTime = length + Time.time * 1000;
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
    }
}