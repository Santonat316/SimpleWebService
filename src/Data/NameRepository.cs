using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebApplication.Data
{
    /// <summary>
    /// We initialize a new list. The list is used for Seeding data
    /// </summary>
    public static class NameRepository
    {
        public static List<string> NameCollection { get; } = new List<string>();
    }
}
