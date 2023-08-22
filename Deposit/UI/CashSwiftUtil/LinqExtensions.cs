
// Type: CashSwiftUtil.LinqExtensions
// Assembly: CashSwiftUtil, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 885F1C6C-21D2-4135-B89E-154B0975A233
// Assembly location: C:\DEV\maniwa\Coop\Coop\CashSwiftDeposit\App\UI\6.0\CashSwiftUtil.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace CashSwiftUtil
{
    public static class LinqExtensions
    {
        public static IEnumerable<T> Flatten<T>(
          this IEnumerable<T> source,
          Func<T, IEnumerable<T>> childPropertySelector)
        {
            return source.Flatten((itemBeingFlattened, objectsBeingFlattened) => childPropertySelector(itemBeingFlattened));
        }

        public static IEnumerable<T> Flatten<T>(
          this IEnumerable<T> source,
          Func<T, IEnumerable<T>, IEnumerable<T>> childPropertySelector)
        {
            return source.Concat(source.Where(item => childPropertySelector(item, source) != null).SelectMany(itemBeingFlattened => childPropertySelector(itemBeingFlattened, source).Flatten(childPropertySelector)));
        }
    }
}
