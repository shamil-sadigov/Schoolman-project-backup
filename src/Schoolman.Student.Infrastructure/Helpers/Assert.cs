using System;
using System.Collections.Generic;
using System.Text;

namespace Schoolman.Student.Infrastructure.Helpers
{
    public static class Is
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

        public static bool Null<T>(T obj)
        {
            if (obj == null)
                return true;
            else
                return false;
        }

        public static bool NotNull<T>(T obj)
        {
            return !Null(obj);
        }
    }
}
