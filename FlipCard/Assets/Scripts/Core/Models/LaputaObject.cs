using System;
using System.Collections.Generic;
using UnityEngine;

namespace Model
{
    [Serializable]
    public class LaputaObject
    {
        public Identity Identity;

        public LaputaObject(Identity identity)
        {
            Identity = identity;
        }

        public LaputaObject() { }
    }
}
