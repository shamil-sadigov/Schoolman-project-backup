using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Helpers
{
    public class Assert
    {
        public class Is
        {
            public static bool NullOrWhiteSpace(params string[] values)
            {
                foreach (var item in values)
                {
                    if (string.IsNullOrWhiteSpace(item))
                        return true;
                }
                return false;
            }
        }
    }
}
