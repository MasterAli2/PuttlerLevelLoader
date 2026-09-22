using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MelonLoader;
using UnityEngine;

namespace test.Test
{
    public class InJectedBundle : MonoBehaviour
    {

        public float fish = 6.7f;

        public InJectedBundle(IntPtr ptr) : base(ptr) {}

        void Start()
        {
            MelonLogger.Msg($"im here with fish number: {fish}");
        }
    }
}