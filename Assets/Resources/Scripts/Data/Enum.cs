using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enum
{
    public class Enum
    {
        public enum State
        {
            Move,
            Wait
        }
        public enum Bomb
        {
            None,
            columnBomb,
            rowBomb,
            areaBomb
        }
    }
}
