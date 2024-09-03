using System;
using System.Collections.Generic;
using System.Text;

namespace DIP.Templates
{
    public class XmlTemplate<T>
    {
        public DB4002XMLNode DB4002XML { get; set; }

        public class DB4002XMLNode
        {
            public FileNode File { get; set; }

            public class FileNode
            {
                public T[] Record { get; set; }
            }
        }
    }
}
