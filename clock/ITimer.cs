﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clock
{
    interface ITimer
    {
        void Start();
        void Pause();
        void Clear();
        void Increment(string time);
        void ChangeFormat();
    }
}
