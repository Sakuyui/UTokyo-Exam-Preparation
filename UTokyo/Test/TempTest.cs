using System;
using System.Linq;

namespace UTokyo.Test
{
    public class TempTest
    {
        public static void Test()
        {
            var x = 121;
            string.Compare(x.ToString(), new string(x.ToString().Reverse().ToArray())
                , StringComparison.Ordinal).PrintToConsole();
        
            
        }

    
         
          
        
    }
}