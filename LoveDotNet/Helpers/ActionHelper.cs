using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoveDotNet.Helpers
{
    public enum ActionType
    {
        Apply
    }

    public static class ActionHelper
    {
        public static Dictionary<string, (int, ActionType)> ActionDict { get; set; } 
            = new Dictionary<string, (int, ActionType)>();
    }
}
