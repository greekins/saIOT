using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedTest.Poco
{
    class TestMessage
    {
        public string Foo { get; set; }
        public string Bar { get; set; }

        public TestMessage()
        {
            Foo = "foo";
            Bar = "bar";
        }
    }
}
